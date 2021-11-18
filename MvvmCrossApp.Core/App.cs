using System;
using MvvmCross.IoC;
using MvvmCross.ViewModels;

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
        }
    }
}
