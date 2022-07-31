using System;
using System.Diagnostics;
using System.Net;
using System.Text.Json;
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
            
            MedicineClickCommand = new MvxCommand<Medicines>(OnMedicineClick);
        }

        public IMvxCommand<Medicines> MedicineClickCommand { get; private set; }

        ObservableRangeCollection<Medicines> _medicines;
        public ObservableRangeCollection<Medicines> Medicines
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
                }
                else if (value.Length >= 3)
                {
                    try
                    {
                        SearchMedicinesAsync(value).ConfigureAwait(false);
                    }
                    catch (Exception)
                    {
                        Debug.WriteLine("Failed to load medicines");
                    }
                }
                RaisePropertyChanged(() => SearchTerm);
            }
        }

        async Task SearchMedicinesAsync(string query)
        {
            IsLoading = true;
            var response = await _cimaService.GetMedicinesAsync(query);

            if (response.IsSuccessStatusCode)
            {
                IsLoading = false;
                Medicines.Clear();
                var responseContent = await response.Content.ReadAsStringAsync();
                var medicines = JsonSerializer.Deserialize<PagedResult<Medicines>>(responseContent,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                Medicines.AddRange(medicines.Resultados);
            }
            else
            {
                IsLoading = false;
            }
        }

        void OnMedicineClick(Medicines medicine)
        {
            _navigationService.Navigate<DetailMedicineViewModel, Medicines>(medicine);
        }
    }
}
