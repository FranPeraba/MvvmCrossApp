using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using MvvmCrossApp.Core.Models;
using MvvmCrossApp.Core.Services;

namespace MvvmCrossApp.Core.ViewModels
{
    public class SearchMainViewModel : MvxViewModel
    {
        readonly IMvxNavigationService _navigationService;
        readonly ILogger<SearchMainViewModel> _logger;

        public SearchMainViewModel(IMvxNavigationService navigationService, ILogger<SearchMainViewModel> logger)
        {
            _navigationService = navigationService;
            _logger = logger;

            Medicines = new MvxObservableCollection<Medicines>();

            SearchMecinesCommand = new MvxAsyncCommand<string>(SearchMedicinesAsync);
        }

        public IMvxAsyncCommand<string> SearchMecinesCommand { get; }

        bool _isLoading;
        public bool IsLoading
        {
            get { return _isLoading; }
            protected set { SetProperty(ref _isLoading, value); }
        }

        MvxObservableCollection<Medicines> _medicines;
        public MvxObservableCollection<Medicines> Medicines
        {
            get => _medicines;
            private set => SetProperty(ref _medicines, value);
        }

        async Task SearchMedicinesAsync(string query)
        {
            IsLoading = true;

            try
            {
                ICimaService cimaService = CimaService.GetCimaService();
                await cimaService.GetMedicinesAsync(query)
                    .ContinueWith(response =>
                    {
                        if (response.IsCompleted && response.Status == TaskStatus.RanToCompletion)
                        {
                            IsLoading = false;
                            Medicines.AddRange(response.Result.Resultados);
                        }
                        else if (response.IsFaulted)
                            IsLoading = false;
                    }, TaskScheduler.FromCurrentSynchronizationContext()).ConfigureAwait(false);

            }
            catch (Exception e)
            {
                _logger.LogError("Fail to get medicines", e);
            }

            IsLoading = false;
        }
    }
}
