using System;
using System.Collections.Generic;
using Moq;
using PlanningPoker.Client.MessageFactories;
using PlanningPoker.Client.Messages;
using PlanningPoker.Client.Utilities;
using Xunit;

namespace PlanningPoker.Client.Tests.MessageFactoriesTests.ResponseMessageParserTests
{
    public class GetTests
    {
        ResponseMessageParser _responseMessageFactory;
        MessageParser _messageParser;
        NewSessionResponseMessageFactory _newSessionResponseMessageFactory;
        public GetTests()
        {
            _messageParser = new MessageParser();

            var responseMessageFactories = new List<IResponseMessageFactory>();
            responseMessageFactories.Add(new NewSessionResponseMessageFactory(_messageParser));
            responseMessageFactories.Add(new SubscribeSessionResponseMessageFactory(_messageParser));
            responseMessageFactories.Add(new JoinSessionResponseMessageFactory(_messageParser));

            _responseMessageFactory = new ResponseMessageParser(_messageParser, responseMessageFactories);
        }
        [Fact]
        public void GivenGetIsCalled_WhenMessageIsNull_ThenExceptionIsThrown()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                _responseMessageFactory.Get(null);
            });
        }
        [Fact]
        public void GivenGetIsCalled_WhenMessageIsEmpty_ThenExceptionIsThrown()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                _responseMessageFactory.Get("");
            });
        }
        [Fact]
        public void GivenGetIsCalled_WhenMessageTypeDoesNotExistInMessage_ThenExceptionIsThrown()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                _responseMessageFactory.Get("SomeField:SomeValue\n");
            });
        }
        [Fact]
        public void GivenGetIsCalled_WhenMessageTypeIsNotRecognised_ThenExceptionIsThrown()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                _responseMessageFactory.Get("MessageType:Nonesense\n");
            });
        }
        [Fact]
        public void GivenGetIsCalled_WhenMessageTypeIsNewSessionMessage_ThenNewSessionResponseMessageIsReturned()
        {
            var result = _responseMessageFactory.Get("MessageType:NewSessionResponse\nSuccess:True\nSessionId:1234\nUserId:56789\nUserToken:q2\n");

            Assert.IsType<NewSessionResponse>(result);
        }
        [Fact]
        public void GivenGetIsCalled_WhenMessageTypeIsSubscribeSessionResponse_ThenNewSessionResponseMessageIsReturned()
        {
            var result = _responseMessageFactory.Get($"PP 1.0\nMessageType:SubscribeSessionResponse\nSuccess:true\nSessionId:12345\n");

            Assert.IsType<SubscribeSessionResponse>(result);
        }
        [Fact]
        public void GivenGetIsCalled_WhenMessageTypeIsJoinSessionResponse_ThenNewSessionResponseMessageIsReturned()
        {
            var result = _responseMessageFactory.Get($"PP 1.0\nMessageType:JoinSessionResponse\nSuccess:true\nSessionId:12345\nUserId:72635\nUserToken:8934848\n");

            Assert.IsType<JoinSessionResponse>(result);
        }
    }
}