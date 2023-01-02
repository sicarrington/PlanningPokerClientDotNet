using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

namespace PlanningPoker.Client.Model
{
    public class PokerSession
    {
        public virtual string SessionId { get; set; }
        public virtual int StoryPointType { get; set; }

        public virtual IList<PokerSessionUser> Participants { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            var typedObj = obj as PokerSession;
            if (typedObj == null)
            {
                return false;
            }

            return SessionId == typedObj.SessionId &&
                StoryPointType == typedObj.StoryPointType &&
                Participants.SequenceEqual(typedObj.Participants);
        }

        public override int GetHashCode()
        {
            return SessionId.GetHashCode() ^
                StoryPointType.GetHashCode() ^
            Participants.GetHashCode();
        }
    }
}