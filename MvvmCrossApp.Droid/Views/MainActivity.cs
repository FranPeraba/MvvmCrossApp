using Android.App;
using Android.OS;
using Android.Views;
using AndroidX.RecyclerView.Widget;
using MvvmCross.DroidX.RecyclerView;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.Platforms.Android.Views;
using MvvmCrossApp.Core.Resources;
using MvvmCrossApp.Core.ViewModels;
using SearchView = AndroidX.AppCompat.Widget.SearchView;

namespace MvvmCrossApp.Droid.Views
{
    [MvxActivityPresentation]
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true,
        WindowSoftInputMode = SoftInput.AdjustResize | SoftInput.StateHidden)]
    public class MainActivity : MvxActivity<SearchMedicinesViewModel>, SearchView.IOnQueryTextListener
    {
        MvxRecyclerView _recyclerView;
        SearchView _searchView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            
            SetContentView(Resource.Layout.activity_main);

            SetupSearchView();

            SetupRecyclerView();

            Title = Strings.SearchMedicine;
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
            _recyclerView = FindViewById<MvxRecyclerView>(Resource.Id.main_recycler_view);
            var layoutManager = new LinearLayoutManager(this);
            _recyclerView.SetLayoutManager(layoutManager);
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
