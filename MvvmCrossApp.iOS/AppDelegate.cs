using Foundation;
using MvvmCross.Platforms.Ios.Core;
using MvvmCrossApp.Core;
using UIKit;

namespace MvvmCrossApp.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the
    // User Interface of the application, as well as listening (and optionally responding) to application events from iOS.
    [Register(nameof(AppDelegate))]
    public class AppDelegate : MvxApplicationDelegate<Setup, App>
    {
        public override UIWindow Window { get; set; }

        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            var result = base.FinishedLaunching(application, launchOptions);

            return result;
        }

    }
}

