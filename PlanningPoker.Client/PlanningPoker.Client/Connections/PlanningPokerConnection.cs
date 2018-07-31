using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebSockets;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using PlanningPoker.Client.Connections;
using PlanningPoker.Client.Exceptions;
using PlanningPoker.Client.MessageFactories;
using PlanningPoker.Client.Messages;
using PlanningPoker.Client.Model;
using PlanningPoker.Client.Services;
using PlanningPoker.Client.Utilities;

namespace PlanningPoker.Client.Connections
{
    public sealed class PlanningPokerConnection
    {
        private ConnectionSettings _planningSettings;
        private CancellationToken _cancellationToken;
        private IResponseMessageParser _responseMessageParser;
        private IPokerConnection _pokerConnection;
        private UserCacheProvider _userCacheProvider;
        private IPlanningPokerService _planningPokerService;

        private Action<Exception> _onError;
        private Action _onDisconnected;
        private Action<(string sessionId, string userId)> _onSessionCreationSucceeded;
        private Action _onSessionCreationFailed;
        private Action _onSessionSubscribeSucceeded;
        private Action _onSessionSubscribeFailed;
        private Action _onJoinSessionSucceeded;
        private Action _onJoinSessionFailed;
        private Action<PokerSession> _onSessionInformationUpdated;

        internal PlanningPokerConnection(IOptions<ConnectionSettings> connectionSettings,
            IResponseMessageParser responseMessageParser, IPokerConnection pokerConnection,
            UserCacheProvider userCacheProvider, IPlanningPokerService planningPokerService)
        {
            _planningSettings = connectionSettings.Value;
            _responseMessageParser = responseMessageParser;
            _pokerConnection = pokerConnection;
            _userCacheProvider = userCacheProvider;
            _planningPokerService = planningPokerService;
        }

        public Task Start(CancellationToken cancellationToken)
        {
            _cancellationToken = cancellationToken;
            return _pokerConnection.Initialize(ProcessMessageFromServer, _onDisconnected, cancellationToken);
        }

        public async Task CreateSession(string hostName)
        {
            if (string.IsNullOrWhiteSpace(hostName))
            {
                throw new ArgumentNullException(nameof(hostName));
            }
            await _pokerConnection.Send("PP 1.0\nMessageType:NewSession\nUserName:" + hostName);
        }

        public async Task SubscribeSession(string userId, string sessionId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentNullException(nameof(userId));
            }
            if (string.IsNullOrWhiteSpace(sessionId))
            {
                throw new ArgumentNullException(nameof(sessionId));
            }
            var userDetails = await _userCacheProvider.GetUser(sessionId, userId);
            await _pokerConnection.Send($"PP 1.0\nMessageType:SubscribeMessage\nUserId:{userId}\nSessionId:{sessionId}\nToken:{userDetails.Token}");
        }
        public async Task JoinSession(string sessionId, string userName)
        {
            if (string.IsNullOrWhiteSpace(sessionId))
            {
                throw new ArgumentNullException(nameof(sessionId));
            }
            if (string.IsNullOrWhiteSpace(userName))
            {
                throw new ArgumentNullException(nameof(userName));
            }
            await _pokerConnection.Send($"PP 1.0\nMessageType:JoinSession\nUserName:{userName}\nSessionId:{sessionId}\nIsObserver:false");
        }

        private async void ProcessMessageFromServer(string message)
        {
            try
            {
                var parsedMessage = _responseMessageParser.Get(message);

                if (parsedMessage is NewSessionResponse)
                {
                    var typedMessage = parsedMessage as NewSessionResponse;
                    if (typedMessage.Success)
                    {
                        await _userCacheProvider.AddUser(typedMessage.SessionId, typedMessage.UserId, typedMessage.UserToken);
                        if (_onSessionCreationSucceeded != null)
                        {
                            RunInTask(() => _onSessionCreationSucceeded((
                                typedMessage.SessionId,
                                typedMessage.UserId
                            )));
                        }
                    }
                    else
                    {
                        if (_onSessionCreationFailed != null)
                        {
                            RunInTask(() => _onSessionCreationFailed());
                        }
                    }
                }
                else if (parsedMessage is SubscribeSessionResponse)
                {
                    var typedMessage = parsedMessage as SubscribeSessionResponse;
                    if (typedMessage.Success)
                    {
                        if (_onSessionSubscribeSucceeded != null)
                        {
                            RunInTask(() => _onSessionSubscribeSucceeded());
                        }
                    }
                    else
                    {
                        if (_onSessionSubscribeFailed != null)
                        {
                            RunInTask(() => _onSessionSubscribeFailed());
                        }
                    }
                }
                else if (parsedMessage is JoinSessionResponse)
                {
                    var typedMessage = parsedMessage as JoinSessionResponse;
                    if (typedMessage.Success)
                    {
                        await _userCacheProvider.AddUser(typedMessage.SessionId, typedMessage.UserId, typedMessage.UserToken);
                        if (_onJoinSessionSucceeded != null)
                        {
                            RunInTask(() => _onJoinSessionSucceeded());
                        }
                    }
                    else
                    {
                        if (_onJoinSessionFailed != null)
                        {
                            RunInTask(() => _onJoinSessionFailed());
                        }
                    }
                }
                else if (parsedMessage is RefreshSessionResponse)
                {
                    var typedMessage = parsedMessage as RefreshSessionResponse;
                    var sessionInformation = await _planningPokerService.GetSessionDetails(typedMessage.SessionId);

                    await UpdateCachedUserDetails(sessionInformation);

                    if (_onSessionInformationUpdated != null)
                    {

                        RunInTask(() => _onSessionInformationUpdated(sessionInformation));
                    }
                }
            }
            catch (InvalidOperationException ex)
            {
                if (_onError != null)
                {
                    RunInTask(() => _onError(ex));
                }
            }
        }
        private async Task UpdateCachedUserDetails(PokerSession sessionInformation)
        {
            if (sessionInformation?.Participants != null)
            {
                foreach (var sessionUser in sessionInformation.Participants)
                {
                    try
                    {
                        var cachedUser = await _userCacheProvider.GetUser(sessionInformation.SessionId, sessionUser.Id);
                        await _userCacheProvider.UpdateUser(sessionInformation.SessionId, sessionUser.Id, cachedUser.Token,
                            sessionUser.Name, sessionUser.IsHost, sessionUser.IsObserver);
                    }
                    catch (NotFoundException e)
                    {
                        //User is not in cache, we don't want to update
                    }
                }
            }
        }
        private static void RunInTask(Action action)
        {
            Task.Factory.StartNew(action);
        }

        public PlanningPokerConnection OnError(Action<Exception> onError)
        {
            _onError = onError;
            return this;
        }
        public PlanningPokerConnection OnSessionSuccesfullyCreated(Action<(string sessionId, string userId)> onSessionSuccesfullyCreated)
        {
            _onSessionCreationSucceeded = onSessionSuccesfullyCreated;
            return this;
        }
        public PlanningPokerConnection OnSessionCreationFailed(Action onsessionCreationFailed)
        {
            _onSessionCreationFailed = onsessionCreationFailed;
            return this;
        }
        public PlanningPokerConnection OnDisconnected(Action onDisconnected)
        {
            _onDisconnected = onDisconnected;
            return this;
        }
        public PlanningPokerConnection OnSessionSubscribeSucceeded(Action sessionSubscribeSucceeded)
        {
            _onSessionSubscribeSucceeded = sessionSubscribeSucceeded;
            return this;
        }
        public PlanningPokerConnection OnSessionSubscribeFailed(Action sessionSubscribeFailed)
        {
            _onSessionSubscribeFailed = sessionSubscribeFailed;
            return this;
        }
        public PlanningPokerConnection OnJoinSessionSucceeded(Action joinSessionSuceeded)
        {
            _onJoinSessionSucceeded = joinSessionSuceeded;
            return this;
        }
        public PlanningPokerConnection OnJoinSessionFailed(Action joinSessionFailed)
        {
            _onJoinSessionFailed = joinSessionFailed;
            return this;
        }
        public PlanningPokerConnection OnSessionInformationUpdated(Action<PokerSession> sessionInformationUpdated)
        {
            _onSessionInformationUpdated = sessionInformationUpdated;
            return this;
        }
    }
}