﻿// This file has been autogenerated from a class added in the UI designer.

using System;
using System.ComponentModel;
using MvvmCross.Platforms.Ios.Views;
using MvvmCrossApp.Core.Resources;
using MvvmCrossApp.Core.ViewModels;
using UIKit;

namespace MvvmCrossApp.iOS.Views
{
	[MvxFromStoryboard("MainView")]
	public partial class DetailView : MvxViewController<DetailMedicineViewModel>
	{
		public DetailView(IntPtr handle) : base(handle)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			
			SetupUI();
			
			SetupBindings();
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

		void SetupUI()
		{
			ActivityIndicator.StartAnimating();
			
			MedicineLabel.Text = ViewModel.Medicine;
			
			ProspectButton.SetTitle(Strings.Prospect, UIControlState.Normal);
			ProspectButton.Font = UIFont.SystemFontOfSize(20, UIFontWeight.Bold);

			NavigationItem.BackBarButtonItem = new UIBarButtonItem(string.Empty, UIBarButtonItemStyle.Plain, null, null);
			
			Title = Strings.Medicine;
		}

		void SetupBindings()
		{
			ViewModel.PropertyChanged += ViewModel_PropertyChanged;
			ProspectButton.TouchUpInside += ProspectButton_OnClick;
		}

		void ClearBindings()
		{
			ViewModel.PropertyChanged -= ViewModel_PropertyChanged;
			ProspectButton.TouchUpInside -= ProspectButton_OnClick;
		}

		void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			switch (e.PropertyName)
			{
				case nameof(ViewModel.IsLoading):
					if (ViewModel.IsLoading)
						ActivityIndicator.StartAnimating();
					else
						ActivityIndicator.StopAnimating();
					break;
				case nameof(ViewModel.Medicine):
					MedicineLabel.Text = ViewModel.Medicine;
					break;
			}
		}

		void ProspectButton_OnClick(object sender, EventArgs e)
		{
			if (ViewModel.OpenDocumentCommandAsync.CanExecute())
				ViewModel.OpenDocumentCommandAsync.ExecuteAsync();
		}
	}
}
