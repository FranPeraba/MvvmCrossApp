using System;
using Microsoft.Extensions.Logging;
using MvvmCross.Forms.Platforms.Ios.Core;
using MvvmCrossApp.Core;
using MvvmCrossApp.Forms.UI;
using Serilog;
using Serilog.Extensions.Logging;

namespace MvvmCrossApp.Forms.iOS
{
    public class Setup : MvxFormsIosSetup<App, FormsApp>
    {
        protected override ILoggerFactory CreateLogFactory()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.NSLog()
                .CreateLogger();

            return new SerilogLoggerFactory();
        }

        protected override ILoggerProvider CreateLogProvider()
        {
            return new SerilogLoggerProvider();
        }
    }
}

