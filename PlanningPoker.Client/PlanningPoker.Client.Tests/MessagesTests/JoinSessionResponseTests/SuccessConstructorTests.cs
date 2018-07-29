using System;
using PlanningPoker.Client.Messages;
using Xunit;
namespace PlanningPoker.Client.Tests.MessagesTests.JoinSessionResponseTests
{
    public class SuccessConstructorTests
    {
        [Fact]
        public void GIvenConstructorIsCalled_WhenSessionIdIsNull_ThenExceptionIsThrown()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new JoinSessionResponse(null, "UserId", "UserToken");
            });
        }
        [Fact]
        public void GIvenConstructorIsCalled_WhenSessionIdIsEmpty_ThenExceptionIsThrown()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new JoinSessionResponse("", "UserId", "UserToken");
            });
        }
        [Fact]
        public void GIvenConstructorIsCalled_WhenUserIdIsNull_ThenExceptionIsThrown()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new JoinSessionResponse("SessionId", null, "UserToken");
            });
        }
        [Fact]
        public void GIvenConstructorIsCalled_WhenUserIdIsEmpty_ThenExceptionIsThrown()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new JoinSessionResponse("SessionId", "", "UserToken");
            });
        }
        [Fact]
        public void GIvenConstructorIsCalled_WhenUserTokensNull_ThenExceptionIsThrown()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new JoinSessionResponse("SessionId", "UserId", null);
            });
        }
        [Fact]
        public void GIvenConstructorIsCalled_WhenUserTokenIsEmpty_ThenExceptionIsThrown()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new JoinSessionResponse("SessionId", "UserId", "");
            });
        }
        [Fact]
        public void GivenConstructorIsCalled_WhenArgumentsPassedAreValid_ThenValuesAreMappedCorrectly()
        {
            var sessionId = "12345";
            var userId = "98765";
            var userToken = "19283746";

            var result = new JoinSessionResponse(sessionId, userId, userToken);

            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.Equal(sessionId, result.SessionId);
            Assert.Equal(userId, result.UserId);
            Assert.Equal(userToken, result.UserToken);
            Assert.Null(result.ErrorMessage);
        }
    }
}