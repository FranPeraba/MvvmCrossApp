using System;
using System.Threading.Tasks;
using MvvmCrossApp.Core.Models;
using Refit;

namespace MvvmCrossApp.Core.Services
{
    public interface ICimaService
    {
        [Get("/medicamentos?multiple={query}&pagesize=25")]
        Task<PagedResult<Medicines>> GetMedicinesAsync(string query);

        [Get("/medicamento?{param1}={param2}")]
        Task<PagedResult<Medicine>> GetMedicineAsync(string param1, string param2);
    }
}
