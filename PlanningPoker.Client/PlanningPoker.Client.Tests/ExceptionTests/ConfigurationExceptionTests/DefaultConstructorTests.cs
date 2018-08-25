using PlanningPoker.Client.Exceptions;
using Xunit;

namespace PlanningPoker.Client.Tests.ExceptionTests.ConfigurationExceptionTests
{
    public class DefaultConstructorTests
    {
        [Fact]
        public void GivenConstructorIsCalled_ThenConstructionSucceeds()
        {
            var exception = new ConfigurationException();

            Assert.NotNull(exception);
        }
        [Fact]
        public void GivenConstructorIsCalled_ThenExceptionIsConstructedWithDefaultMessage()
        {
            var exception = new ConfigurationException();

            Assert.Equal(exception.Message, "Exception of type 'PlanningPoker.Client.Exceptions.ConfigurationException' was thrown.");
        }
        [Fact]
        public void GivenConstructorIsCalled_ThenExceptionIsConstructedWithEmptyInnerException()
        {
            var exception = new ConfigurationException();

            Assert.Null(exception.InnerException);
        }
    }
}