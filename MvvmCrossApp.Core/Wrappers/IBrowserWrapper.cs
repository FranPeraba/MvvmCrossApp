using System.Threading.Tasks;
using Xamarin.Essentials;

namespace MvvmCrossApp.Core.Wrappers
{
    public interface IBrowserWrapper
    {
        Task Browser(string uri, BrowserLaunchMode browserLaunchMode);
    }
}