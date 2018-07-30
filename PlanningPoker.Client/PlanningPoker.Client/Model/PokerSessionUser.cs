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
    }
}