using System;
using PlanningPoker.Client.Utilities;
using Xunit;

namespace PlanningPoker.Client.Tests.UtilitiesTests.MessageParserTests
{
    public class GetFieldFromMessageTests
    {
        MessageParser _messageParser;
        public GetFieldFromMessageTests()
        {
            _messageParser = new MessageParser();
        }
        [Fact]
        public void GivenGetFieldFromMessageIsCalled_WhenMessagePassedIsNull_ThenExceptionIsThrown()
        {
            Assert.Throws<ArgumentNullException>(()=>{
                _messageParser.GetFieldFromMessage(null, "AField");
            });
        }
        [Fact]
        public void GivenGetFieldFromMessageIsCalled_WhenMessagePassedIsEmpty_ThenExceptionIsThrown()
        {
            Assert.Throws<ArgumentNullException>(()=>{
                _messageParser.GetFieldFromMessage("", "AField");
            });
        }
        [Fact]
        public void GivenGetFieldFromMessageIsCalled_WhenFieldPassedIsNull_ThenExceptionIsThrown()
        {
            Assert.Throws<ArgumentNullException>(()=>{
                _messageParser.GetFieldFromMessage("AMessage", null);
            });
        }
        [Fact]
        public void GivenGetFieldFromMessageIsCalled_WhenFieldPassedIsEmpty_ThenExceptionIsThrown()
        {
            Assert.Throws<ArgumentNullException>(()=>{
                _messageParser.GetFieldFromMessage("AMessage", "");
            });
        }
        [Fact]
        public void GivenGetFieldFromMessageIsCalled_WhenFieldIsMissingFromMessage_ThenNullIsReturned()
        {
            var result = 
                _messageParser.GetFieldFromMessage("MessageType:NewSessionResponse\n", "FieldA");
            
            Assert.Null(result);
        } 
        [Fact]
        public void GivenGetFieldFromMessageIsCalled_WhenFieldIsPresentButDoesNotHaveValue_ThenNullIsReturned()
        {
            var result = 
                _messageParser.GetFieldFromMessage("MessageType:NewSessionResponse\nFieldA:\n", "FieldA");
            
            Assert.Null(result);
        }
        [Fact]
        public void GivenGetFieldIsFromMessageIsCalled_WhenFieldIsPresentWIthValue_ThenValueIsReturned()
        {
            var result = 
                _messageParser.GetFieldFromMessage("MessageType:NewSessionResponse\nFieldA:AValue\n", "FieldA");
            
            Assert.Equal("AValue", result);
        }
    }
}