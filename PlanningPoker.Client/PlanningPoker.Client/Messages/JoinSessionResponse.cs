using System;

namespace PlanningPoker.Client.Messages
{
    internal class JoinSessionResponse : ResponseMessage
    {
        public virtual string SessionId { get; private set; }
        public virtual string UserId { get; private set; }
        public virtual string UserToken { get; private set; }
        public virtual string ErrorMessage { get; private set; }
        public JoinSessionResponse(string sessionId, string userId, string userToken) : base(true)
        {
            if (string.IsNullOrWhiteSpace(sessionId))
            {
                throw new ArgumentNullException(nameof(sessionId));
            }
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentNullException(nameof(userId));
            }
            if (string.IsNullOrWhiteSpace(userToken))
            {
                throw new ArgumentNullException(nameof(userToken));
            }

            this.SessionId = sessionId;
            this.UserId = userId;
            this.UserToken = userToken;
        }
        public JoinSessionResponse(string sessionId, string errorMessage = null) : base(false)
        {
            if (string.IsNullOrWhiteSpace(sessionId))
            {
                throw new ArgumentNullException(nameof(sessionId));
            }

            this.SessionId = sessionId;
            this.ErrorMessage = errorMessage;
        }
    }
}