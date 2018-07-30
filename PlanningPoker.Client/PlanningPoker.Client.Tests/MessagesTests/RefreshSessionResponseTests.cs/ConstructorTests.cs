using System;
using PlanningPoker.Client.Messages;
using Xunit;

namespace PlanningPoker.Client.Tests.MessagesTests.RefreshSessionResponseTests
{
    public class ConstructorTests
    {
        [Fact]
        public void GivenConstructorIsCalled_WhenSessionidIsNull_ThenExceptionIsThrown()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new RefreshSessionResponse(null);
            });
        }
        [Fact]
        public void GivenConstructorIsCalled_WhenSessionIdIsEmpty_ThenExceptionIsThrown()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new RefreshSessionResponse("");
            });
        }
        [Fact]
        public void GivenConstructorIsCalled_WhenParametersAreValid_ThenModelIsCorrectlyConstructed()
        {
            var sessionId = "1234";

            var result = new RefreshSessionResponse(sessionId);

            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.Equal(sessionId, result.SessionId);
        }
    }
}