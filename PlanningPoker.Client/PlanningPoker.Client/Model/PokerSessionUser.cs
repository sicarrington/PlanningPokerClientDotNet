using System;

namespace PlanningPoker.Client.Model
{
    public class PokerSessionUser
    {
        public virtual string Id { get; set; }
        public virtual string Name { get; set; }
        public virtual StoryPoint CurrentVote { get; set; }
        public virtual string SessionId { get; set; }
        public virtual string CurrentVoteDescription { get; set; }
        public virtual bool IsHost { get; set; }
        public virtual bool IsObserver { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            var typedObj = obj as PokerSessionUser;
            if(typedObj == null)
            {
                return false;
            }

            return Id == typedObj.Id &&
                Name == typedObj.Name &&
                CurrentVote == typedObj.CurrentVote &&
                SessionId == typedObj.SessionId &&
                IsHost == typedObj.IsHost &&
                IsObserver == typedObj.IsObserver;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode() ^
                Name.GetHashCode() ^
                CurrentVote.GetHashCode() ^
                SessionId.GetHashCode() ^
                IsHost.GetHashCode() ^
                IsObserver.GetHashCode();
        }
    }
}