using System;
using PlanningPoker.Client.Messages;
using PlanningPoker.Client.Model;
using PlanningPoker.Client.Utilities;

namespace PlanningPoker.Client.MessageFactories
{
    internal class JoinSessionResponseMessageFactory : IResponseMessageFactory
    {
        private MessageParser _messageParser;
        public JoinSessionResponseMessageFactory(MessageParser messageParser)
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
            if (messageType != ResponseMessageType.JoinSessionResponse)
            {
                throw new InvalidOperationException($"{this.GetType()} cannot proccess message of type {messageType}");
            }

            var sessionId = _messageParser.GetFieldFromMessage(message, "SessionId");
            if (string.IsNullOrWhiteSpace(sessionId))
            {
                throw new InvalidOperationException("SessionId is missing from message");
            }

            var messageSuccesful = _messageParser.IsSuccessfulMessage(message);
            if (messageSuccesful)
            {
                var userId = _messageParser.GetFieldFromMessage(message, "UserId");
                if (string.IsNullOrWhiteSpace(userId))
                {
                    throw new InvalidOperationException("UserId is missing from message");
                }
                var userToken = _messageParser.GetFieldFromMessage(message, "Token");
                if (string.IsNullOrWhiteSpace(userToken))
                {
                    throw new InvalidOperationException("UserToken is missing from message");
                }
                return new JoinSessionResponse(sessionId, userId, userToken);
            }
            else
            {
                var errorMessage = _messageParser.GetFieldFromMessage(message, "ErrorMessage");
                return new JoinSessionResponse(sessionId, errorMessage);
            }
        }
    }
}