using System;
using System.ComponentModel;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Widget;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.Platforms.Android.Views;
using MvvmCrossApp.Core.Resources;
using MvvmCrossApp.Core.ViewModels;

namespace MvvmCrossApp.Droid.Views
{
    [MvxActivityPresentation]
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", LaunchMode = LaunchMode.SingleTop)]
    public class DetailActivity : MvxActivity<DetailMedicineViewModel>
    {
        TextView _medicineTextView;
        Button _documentButton;
        ProgressBar _progressBar;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            
            SetContentView(Resource.Layout.activity_detail);

            _medicineTextView = FindViewById<TextView>(Resource.Id.medicine);
            _documentButton = FindViewById<Button>(Resource.Id.prospecto_button);
            _progressBar = FindViewById<ProgressBar>(Resource.Id.progressBar);
            
            SetupView();
            
            SetupBindings();
            
            Title = Strings.Medicine;
        }

        protected override void OnPause()
        {
            base.OnPause();
            ClearBindings();
        }

        void SetupView()
        {
            _medicineTextView.Text = ViewModel.Name;
        }

        void SetupBindings()
        {
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;
            _documentButton.Click += DocumentButton_OnClick;
        }

        void ClearBindings()
        {
            ViewModel.PropertyChanged -= ViewModel_PropertyChanged;
            _documentButton.Click -= DocumentButton_OnClick;
        }

        void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(ViewModel.IsLoading):
                    _progressBar.Visibility = !ViewModel.IsLoading ? ViewStates.Invisible : ViewStates.Visible;
                    break;
                case nameof(ViewModel.Name):
                    _medicineTextView.Text = ViewModel.Name;
                    break;
            }
        }

        void DocumentButton_OnClick(object sender, EventArgs e)
        {
            if (ViewModel.OpenDocumentAsyncCommand.CanExecute())
                ViewModel.OpenDocumentAsyncCommand.ExecuteAsync();
        }
    }
}