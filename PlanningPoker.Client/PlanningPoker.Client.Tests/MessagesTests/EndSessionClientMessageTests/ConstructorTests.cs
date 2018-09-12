using System;
using PlanningPoker.Client.Messages;
using Xunit;
namespace PlanningPoker.Client.Tests.MessagesTests.EndSessionClientMessageTests
{
    public class ConstructorTests
    {
        [Fact]
        public void GivenConstructorIsCalled_WhenSessionIdPassedIsNull_ThenExceptionIsThrown()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new EndSessionClientMessage(null);
            });
        }
        [Fact]
        public void GivenConstructorIsCalled_WhenSessionIdPassedIsEmpty_ThenExceptionIsThrown()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new EndSessionClientMessage("");
            });
        }
        [Fact]
        public void GivenConstructorIsCalled_WhenSessionIdIsPassed_ThenValueUsCorrectlyMappedToProperty()
        {
            var sessionId = "1234";
            var result = new EndSessionClientMessage(sessionId);

            Assert.NotNull(result);
            Assert.Equal(sessionId, result.SessionId);
        }
    }
}