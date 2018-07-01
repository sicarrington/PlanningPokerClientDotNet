using PlanningPoker.Client.Messages;

namespace PlanningPoker.Client.MessageFactories
{
    internal interface IResponseMessageFactory
    {
        ResponseMessage Get(string message);
    }
}