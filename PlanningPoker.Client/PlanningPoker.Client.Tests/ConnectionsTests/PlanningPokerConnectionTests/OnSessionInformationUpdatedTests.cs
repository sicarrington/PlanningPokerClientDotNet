using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Moq;
using PlanningPoker.Client.Connections;
using PlanningPoker.Client.MessageFactories;
using PlanningPoker.Client.Messages;
using PlanningPoker.Client.Model;
using PlanningPoker.Client.Services;
using PlanningPoker.Client.Utilities;
using Xunit;

namespace PlanningPoker.Client.Tests.ConnectionsTests.PlanningPokerConnectionTests
{
    public class OnSessionInformationUpdatedTests
    {
        private readonly Mock<IOptions<PokerConnectionSettings>> _options;
        private readonly Mock<PokerConnectionSettings> _connectionSettings;
        private readonly Mock<IResponseMessageParser> _responseMessageParser;
        private readonly Mock<IPokerConnection> _pokerConnection;
        private readonly Mock<UserCacheProvider> _userCacheProvider;
        private readonly PlanningPokerConnection _planningPokerConnection;
        private readonly Mock<IPlanningPokerService> _planningPokerService;
        private readonly Mock<ILogger<PlanningPokerConnection>> _logger;

        string _expectedSessionId = "12345";
        Action<string> _callbackMethod = null;

        public OnSessionInformationUpdatedTests()
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

            _responseMessageParser.Setup(x => x.Get(It.IsAny<string>())).Returns(new RefreshSessionResponse(_expectedSessionId));

            _pokerConnection.Setup(x => x.Initialize(It.IsAny<Action<string>>(), It.IsAny<Action>(), It.IsAny<CancellationToken>()))
                .Callback<Action<string>, Action, CancellationToken>((success, error, cancel) => { _callbackMethod = success; })
                .Returns(Task.CompletedTask);
        }
        [Fact]
        public async void GivenConnectedSession_WhenConnectionRaisesRefreshEventAndSessionInformationIsNull_ThenSessioninformationIsRetrievedUsingSessionService()
        {
            _planningPokerConnection.OnSessionInformationUpdated((pokerSession) =>
               { });
            _planningPokerService.Setup(x => x.GetSessionDetails(_expectedSessionId)).Returns(Task.FromResult(new PokerSession()
            { }));

            await _planningPokerConnection.Start(CancellationToken.None);
            _callbackMethod($"PP 1.0\nMessageType:RefreshSession\nSuccess:true\nSessionId:{_expectedSessionId}");

            Thread.Sleep(500);

            _planningPokerService.Verify(x => x.GetSessionDetails(It.Is<string>(y => y == _expectedSessionId)), Times.Once);
        }
        [Fact]
        public async void GivenConnectedSession_WhenConnectionRaisesRefreshEvent_ThenUserCacheIsUpdatedWithUserDetails()
        {
            var userId = "918173748";
            var userToken = "AToken";
            var userName = "AUsser";
            var isHost = true;
            var isObserver = true;

            _userCacheProvider.Setup(x => x.GetUser(_expectedSessionId, userId)).Returns(Task.FromResult(new UserCacheItem
            {
                UserId = userId,
                SessionId = _expectedSessionId,
                Token = userToken
            }));
            _userCacheProvider.Setup(x => x.UpdateUser(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>())).Returns(Task.CompletedTask);

            _planningPokerConnection.OnSessionInformationUpdated((pokerSession) =>
            {
            });

            var pokerSessionUsers = new List<PokerSessionUser>();
            pokerSessionUsers.Add(new PokerSessionUser
            {
                Id = userId,
                SessionId = _expectedSessionId,
                Name = userName,
                IsHost = isHost,
                IsObserver = isObserver
            });

            var expectedPokerSession = new PokerSession()
            {
                SessionId = _expectedSessionId,
                Participants = pokerSessionUsers
            };
            var sessionJson = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(System.Text.Json.JsonSerializer.Serialize(expectedPokerSession, new System.Text.Json.JsonSerializerOptions
            {
                PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase
            })));

            _responseMessageParser.Setup(x => x.Get(It.IsAny<string>())).Returns(new RefreshSessionResponse(_expectedSessionId) { PokerSessionInformation = expectedPokerSession });

            await _planningPokerConnection.Start(CancellationToken.None);
            _callbackMethod($"PP 1.0\nMessageType:RefreshSession\nSuccess:true\nSessionId:{_expectedSessionId}\nSessionInformation:{sessionJson}\n");

            Thread.Sleep(500);

            _userCacheProvider.Verify(x => x.UpdateUser(
                It.Is<string>(y => y == _expectedSessionId),
                It.Is<string>(y => y == userId),
                It.Is<string>(y => y == userToken),
                It.Is<string>(y => y == userName),
                It.Is<bool>(y => y == isHost),
                It.Is<bool>(y => y == isObserver)), Times.Once);
        }
        [Fact]
        public async void GivenConnectedSession_WhenConnectionRaisesRefreshEvent_ThenNewSessionInformationIsRaised()
        {
            var callbackHappened = false;

            _userCacheProvider.Setup(x => x.UpdateUser(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>())).Returns(Task.CompletedTask);

            _planningPokerConnection.OnSessionInformationUpdated((pokerSession) =>
            {
                callbackHappened = true;
            });
            _planningPokerService.Setup(x => x.GetSessionDetails(_expectedSessionId)).Returns(Task.FromResult(new PokerSession()
            {

            }));

            await _planningPokerConnection.Start(CancellationToken.None);
            _callbackMethod($"PP 1.0\nMessageType:RefreshSession\nSuccess:true\nSessionId:{_expectedSessionId}");

            Thread.Sleep(500);

            Assert.True(callbackHappened);
        }
    }
}