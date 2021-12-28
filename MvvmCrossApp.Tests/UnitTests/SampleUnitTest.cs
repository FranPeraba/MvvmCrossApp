using Autofac.Extras.Moq;
using Xunit;

namespace MvvmCrossApp.Tests.UnitTests
{
    public class SampleUnitTest
    {
        [Fact]
        public void Should_When_And_UsingWith()
        {
            using (var mock = AutoMock.GetLoose())
            {
                #region Arrange
                //Code used to build the test case state before the test execution.
                //Here you will instantiate and configure your real class to be tested and the inputs and mocks for the execution.
                #endregion
            
                #region Action
                //The trigger that will execute the parts of code needed to be tested, like a call to a method.
                #endregion
            
                #region Assert
                //Where the expected results are verified. Normally it should be checked on the result of the call, on internal or external set properties
                //or on mocked objects.
                #endregion
            }
        }
    }
}

