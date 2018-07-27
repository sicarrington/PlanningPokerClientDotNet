using System;

namespace PlanningPoker.Client
{
    internal class ConnectionSettings
    {
        public virtual Uri PlanningSocketUri { get; set; }
        public virtual Uri PlanningApiUri { get; set; }

        public ConnectionSettings()
        {
            this.PlanningApiUri = new Uri("https://sicarringtonplanningpokerapinew.azurewebsites.net/api");
            this.PlanningSocketUri = new Uri("wss://planningpokercore.azurewebsites.net/ws");
        }
    }
}
