using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
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

            Medicines = new ObservableCollection<Medicines>();
            
            _medicineClickCommand = new MvxCommand<Medicines>(OnMedicineClick);
        }

        IMvxCommand<Medicines> _medicineClickCommand;
        public IMvxCommand<Medicines> MedicineClickCommand => _medicineClickCommand;

        ObservableCollection<Medicines> _medicines;
        public ObservableCollection<Medicines> Medicines
        {
            get => _medicines;
            private set
            {
                _medicines = value;
                RaisePropertyChanged(() => Medicines);
            }
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
            await _cimaService.GetMedicinesAsync(query)
                .ContinueWith(response =>
                {
                    if (response.IsCompleted && response.Status == TaskStatus.RanToCompletion)
                    {
                        IsLoading = false;
                        Medicines.Clear();
                        foreach (var medicine in response.Result.Resultados)
                        {
                            Medicines.Add(medicine);
                        }
                    }
                    else if (response.IsFaulted)
                    {
                        IsLoading = false;
                        _logger.LogError("Fail to get medicines");
                    }
                }, TaskScheduler.FromCurrentSynchronizationContext()).ConfigureAwait(false);
        }

        void OnMedicineClick(Medicines medicine)
        {
            _navigationService.Navigate<DetailMedicineViewModel, Medicines>(medicine);
        }
    }
}
