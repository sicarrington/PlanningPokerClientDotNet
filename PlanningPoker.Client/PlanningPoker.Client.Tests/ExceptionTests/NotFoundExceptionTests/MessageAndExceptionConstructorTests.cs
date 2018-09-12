using PlanningPoker.Client.Exceptions;
using Xunit;

namespace PlanningPoker.Client.Tests.ExceptionTests.NotFoundExceptionTests
{
    public class MessageAndExceptionConstructorTests
    {
        [Fact]
        public void GivenConstructorIsCalled_WhenMessagePassedIsNull_ThenExceptionIsConstructedWithEmptyMessage()
        {
            try
            {
                new NotFoundException(null, new System.Exception());
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
            var result = new NotFoundException(message, new System.Exception());

            Assert.NotNull(result);
            Assert.Equal(result.Message, message);
        }
        [Fact]
        public void GivenConstructorIsCalled_WhenMessageIsPassedButExceptionIsNull_ThenConstructionSuceeds()
        {
            string message = "Bad things";
            var result = new NotFoundException(message, null);

            Assert.NotNull(result);
            Assert.Equal(result.Message, message);
            Assert.Null(result.InnerException);
        }
        [Fact]
        public void GivenConstructorIsCalled_WhenMessageAndExceptioNArePassed_ThenConstructionSuceeds()
        {
            string message = "Bad things";
            var anException = new System.Exception();
            var result = new NotFoundException(message, anException);

            Assert.NotNull(result);
            Assert.Equal(result.Message, message);
            Assert.Equal(anException, result.InnerException);
        }
    }
}