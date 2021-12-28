using System;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Moq;
using MvvmCross.Base;
using MvvmCross.IoC;

namespace MvvmCrossApp.Tests.UnitTests;

public static class MockExtensions
{
    public static void CanExecuteOnMainThread(this AutoMock mock)
    {
        mock.Mock<IMvxMainThreadAsyncDispatcher>()
            .Setup(h => h.ExecuteOnMainThreadAsync(It.IsAny<Action>(), It.IsAny<bool>()))
            .Callback<Action, bool>((a, b) =>
            {
                a.Invoke();
            }).Returns(Task.CompletedTask);

        mock.Mock<IMvxMainThreadAsyncDispatcher>()
            .Setup(h => h.ExecuteOnMainThreadAsync(It.IsAny<Func<Task>>(), It.IsAny<bool>()))
            .Callback<Func<Task>, bool>((a, b) =>
            {
                a.Invoke().Wait();
            }).Returns(Task.CompletedTask);

        mock.Mock<IMvxIoCProvider>().Setup(ioc => ioc.Resolve<IMvxMainThreadAsyncDispatcher>())
            .Returns(mock.Mock<IMvxMainThreadAsyncDispatcher>().Object);
    }
    
}