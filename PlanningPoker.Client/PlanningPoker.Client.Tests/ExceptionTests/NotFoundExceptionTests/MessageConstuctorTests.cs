using PlanningPoker.Client.Exceptions;
using Xunit;

namespace PlanningPoker.Client.Tests.ExceptionTests.NotFoundExceptionTests
{
    public class MessageConstructorTests
    {
        [Fact]
        public void GivenConstructorIsCalled_WhenMessagePassedIsNull_ThenExceptionIsConstructedWithEmptyMessage()
        {
            NotFoundException result = null;
            try
            {
                string message = null;
                new NotFoundException(null);
            }
            catch (NotFoundException ex)
            {
                Assert.NotNull(ex);
                Assert.Null(ex.Message);
            }
        }
        [Fact]
        public void GivenConstructorIsCalled_WhenMessagePassedIsEmpty_ThenExceptionIsConstructedWithEmptyMessage()
        {
            string message = "";
            var result = new NotFoundException(message);

            Assert.NotNull(result);
            Assert.Equal(result.Message, message);
        }
        [Fact]
        public void GivenConstructorIsCalled_WhenMessageIsPassed_ThenMessageIsMappedToFieldCorrectly()
        {
            string message = "Bad stuff happened";
            var result = new NotFoundException(message);

            Assert.NotNull(result);
            Assert.Equal(result.Message, message);
        }
    }
}