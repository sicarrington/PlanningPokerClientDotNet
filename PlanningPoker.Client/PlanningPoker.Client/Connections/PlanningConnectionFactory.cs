using Microsoft.Extensions.Options;
using PlanningPoker.Client.MessageFactories;
using PlanningPoker.Client.Services;
using PlanningPoker.Client.Utilities;

namespace PlanningPoker.Client.Connections
{
    public sealed class PlanningConnectionFactory
    {
        private IOptions<ConnectionSettings> _connectionSettings;
        private IResponseMessageParser _responseMessageParser;
        private IPokerConnection _pokerConnection;
        private UserCacheProvider _userCacheProvider;
        private IPlanningPokerService _planningPokerService;
        internal PlanningConnectionFactory(IOptions<ConnectionSettings> connectionSettings,
            IResponseMessageParser responseMessageParser, IPokerConnection pokerConnection,
            UserCacheProvider userCacheProvider, IPlanningPokerService planningPokerService)
        {
            _connectionSettings = connectionSettings;
            _responseMessageParser = responseMessageParser;
            _pokerConnection = pokerConnection;
            _userCacheProvider = userCacheProvider;
            _planningPokerService = planningPokerService;
        }
        public PlanningPokerConnection NewConnection()
        {
            return new PlanningPokerConnection(_connectionSettings, _responseMessageParser, _pokerConnection, _userCacheProvider, _planningPokerService);
        }
    }
}