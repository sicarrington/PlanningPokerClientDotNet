using System;

namespace PlanningPoker.Client.Model
{
    public enum RequestMessageType
    {
        NewSession,
        JoinSession,
        UpdateSessionUser,

        RefreshSession,
        ResetVotesMessage,
        SubscribeMessage,
        UpdateSessionPropertiesMessage,
        EndSessionMessage,
        LeaveSessionMessage,
        RemoveUserFromSessionMessage,
        UpdateSessionMemberMessage
    }
}
