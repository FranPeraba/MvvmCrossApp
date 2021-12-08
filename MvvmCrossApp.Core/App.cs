using System;
using MvvmCross;
using MvvmCross.IoC;
using MvvmCross.ViewModels;
using MvvmCrossApp.Core.Services;
using Refit;

namespace MvvmCrossApp.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            RegisterCustomAppStart<AppStart>();
            
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton(() => RestService.For<ICimaService>(Constants.BaseUrl));
        }
    }
}
