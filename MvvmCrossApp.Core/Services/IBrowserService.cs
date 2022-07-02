using System;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace MvvmCrossApp.Core.Services
{
    public interface IBrowserService
    {
        Task Browser(string uri, BrowserLaunchMode browserLaunchMode);
    }
}

