using System;
using Android.App;
using Android.Runtime;
using MvvmCross.Platforms.Android.Views;
using MvvmCrossApp.Core;

namespace MvvmCrossApp.Droid
{
    [Application]
    public class MainApplication : MvxAndroidApplication<Setup, App>
    {
        public MainApplication()
        {
        }

        public MainApplication(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }
    }
}
