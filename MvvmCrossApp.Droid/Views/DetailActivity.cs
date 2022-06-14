using Android.App;
using Android.Content.PM;
using Android.OS;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.Platforms.Android.Views;
using MvvmCrossApp.Core.Resources;
using MvvmCrossApp.Core.ViewModels;

namespace MvvmCrossApp.Droid.Views
{
    [MvxActivityPresentation]
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", LaunchMode = LaunchMode.SingleTop, ParentActivity = typeof(MainActivity))]
    public class DetailActivity : MvxActivity<DetailMedicineViewModel>
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            
            SetContentView(Resource.Layout.activity_detail);

            Title = Strings.Medicine;

            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
        }
    }
}