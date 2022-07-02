using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCrossApp.Core.Models;
using MvvmCrossApp.Core.Services;
using Xamarin.Essentials;

namespace MvvmCrossApp.Core.ViewModels
{
    public class DetailMedicineViewModel : BaseViewModel<Medicines>
    {
        readonly ICimaService _cimaService;
        readonly IBrowserService _browserService;

        string _nregistro;

        public DetailMedicineViewModel(ICimaService cimaService, IBrowserService browserService)
        {
            _cimaService = cimaService;
            _browserService = browserService;

            OpenDocumentAsyncCommand = new MvxAsyncCommand(OpenDocumentAsync);

            Documents = new List<Document>();
        }

        public IMvxAsyncCommand OpenDocumentAsyncCommand { get; private set; }

        string _name;
        public string Name
        {
            get => _name;
            private set => SetProperty(ref _name, value);
        }

        public List<Document> Documents { get; private set; }

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
            catch (Exception)
            {
                Debug.WriteLine("Failed to load medicine");
            }
        }

        async Task GetMedicineAsync(string query)
        {
            IsLoading = true;
            await _cimaService.GetMedicineAsync(query)
                .ContinueWith(response =>
                {
                    if (response.IsCompleted && response.Status == TaskStatus.RanToCompletion)
                    {
                        IsLoading = false;
                        Name = response.Result.Nombre;
                        Documents = response.Result.Docs;
                    }
                    else if (response.IsFaulted)
                    {
                        IsLoading = false;
                    }

                }, TaskScheduler.FromCurrentSynchronizationContext()).ConfigureAwait(false);
        }

        async Task OpenDocumentAsync()
        {
            if (Documents.Count >= 2)
            {
                if (!string.IsNullOrEmpty(Documents[1].UrlHtml))
                    await _browserService.Browser(Documents[1].UrlHtml, BrowserLaunchMode.SystemPreferred);
                else
                    await _browserService.Browser(Documents[1].Url, BrowserLaunchMode.SystemPreferred);
            }
            else if (Documents[0].Tipo == 2)
            {
                if (!string.IsNullOrEmpty(Documents[0].UrlHtml))
                    await _browserService.Browser(Documents[0].UrlHtml, BrowserLaunchMode.SystemPreferred);
                else
                    await _browserService.Browser(Documents[0].Url, BrowserLaunchMode.SystemPreferred);
            }
        }
    }
}