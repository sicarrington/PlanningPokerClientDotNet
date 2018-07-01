using System;

namespace PlanningPoker.Client
{
    public sealed class ConnectionSettings
    {
        public Uri PlanningSocketUri { get; set; }
        public Uri PlanningApiUri { get; set; }

        public ConnectionSettings()
        {
            this.PlanningApiUri = new Uri("https://sicarringtonplanningpokerapinew.azurewebsites.net/api");
            this.PlanningSocketUri = new Uri("wss://planningpokercore.azurewebsites.net/ws");
        }
    }
}
