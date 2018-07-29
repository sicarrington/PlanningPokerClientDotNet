using System;
using PlanningPoker.Client.Messages;
using PlanningPoker.Client.Utilities;
using Xunit;

namespace PlanningPoker.Client.Tests.MessagesTests.SubscribeSessionResponseTests
{
    public class ConstructorTests
    {
        [Fact]
        public void GivenConstructorIsCalled_WhenErrorMessageIsSent_ThenFieldsAreCorrectlyMapped()
        {
            var expectedErrorMessage = "Something bad happened";

            var result = new SubscribeSessionResponse(false, "123456", expectedErrorMessage);

            Assert.NotNull(result);
            Assert.False(result.Success);
            Assert.Equal(expectedErrorMessage, result.ErrorMessage);
        }
        [Fact]
        public void GivenConstructorIsCalled_WhenFailureScenario_ErrorMessageIsNotRequired()
        {
            var result = new SubscribeSessionResponse(false, "123456");

            Assert.NotNull(result);
            Assert.False(result.Success);
        }
        [Fact]
        public void GivenConstructorIsCalled_WhenSuccessIsTrueButSessionIdIsNull_ThenErrorIsThrown()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                var result = new SubscribeSessionResponse(true, null);
            });
        }
        [Fact]
        public void GivenConstructorIsCalled_WhenSuccessIsTrueButSessionIdIsEmpty_ThenErrorIsThrown()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                var result = new SubscribeSessionResponse(true, "");
            });
        }
        [Fact]
        public void GivenConstructorIsCalled_WhenSuccessIsFalseButSessionIdIsNull_ThenErrorIsThrown()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                var result = new SubscribeSessionResponse(false, null);
            });
        }
        [Fact]
        public void GivenConstructorIsCalled_WhenSuccessIsFalseButSessionIdIsEmpty_ThenErrorIsThrown()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                var result = new SubscribeSessionResponse(false, "");
            });
        }
        [Fact]
        public void GivenConstructorIsCalled_WhenSuccessScenario_ThenFieldsAreCorrectlyMapped()
        {
            var sessionId = "12345";
            var result = new SubscribeSessionResponse(true, sessionId);

            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.Equal(sessionId, result.SessionId);
        }
    }
}