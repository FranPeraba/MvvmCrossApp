using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.Logging;
using MvvmCross;
using MvvmCross.DroidX.RecyclerView;
using MvvmCross.IoC;
using MvvmCross.Platforms.Android.Core;
using MvvmCross.Platforms.Android.Presenters;
using MvvmCrossApp.Core;
using MvvmCrossApp.Core.Services;
using Serilog;
using Serilog.Extensions.Logging;

namespace MvvmCrossApp.Droid
{
    public class Setup : MvxAndroidSetup<App>
    {
        protected override void InitializeFirstChance(IMvxIoCProvider iocProvider)
        {
            base.InitializeFirstChance(iocProvider);
        }

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

        protected override IEnumerable<Assembly> AndroidViewAssemblies => new List<Assembly>(base.AndroidViewAssemblies)
        {
            typeof(MvxRecyclerView).Assembly
        };

        protected override IMvxAndroidViewPresenter CreateViewPresenter()
        {
            return new MvxAndroidViewPresenter(AndroidViewAssemblies);
        }
    }
}
