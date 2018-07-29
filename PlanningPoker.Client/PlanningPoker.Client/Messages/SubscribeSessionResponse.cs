using System;
using System.Collections.Generic;

namespace PlanningPoker.Client.Messages
{
    internal class SubscribeSessionResponse : ResponseMessage
    {
        public string ErrorMessage { get; private set; }
        public string SessionId { get; private set; }

        public SubscribeSessionResponse(bool success, string sessionId, string errorMessage = null) : base(success)
        {
            if (string.IsNullOrWhiteSpace(sessionId))
            {
                throw new InvalidOperationException("SessionId must be supplied for successful response");
            }
            this.SessionId = sessionId;
            this.ErrorMessage = errorMessage;
        }
    }
}