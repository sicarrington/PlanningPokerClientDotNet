using System;

namespace PlanningPoker.Client
{
    internal class PokerConnectionSettings
    {
        public virtual Uri PlanningSocketUri { get; set; }
        public virtual Uri PlanningApiUri { get; set; }
        public virtual string ApiKey { get; set; }


        public PokerConnectionSettings() { }

        public PokerConnectionSettings(Uri planningSocktUri, Uri planningApiUrl, string apiKey)
        {
            this.PlanningSocketUri = planningSocktUri;
            this.PlanningApiUri = planningApiUrl;
            this.ApiKey = apiKey;
        }
    }
}
