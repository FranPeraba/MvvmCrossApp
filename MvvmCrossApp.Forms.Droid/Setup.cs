using System;
using Microsoft.Extensions.Logging;
using MvvmCross.Forms.Platforms.Android.Core;
using MvvmCrossApp.Core;
using MvvmCrossApp.Forms.UI;
using Serilog;
using Serilog.Extensions.Logging;

namespace MvvmCrossApp.Forms.Droid
{
    public class Setup : MvxFormsAndroidSetup<App, FormsApp>
    {
        protected override ILoggerFactory CreateLogFactory()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.AndroidLog()
                .CreateLogger();

            return new SerilogLoggerFactory();
        }

        protected override ILoggerProvider CreateLogProvider()
        {
            return new SerilogLoggerProvider();
        }
    }
}

