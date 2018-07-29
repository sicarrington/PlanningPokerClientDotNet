using System;
using System.Text.RegularExpressions;
using PlanningPoker.Client.Messages;
using PlanningPoker.Client.Model;

namespace PlanningPoker.Client.Utilities
{
    internal class MessageParser
    {
        public MessageParser()
        {

        }
        public virtual string GetFieldFromMessage(string message, string fieldName)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentNullException(nameof(message));
            }
            if (string.IsNullOrWhiteSpace(fieldName))
            {
                throw new ArgumentNullException(nameof(fieldName));
            }
            var fieldMatch = Regex.Match(message, $"{fieldName}:(.*)$", RegexOptions.Multiline);
            if (!fieldMatch.Success)
            {
                return null;
            }

            var extractedField = fieldMatch.Value.Replace($"{fieldName}:", "").Trim();
            return string.IsNullOrWhiteSpace(extractedField) ? null : extractedField;
        }
        public virtual ResponseMessageType GetTypeOfMessage(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentNullException(nameof(message));
            }
            var result = Regex.Match(message, "MessageType:(.*)$", RegexOptions.Multiline);
            if (!result.Success)
            {
                throw new InvalidOperationException("Message Type is missing");
            }

            var messageTypeString = result.Value.Replace("MessageType:", "").Trim();
            if (string.IsNullOrWhiteSpace(messageTypeString))
            {
                throw new InvalidOperationException("MessageType is empty");
            }
            ResponseMessageType responseMessageType;
            try
            {
                responseMessageType = (ResponseMessageType)Enum.Parse(typeof(ResponseMessageType), messageTypeString);
            }
            catch (ArgumentException)
            {
                throw new InvalidOperationException($"MessageType {messageTypeString} is not valid");
            }

            return responseMessageType;
        }
        public virtual bool IsSuccessfulMessage(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentNullException(nameof(message));
            }
            var successField = GetFieldFromMessage(message, "Success");
            bool success;
            if (bool.TryParse(successField, out success))
            {
                return success;
            }
            else
            {
                return false;
            }
        }
    }
}
