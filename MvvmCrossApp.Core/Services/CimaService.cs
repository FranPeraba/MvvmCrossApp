using Refit;

namespace MvvmCrossApp.Core.Services
{
    public static class CimaService
    {
        public static ICimaService cimaService;

        public static ICimaService GetCimaService()
        {
            cimaService = RestService.For<ICimaService>(Constants.BaseUrl);
            return cimaService;
        }
    }
}
