using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebSockets;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using PlanningPoker.Client.Connections;
using PlanningPoker.Client.MessageFactories;
using PlanningPoker.Client.Messages;
using PlanningPoker.Client.Model;
using PlanningPoker.Client.Utilities;

namespace PlanningPoker.Client
{
    public sealed class PlanningPokerConnection
    {
        private ConnectionSettings _planningSettings;
        private CancellationToken _cancellationToken;
        private IResponseMessageParser _responseMessageParser;
        private IPokerConnection _pokerConnection;

        private Action<Exception> _onError;
        private Action _onDisconnected;
        private Action<(string sessionId, string userId, string userToken)> _onSessionCreationSucceeded;
        private Action _onSessionCreationFailed;

        internal PlanningPokerConnection(IOptions<ConnectionSettings> connectionSettings,
            IResponseMessageParser responseMessageFactory, IPokerConnection pokerConnection)
        {
            _planningSettings = connectionSettings.Value;
            _responseMessageParser = responseMessageFactory;
            _pokerConnection = pokerConnection;
        }

        public Task Start(CancellationToken cancellationToken)
        {
            _cancellationToken = cancellationToken;
            return _pokerConnection.Initialize(ProcessMessageFromServer, _onDisconnected, cancellationToken);
        }

        public async Task CreateSession(string hostName)
        {
            await _pokerConnection.Send("PP 1.0\nMessageType:NewSession\nUserName:" + hostName);
        }

        private void ProcessMessageFromServer(string message)
        {
            try
            {
                var parsedMessage = _responseMessageParser.Get(message);

                if (parsedMessage is NewSessionResponse)
                {
                    var typedMessage = parsedMessage as NewSessionResponse;
                    if (typedMessage.Success)
                    {
                        if (_onSessionCreationSucceeded != null)
                        {
                            RunInTask(() => _onSessionCreationSucceeded((
                                typedMessage.SessionId,
                                typedMessage.UserId,
                                typedMessage.UserToken
                            )));
                        }
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
        private static void RunInTask(Action action)
        {
            Task.Factory.StartNew(action);
        }

        public PlanningPokerConnection OnError(Action<Exception> onError)
        {
            _onError = onError;
            return this;
        }
        public PlanningPokerConnection OnSessionSuccesfullyCreated(Action<(string sessionId, string userId, string userToken)> onSessionSuccesfullyCreated)
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
    }
}