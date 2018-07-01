using System;
using PlanningPoker.Client.Messages;
using PlanningPoker.Client.Utilities;
using Xunit;

namespace PlanningPoker.Client.Tests.MessagesTests.NewSessionResponseTests
{
    public class SuccessConstructorTests
    {
        [Fact]
        public void GivenConstructorIsCalled_WhenSessionIdIsNull_ThenExceptionIsThrown()
        {
            Assert.Throws<ArgumentNullException>(()=>
            {
                new NewSessionResponse(null, "UserId", "UserToken");
            });
        }
        [Fact]
        public void GivenConstructorIsCalled_WhenSessionIdIsEmpty_ThenExceptionIsThrown()
        {
            Assert.Throws<ArgumentNullException>(()=>
            {
                new NewSessionResponse("", "UserId", "UserToken");
            });
        }
        [Fact]
        public void GivenConstructorIsCalled_WhenUserIdIsNull_ThenExceptionIsThrown()
        {
            Assert.Throws<ArgumentNullException>(()=>
            {
                new NewSessionResponse("SessionId", null, "UserToken");
            });
        }
        [Fact]
        public void GivenConstructorisCalled_WhenUserIdIsEmpty_ThenExceptionIsThrown()
        {
            Assert.Throws<ArgumentNullException>(()=>
            {
                new NewSessionResponse("SessionId", "", "UserToken");
            });
        }
        [Fact]
        public void GivenConstructorIsCalled_WhenUserTokenIsNull_ThenExceptionIsThrown()
        {
            Assert.Throws<ArgumentNullException>(()=>
            {
                new NewSessionResponse("SessionId", "UserId", null);
            });
        }
        [Fact]
        public void GivenConstructorIsCalled_WhenUserTokenIsEmpty_ThenExceptionIsThrown()
        {
            Assert.Throws<ArgumentNullException>(()=>
            {
                new NewSessionResponse("SessionId", "UserId", "");
            });
        }
        [Fact]
        public void GivenConstructorIsCalled_WhenValidParametersArePassed_ThenPropertiesAreCorrectlyMapped()
        {
            var expectedSessionId = "1234";
            var expectedUserId = "5678";
            var expectedUserToken = "91011";

            var result = new NewSessionResponse(expectedSessionId, expectedUserId, expectedUserToken);

            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.Equal(expectedSessionId, result.SessionId);
            Assert.Equal(expectedUserId, result.UserId);
            Assert.Equal(expectedUserToken, result.UserToken);
        }
    }
}