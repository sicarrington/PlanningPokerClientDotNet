using System;
using PlanningPoker.Client.MessageFactories;
using PlanningPoker.Client.Messages;
using PlanningPoker.Client.Utilities;
using Xunit;

namespace PlanningPoker.Client.Tests.MessageFactoriesTests.JoinSessionResponseMessageFactoryTests
{
    public class GetTests
    {
        JoinSessionResponseMessageFactory _responseFactory;
        MessageParser _messageParser;
        public GetTests()
        {
            _messageParser = new MessageParser();
            _responseFactory = new JoinSessionResponseMessageFactory(_messageParser);
        }
        [Fact]
        public void GivenGetIsCalled_WhenMessageIsNull_ThenExceptionIsThrown()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                _responseFactory.Get(null);
            });
        }
        [Fact]
        public void GivenGetIsCalled_WhenMessageIsEmpty_ThenExceptionIsThrown()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                _responseFactory.Get("");
            });
        }
        [Fact]
        public void GivenGetIsCalled_WhenMessageDoesNotContainMessageType_ThenExceptionIsThrown()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                _responseFactory.Get("AField:AValue\n");
            });
        }
        [Fact]
        public void GivenGetIsCalled_WhenMessageTypeIsNoJoinSessionResponse_ThenExceptionIsThrown()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                _responseFactory.Get("MessageType:JoinSessionResponse\n");
            });
        }
        [Fact]
        public void GivenGetIsCalled_WhenMessageIsSuccessMessageButDoesNotContainSessionId_ThenExceptionIsThrown()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                _responseFactory.Get("MessageType:JoinSessionResponse\nSuccess:True\nUserId:1234\nUserToken:AToken\n");
            });
        }
        [Fact]
        public void GivenGetIsCalled_WhenMessageIsSuccessMessageButDoesNotContainUserId_ThenExceptionIsThrown()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                _responseFactory.Get("MessageType:JoinSessionResponse\nSuccess:True\nSessionId:9876\nUserToken:AToken\n");
            });
        }
        [Fact]
        public void GivenGetIsCalled_WhenMessageIsSuccessMessageButMessageDoesNotContainUserToken_ThenExceptionIsThrown()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                _responseFactory.Get("MessageType:JoinSessionResponse\nSuccess:True\nUserId:1234\nSessionId:9876\n");
            });
        }
        [Fact]
        public void GivenGetIsCalled_WhenMessageIsValidSuccessMessage_ThenAllFieldsAreCorrectlyMappedToMessage()
        {
            var expectedSessionId = "9876";
            var expectedUserId = "1234";
            var expectedUserToken = "AToken";

            var result = _responseFactory.Get($"MessageType:JoinSessionResponse\nSuccess:True\nUserId:{expectedUserId}\nSessionId:{expectedSessionId}\nUserToken:{expectedUserToken}\n");

            Assert.NotNull(result);
            Assert.IsType<JoinSessionResponse>(result);
            var joinSessionResponseMessage = result as JoinSessionResponse;

            Assert.True(joinSessionResponseMessage.Success);
            Assert.Equal(expectedSessionId, joinSessionResponseMessage.SessionId);
            Assert.Equal(expectedUserId, joinSessionResponseMessage.UserId);
            Assert.Equal(expectedUserToken, joinSessionResponseMessage.UserToken);
        }
        [Fact]
        public void GivenGetIsCalled_WhenMessageIsValidUnsuccesfulMessageWithNoErrorMessage_ThenFieldsAreMappedAsExpected()
        {
            var expectedSessionId = "9876";

            var result = _responseFactory.Get($"MessageType:JoinSessionResponse\nSuccess:False\nSessionId:{expectedSessionId}\n");

            Assert.False(result.Success);

            Assert.IsType<JoinSessionResponse>(result);
            var joinSessionResponseMessage = result as JoinSessionResponse;

            Assert.Equal(expectedSessionId, joinSessionResponseMessage.SessionId);
            Assert.IsType<JoinSessionResponse>(result);
        }
        [Fact]
        public void GivenGetIsCalled_WhenMessageIsValidUnsuccesfulMessageWithErrorMessage_ThenFieldsAreMappedAsExpected()
        {
            var expectedErrorMessage = "Bad stuff";
            var expectedSessionId = "9876";
            var result = _responseFactory.Get($"MessageType:JoinSessionResponse\nSuccess:False\nSessionId:{expectedSessionId}\nErrorMessage:{expectedErrorMessage}");

            Assert.False(result.Success);
            Assert.IsType<JoinSessionResponse>(result);

            var joinSessionResponseMessage = result as JoinSessionResponse;
            Assert.Equal(expectedSessionId, joinSessionResponseMessage.SessionId);
            Assert.Equal(expectedErrorMessage, joinSessionResponseMessage.ErrorMessage);
        }
    }
}