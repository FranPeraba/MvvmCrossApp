using MvvmCross.ViewModels;
using MvvmCrossApp.Core.Resources;

namespace MvvmCrossApp.Core.ViewModels
{
    public abstract class BaseViewModel : MvxViewModel
    {
        protected BaseViewModel()
        {
        }

        public string this[string index] => Strings.ResourceManager.GetString(index);

        bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            protected set => SetProperty(ref _isLoading, value);
        }
    }
}