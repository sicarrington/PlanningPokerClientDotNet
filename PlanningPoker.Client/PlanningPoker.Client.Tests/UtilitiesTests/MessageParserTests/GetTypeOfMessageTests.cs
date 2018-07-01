using System;
using PlanningPoker.Client.Model;
using PlanningPoker.Client.Utilities;
using Xunit;

namespace PlanningPoker.Client.Tests.UtilitiesTests.MessageParserTests
{
    public class GetTypeOfMessageTests
    {
        MessageParser _messageParser;
        public GetTypeOfMessageTests()
        {
            _messageParser = new MessageParser();
        }
        [Fact]
        public void GivenGetTypeOfMessageIsCalled_WhenMessageSentIsNull_ThenExceptionIsThrown()
        {
            Assert.Throws<ArgumentNullException>(()=>{
                _messageParser.GetTypeOfMessage(null);
            });
        }
        [Fact]
        public void GivenGetTypeOfMessageIsCalled_WhenMessageSentIsEmpty_ThenExceptionIsThrown()
        {
            Assert.Throws<ArgumentNullException>(()=>{
                _messageParser.GetTypeOfMessage("");
            });
        }
        [Fact]
        public void GivenGetTypeOfMessageIsCalled_WhenMessageDoesNotContainMessageTypeField_ThenExceptionIsThrown()
        {
            Assert.Throws<InvalidOperationException>(()=>{
                _messageParser.GetTypeOfMessage("Success:True\n");
            });
        }
        [Fact]
        public void GivenGetTypeOfMessageIsCalled_WhenMessageContainsMessageTypeFieldButNoValue_ThenExceptionIsThrown()
        {
            Assert.Throws<InvalidOperationException>(()=>{
                _messageParser.GetTypeOfMessage("MessageType:\nSuccess:True\n");
            });
        }
        [Fact]
        public void GivenGetTypeOfMessageIsCalled_WhenMessageContainsInvalidMessageType_ThenExceptionIsThrown()
        {
            Assert.Throws<InvalidOperationException>(()=>{
                _messageParser.GetTypeOfMessage("MessageType:Nonesense\nSuccess:True\n");
            });
        }
        [Fact]
        public void GivenGetTypeOfMessageIsCalled_WhenMessageContainsValidMessageType_ThenMessageTypeIsCorrectlyReturned()
        {
            var result = 
                _messageParser.GetTypeOfMessage("MessageType:NewSessionResponse\nSuccess:True\n");
            
            Assert.Equal(ResponseMessageType.NewSessionResponse, result);
        }
    }
}