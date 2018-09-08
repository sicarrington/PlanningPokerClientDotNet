using System;
using System.Threading;
using System.Threading.Tasks;
using PlanningPoker.Client.Model;

namespace PlanningPoker.Client.Connections
{
    public interface IPlanningPokerConnection
    {
        Task Start(CancellationToken cancellationToken);
        Task CreateSession(string hostName);
        Task SubscribeSession(string userId, string sessionId);
        Task JoinSession(string sessionId, string userName);
        IPlanningPokerConnection OnError(Action<Exception> onError);
        IPlanningPokerConnection OnSessionSuccesfullyCreated(Action<(string sessionId, string userId)> onSessionSuccesfullyCreated);
        IPlanningPokerConnection OnSessionCreationFailed(Action onsessionCreationFailed);
        IPlanningPokerConnection OnDisconnected(Action onDisconnected);
        IPlanningPokerConnection OnSessionSubscribeSucceeded(Action sessionSubscribeSucceeded);
        IPlanningPokerConnection OnSessionSubscribeFailed(Action sessionSubscribeFailed);
        IPlanningPokerConnection OnJoinSessionSucceeded(Action joinSessionSuceeded);
        IPlanningPokerConnection OnJoinSessionFailed(Action joinSessionFailed);
        IPlanningPokerConnection OnSessionInformationUpdated(Action<PokerSession> sessionInformationUpdated);
    }
}