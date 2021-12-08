﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using MvvmCrossApp.Core.Models;
using MvvmCrossApp.Core.Services;

namespace MvvmCrossApp.Core.ViewModels
{
    public class SearchViewModel : MvxViewModel
    {
        readonly IMvxNavigationService _navigationService;
        readonly ILogger<SearchViewModel> _logger;
        readonly ICimaService _cimaService;

        public SearchViewModel(IMvxNavigationService navigationService, ILogger<SearchViewModel> logger, ICimaService cimaService)
        {
            _navigationService = navigationService;
            _logger = logger;
            _cimaService = cimaService;

            Medicines = new List<Medicines>();
        }

        bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            private set => SetProperty(ref _isLoading, value);
        }

        List<Medicines> _medicines;
        public  List<Medicines> Medicines
        {
            get => _medicines;
            private set => SetProperty(ref _medicines, value);
        }

        string _searchTerm;
        public string SearchTerm
        {
            get => _searchTerm;
            set
            {
                _searchTerm = value;
                if (string.IsNullOrEmpty(value))
                {
                    _medicines = new List<Medicines>();
                }
                else if (value.Length >= 3)
                {
                    SearchMedicinesAsync(value).ConfigureAwait(false);
                }
                RaisePropertyChanged(() => SearchTerm);
                RaisePropertyChanged(() => Medicines);
            }
        }

        async Task SearchMedicinesAsync(string query)
        {
            IsLoading = true;
            
            try
            {
                await _cimaService.GetMedicinesAsync(query)
                    .ContinueWith(response =>
                    {
                        if (response.IsCompleted && response.Status == TaskStatus.RanToCompletion)
                        {
                            IsLoading = false;
                            _medicines = new List<Medicines>();
                            Medicines.AddRange(response.Result.Resultados);
                            RaisePropertyChanged(() => Medicines);
                        }
                        else if (response.IsFaulted)
                            IsLoading = false;
                    }, TaskScheduler.FromCurrentSynchronizationContext()).ConfigureAwait(false);

            }
            catch (Exception e)
            {
                _logger.LogError("Fail to get medicines", e);
            }
        }
    }
}
