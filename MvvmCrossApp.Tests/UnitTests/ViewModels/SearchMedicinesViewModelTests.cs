using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using FluentAssertions;
using Moq;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using MvvmCrossApp.Core.Models;
using MvvmCrossApp.Core.Services;
using MvvmCrossApp.Core.ViewModels;
using Xunit;

namespace MvvmCrossApp.Tests.UnitTests.ViewModels
{
    public class SearchMedicinesViewModelTests
    {
        [Fact]
        public async Task ShouldSetIsLoadingToFalse_WhenSetSearchTerm_And_SetMedicines()
        {
            using (var mock = AutoMock.GetLoose())
            {
                #region Arrange
                
                mock.CanExecuteOnMainThread();
                
                SearchMedicinesViewModel searchMedicinesViewModel = mock.Create<SearchMedicinesViewModel>();
                string query = "Medicine";
                
                mock.Mock<ICimaService>().Setup(cs => cs.GetMedicinesAsync(It.IsAny<string>()))
                    .Returns(Task.FromResult(new PagedResult<Medicines>{ Resultados = new List<Medicines>() }));
                
                var tcs = new TaskCompletionSource<bool>();
                var timeout = Task.Delay(10000);
                
                searchMedicinesViewModel.PropertyChanged += (o, e) =>
                {
                    if (e.PropertyName == nameof(SearchMedicinesViewModel.Medicines))
                        tcs.SetResult(true);
                };
                
                #endregion
            
                #region Action
                
                searchMedicinesViewModel.SearchTerm = query;
                await Task.WhenAny(new List<Task> { tcs.Task, timeout });

                #endregion

                #region Assert
                tcs.Task.IsCompleted.Should().BeTrue();
                tcs.Task.Result.Should().BeTrue();
                searchMedicinesViewModel.IsLoading.Should().BeFalse();

                #endregion
            }
        }
        
        [Fact]
        public void ShouldSetMedicinesToExpectedMedicines_WhenSetSearchTerm()
        {
            using (var mock = AutoMock.GetLoose())
            {
                #region Arrange
                
                mock.CanExecuteOnMainThread();
                
                SearchMedicinesViewModel searchMedicinesViewModel = mock.Create<SearchMedicinesViewModel>();

                string query = "Medicine";

                List<Medicines> expectedMedicines = new List<Medicines>
                {
                    new Medicines {Nombre = "Medicine 1", Nregistro = "1"}, new Medicines {Nombre = "Medicine 2", Nregistro = "2"},
                    new Medicines {Nombre = "Medicine 3", Nregistro = "3"}, new Medicines {Nombre = "Medicine 4", Nregistro = "4"}
                };

                mock.Mock<ICimaService>().Setup(cs => cs.GetMedicinesAsync(It.IsAny<string>()))
                    .Returns(Task.FromResult(new PagedResult<Medicines>{ Resultados = expectedMedicines }));

                #endregion
            
                #region Action

                searchMedicinesViewModel.SearchTerm = query;

                #endregion

                #region Assert

                searchMedicinesViewModel.Medicines.Should().BeEquivalentTo(expectedMedicines);

                #endregion
            }
        }
        
        [Fact]
        public void ShouldNavigateToDetailMedicine_WhenClickOnMedicine()
        {
            using (var mock = AutoMock.GetLoose())
            {
                #region Arrange

                SearchMedicinesViewModel searchMedicinesViewModel = mock.Create<SearchMedicinesViewModel>();

                Medicines medicine = new Medicines
                {
                    Nombre = "Medicine",
                    Nregistro = "Number"
                };

                #endregion

                #region Action
                
                searchMedicinesViewModel.MedicineClickCommand.Execute(medicine);

                #endregion

                #region Assert

                mock.Mock<IMvxNavigationService>().Verify(ns => ns.Navigate<DetailMedicineViewModel, Medicines>(
                    It.Is<Medicines>(m => m.Nombre == medicine.Nombre && m.Nregistro == medicine.Nregistro),
                    It.IsAny<IMvxBundle>(), It.IsAny<CancellationToken>()), Times.Once);

                #endregion
            }
        }
    }
}