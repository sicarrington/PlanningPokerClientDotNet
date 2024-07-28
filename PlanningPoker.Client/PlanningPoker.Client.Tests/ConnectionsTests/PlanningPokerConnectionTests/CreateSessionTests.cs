using System;
using System.Threading;
using System.Threading.Tasks;
using Castle.Core.Logging;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using PlanningPoker.Client.Connections;
using PlanningPoker.Client.MessageFactories;
using PlanningPoker.Client.Messages;
using PlanningPoker.Client.Services;
using PlanningPoker.Client.Utilities;
using Xunit;

namespace PlanningPoker.Client.Tests.ConnectionsTests.PlanningPokerConnectionTests
{
    public class CreateSessionTests
    {
        private readonly Mock<IOptions<PokerConnectionSettings>> _options;
        private readonly Mock<PokerConnectionSettings> _connectionSettings;
        private readonly Mock<IResponseMessageParser> _responseMessageParser;
        private readonly Mock<IPokerConnection> _pokerConnection;
        private readonly Mock<UserCacheProvider> _userCacheProvider;
        private readonly PlanningPokerConnection _planningPokerConnection;
        private readonly Mock<IPlanningPokerService> _planningPokerService;
        private readonly Mock<ILogger<PlanningPokerConnection>> _logger;

        public CreateSessionTests()
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
        public async void GivenCreateSessionIsCalled_WhenHostNamePassedIsNull_ThenExceptionIsThrown()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await _planningPokerConnection.CreateSession(null);
            });
        }
        [Fact]
        public async void GivenCreateSessionIsCalled_WhenHostNamePassedIsEmpty_ThenExceptionIsThrown()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await _planningPokerConnection.CreateSession("");
            });
        }
        [Fact]
        public async void GivenCreateSessionIsCalled_WhenHostNameIsSpecified_ThenNewSessionMessageIsSent()
        {
            var userName = "ANewUser";

            await _planningPokerConnection.CreateSession(userName);

            _pokerConnection.Verify(x => x.Send("PP 1.0\nMessageType:NewSession\nUserName:" + userName), Times.Once);
        }
        [Fact]
        public async void GivenCreateSessionIsCalled_WhenConnectionSendsSuccesfulResponse_OnSuccessIsCalled()
        {
            var sessionId = "665330";
            var userId = "2db90720-f234-4ec6-88d7-56eeca3be56b";
            var userToken = "ABigToken";
            Action<string> callbackMethod = null;
            _pokerConnection.Setup(x => x.Initialize(It.IsAny<Action<string>>(), It.IsAny<Action>(), It.IsAny<CancellationToken>()))
                .Callback<Action<string>, Action, CancellationToken>((success, error, cancel) => { callbackMethod = success; })
                .Returns(Task.CompletedTask);

            _pokerConnection.Setup(x => x.Send(It.IsAny<String>())).Returns(Task.CompletedTask);

            _responseMessageParser.Setup(x => x.Get(It.IsAny<string>())).Returns(new NewSessionResponse(sessionId, userId, userToken));

            var callbackHappened = false;
            _planningPokerConnection.OnSessionSuccesfullyCreated((x) =>
                {
                    callbackHappened = true;
                });

            await _planningPokerConnection.Start(CancellationToken.None);
            await _planningPokerConnection.CreateSession("Fred");
            callbackMethod($"PP 1.0\nMessageType:NewSessionResponse\nSuccess:true\nSessionId:{sessionId}\nUserId:{userId}\nToken:{userToken}");

            Thread.Sleep(1000);

            Assert.True(callbackHappened);
        }
        [Fact]
        public async void GivenCreateSessionIsCalled_WhenConnectionSendsBadResult_OnErrorIsCalled()
        {
            Action<string> callbackMethod = null;
            _pokerConnection.Setup(x => x.Initialize(It.IsAny<Action<string>>(), It.IsAny<Action>(), It.IsAny<CancellationToken>()))
                .Callback<Action<string>, Action, CancellationToken>((success, error, cancel) => { callbackMethod = success; })
                .Returns(Task.CompletedTask);

            _pokerConnection.Setup(x => x.Send(It.IsAny<String>())).Returns(Task.CompletedTask);

            _responseMessageParser.Setup(x => x.Get(It.IsAny<string>())).Returns(new NewSessionResponse());

            var callbackHappened = false;
            _planningPokerConnection.OnSessionCreationFailed(() =>
                {
                    callbackHappened = true;
                });

            await _planningPokerConnection.Start(CancellationToken.None);
            await _planningPokerConnection.CreateSession("Fred");
            callbackMethod($"PP 1.0\nMessageType:NewSessionResponse\nSuccess:false\n");

            Thread.Sleep(500);

            Assert.True(callbackHappened);
        }
        [Fact]
        public async void GivenCreateSessionIsCalled_WhenConnectionReturnsSuccess_ThenUserIsAddedToCache()
        {
            var sessionId = "665330";
            var userId = "2db90720-f234-4ec6-88d7-56eeca3be56b";
            var userToken = "ABigToken";
            Action<string> callbackMethod = null;
            _pokerConnection.Setup(x => x.Initialize(It.IsAny<Action<string>>(), It.IsAny<Action>(), It.IsAny<CancellationToken>()))
                .Callback<Action<string>, Action, CancellationToken>((success, error, cancel) => { callbackMethod = success; })
                .Returns(Task.CompletedTask);
            _pokerConnection.Setup(x => x.Send(It.IsAny<String>())).Returns(Task.CompletedTask);
            _responseMessageParser.Setup(x => x.Get(It.IsAny<string>())).Returns(new NewSessionResponse(sessionId, userId, userToken));

            _userCacheProvider.Setup(x => x.AddUser(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);

            await _planningPokerConnection.Start(CancellationToken.None);
            await _planningPokerConnection.CreateSession("Fred");
            callbackMethod($"PP 1.0\nMessageType:NewSessionResponse\nSuccess:true\nSessionId:{sessionId}\nUserId:{userId}\nToken:{userToken}");

            Thread.Sleep(500);

            _userCacheProvider.Verify(x => x.AddUser(
                It.Is<string>(y => y == sessionId),
                It.Is<string>(y => y == userId),
                It.Is<string>(y => y == userToken)
            ), Times.Once);
        }
    }
}