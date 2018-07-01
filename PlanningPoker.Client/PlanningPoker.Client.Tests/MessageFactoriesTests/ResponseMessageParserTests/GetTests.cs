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
            _newSessionResponseMessageFactory = new NewSessionResponseMessageFactory(_messageParser);
            var responseMessageFactories = new List<IResponseMessageFactory>();
            responseMessageFactories.Add(_newSessionResponseMessageFactory);
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
            //_newSessionResponseMessageFactory.Setup(x => x.Get(It.IsAny<string>())).Returns(new NewSessionResponse("1234", "5678", "AToken"));

            var result = _responseMessageFactory.Get("MessageType:NewSessionResponse\nSuccess:True\nSessionId:1234\nUserId:56789\nUserToken:q2\n");

            //_newSessionResponseMessageFactory.Verify(x => x.Get(It.IsAny<string>()), Times.Once);
            Assert.IsType<NewSessionResponse>(result);
        }
    }
}