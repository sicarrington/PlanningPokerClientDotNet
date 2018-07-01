using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace PlanningPoker.Client.Connections
{
    internal sealed class PlanningPokerSocket : IPokerConnection
    {
        private const int ReceiveChunkSize = 1024;
        private ClientWebSocket _planningConnection;
        private ConnectionSettings _connectionSettings;
        private CancellationToken _cancellationToken;
        public PlanningPokerSocket(IOptions<ConnectionSettings> connectionSettings)
        {
            _connectionSettings = connectionSettings.Value;
            _planningConnection = new ClientWebSocket();
        }
        public async Task Initialize(Action<string> onMessageFromServer, Action onDisconnected, CancellationToken cancellationToken)
        {
            _cancellationToken = cancellationToken;
            _planningConnection = new ClientWebSocket();
            await _planningConnection.ConnectAsync(_connectionSettings.PlanningSocketUri, _cancellationToken);
            StartListen(onMessageFromServer, onDisconnected);
        }
        public async Task Send(string message)
        {
            EnsureConnection();
            await _planningConnection.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(message)), WebSocketMessageType.Text, true, _cancellationToken);

        }
        private void EnsureConnection()
        {
            if (_planningConnection.State != WebSocketState.Open)
            {
                throw new InvalidOperationException("Socket is not open");
            }
            if (_cancellationToken.IsCancellationRequested)
            {
                throw new InvalidOperationException("Cancellation has been requested");
            }
        }
        private async void StartListen(Action<string> onMessageFromServer, Action onDisconnected)
        {
            var buffer = new byte[ReceiveChunkSize];

            try
            {
                while (_planningConnection.State == WebSocketState.Open)
                {
                    var fullMessage = new StringBuilder();

                    WebSocketReceiveResult result;
                    do
                    {
                        result = await _planningConnection.ReceiveAsync(new ArraySegment<byte>(buffer), _cancellationToken);

                        if (result.MessageType == WebSocketMessageType.Close)
                        {
                            await
                                _planningConnection.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
                        }
                        else
                        {
                            var messagePart = Encoding.UTF8.GetString(buffer, 0, result.Count);
                            fullMessage.Append(messagePart);
                        }
                    } while (!result.EndOfMessage);

                    onMessageFromServer(fullMessage.ToString());
                }
            }
            catch (Exception)
            {
                //CallOnDisconnected();
                onDisconnected();
            }
            finally
            {
                _planningConnection.Dispose();
            }
        }
    }
}
