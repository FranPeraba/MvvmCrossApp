using System;
using Microsoft.Extensions.DependencyInjection;
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

            // Initialize Refit using HttpClient
            //Mvx.IoCProvider.LazyConstructAndRegisterSingleton(() => RestService.For<ICimaService>(Constants.BaseUrl));

            // Initialize Refit using HttpClientFactory
            InitializeServiceCollection();
        }

        static void InitializeServiceCollection()
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

            MapServiceCollectionToMvx(serviceProvider, serviceCollection);
        }

        static void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddRefitClient<ICimaService>().ConfigureHttpClient(c => c.BaseAddress = new Uri(Constants.BaseUrl));
        }

        static void MapServiceCollectionToMvx(IServiceProvider serviceProvider, IServiceCollection serviceCollection)
        {
            foreach (var serviceDescriptor in serviceCollection)
            {
                if (serviceDescriptor.ImplementationType != null)
                {
                    Mvx.IoCProvider.RegisterType(serviceDescriptor.ServiceType, serviceDescriptor.ImplementationType);
                }
                else if (serviceDescriptor.ImplementationFactory != null)
                {
                    var instance = serviceDescriptor.ImplementationFactory(serviceProvider);
                    Mvx.IoCProvider.RegisterSingleton(serviceDescriptor.ServiceType, instance);
                }
                else if (serviceDescriptor.ImplementationInstance != null)
                {
                    Mvx.IoCProvider.RegisterSingleton(serviceDescriptor.ServiceType, serviceDescriptor.ImplementationInstance);
                }
                else
                {
                    throw new InvalidOperationException("Unsupported registration type");
                }
            }
        }
    }
}
