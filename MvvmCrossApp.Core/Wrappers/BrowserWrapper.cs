using System.Threading.Tasks;
using Xamarin.Essentials;

namespace MvvmCrossApp.Core.Wrappers
{
    public class BrowserWrapper : IBrowserWrapper
    {
        public async Task Browser(string uri, BrowserLaunchMode browserLaunchMode)
        {
             await Xamarin.Essentials.Browser.OpenAsync(uri, browserLaunchMode);
        }
    }
}