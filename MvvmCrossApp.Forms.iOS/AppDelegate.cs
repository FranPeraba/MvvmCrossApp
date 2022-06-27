using Foundation;
using MvvmCross.Forms.Platforms.Ios.Core;
using MvvmCrossApp.Core;
using MvvmCrossApp.Forms.UI;
using UIKit;

namespace MvvmCrossApp.Forms.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the
    // User Interface of the application, as well as listening (and optionally responding) to application events from iOS.
    [Register ("AppDelegate")]
    public class AppDelegate : MvxFormsApplicationDelegate<Setup, App, FormsApp>
    {
        public override UIWindow Window { get; set; }

        
        public override bool FinishedLaunching (UIApplication application, NSDictionary launchOptions)
        {
            // Override point for customization after application launch.
            // If not required for your application you can safely delete this method

            return base.FinishedLaunching(application, launchOptions);
        }
    }
}


