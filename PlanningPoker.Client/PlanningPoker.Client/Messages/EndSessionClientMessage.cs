using System;

namespace PlanningPoker.Client.Messages
{
    internal class EndSessionClientMessage : ResponseMessage
    {
        public string SessionId { get; set; }
        public EndSessionClientMessage(string sessionId) : base(true)
        {
            if (string.IsNullOrWhiteSpace(sessionId))
            {
                throw new ArgumentNullException(nameof(sessionId));
            }
            SessionId = sessionId;
        }
    }
}
