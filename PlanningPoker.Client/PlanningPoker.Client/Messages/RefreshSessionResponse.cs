using System;
using PlanningPoker.Client.Model;

namespace PlanningPoker.Client.Messages
{
    internal class RefreshSessionResponse : ResponseMessage
    {
        public string SessionId { get; private set; }

        public PokerSession PokerSessionInformation { get; set; }

        public RefreshSessionResponse(string sessionId) : base(true)
        {
            if (string.IsNullOrWhiteSpace(sessionId))
            {
                throw new ArgumentNullException(nameof(sessionId));
            }
            this.SessionId = sessionId;
        }
    }
}