using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace PlanningPoker.Client.Connections
{
    internal sealed class PlanningPokerSocket : IPokerConnection
    {
        private const int ReceiveChunkSize = 1024;
        private ClientWebSocket _websocket;
        private PokerConnectionSettings _connectionSettings;
        private CancellationToken _cancellationToken;
        private readonly ILogger<PlanningPokerSocket> _logger;

        public PlanningPokerSocket(IOptions<PokerConnectionSettings> connectionSettings, ILogger<PlanningPokerSocket> logger)
        {
            _connectionSettings = connectionSettings.Value;
            _websocket = new ClientWebSocket();
        }
        public async Task Initialize(Action<string> onMessageFromServer, Action onDisconnected, CancellationToken cancellationToken)
        {
            _cancellationToken = cancellationToken;
            _websocket = new ClientWebSocket();
            await _websocket.ConnectAsync(_connectionSettings.PlanningSocketUri, _cancellationToken);
            StartListen(onMessageFromServer, onDisconnected);
        }
        public async Task Send(string message)
        {
            EnsureConnection();
            await _websocket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(message)), WebSocketMessageType.Text, true, _cancellationToken);

        }
        public async Task Disconnect()
        {
            if (_websocket?.State == WebSocketState.Open)
            {
                try
                {
                    _logger.LogInformation("Disconnecting socket");
                    await _websocket.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, "User requested disconnect", _cancellationToken);
                }
                catch (Exception ex) {
                    _logger.LogError("Error closing socket", ex);
                }
            }
        }
        private void EnsureConnection()
        {
            if (_websocket.State != WebSocketState.Open)
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
                while (_websocket.State == WebSocketState.Open)
                {
                    var fullMessage = new StringBuilder();

                    WebSocketReceiveResult result;
                    do
                    {
                        result = await _websocket.ReceiveAsync(new ArraySegment<byte>(buffer), _cancellationToken);

                        if (result.MessageType == WebSocketMessageType.Close)
                        {
                            await
                                _websocket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
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
            catch (Exception ex)
            {
                _logger.LogError($"Error communicating with socket", ex);
                onDisconnected();
            }
            finally
            {
                _websocket.Dispose();
            }
        }
    }
}
