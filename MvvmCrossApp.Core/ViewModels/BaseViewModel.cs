using Microsoft.Extensions.Logging;
using MvvmCross.IoC;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace MvvmCrossApp.Core.ViewModels
{
    public abstract class BaseViewModel : MvxViewModel
    {
        protected readonly IMvxNavigationService _navigationService;
        protected readonly ILogger _logger;
        protected readonly IMvxIoCProvider _ioCProvider;

        protected BaseViewModel(IMvxNavigationService navigationService, ILogger logger, IMvxIoCProvider ioCProvider)
        {
            _navigationService = navigationService;
            _logger = logger;
            _ioCProvider = ioCProvider;
        }
        
        bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            protected set
            {
                _isLoading = value;
                RaisePropertyChanged(() => IsLoading);
            }
        }
    }
}