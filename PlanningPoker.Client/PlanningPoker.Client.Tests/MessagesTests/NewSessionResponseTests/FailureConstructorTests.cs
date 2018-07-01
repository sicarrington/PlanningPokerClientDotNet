using System;
using PlanningPoker.Client.Messages;
using PlanningPoker.Client.Utilities;
using Xunit;

namespace PlanningPoker.Client.Tests.MessagesTests.NewSessionResponseTests
{
    public class FailureConstructorTests
    {
        [Fact]
        public void GivenConstructorIsCalled_WhenErrorMessageIsNotSent_ThenFieldsAreMappedAsExpected()
        {
            var result = new NewSessionResponse();

            Assert.NotNull(result);
            Assert.False(result.Success);
            Assert.Null(result.ErrorMessage);
        }
        [Fact]
        public void GivenConstructorIsCalled_WhenErrorMessageIsSent_ThenFieldsAreCorrectlyMapped()
        {
            var expectedErrorMessage = "Something bad happened";

            var result = new NewSessionResponse(expectedErrorMessage);

            Assert.NotNull(result);
            Assert.False(result.Success);
            Assert.Equal(expectedErrorMessage, result.ErrorMessage);
        }
    }
}