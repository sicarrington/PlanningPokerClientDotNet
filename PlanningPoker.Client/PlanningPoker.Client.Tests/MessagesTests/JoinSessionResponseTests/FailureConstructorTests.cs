using System;
using PlanningPoker.Client.Messages;
using Xunit;
namespace PlanningPoker.Client.Tests.MessagesTests.JoinSessionResponseTests
{
    public class FailureConstructorTests
    {
        [Fact]
        public void GivenConstructorIsCalled_WhenSessionIdIsNull_ThenExceptionIsThrown()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new JoinSessionResponse(null, "ErrorMessage");
            });
        }
        [Fact]
        public void GivenConstructorIsCalled_WhenSessionIdIsEmpty_ThenExceptionIsThrown()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new JoinSessionResponse("", "ErrorMessage");
            });
        }
        [Fact]
        public void GivenConstructorIsCalled_WhenErrorMessageIsNull_ConstructionSucceeds()
        {
            var result = new JoinSessionResponse("SessionId");

            Assert.NotNull(result);
            Assert.False(result.Success);
            Assert.Null(result.ErrorMessage);
        }
        [Fact]
        public void GivenCOnstructorIsCalled_WhenPArametersAreValid_ThenFieldsAreMappedAsExcepted()
        {
            var sessionId = "1234";
            var errorMessage = "Something bad happened";

            var result = new JoinSessionResponse(sessionId, errorMessage);

            Assert.NotNull(result);
            Assert.False(result.Success);
            Assert.Equal(sessionId, result.SessionId);
            Assert.Equal(errorMessage, result.ErrorMessage);
        }
    }
}