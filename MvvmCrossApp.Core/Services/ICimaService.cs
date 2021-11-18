using System;
using System.Threading.Tasks;
using MvvmCrossApp.Core.Models;

namespace MvvmCrossApp.Core.Services
{
    public interface ICimaService
    {
        Task<PagedResult<Medicines>> GetMedicinesAsync(string query);
    }
}
