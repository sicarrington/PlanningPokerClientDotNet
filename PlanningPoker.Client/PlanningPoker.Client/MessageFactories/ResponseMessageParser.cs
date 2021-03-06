using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using PlanningPoker.Client.Messages;
using PlanningPoker.Client.Model;
using PlanningPoker.Client.Utilities;

namespace PlanningPoker.Client.MessageFactories
{
    internal sealed class ResponseMessageParser : IResponseMessageParser
    {
        MessageParser _messageParser;
        IEnumerable<IResponseMessageFactory> _responseMessageFactories;
        public ResponseMessageParser(MessageParser messageParser, IEnumerable<IResponseMessageFactory> responseMessageFactories)
        {
            _messageParser = messageParser;
            _responseMessageFactories = responseMessageFactories;
        }
        public ResponseMessage Get(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentNullException(message);
            }
            var messageType = _messageParser.GetTypeOfMessage(message);
            switch (messageType)
            {
                case ResponseMessageType.NewSessionResponse:
                    return GetFactory(typeof(NewSessionResponseMessageFactory)).Get(message);
                case ResponseMessageType.SubscribeSessionResponse:
                    return GetFactory(typeof(SubscribeSessionResponseMessageFactory)).Get(message);
                case ResponseMessageType.JoinSessionResponse:
                    return GetFactory(typeof(JoinSessionResponseMessageFactory)).Get(message);
                case ResponseMessageType.RefreshSession:
                    return GetFactory(typeof(RefreshSessionMessageFactory)).Get(message);
                case ResponseMessageType.SessionEndedMessage:
                    return GetFactory(typeof(EndSessionClientMessageFactory)).Get(message);
                default:
                    throw new InvalidOperationException($"Message type {messageType} is not supported");
            }
            throw new InvalidOperationException($"Message type {messageType} is not supported");
        }
        private IResponseMessageFactory GetFactory(Type factoryType)
        {
            var factory = _responseMessageFactories.FirstOrDefault(x => x.GetType() == factoryType);
            if (factory == null)
            {
                throw new InvalidOperationException($"No factory found for {factoryType}");
            }
            return factory;
        }
    }
}