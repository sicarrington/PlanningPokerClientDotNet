using PlanningPoker.Client.Messages;

namespace PlanningPoker.Client.MessageFactories
{
    public interface IResponseMessageFactory
    {
        ResponseMessage Get(string message);
    }
}