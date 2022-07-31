using System;
using System.Net.Http;
using System.Threading.Tasks;
using MvvmCrossApp.Core.Models;
using Refit;

namespace MvvmCrossApp.Core.Services
{
    public interface ICimaService
    {
        [Get("/medicamentos?multiple={query}&pagesize=25")]
        Task<HttpResponseMessage> GetMedicinesAsync(string query);

        [Get("/medicamento?nregistro={query}")]
        Task<Medicine> GetMedicineAsync(string query);
    }
}
