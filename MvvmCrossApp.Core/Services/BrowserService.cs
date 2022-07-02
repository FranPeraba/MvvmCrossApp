using System;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace MvvmCrossApp.Core.Services
{
    public class BrowserService : IBrowserService
    {
        public async Task Browser(string uri, BrowserLaunchMode browserLaunchMode)
        {
            await Xamarin.Essentials.Browser.OpenAsync(uri, browserLaunchMode);
        }
    }
}

