using System;

namespace PlanningPoker.Client.Messages
{
    internal abstract class ResponseMessage
    {
        public bool Success { get; set; }
        public ResponseMessage(bool success)
        {
            this.Success = success;
        }
    }
}
