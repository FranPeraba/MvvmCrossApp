using Microsoft.Extensions.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using MvvmCrossApp.Core.Services;

namespace MvvmCrossApp.Core.ViewModels
{
    public class DetailMedicineViewModel : MvxViewModel
    {
        readonly IMvxNavigationService _navigationService;
        readonly ILogger<SearchMedicinesViewModel> _logger;
        readonly ICimaService _cimaService;

        public DetailMedicineViewModel(IMvxNavigationService navigationService, ILogger<SearchMedicinesViewModel> logger, 
            ICimaService cimaService)
        {
            _navigationService = navigationService;
            _logger = logger;
            _cimaService = cimaService;
        }
        
        bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            private set => SetProperty(ref _isLoading, value);
        }

        string _medicineName;
        public string MedicineName
        {
            get => _medicineName;
            private set => SetProperty(ref _medicineName, value);
        }

        string _nRegistro;
        public string NRegistro
        {
            get => _nRegistro;
            private set => SetProperty(ref _nRegistro, value);
        }
    }
}