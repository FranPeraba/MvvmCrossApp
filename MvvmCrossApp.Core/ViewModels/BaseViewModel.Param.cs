using Microsoft.Extensions.Logging;
using MvvmCross.IoC;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace MvvmCrossApp.Core.ViewModels
{
    public abstract class BaseViewModel<TParameter> : BaseViewModel, IMvxViewModel<TParameter> where TParameter : notnull
    {
        protected BaseViewModel(IMvxNavigationService navigationService, ILogger logger, IMvxIoCProvider ioCProvider) 
            : base(navigationService, logger, ioCProvider)
        {
        }

        public abstract void Prepare(TParameter parameter);
    }
}