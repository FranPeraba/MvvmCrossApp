using System;
using System.Threading.Tasks;
using MvvmCross.Base;
using MvvmCross.Commands;
using MvvmCross.IoC;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using MvvmCrossApp.Core.Models;
using MvvmCrossApp.Core.Services;

namespace MvvmCrossApp.Core.ViewModels
{
    public class SearchMainViewModel : MvxViewModel
    {
        readonly IMvxNavigationService _navigationService;
        readonly ICimaService _cimaService;
        readonly IMvxIoCProvider _ioCProvider;

        public SearchMainViewModel(IMvxNavigationService navigationService, ICimaService cimaService, IMvxIoCProvider ioCProvider)
        {
            _navigationService = navigationService;
            _cimaService = cimaService;
            _ioCProvider = ioCProvider;

            Medicines = new MvxObservableCollection<Medicines>();

            SearchMecinesCommand = new MvxAsyncCommand<string>(SearchMedicinesAsync);
        }

        public IMvxAsyncCommand<string> SearchMecinesCommand { get; }

        bool _isLoading;
        public bool IsLoading
        {
            get { return _isLoading; }
            protected set { SetProperty(ref _isLoading, value); }
        }

        MvxObservableCollection<Medicines> _medicines;
        public MvxObservableCollection<Medicines> Medicines
        {
            get => _medicines;
            private set => SetProperty(ref _medicines, value);
        }

        async Task SearchMedicinesAsync(string query)
        {
            IsLoading = true;

            try
            {
                var result = await _cimaService.GetMedicinesAsync(query);

                await _ioCProvider.Resolve<IMvxMainThreadAsyncDispatcher>().ExecuteOnMainThreadAsync(() =>
                {
                    Medicines.AddRange(result.Resultados);
                });

            }
            catch (Exception e)
            {

            }

            IsLoading = false;
        }
    }
}
