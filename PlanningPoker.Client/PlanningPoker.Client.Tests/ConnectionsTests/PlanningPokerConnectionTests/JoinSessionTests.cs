using System;
using System.Threading;
using System.Threading.Tasks;
using Castle.Core.Logging;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using PlanningPoker.Client.Connections;
using PlanningPoker.Client.Exceptions;
using PlanningPoker.Client.MessageFactories;
using PlanningPoker.Client.Messages;
using PlanningPoker.Client.Model;
using PlanningPoker.Client.Services;
using PlanningPoker.Client.Utilities;
using Xunit;

namespace PlanningPoker.Client.Tests.ConnectionsTests.PlanningPokerConnectionTests
{
    public class JoinSessionTests
    {
        private readonly Mock<IOptions<PokerConnectionSettings>> _options;
        private readonly Mock<PokerConnectionSettings> _connectionSettings;
        private readonly Mock<IResponseMessageParser> _responseMessageParser;
        private readonly Mock<IPokerConnection> _pokerConnection;
        private readonly Mock<UserCacheProvider> _userCacheProvider;
        private readonly PlanningPokerConnection _planningPokerConnection;
        private readonly Mock<IPlanningPokerService> _planningPokerService;
        private readonly Mock<ILogger<PlanningPokerConnection>> _logger;

        public JoinSessionTests()
        {
            _connectionSettings = new Mock<PokerConnectionSettings>();
            _options = new Mock<IOptions<PokerConnectionSettings>>();
            _options.Setup(x => x.Value).Returns(_connectionSettings.Object);
            _responseMessageParser = new Mock<IResponseMessageParser>();
            _pokerConnection = new Mock<IPokerConnection>();
            _userCacheProvider = new Mock<UserCacheProvider>();
            _planningPokerService = new Mock<IPlanningPokerService>();
            _logger = new Mock<ILogger<PlanningPokerConnection>>();

            _planningPokerConnection = new PlanningPokerConnection(_options.Object, _responseMessageParser.Object,
                _pokerConnection.Object, _userCacheProvider.Object, _planningPokerService.Object, _logger.Object);
        }
        [Fact]
        public async void GivenJoinSessionIsCalled_WhenSessionIdIsNull_ThenExceptinoIsThrown()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
          {
              await _planningPokerConnection.JoinSession(null, "AUser");
          });
        }
        [Fact]
        public async void GivenJoinSessionIsCalled_WhenSessionIdIsEmpty_ThenExceptinoIsThrown()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
          {
              await _planningPokerConnection.JoinSession("", "AUser");
          });
        }
        [Fact]
        public async void GivenJoinSessionIsCalled_WhenUserNamesNull_ThenExceptinoIsThrown()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
          {
              await _planningPokerConnection.JoinSession("SessionId", null);
          });
        }
        [Fact]
        public async void GivenJoinSessionIsCalled_WhenUserNameIsEmpty_ThenExceptinoIsThrown()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
          {
              await _planningPokerConnection.JoinSession("SessionId", "");
          });
        }
        [Fact]
        public async void GivenJoinSessionIsCalled_WhenValidUserSessionIsSpecified_ThenSubscribeMessageIsSentToConnection()
        {
            var sessionId = "1234";
            var userName = "username123";
            _pokerConnection.Setup(x => x.Send(It.IsAny<string>())).Returns(Task.CompletedTask);

            await _planningPokerConnection.JoinSession(sessionId, userName);

            _pokerConnection.Verify(x => x.Send(It.Is<string>(
                y => y == $"PP 1.0\nMessageType:JoinSession\nUserName:{userName}\nSessionId:{sessionId}\nIsObserver:false"
                )), Times.Once);
        }
        [Fact]
        public async void GivenJoinSessionIsCalled_WhenConnectionReturnsSuccesfulResult_ThenSuccessCallbackIsInvoked()
        {
            var sessionId = "12345";
            var userId = "98876";
            var userToken = "8482910";

            Action<string> callbackMethod = null;
            _pokerConnection.Setup(x => x.Initialize(It.IsAny<Action<string>>(), It.IsAny<Action>(), It.IsAny<CancellationToken>()))
                .Callback<Action<string>, Action, CancellationToken>((success, error, cancel) => { callbackMethod = success; })
                .Returns(Task.CompletedTask);

            _pokerConnection.Setup(x => x.Send(It.IsAny<String>())).Returns(Task.CompletedTask);

            _responseMessageParser.Setup(x => x.Get(It.IsAny<string>())).Returns(new JoinSessionResponse(sessionId, userId, userToken));

            var callbackHappened = false;
            _planningPokerConnection.OnJoinSessionSucceeded(() =>
                {
                    callbackHappened = true;
                });

            await _planningPokerConnection.Start(CancellationToken.None);
            await _planningPokerConnection.JoinSession(sessionId, "Fred");
            callbackMethod($"PP 1.0\nMessageType:JoinSessionResponse\nSuccess:true\nSessionId:{sessionId}\nUserId:{userId}\nUserToken:{userToken}\n");

            Thread.Sleep(1000);

            Assert.True(callbackHappened);
        }
        [Fact]
        public async void GivenJoinSessionIsCalled_WhenConnectionReturnsFailureResult_ThenFailureCallbackIsInvoked()
        {
            var sessionId = "12345";
            var errorMessage = "Bad Things";

            Action<string> callbackMethod = null;
            _pokerConnection.Setup(x => x.Initialize(It.IsAny<Action<string>>(), It.IsAny<Action>(), It.IsAny<CancellationToken>()))
                .Callback<Action<string>, Action, CancellationToken>((success, error, cancel) => { callbackMethod = success; })
                .Returns(Task.CompletedTask);

            _pokerConnection.Setup(x => x.Send(It.IsAny<String>())).Returns(Task.CompletedTask);

            _responseMessageParser.Setup(x => x.Get(It.IsAny<string>())).Returns(new JoinSessionResponse(sessionId, errorMessage));

            var callbackHappened = false;
            _planningPokerConnection.OnJoinSessionFailed(() =>
                {
                    callbackHappened = true;
                });

            await _planningPokerConnection.Start(CancellationToken.None);
            await _planningPokerConnection.JoinSession(sessionId, "Fred");
            callbackMethod($"PP 1.0\nMessageType:JoinSessionResponse\nSuccess:false\nErrorMessage:{errorMessage}");

            Thread.Sleep(500);

            Assert.True(callbackHappened);
        }
        [Fact]
        public async void GivenJoinSessionIsCalled_WhenConnectionReturnsSuccess_ThenUserIsAddedToCache()
        {
            var sessionId = "665330";
            var userId = "2db90720-f234-4ec6-88d7-56eeca3be56b";
            var userToken = "ABigToken";
            Action<string> callbackMethod = null;
            _pokerConnection.Setup(x => x.Initialize(It.IsAny<Action<string>>(), It.IsAny<Action>(), It.IsAny<CancellationToken>()))
                .Callback<Action<string>, Action, CancellationToken>((success, error, cancel) => { callbackMethod = success; })
                .Returns(Task.CompletedTask);
            _pokerConnection.Setup(x => x.Send(It.IsAny<String>())).Returns(Task.CompletedTask);
            _responseMessageParser.Setup(x => x.Get(It.IsAny<string>())).Returns(new JoinSessionResponse(sessionId, userId, userToken));

            _userCacheProvider.Setup(x => x.AddUser(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);

            await _planningPokerConnection.Start(CancellationToken.None);
            await _planningPokerConnection.CreateSession("Fred");
            callbackMethod($"PP 1.0\nMessageType:JoinSessionResponse\nSuccess:true\nSessionId:{sessionId}\nUserId:{userId}\nToken:{userToken}");

            Thread.Sleep(500);

            _userCacheProvider.Verify(x => x.AddUser(
                It.Is<string>(y => y == sessionId),
                It.Is<string>(y => y == userId),
                It.Is<string>(y => y == userToken)
            ), Times.Once);
        }
    }
}