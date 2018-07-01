using System;
using System.Threading;
using System.Threading.Tasks;

namespace PlanningPoker.Client.Connections
{
    public interface IPokerConnection
    {
        Task Initialize(Action<string> onMessageFromServer, Action onDisconnected, CancellationToken cancellationToken);
        Task Send(string message);
    }
}