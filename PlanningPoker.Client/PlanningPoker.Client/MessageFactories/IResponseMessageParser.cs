using PlanningPoker.Client.Messages;

namespace PlanningPoker.Client.MessageFactories
{
    public interface IResponseMessageParser
    {
        ResponseMessage Get(string message);
    }
}