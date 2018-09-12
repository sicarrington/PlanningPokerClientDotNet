using PlanningPoker.Client.Exceptions;
using Xunit;

namespace PlanningPoker.Client.Tests.ExceptionTests.ConfigurationExceptionTests
{
    public class MessageConstructorTests
    {
        [Fact]
        public void GivenConstructorIsCalled_WhenMessagePassedIsNull_ThenExceptionIsConstructedWithEmptyMessage()
        {
            try
            {
                new ConfigurationException(null);
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
            var result = new ConfigurationException(message);

            Assert.NotNull(result);
            Assert.Equal(result.Message, message);
        }
        [Fact]
        public void GivenConstructorIsCalled_WhenMessageIsPassed_ThenMessageIsMappedToFieldCorrectly()
        {
            string message = "Bad stuff happened";
            var result = new ConfigurationException(message);

            Assert.NotNull(result);
            Assert.Equal(result.Message, message);
        }
    }
}