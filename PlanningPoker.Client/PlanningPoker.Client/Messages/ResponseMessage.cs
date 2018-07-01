using System;

namespace PlanningPoker.Client.Messages
{
    public abstract class ResponseMessage
    {
        public bool Success { get; set; }
        public ResponseMessage(bool success)
        {
            this.Success = success;
        }
    }
}
