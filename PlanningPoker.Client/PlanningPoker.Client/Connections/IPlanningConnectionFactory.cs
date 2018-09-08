namespace PlanningPoker.Client.Connections
{
    public interface IPlanningConnectionFactory
    {
        IPlanningPokerConnection NewConnection();
    }
}