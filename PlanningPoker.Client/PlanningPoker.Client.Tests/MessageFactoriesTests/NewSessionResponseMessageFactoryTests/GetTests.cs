using System;
using Moq;
using PlanningPoker.Client.MessageFactories;
using PlanningPoker.Client.Messages;
using PlanningPoker.Client.Utilities;
using Xunit;

namespace PlanningPoker.Client.Tests.MessageFactoriesTests.NewSessionResponseMessageFactoryTests
{
    public class GetTests
    {
        NewSessionResponseMessageFactory _responseFactory;
        MessageParser _messageParser;
        public GetTests()
        {
            _messageParser = new MessageParser();
            _responseFactory = new NewSessionResponseMessageFactory(_messageParser);
        }
        [Fact]
        public void GivenGetIsCalled_WhenMessagePassedIsNull_ThenExceptionIsThrown()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                _responseFactory.Get(null);
            });
        }
        [Fact]
        public void GivenGetIsCalled_WhenMessagePassedIsEmpty_ThenExceptionIsThrown()
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
        public void GivenGetIsCalled_WhenMessageTypeIsNotNewSessionResponse_ThenExceptionIsThrown()
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
                _responseFactory.Get("MessageType:NewSessionResponse\nSuccess:True\nUserId:1234\nUserToken:AToken\n");
            });
        }
        [Fact]
        public void GivenGetIsCalled_WhenMessageIsSuccessMessageButDoesNotContainUserId_ThenExceptionIsThrown()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                _responseFactory.Get("MessageType:NewSessionResponse\nSuccess:True\nSessionId:9876\nUserToken:AToken\n");
            });
        }
        [Fact]
        public void GivenGetIsCalled_WhenMessageIsSuccessMessageButMessageDoesNotContainUserToken_ThenExceptionIsThrown()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                _responseFactory.Get("MessageType:NewSessionResponse\nSuccess:True\nUserId:1234\nSessionId:9876\n");
            });
        }
        [Fact]
        public void GivenGetIsCalled_WhenMessageIsValidSuccessMessage_ThenAllFieldsAreCorrectlyMappedToMessage()
        {
            var expectedSessionId = "9876";
            var expectedUserId = "1234";
            var expectedUserToken = "AToken";

            var result = _responseFactory.Get($"MessageType:NewSessionResponse\nSuccess:True\nUserId:{expectedUserId}\nSessionId:{expectedSessionId}\nUserToken:{expectedUserToken}\n");

            Assert.NotNull(result);
            Assert.IsType<NewSessionResponse>(result);
            var newSessionResponseMessage = result as NewSessionResponse;

            Assert.True(newSessionResponseMessage.Success);
            Assert.Equal(expectedSessionId, newSessionResponseMessage.SessionId);
            Assert.Equal(expectedUserId, newSessionResponseMessage.UserId);
            Assert.Equal(expectedUserToken, newSessionResponseMessage.UserToken);
        }
        [Fact]
        public void GivenGetIsCalled_WhenMessageIsValidUnsuccesfulMessageWithNoErrorMessage_ThenFieldsAreMappedAsExpected()
        {
            var result = _responseFactory.Get($"MessageType:NewSessionResponse\nSuccess:False\n");

            Assert.False(result.Success);
            Assert.IsType<NewSessionResponse>(result);
        }
        [Fact]
        public void GivenGetIsCalled_WhenMessageIsValidUnsuccesfulMessageWithErrorMessage_ThenFieldsAreMappedAsExpected()
        {
            var expectedErrorMessage = "Bad stuff";
            var result = _responseFactory.Get($"MessageType:NewSessionResponse\nSuccess:False\nErrorMessage:{expectedErrorMessage}");

            Assert.False(result.Success);
            Assert.IsType<NewSessionResponse>(result);

            var newSessionResponseMessage = result as NewSessionResponse;
            Assert.Equal(expectedErrorMessage, newSessionResponseMessage.ErrorMessage);
        }
    }
}