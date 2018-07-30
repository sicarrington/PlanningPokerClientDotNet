using System.Collections.Generic;

namespace PlanningPoker.Client.Model
{
    public class PokerSession
    {
        public virtual string SessionId { get; set; }
        public virtual int StoryPointType { get; set; }

        public virtual IList<PokerSessionUser> Participants { get; set; }
    }
}