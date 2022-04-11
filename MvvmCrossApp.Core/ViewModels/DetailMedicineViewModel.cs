using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MvvmCross.Base;
using MvvmCross.Commands;
using MvvmCross.IoC;
using MvvmCross.Navigation;
using MvvmCrossApp.Core.Models;
using MvvmCrossApp.Core.Services;
using MvvmCrossApp.Core.Wrappers;
using Xamarin.Essentials;

namespace MvvmCrossApp.Core.ViewModels
{
    public class DetailMedicineViewModel : BaseViewModel<Medicines>
    {
        readonly ICimaService _cimaService;
        readonly IBrowserWrapper _browserWrapper;
        string _nregistro;

        public DetailMedicineViewModel(IMvxNavigationService navigationService, ILogger<DetailMedicineViewModel> logger, 
            ICimaService cimaService, IMvxIoCProvider ioCProvider, IBrowserWrapper browserWrapper) 
            : base(navigationService, logger, ioCProvider)
        {
            _cimaService = cimaService;
            _browserWrapper = browserWrapper;

            _openDocumentAsyncCommand = new MvxAsyncCommand(OpenDocumentAsync);

            _documents = new List<Document>();
        }

        IMvxAsyncCommand _openDocumentAsyncCommand;
        public IMvxAsyncCommand OpenDocumentAsyncCommand => _openDocumentAsyncCommand;

        string _name;
        public string Name
        {
            get => _name;
            private set => SetProperty(ref _name, value);
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
        }

        public override async Task Initialize()
        {
            await base.Initialize();
            try
            {
                if (!string.IsNullOrEmpty(_nregistro))
                    await GetMedicineAsync(_nregistro);
            }
            catch (Exception e)
            {
                _logger.LogError("Fail to get medicine", e.Message);
            }
        }

        async Task GetMedicineAsync(string query)
        {
            await _ioCProvider.Resolve<IMvxMainThreadAsyncDispatcher>()
                .ExecuteOnMainThreadAsync(() => { IsLoading = true; });
            await _cimaService.GetMedicineAsync(query)
                .ContinueWith(response =>
                {
                    if (response.IsCompleted && response.Status == TaskStatus.RanToCompletion)
                    {
                        var name = response.Result.Nombre;
                        var documents = response.Result.Docs;

                        _ioCProvider.Resolve<IMvxMainThreadAsyncDispatcher>()
                            .ExecuteOnMainThreadAsync(() =>
                            {
                                IsLoading = false;
                                Name = name;
                                Documents = documents;
                            });
                    }
                    else if (response.IsFaulted)
                    {
                        _ioCProvider.Resolve<IMvxMainThreadAsyncDispatcher>()
                            .ExecuteOnMainThreadAsync(() => { IsLoading = false; });
                        _logger.LogError("Fail to get medicine");
                    }

                }, TaskScheduler.FromCurrentSynchronizationContext()).ConfigureAwait(false);
        }

        async Task OpenDocumentAsync()
        {
            if (Documents.Count >= 2)
            {
                if (!string.IsNullOrEmpty(Documents[1].UrlHtml))
                    await _browserWrapper.Browser(Documents[1].UrlHtml, BrowserLaunchMode.SystemPreferred);
                else
                    await _browserWrapper.Browser(Documents[1].Url, BrowserLaunchMode.SystemPreferred);
            }
            else if (Documents[0].Tipo == 2)
            {
                if (!string.IsNullOrEmpty(Documents[0].UrlHtml))
                    await _browserWrapper.Browser(Documents[0].UrlHtml, BrowserLaunchMode.SystemPreferred);
                else
                    await _browserWrapper.Browser(Documents[0].Url, BrowserLaunchMode.SystemPreferred);
            }
        }
    }
}