using System;
using PlanningPoker.Client.MessageFactories;
using PlanningPoker.Client.Messages;
using PlanningPoker.Client.Utilities;
using Xunit;

namespace PlanningPoker.Client.Tests.MessageFactoriesTests.EndSessionClientMessageFactoryTests
{
    public class GetTests
    {
        EndSessionClientMessageFactory _responseFactory;
        MessageParser _messageParser;
        public GetTests()
        {
            _messageParser = new MessageParser();
            _responseFactory = new EndSessionClientMessageFactory(_messageParser);
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
        public void GivenGetIsCalled_WhenMessageTypeIsNotEndSessionClientMessage_ThenExceptionIsThrown()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                _responseFactory.Get("MessageType:JoinSessionResponse\n");
            });
        }
        [Fact]
        public void GivenGetIsCalled_WhenMessageIsSessionEndedMessageButDoesNotContainSessionId_ThenExceptionIsThrown()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                _responseFactory.Get("MessageType:SessionEndedMessage\n");
            });
        }
        [Fact]
        public void GivenGetIsCalled_WhenMessageIsSessionEndedMessageButSessionIdIsEmpty_ThenExceptionIsThrown()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                _responseFactory.Get("MessageType:SessionEndedMessage\nSessionId:\n");
            });
        }
        [Fact]
        public void GivenGetIsCalled_WhenMessageIsSessionEndedMessageAndSessionIdIsPresent_ThenModelIsCreatedCorrectly()
        {
            var result = _responseFactory.Get("MessageType:SessionEndedMessage\nSessionId:9876\n");

            Assert.NotNull(result);
            Assert.IsType<EndSessionClientMessage>(result);
            var endSessionClientMessage = result as EndSessionClientMessage;
            Assert.Equal("9876", endSessionClientMessage.SessionId);
        }
    }
}