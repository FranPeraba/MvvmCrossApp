using Microsoft.Extensions.Logging;
using MvvmCross.Platforms.Android.Core;
using MvvmCrossApp.Core;
using Serilog;
using Serilog.Extensions.Logging;

namespace MvvmCrossApp.Droid
{
    public class Setup : MvxAndroidSetup<App>
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
