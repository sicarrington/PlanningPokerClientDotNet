using System;
using PlanningPoker.Client.Messages;
using PlanningPoker.Client.Utilities;

namespace PlanningPoker.Client.MessageFactories
{
    internal class SubscribeSessionResponseMessageFactory : IResponseMessageFactory
    {
        private MessageParser _messageParser;
        public SubscribeSessionResponseMessageFactory(MessageParser messageParser)
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
            var messageSuccesful = _messageParser.IsSuccessfulMessage(message);

            var sessionId = _messageParser.GetFieldFromMessage(message, "SessionId");
            if (string.IsNullOrWhiteSpace(sessionId))
            {
                throw new InvalidOperationException("SessionId is missing from message");
            }
            string errorMessage = null;
            if (!messageSuccesful)
            {
                errorMessage = _messageParser.GetFieldFromMessage(message, "ErrorMessage");
            }
            return new SubscribeSessionResponse(messageSuccesful, sessionId, errorMessage);
        }
    }
}