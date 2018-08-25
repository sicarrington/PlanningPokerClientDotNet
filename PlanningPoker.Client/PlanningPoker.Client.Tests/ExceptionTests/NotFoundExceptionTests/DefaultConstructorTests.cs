using PlanningPoker.Client.Exceptions;
using Xunit;

namespace PlanningPoker.Client.Tests.ExceptionTests.NotFoundExceptionTests
{
    public class DefaultConstructorTests
    {
        [Fact]
        public void GivenConstructorIsCalled_ThenConstructionSucceeds()
        {
            var exception = new NotFoundException();

            Assert.NotNull(exception);
        }
        [Fact]
        public void GivenConstructorIsCalled_ThenExceptionIsConstructedWithDefaultMessage()
        {
            var exception = new NotFoundException();

            Assert.Equal(exception.Message, "Exception of type 'PlanningPoker.Client.Exceptions.NotFoundException' was thrown.");
        }
        [Fact]
        public void GivenConstructorIsCalled_ThenExceptionIsConstructedWithEmptyInnerException()
        {
            var exception = new NotFoundException();

            Assert.Null(exception.InnerException);
        }
    }
}