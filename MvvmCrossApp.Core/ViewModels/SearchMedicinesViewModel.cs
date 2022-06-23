using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCrossApp.Core.Helpers;
using MvvmCrossApp.Core.Models;
using MvvmCrossApp.Core.Services;

namespace MvvmCrossApp.Core.ViewModels
{
    public class SearchMedicinesViewModel : BaseViewModel
    {
        readonly ICimaService _cimaService;
        readonly IMvxNavigationService _navigationService;

        public SearchMedicinesViewModel(IMvxNavigationService navigationService, ICimaService cimaService)
        {
            _cimaService = cimaService;
            _navigationService = navigationService;

            Medicines = new ObservableRangeCollection<Medicines>();
            
            _medicineClickCommand = new MvxCommand<Medicines>(OnMedicineClick);
        }

        IMvxCommand<Medicines> _medicineClickCommand;
        public IMvxCommand<Medicines> MedicineClickCommand => _medicineClickCommand;

        ObservableRangeCollection<Medicines> _medicines;
        public ObservableRangeCollection<Medicines> Medicines
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
                        Medicines.AddRange(response.Result.Resultados);
                    }
                    else if (response.IsFaulted)
                    {
                        IsLoading = false;
                    }
                }, TaskScheduler.FromCurrentSynchronizationContext()).ConfigureAwait(false);
        }

        void OnMedicineClick(Medicines medicine)
        {
            _navigationService.Navigate<DetailMedicineViewModel, Medicines>(medicine);
        }
    }
}
