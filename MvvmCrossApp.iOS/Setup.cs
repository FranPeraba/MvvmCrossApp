using System;
using Microsoft.Extensions.Logging;
using MvvmCross.Platforms.Ios.Core;
using MvvmCrossApp.Core;
using Serilog;
using Serilog.Extensions.Logging;

namespace MvvmCrossApp.iOS
{
    public class Setup : MvxIosSetup<App>
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
