using System;
using PlanningPoker.Client.Messages;
using PlanningPoker.Client.Model;
using PlanningPoker.Client.Utilities;

namespace PlanningPoker.Client.MessageFactories
{
    internal class EndSessionClientMessageFactory : IResponseMessageFactory
    {
        private MessageParser _messageParser;
        public EndSessionClientMessageFactory(MessageParser messageParser)
        {
            _messageParser = messageParser;
        }
        public ResponseMessage Get(string message)
        {
            if (String.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentNullException(nameof(message));
            }
            var messageType = _messageParser.GetTypeOfMessage(message);
            if (messageType != ResponseMessageType.SessionEndedMessage)
            {
                throw new InvalidOperationException($"{this.GetType()} cannot proccess message of type {messageType}");
            }
            var sessionId = _messageParser.GetFieldFromMessage(message, "SessionId");
            if (string.IsNullOrWhiteSpace(sessionId))
            {
                throw new InvalidOperationException("SessionId is missing from message");
            }
            return new EndSessionClientMessage(sessionId);
        }
    }
}