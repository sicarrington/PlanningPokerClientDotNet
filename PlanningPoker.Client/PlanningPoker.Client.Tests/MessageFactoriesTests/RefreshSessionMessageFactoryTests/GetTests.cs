using System;
using PlanningPoker.Client.MessageFactories;
using PlanningPoker.Client.Messages;
using PlanningPoker.Client.Utilities;
using Xunit;

namespace PlanningPoker.Client.Tests.MessageFactoriesTests.RefreshSessionMessageFactoryTests
{
    public class GetTests
    {
        RefreshSessionMessageFactory _responseFactory;
        MessageParser _messageParser;
        public GetTests()
        {
            _messageParser = new MessageParser();
            _responseFactory = new RefreshSessionMessageFactory(_messageParser);
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
                _responseFactory.Get("MessageType:RefreshSession\nSuccess:True\n");
            });
        }
        [Fact]
        public void GivenGetIsCalled_WhenMessageIsValidSuccessMessage_ThenAllFieldsAreCorrectlyMappedToMessage()
        {
            var expectedSessionId = "9876";

            var result = _responseFactory.Get($"MessageType:RefreshSession\nSuccess:True\n\nSessionId:{expectedSessionId}\n");

            Assert.NotNull(result);
            Assert.IsType<RefreshSessionResponse>(result);
            var refreshSessionResponseMessage = result as RefreshSessionResponse;

            Assert.True(refreshSessionResponseMessage.Success);
            Assert.Equal(expectedSessionId, refreshSessionResponseMessage.SessionId);
        }
    }
}