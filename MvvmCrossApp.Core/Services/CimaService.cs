using System;
using System.Net.Http;
using System.Threading.Tasks;
using MvvmCrossApp.Core.Models;
using MvvmCrossApp.Core.Rest;

namespace MvvmCrossApp.Core.Services
{
    public class CimaService : ICimaService
    {
        readonly IRestClient _restClient;

        public CimaService(IRestClient restClient)
        {
            _restClient = restClient;
        }

        public Task<PagedResult<Medicines>> GetMedicinesAsync(string query)
        {
            return _restClient.MakeApiCall<PagedResult<Medicines>>($"{Constants.BaseUrl}/medicamentos?multiple={query}",
                HttpMethod.Get);
        }
    }
}
