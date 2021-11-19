using Android.App;
using Android.OS;
using AndroidX.AppCompat.Widget;
using AndroidX.RecyclerView.Widget;
using MvvmCross.DroidX.RecyclerView;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.Platforms.Android.Views;
using MvvmCrossApp.Core.Resources;
using MvvmCrossApp.Core.ViewModels;
using MvvmCross.Binding.BindingContext;

namespace MvvmCrossApp.Droid.Views
{
    [MvxActivityPresentation]
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : MvxActivity<SearchMainViewModel>, SearchView.IOnQueryTextListener
    {
        MvxRecyclerView _recyclerView;
        MedicinesAdapter _adapter;
        SearchView _searchView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            SetupSearchView();

            SetupRecyclerView();

            SetupBindings();
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
            var bindingSet = this.CreateBindingSet<MainActivity, SearchMainViewModel>();
            bindingSet.Bind(_adapter).For(v => v.ItemsSource).To(vm => vm.Medicines);
            bindingSet.Apply();
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
