using MvvmCross.ViewModels;

namespace MvvmCrossApp.Core.ViewModels
{
    public abstract class BaseViewModel<TParameter> : BaseViewModel, IMvxViewModel<TParameter> where TParameter : class
    {
        protected BaseViewModel() 
        {
        }

        public abstract void Prepare(TParameter parameter);
    }
}