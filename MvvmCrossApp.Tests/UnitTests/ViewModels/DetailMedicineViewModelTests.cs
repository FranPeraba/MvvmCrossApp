using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using FluentAssertions;
using Moq;
using MvvmCrossApp.Core.Models;
using MvvmCrossApp.Core.Services;
using MvvmCrossApp.Core.ViewModels;
using MvvmCrossApp.Core.Wrappers;
using Xamarin.Essentials;
using Xunit;

namespace MvvmCrossApp.Tests.UnitTests.ViewModels
{
    public class DetailMedicineViewModelTests
    {
        [Fact]
        public async Task ShouldSetIsLoadingToFalse_WhenSetMedicine_OnPrepare()
        {
            using (var mock = AutoMock.GetLoose())
            {
                #region Arrange
                
                mock.CanExecuteOnMainThread();

                DetailMedicineViewModel detailMedicineViewModel = mock.Create<DetailMedicineViewModel>();
                
                Medicines medicine = new Medicines
                {
                    Nombre = "Medicine 1",
                    Nregistro = "1"
                };
                
                List<Document> documents = new List<Document>();
                
                mock.Mock<ICimaService>().Setup(cs => cs.GetMedicineAsync(It.IsAny<string>()))
                    .Returns(Task.FromResult(new Medicine
                    {
                        Nombre = medicine.Nombre,
                        Nregistro = medicine.Nregistro,
                        Docs = documents
                    }));
                
                var tcs = new TaskCompletionSource<bool>();
                var timeout = Task.Delay(10000);

                detailMedicineViewModel.PropertyChanged += (o, e) =>
                {
                    if (e.PropertyName == nameof(DetailMedicineViewModel.IsLoading) && !detailMedicineViewModel.IsLoading)
                        tcs.SetResult(true);
                };
                
                #endregion
            
                #region Action
                
                detailMedicineViewModel.Prepare(medicine);
                await Task.WhenAny(new List<Task> { tcs.Task, timeout });
                
                #endregion
            
                #region Assert

                tcs.Task.IsCompleted.Should().BeTrue();
                tcs.Task.Result.Should().BeTrue();
                detailMedicineViewModel.IsLoading.Should().BeFalse();

                #endregion
            }
        }
        
        [Fact]
        public async Task ShouldSetNameToExpectedName_OnPrepare()
        {
            using (var mock = AutoMock.GetLoose())
            {
                #region Arrange

                mock.CanExecuteOnMainThread();

                DetailMedicineViewModel detailMedicineViewModel = mock.Create<DetailMedicineViewModel>();

                var expectedName = "Medicine 1";

                Medicines expectedMedicine = new Medicines
                {
                    Nombre = expectedName,
                    Nregistro = "1"
                };

                List<Document> documents = new List<Document>();

                mock.Mock<ICimaService>().Setup(cs => cs.GetMedicineAsync(It.IsAny<string>()))
                    .Returns(Task.FromResult(new Medicine
                    {
                        Nombre = expectedMedicine.Nombre,
                        Nregistro = expectedMedicine.Nregistro,
                        Docs = documents
                    }));

                var tcs = new TaskCompletionSource<bool>();
                var timeout = Task.Delay(10000);

                detailMedicineViewModel.PropertyChanged += (o, e) =>
                {
                    if (e.PropertyName == nameof(DetailMedicineViewModel.Name))
                        tcs.SetResult(true);
                };

                #endregion

                #region Action

                detailMedicineViewModel.Prepare(expectedMedicine);
                await Task.WhenAny(new List<Task> { tcs.Task, timeout });

                #endregion

                #region Assert

                tcs.Task.IsCompleted.Should().BeTrue();
                tcs.Task.Result.Should().BeTrue();
                detailMedicineViewModel.Name.Should().Be(expectedName);

                #endregion
            }
        }

        [Fact]
        public async Task ShouldSetDocumentsToExpectedDocuments_OnPrepare()
        {
            using (var mock = AutoMock.GetLoose())
            {
                #region Arrange

                mock.CanExecuteOnMainThread();

                DetailMedicineViewModel detailMedicineViewModel = mock.Create<DetailMedicineViewModel>();

                Medicines medicine = new Medicines
                {
                    Nombre = "Medicine 1",
                    Nregistro = "1"
                };

                var expectedDocuments = new List<Document>
                {
                    new Document { Tipo = 1, UrlHtml = "https://cima.aemps.es/cima/rest", Url = string.Empty },
                    new Document { Tipo = 1, UrlHtml = "https://cima.aemps.es/cima/rest", Url = string.Empty }
                };

                mock.Mock<ICimaService>().Setup(cs => cs.GetMedicineAsync(It.IsAny<string>()))
                    .Returns(Task.FromResult(new Medicine
                    {
                        Nombre = medicine.Nombre,
                        Nregistro = medicine.Nregistro,
                        Docs = expectedDocuments
                    }));

                var tcs = new TaskCompletionSource<bool>();
                var timeout = Task.Delay(10000);

                detailMedicineViewModel.PropertyChanged += (o, e) =>
                {
                    if (e.PropertyName == nameof(detailMedicineViewModel.Documents))
                        tcs.SetResult(true);
                };

                #endregion

                #region Action

                detailMedicineViewModel.Prepare(medicine);
                await Task.WhenAny(new List<Task> { tcs.Task, timeout });

                #endregion

                #region Assert

                tcs.Task.IsCompleted.Should().BeTrue();
                tcs.Task.Result.Should().BeTrue();
                detailMedicineViewModel.Documents.Should().BeEquivalentTo(expectedDocuments);

                #endregion
            }
        }

        [Fact]
        public async Task ShouldOpenDocumentWithExpectedDocument_WhenDocumentButtonClicked()
        {
            using (var mock = AutoMock.GetLoose())
            {
                #region Arrange

                DetailMedicineViewModel detailMedicineViewModel = mock.Create<DetailMedicineViewModel>();
                var documents = new List<Document>
                {
                    new Document { Tipo = 1, UrlHtml = "https://cima.aemps.es/cima/rest", Url = string.Empty },
                    new Document { Tipo = 1, UrlHtml = "https://cima.aemps.es/cima/rest", Url = string.Empty }
                };

                var expectedDocument = documents[1].UrlHtml;

                detailMedicineViewModel.Documents.AddRange(documents);

                mock.Mock<IBrowserWrapper>().Setup(br => br.Browser(expectedDocument, BrowserLaunchMode.SystemPreferred))
                    .Returns(Task.CompletedTask);

                #endregion

                #region Action

                await detailMedicineViewModel.OpenDocumentAsyncCommand.ExecuteAsync();

                #endregion

                #region Assert

                mock.Mock<IBrowserWrapper>().Verify(br => br.Browser(expectedDocument, BrowserLaunchMode.SystemPreferred), Times.Once);

                #endregion
            }
        }
    }
}