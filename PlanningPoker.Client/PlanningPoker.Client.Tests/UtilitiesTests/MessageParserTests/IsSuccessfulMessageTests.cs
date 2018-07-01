using System;
using PlanningPoker.Client.Model;
using PlanningPoker.Client.Utilities;
using Xunit;

namespace PlanningPoker.Client.Tests.UtilitiesTests.MessageParserTests
{
    public class IsSuccesfulMessageTests
    {
        MessageParser _messageParser;
        public IsSuccesfulMessageTests()
        {
            _messageParser = new MessageParser();
        }
        [Fact]
        public void GivenIsSuccesfullMessageIsCalled_WhenMessagePassedIsNull_ThenExceptionIsThrown()
        {
            Assert.Throws<ArgumentNullException>(()=>{
                _messageParser.IsSuccessfulMessage(null);
            });
        }
        [Fact]
        public void GivenIsSuccesfulMessageIsCalled_WhenMessagePassedIsEmpty_ThenExceptionIsThrown()
        {
            Assert.Throws<ArgumentNullException>(()=>{
                _messageParser.IsSuccessfulMessage("");
            });
        }
        [Fact]
        public void GivenIsSuccesfulMessageIsCalled_WhenMessageDoesNotContainSuccessField_ThenFalseIsReturned()
        {
            var result = 
                _messageParser.IsSuccessfulMessage("MessageType:JoinSessionMessage\n");
            
            Assert.False(result);
        }
        [Fact]
        public void GivenIsSuccesfulMessageIsCalled_WhenMessageContainsSuccessFieldWithNoValue_ThenFalseIsReturned()
        {
            var result =
                _messageParser.IsSuccessfulMessage("MessageType:JoinSessionMessage\nSuccess:\n");
            
            Assert.False(result);
        }
        [Fact]
        public void GivenIsSuccesfulMessageIsCalled_WhenMessageContainsSuccessFieldWithInvalidValue_ThenFalseIsReturned()
        {
            var result = 
                _messageParser.IsSuccessfulMessage("MessageType:JoinSessionMessage\nSuccess:Nonesense\n");
            
            Assert.False(result);
        }
        [Fact]
        public void GivenIsSuccesfulMessageIsCalled_WhenMessageContainsSuccessFieldWithTrueValue_ThenTrueIsReturned()
        {
            var result = 
                _messageParser.IsSuccessfulMessage("MessageType:JoinSessionMessage\nSuccess:True\n");
            
            Assert.True(result);
        }
        [Fact]
        public void GivenIsSuccesfulMessageIsCalled_WhenMessageContainsSuccessFieldWithFalseValue_ThenFalseIsReturned()
        {
            var result = 
                _messageParser.IsSuccessfulMessage("MessageType:JoinSessionMessage\nSuccess:False\n");
            
            Assert.False(result);
        }
    }
}