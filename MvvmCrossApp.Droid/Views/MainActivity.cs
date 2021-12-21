using System.ComponentModel;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using MvvmCross.DroidX.RecyclerView;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.Platforms.Android.Views;
using MvvmCrossApp.Core.Resources;
using MvvmCrossApp.Core.ViewModels;
using MvvmCross.Binding.BindingContext;
using SearchView = AndroidX.AppCompat.Widget.SearchView;

namespace MvvmCrossApp.Droid.Views
{
    [MvxActivityPresentation]
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : MvxActivity<SearchMedicinesViewModel>, SearchView.IOnQueryTextListener
    {
        MvxRecyclerView _recyclerView;
        MedicinesAdapter _adapter;
        SearchView _searchView;
        ProgressBar _progressBar;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            _progressBar = FindViewById<ProgressBar>(Resource.Id.progress_bar);
            _progressBar.Visibility = ViewStates.Gone;

            SetupSearchView();

            SetupRecyclerView();

            SetupBindings();

            Title = Strings.SearchMedicine;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            ClearBindings();
        }

        void SetupSearchView()
        {
            _searchView = FindViewById<SearchView>(Resource.Id.main_search_view);
            _searchView.QueryHint = Strings.QueryHintSearch;
            _searchView.SetOnQueryTextListener(this);
            _searchView.SetIconifiedByDefault(false);
            _searchView.Focusable = false;
        }

        void SetupRecyclerView()
        {
            _adapter = new MedicinesAdapter((IMvxAndroidBindingContext)BindingContext)
            {
                VM = ViewModel
            };

            _recyclerView = FindViewById<MvxRecyclerView>(Resource.Id.main_recycler_view);
            var layoutManager = new LinearLayoutManager(this);
            _recyclerView.SetLayoutManager(layoutManager);
            _recyclerView.Adapter = _adapter;
        }

        void SetupBindings()
        {
            var bindingSet = this.CreateBindingSet<MainActivity, SearchMedicinesViewModel>();
            bindingSet.Bind(_adapter).For(v => v.ItemsSource).To(vm => vm.Medicines);
            bindingSet.Apply();
            
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        void ClearBindings()
        {
            ViewModel.PropertyChanged -= ViewModel_PropertyChanged;
        }

        void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(ViewModel.IsLoading):
                    _progressBar.Visibility = !ViewModel.IsLoading ? ViewStates.Gone : ViewStates.Visible;
                    break;
            }
        }

        public bool OnQueryTextChange(string newText)
        {
            ViewModel.SearchTerm = newText;
            return false;
        }

        public bool OnQueryTextSubmit(string newText)
        {
            return false;
        }
    }
}
