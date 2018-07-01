using System;

namespace PlanningPoker.Client.Messages
{
    public class NewSessionResponse : ResponseMessage
    {
        public virtual string SessionId { get; private set; }
        public virtual string UserId { get; private set; }
        public virtual string UserToken { get; private set; }
        public virtual string ErrorMessage { get; private set; }
        internal NewSessionResponse(string sessionId, string userId, string userToken) : base(true)
        {
            if(string.IsNullOrWhiteSpace(sessionId))
            {
                throw new ArgumentNullException(nameof(sessionId));
            }
            if(string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentNullException(nameof(userId));
            }
            if(string.IsNullOrWhiteSpace(userToken))
            {
                throw new ArgumentNullException(nameof(userToken));
            }
            SessionId = sessionId;
            UserId = userId;
            UserToken = userToken;
        }
        internal NewSessionResponse(string errorMessage = null): base(false)
        {
            this.ErrorMessage = errorMessage;
        }
    }
}
