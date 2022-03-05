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

namespace MvvmCrossApp.Core.ViewModels
{
    public class SearchMedicinesViewModel : BaseViewModel
    {
        readonly ICimaService _cimaService;

        public SearchMedicinesViewModel(IMvxNavigationService navigationService, ILogger<SearchMedicinesViewModel> logger, 
            ICimaService cimaService, IMvxIoCProvider ioCProvider) : base(navigationService, logger, ioCProvider)
        {
            _cimaService = cimaService;

            _medicines = new List<Medicines>();
            
            _medicineClickCommand = new MvxCommand<Medicines>(OnMedicineClick);
        }

        IMvxCommand<Medicines> _medicineClickCommand;
        public IMvxCommand<Medicines> MedicineClickCommand => _medicineClickCommand;

        List<Medicines> _medicines;
        public List<Medicines> Medicines
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
                    Medicines.Clear();
                    RaisePropertyChanged(() => Medicines);
                }
                else if (value.Length >= 3)
                {
                    SearchMedicinesAsync(value).ConfigureAwait(false);
                }
                RaisePropertyChanged(() => SearchTerm);
            }
        }

        async Task SearchMedicinesAsync(string query)
        {
            await _ioCProvider.Resolve<IMvxMainThreadAsyncDispatcher>()
                .ExecuteOnMainThreadAsync((() => { IsLoading = true; }));
            
            try
            {
                await _cimaService.GetMedicinesAsync(query)
                    .ContinueWith(response =>
                    {
                        if (response.IsCompleted && response.Status == TaskStatus.RanToCompletion)
                        {
                            _ioCProvider.Resolve<IMvxMainThreadAsyncDispatcher>()
                                .ExecuteOnMainThreadAsync((() =>
                                {
                                    IsLoading = false;
                                    Medicines.Clear();
                                    Medicines.AddRange(response.Result.Resultados);
                                    RaisePropertyChanged(() => Medicines);
                                }));
                        }
                        else if (response.IsFaulted)
                        {
                            _ioCProvider.Resolve<IMvxMainThreadAsyncDispatcher>()
                                .ExecuteOnMainThreadAsync((() => { IsLoading = false; }));
                            _logger.LogError("Fail to get medicines");
                        }
                    }, TaskScheduler.FromCurrentSynchronizationContext()).ConfigureAwait(false);

            }
            catch (Exception e)
            {
                _logger.LogError("Fail to get medicines");
            }
        }

        void OnMedicineClick(Medicines medicine)
        {
            _navigationService.Navigate<DetailMedicineViewModel, Medicines>(medicine);
        }
    }
}
