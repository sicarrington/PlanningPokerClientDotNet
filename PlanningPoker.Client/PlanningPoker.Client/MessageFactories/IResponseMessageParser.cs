using PlanningPoker.Client.Messages;

namespace PlanningPoker.Client.MessageFactories
{
    internal interface IResponseMessageParser
    {
        ResponseMessage Get(string message);
    }
}