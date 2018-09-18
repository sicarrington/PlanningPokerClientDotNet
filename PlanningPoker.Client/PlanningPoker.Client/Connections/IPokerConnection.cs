using System;
using System.Threading;
using System.Threading.Tasks;

namespace PlanningPoker.Client.Connections
{
    internal interface IPokerConnection
    {
        Task Initialize(Action<string> onMessageFromServer, Action onDisconnected, CancellationToken cancellationToken);
        Task Send(string message);
        Task Disconnect();
    }
}