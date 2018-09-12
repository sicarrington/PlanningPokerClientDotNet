using PlanningPoker.Client.Exceptions;
using Xunit;

namespace PlanningPoker.Client.Tests.ExceptionTests.ConfigurationExceptionTests
{
    public class MessageAndExceptionConstructorTests
    {
        [Fact]
        public void GivenConstructorIsCalled_WhenMessagePassedIsNull_ThenExceptionIsConstructedWithEmptyMessage()
        {
            try
            {
                new ConfigurationException(null, new System.Exception());
            }
            catch (ConfigurationException ex)
            {
                Assert.NotNull(ex);
                Assert.Null(ex.Message);
            }
        }
        [Fact]
        public void GivenConstructorIsCalled_WhenMessagePassedIsEmpty_ThenExceptionIsConstructedWithEmptyMessage()
        {
            string message = "";
            var result = new ConfigurationException(message, new System.Exception());

            Assert.NotNull(result);
            Assert.Equal(result.Message, message);
        }
        [Fact]
        public void GivenConstructorIsCalled_WhenMessageIsPassedButExceptionIsNull_ThenConstructionSuceeds()
        {
            string message = "Bad things";
            var result = new ConfigurationException(message, null);

            Assert.NotNull(result);
            Assert.Equal(result.Message, message);
            Assert.Null(result.InnerException);
        }
        [Fact]
        public void GivenConstructorIsCalled_WhenMessageAndExceptioNArePassed_ThenConstructionSuceeds()
        {
            string message = "Bad things";
            var anException = new System.Exception();
            var result = new ConfigurationException(message, anException);

            Assert.NotNull(result);
            Assert.Equal(result.Message, message);
            Assert.Equal(anException, result.InnerException);
        }
    }
}