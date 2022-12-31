using System;
using PlanningPoker.Client.Messages;
using PlanningPoker.Client.Model;
using PlanningPoker.Client.Utilities;

namespace PlanningPoker.Client.MessageFactories
{
    internal class RefreshSessionMessageFactory : IResponseMessageFactory
    {
        private MessageParser _messageParser;
        public RefreshSessionMessageFactory(MessageParser messageParser)
        {
            _messageParser = messageParser;
        }
        public ResponseMessage Get(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentNullException(nameof(message));
            }
            var messageType = _messageParser.GetTypeOfMessage(message);
            if (messageType != ResponseMessageType.RefreshSession)
            {
                throw new InvalidOperationException($"{this.GetType()} cannot proccess message of type {messageType}");
            }
            var messageSuccesful = _messageParser.IsSuccessfulMessage(message);
            if (messageSuccesful)
            {
                var sessionId = _messageParser.GetFieldFromMessage(message, "SessionId");
                if (string.IsNullOrWhiteSpace(sessionId))
                {
                    throw new InvalidOperationException("SessionId is missing from message");
                }

                var refreshMessage = new RefreshSessionResponse(sessionId);

                var sessionInformation = _messageParser.GetFieldFromMessage(message, "SessionInformation");
                if (!string.IsNullOrWhiteSpace(sessionInformation))
                {
                    var deserialized = System.Text.Json.JsonSerializer.Deserialize<PokerSession>(Convert.FromBase64String(sessionInformation),
                        new System.Text.Json.JsonSerializerOptions
                    {
                        PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase,
                        PropertyNameCaseInsensitive = true
                    });

                    refreshMessage.PokerSessionInformation = deserialized;
                }
                return refreshMessage;
            }
            else
            {
                throw new InvalidOperationException("Unexpected unsuccesful refresh message");
            }
        }
    }
}