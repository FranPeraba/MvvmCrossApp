// This file has been autogenerated from a class added in the UI designer.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using Foundation;
using MvvmCross.Platforms.Ios.Views;
using MvvmCrossApp.Core.Models;
using MvvmCrossApp.Core.Resources;
using MvvmCrossApp.Core.ViewModels;
using UIKit;

namespace MvvmCrossApp.iOS.Views
{
	[MvxFromStoryboard("MainView")]
	public partial class SearchView : MvxTableViewController<SearchMedicinesViewModel>, IUISearchResultsUpdating, IUISearchBarDelegate,
		IUITabBarControllerDelegate, IUISearchControllerDelegate
	{
		List<Medicines> _medicines;
		UISearchController _searchController;
		UIActivityIndicatorView _activityIndicator;
		
		public SearchView (IntPtr handle) : base (handle)
		{
		}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            
            SetupActivityIndicator();
            
            SetupTable();
            
            SetupSearchController();
            
            SetupBindings();

            Title = Strings.SearchMedicine;
        }

        public override void ViewDidAppear(bool animated)
        {
	        base.ViewDidAppear(animated);
	        SetupBindings();
        }

        public override void ViewDidDisappear(bool animated)
        {
	        base.ViewDidDisappear(animated);
	        ClearBindings();
        }

        public void UpdateSearchResultsForSearchController(UISearchController searchController)
        {
	        ViewModel.SearchTerm = searchController.SearchBar.Text;
	        UpdateSearchResults();
        }

        void SetupBindings()
        {
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
			        if (ViewModel.IsLoading)
				        _activityIndicator.StartAnimating();
			        else
				        _activityIndicator.StopAnimating();
			        break;
		        case nameof(ViewModel.Medicines):
			        UpdateSearchResults();
			        break;
	        }
        }

        void UpdateSearchResults()
        {
	        _medicines = ViewModel.Medicines;
	        TableView.ReloadData();
        }

        void SetupSearchController()
        {
	        _searchController = new UISearchController
	        {
		        DimsBackgroundDuringPresentation = false,
		        HidesNavigationBarDuringPresentation = true,
		        DefinesPresentationContext = true,
		        Delegate = this
	        };

	        _searchController.SearchBar.Delegate = this;
	        _searchController.SearchResultsUpdater = this;

	        _searchController.SearchBar.Placeholder = Strings.QueryHintSearch;
	        
	        NavigationItem.SearchController = _searchController;
        }
        
        protected override void Dispose(bool disposing)
        {
	        base.Dispose(disposing);
	        if (_searchController != null)
	        {
		        _searchController.Dispose();
		        _searchController = null;
	        }
        }
        
        void SetupActivityIndicator()
        {
	        _activityIndicator = new UIActivityIndicatorView
	        {
		        ActivityIndicatorViewStyle = UIActivityIndicatorViewStyle.Medium,
		        HidesWhenStopped = true
	        };

	        View.AddSubview(_activityIndicator);
	        _activityIndicator.TranslatesAutoresizingMaskIntoConstraints = false;
	        _activityIndicator.CenterXAnchor.ConstraintEqualTo(View.CenterXAnchor).Active = true;
	        _activityIndicator.CenterYAnchor.ConstraintEqualTo(View.CenterYAnchor, -250f).Active = true;
	        _activityIndicator.Color = UIColor.FromRGB(223, 80, 65);
        }
        
        void SetupTable()
        {
	        TableView.RowHeight = UITableView.AutomaticDimension;
	        TableView.EstimatedRowHeight = 44;
	        TableView.TableFooterView = new UIView();
        }

        public override nint RowsInSection(UITableView tableView, nint section)
        {
	        return _medicines?.Count ?? 0;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
	        var cell = (MedicineCell) tableView.DequeueReusableCell("medicineCell", indexPath);
	        cell.UpdateCell(_medicines, indexPath);

	        return cell;
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
	        tableView.DeselectRow(indexPath, true);
	        if (ViewModel.MedicineClickCommand.CanExecute(_medicines[indexPath.Row]))
		        ViewModel.MedicineClickCommand.Execute(_medicines[indexPath.Row]);
        }
	}
}
