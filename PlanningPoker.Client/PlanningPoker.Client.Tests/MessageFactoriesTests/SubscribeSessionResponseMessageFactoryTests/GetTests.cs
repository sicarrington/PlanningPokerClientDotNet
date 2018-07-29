using System;
using PlanningPoker.Client.MessageFactories;
using PlanningPoker.Client.Messages;
using PlanningPoker.Client.Utilities;
using Xunit;

namespace PlanningPoker.Client.Tests.MessageFactoriesTests.SubscribeSessionResponseMessageFactoryTests
{
    public class GetTests
    {
        SubscribeSessionResponseMessageFactory _subscribeSessionResponseMessageFactory;
        MessageParser _messageParser;

        public GetTests()
        {
            _messageParser = new MessageParser();
            _subscribeSessionResponseMessageFactory = new SubscribeSessionResponseMessageFactory(_messageParser);
        }
        [Fact]
        public void GivenGetIsCalled_WhenMessagePassedIsNull_ThenExceptionIsThrown()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                _subscribeSessionResponseMessageFactory.Get(null);
            });
        }
        [Fact]
        public void GivenGetIsCalled_WhenMessagePassedIsEmpty_ThenExceptionIsThrown()
        {
            Assert.Throws<ArgumentNullException>(() =>
                        {
                            _subscribeSessionResponseMessageFactory.Get("");
                        });
        }
        [Fact]
        public void GivenGetIsCalled_WhenMessagePassedIsNotSubScribeSessionResponse_ThenExceptionIsThrown()
        {
            Assert.Throws<InvalidOperationException>(() =>
                        {
                            _subscribeSessionResponseMessageFactory.Get("Nonesense");
                        });
        }
        [Fact]
        public void GivenGetIsCalled_WhenMessagePassedDoesNotContainSuccessField_ThenExceptionIsThrown()
        {
            Assert.Throws<InvalidOperationException>(() =>
                        {
                            _subscribeSessionResponseMessageFactory.Get("PP 1.0\nMessageType:SubscribeSessionResponse\n");
                        });
        }
        [Fact]
        public void GivenGetIsCalled_WhenMessagePassedContainsSuccessFieldWithInvalidValue_THenExceptionIsThrown()
        {
            Assert.Throws<InvalidOperationException>(() =>
                        {
                            _subscribeSessionResponseMessageFactory.Get("PP 1.0\nMessageType:SubscribeSessionResponse\nSuccess:wibble\n");
                        });
        }
        [Fact]
        public void GivenGetIsCalled_WhenMessageIndicatesSuccessButIsMissingSessionId_ThenExceptionIsThrown()
        {
            Assert.Throws<InvalidOperationException>(() =>
                        {
                            _subscribeSessionResponseMessageFactory.Get("PP 1.0\nMessageType:SubscribeSessionResponse\nSuccess:true\n");
                        });
        }
        [Fact]
        public void GivenGetIsCalled_WhenMessageIsValidSuccessMessage_ThenMessageIsReturnedCorrectly()
        {
            var sessionId = "ABC123";

            var result = _subscribeSessionResponseMessageFactory.Get($"PP 1.0\nMessageType:SubscribeSessionResponse\nSuccess:true\nSessionId:{sessionId}");

            var subscribeSessionResult = result as SubscribeSessionResponse;
            Assert.NotNull(subscribeSessionResult);
            Assert.True(subscribeSessionResult.Success);
            Assert.Equal(sessionId, subscribeSessionResult.SessionId);
            Assert.Null(subscribeSessionResult.ErrorMessage);
        }
        [Fact]
        public void GivenGetIsCalled_WhenMessageIsValidFailureMessage_ThenMessageISReturnedCorrectly()
        {
            var sessionId = "ABC123";
            var errorMessage = "Bad stuff happened";

            var result = _subscribeSessionResponseMessageFactory.Get($"PP 1.0\nMessageType:SubscribeSessionResponse\nSuccess:false\nSessionId:{sessionId}\nErrorMessage:{errorMessage}");

            var subscribeSessionResult = result as SubscribeSessionResponse;
            Assert.NotNull(subscribeSessionResult);
            Assert.False(subscribeSessionResult.Success);
            Assert.Equal(sessionId, subscribeSessionResult.SessionId);
            Assert.Equal(errorMessage, subscribeSessionResult.ErrorMessage);
        }
    }
}