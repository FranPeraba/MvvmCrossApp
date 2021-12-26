using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MvvmCross.Base;
using MvvmCross.Commands;
using MvvmCross.IoC;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using MvvmCrossApp.Core.Models;
using MvvmCrossApp.Core.Services;

namespace MvvmCrossApp.Core.ViewModels
{
    public class SearchMedicinesViewModel : MvxViewModel
    {
        readonly IMvxNavigationService _navigationService;
        readonly ILogger<SearchMedicinesViewModel> _logger;
        readonly ICimaService _cimaService;
        readonly IMvxIoCProvider _ioCProvider;

        public SearchMedicinesViewModel(IMvxNavigationService navigationService, ILogger<SearchMedicinesViewModel> logger, 
            ICimaService cimaService, IMvxIoCProvider ioCProvider)
        {
            _navigationService = navigationService;
            _logger = logger;
            _cimaService = cimaService;
            _ioCProvider = ioCProvider;

            Medicines = new List<Medicines>();
            _medicineClickCommand = new MvxCommand<Medicines>(OnMedicineClickCommand);
        }

        IMvxCommand<Medicines> _medicineClickCommand;
        public IMvxCommand<Medicines> MedicineClickCommand => _medicineClickCommand;

        bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            private set => SetProperty(ref _isLoading, value);
        }

        List<Medicines> _medicines;
        public  List<Medicines> Medicines
        {
            get => _medicines;
            private set => SetProperty(ref _medicines, value);
        }

        string _searchTerm;
        public string SearchTerm
        {
            get => _searchTerm;
            set
            {
                _searchTerm = value;
                if (string.IsNullOrEmpty(value))
                {
                    _medicines = new List<Medicines>();
                }
                else if (value.Length >= 3)
                {
                    SearchMedicinesAsync(value).ConfigureAwait(false);
                }
                RaisePropertyChanged(() => SearchTerm);
                RaisePropertyChanged(() => Medicines);
            }
        }

        async Task SearchMedicinesAsync(string query)
        {
            IsLoading = true;
            try
            {
                await _cimaService.GetMedicinesAsync(query)
                    .ContinueWith(response =>
                    {
                        if (response.IsCompleted && response.Status == TaskStatus.RanToCompletion)
                        {
                            _medicines = new List<Medicines>();
                            _ioCProvider.Resolve<IMvxMainThreadAsyncDispatcher>()
                                .ExecuteOnMainThreadAsync((() =>
                                {
                                    IsLoading = false;
                                    Medicines.AddRange(response.Result.Resultados);
                                }));
                        }
                        else if (response.IsFaulted)
                        {
                            IsLoading = false;
                            _logger.LogError("Fail to get medicines");
                        }
                    }, TaskScheduler.FromCurrentSynchronizationContext()).ConfigureAwait(false);

            }
            catch (Exception e)
            {
                _logger.LogError("Fail to get medicines", e.Message);
            }
        }

        void OnMedicineClickCommand(Medicines medicine)
        {
            _navigationService.Navigate<DetailMedicineViewModel, Medicines>(medicine);
        }
    }
}
