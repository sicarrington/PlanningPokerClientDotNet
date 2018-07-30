using System.Threading.Tasks;
using PlanningPoker.Client.Model;

namespace PlanningPoker.Client.Services
{
    internal interface IPlanningPokerService
    {
        Task<PokerSession> GetSessionDetails(string sessionId);
    }
}