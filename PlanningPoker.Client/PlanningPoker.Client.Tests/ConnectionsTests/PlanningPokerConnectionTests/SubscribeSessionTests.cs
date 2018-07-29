using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebSockets;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;
using PlanningPoker.Client.Connections;
using PlanningPoker.Client.Exceptions;
using PlanningPoker.Client.MessageFactories;
using PlanningPoker.Client.Messages;
using PlanningPoker.Client.Model;
using PlanningPoker.Client.Utilities;
using Xunit;

namespace PlanningPoker.Client.Tests.ConnectionsTests
{
    public class SubscribeSessionTests
    {
        private Mock<IOptions<ConnectionSettings>> _options;
        private Mock<ConnectionSettings> _connectionSettings;
        private Mock<IResponseMessageParser> _responseMessageParser;
        private Mock<IPokerConnection> _pokerConnection;
        private Mock<UserCacheProvider> _userCacheProvider;
        private PlanningPokerConnection _planningPokerConnection;
        public SubscribeSessionTests()
        {
            _connectionSettings = new Mock<ConnectionSettings>();
            _options = new Mock<IOptions<ConnectionSettings>>();
            _options.Setup(x => x.Value).Returns(_connectionSettings.Object);
            _responseMessageParser = new Mock<IResponseMessageParser>();
            _pokerConnection = new Mock<IPokerConnection>();
            _userCacheProvider = new Mock<UserCacheProvider>();

            _planningPokerConnection = new PlanningPokerConnection(_options.Object, _responseMessageParser.Object,
                _pokerConnection.Object, _userCacheProvider.Object);
        }
        [Fact]
        public async void GivenSubscribeSessionIsCalled_WhenUserIdPassedIsNull_ThenExceptionIsThrown()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
          {
              await _planningPokerConnection.SubscribeSession(null, "ASession");
          });
        }
        [Fact]
        public async void GivenSubscribeSessionIsCalled_WhenUserIdPassedIsEmpty_ThenExceptionIsThrown()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
          {
              await _planningPokerConnection.SubscribeSession("", "ASession");
          });
        }
        [Fact]
        public async void GivenSubscribeSessionIsCalled_WhenSessionIdPassedIsNull_ThenExceptionIsThrown()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
          {
              await _planningPokerConnection.SubscribeSession("AUser", null);
          });
        }
        [Fact]
        public async void GivenSubscribeSessionIsCalled_WhenSessionIdPassedIsEmpty_ThenExceptionIsThrown()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
          {
              await _planningPokerConnection.SubscribeSession("AUser", "");
          });
        }
        [Fact]
        public async void GivenSubscribeSessionIsCalled_WhenSpecifiedUserSessionCombinationDoesNotExistInUserCache_ThenExceptionIsThrown()
        {
            var sessionId = "1234";
            var userId = "5678";
            _userCacheProvider.Setup(x => x.GetUser(sessionId, userId)).Throws(new NotFoundException("Bad things"));

            await Assert.ThrowsAsync<NotFoundException>(async () =>
            {
                await _planningPokerConnection.SubscribeSession(userId, sessionId);
            });
        }
        [Fact]
        public async void GivenSubscribeSessionIsCalled_WhenValidUserSessionIsSpecified_ThenSubscribeMessageIsSentToConnection()
        {
            var sessionId = "1234";
            var userId = "5678";
            var userToken = "0987654";
            var userName = "Fred";
            _userCacheProvider.Setup(x => x.GetUser(sessionId, userId)).ReturnsAsync(new UserCacheItem()
            {
                SessionId = sessionId,
                UserId = userId,
                UserName = userName,
                Token = userToken
            });
            _pokerConnection.Setup(x => x.Send(It.IsAny<string>())).Returns(Task.CompletedTask);

            await _planningPokerConnection.SubscribeSession(userId, sessionId);

            _pokerConnection.Verify(x => x.Send(It.Is<string>(
                y => y == $"PP 1.0\nMessageType:SubscribeMessage\nUserId:{userId}\nSessionId:{sessionId}\nToken:{userToken}"
            )), Times.Once);
        }
        [Fact]
        public async void GivenSubscribeSessionIsCalled_WhenConnectionReturnsSuccesfulResult_ThenSuccessCallbackIsInvoked()
        {
            var sessionId = "1234";
            var userId = "5678";
            var userToken = "0987654";
            var userName = "Fred";
            _userCacheProvider.Setup(x => x.GetUser(sessionId, userId)).ReturnsAsync(new UserCacheItem()
            {
                SessionId = sessionId,
                UserId = userId,
                UserName = userName,
                Token = userToken
            });
            _pokerConnection.Setup(x => x.Send(It.IsAny<string>())).Returns(Task.CompletedTask);

            Action<string> callbackMethod = null;
            _pokerConnection.Setup(x => x.Initialize(It.IsAny<Action<string>>(), It.IsAny<Action>(), It.IsAny<CancellationToken>()))
                .Callback<Action<string>, Action, CancellationToken>((success, error, cancel) => { callbackMethod = success; })
                .Returns(Task.CompletedTask);
            _pokerConnection.Setup(x => x.Send(It.IsAny<String>())).Returns(Task.CompletedTask);

            _responseMessageParser.Setup(x => x.Get(It.IsAny<string>())).Returns(new SubscribeSessionResponse(true, sessionId));

            var callbackHappened = false;
            _planningPokerConnection.OnSessionSubscribeSucceeded(() =>
                {
                    callbackHappened = true;
                });

            await _planningPokerConnection.Start(CancellationToken.None);
            await _planningPokerConnection.SubscribeSession(userId, sessionId);

            callbackMethod($"PP 1.0\nMessageType:SubscribeSessionResponse\nSuccess:true\nSessionId:{sessionId}");

            Thread.Sleep(500);

            Assert.True(callbackHappened);
        }
        [Fact]
        public async void GivenSubscribeSessionIsCalled_WhenConnectionReturnsFailureResult_ThenFailureCallbackIsInvoked()
        {
            var sessionId = "1234";
            var userId = "5678";
            var userToken = "0987654";
            var userName = "Fred";
            _userCacheProvider.Setup(x => x.GetUser(sessionId, userId)).ReturnsAsync(new UserCacheItem()
            {
                SessionId = sessionId,
                UserId = userId,
                UserName = userName,
                Token = userToken
            });
            _pokerConnection.Setup(x => x.Send(It.IsAny<string>())).Returns(Task.CompletedTask);

            Action<string> callbackMethod = null;
            _pokerConnection.Setup(x => x.Initialize(It.IsAny<Action<string>>(), It.IsAny<Action>(), It.IsAny<CancellationToken>()))
                .Callback<Action<string>, Action, CancellationToken>((success, error, cancel) => { callbackMethod = success; })
                .Returns(Task.CompletedTask);
            _pokerConnection.Setup(x => x.Send(It.IsAny<String>())).Returns(Task.CompletedTask);

            _responseMessageParser.Setup(x => x.Get(It.IsAny<string>())).Returns(new SubscribeSessionResponse(false, sessionId));

            var callbackHappened = false;
            _planningPokerConnection.OnSessionSubscribeFailed(() =>
                {
                    callbackHappened = true;
                });

            await _planningPokerConnection.Start(CancellationToken.None);
            await _planningPokerConnection.SubscribeSession(userId, sessionId);

            callbackMethod($"PP 1.0\nMessageType:SubscribeSessionResponse\nSuccess:false");

            Thread.Sleep(500);

            Assert.True(callbackHappened);
        }
    }
}