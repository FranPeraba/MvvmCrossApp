using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MvvmCross.Base;
using MvvmCross.Commands;
using MvvmCross.IoC;
using MvvmCross.ViewModels;
using MvvmCrossApp.Core.Models;
using MvvmCrossApp.Core.Services;
using Xamarin.Essentials;

namespace MvvmCrossApp.Core.ViewModels
{
    public class DetailMedicineViewModel : MvxViewModel<Medicines>
    {
        readonly ILogger<SearchMedicinesViewModel> _logger;
        readonly ICimaService _cimaService;
        readonly IMvxIoCProvider _ioCProvider;
        string _nregistro;

        public DetailMedicineViewModel(ILogger<SearchMedicinesViewModel> logger, ICimaService cimaService, IMvxIoCProvider ioCProvider)
        {
            _logger = logger;
            _cimaService = cimaService;
            _ioCProvider = ioCProvider;

            _openProspectCommandAsync = new MvxAsyncCommand(OpenProspectAsync);
        }

        IMvxAsyncCommand _openProspectCommandAsync;
        public IMvxAsyncCommand OpenProspectCommandAsync => _openProspectCommandAsync;
        
        bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            private set => SetProperty(ref _isLoading, value);
        }

        string _medicine;
        public string Medicine
        {
            get => _medicine;
            private set => SetProperty(ref _medicine, value);
        }

        List<Document> _documents;
        public List<Document> Documents
        {
            get => _documents;
            private set => SetProperty(ref _documents, value);
        }

        public override void Prepare(Medicines parameter)
        {
            _nregistro = parameter.Nregistro;
            if (_nregistro != null)
                _ = GetMedicineAsync(_nregistro);
        }

        async Task GetMedicineAsync(string query)
        {
            IsLoading = true;
            try
            {
                await _cimaService.GetMedicineAsync(query)
                    .ContinueWith(response =>
                    {
                        if (response.IsCompleted && response.Status == TaskStatus.RanToCompletion)
                        {
                            var medicine = response.Result.Nombre;
                            var documents = response.Result.Docs;

                            _ioCProvider.Resolve<IMvxMainThreadAsyncDispatcher>()
                                .ExecuteOnMainThreadAsync((() =>
                                {
                                    IsLoading = false;
                                    Medicine = medicine;
                                    Documents = documents;
                                }));
                        }
                        else if (response.IsFaulted)
                        {
                            IsLoading = false;
                            _logger.LogError("Fail to get medicine");
                        }

                    }, TaskScheduler.FromCurrentSynchronizationContext()).ConfigureAwait(false);

            }
            catch (Exception e)
            {
                _logger.LogError("Fail to get medicine", e.Message);
            }
        }

        async Task OpenProspectAsync()
        {
            if (Documents.Count >= 2)
            {
                if (!string.IsNullOrEmpty(Documents[1].UrlHtml))
                    await Browser.OpenAsync(Documents[1].UrlHtml, BrowserLaunchMode.SystemPreferred);
                else
                    await Browser.OpenAsync(Documents[1].Url, BrowserLaunchMode.SystemPreferred);
            }
            else if (Documents.Count == 0)
            {
                // Todo
            }
            else if (Documents[0].Tipo == 2)
            {
                if (!string.IsNullOrEmpty(Documents[0].UrlHtml))
                    await Browser.OpenAsync(Documents[0].UrlHtml, BrowserLaunchMode.SystemPreferred);
                else
                    await Browser.OpenAsync(Documents[0].Url, BrowserLaunchMode.SystemPreferred);
            }
            else
            {
                // Todo
            }
        }
    }
}