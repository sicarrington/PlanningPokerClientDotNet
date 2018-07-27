using Microsoft.Extensions.Options;
using PlanningPoker.Client.MessageFactories;
using PlanningPoker.Client.Utilities;

namespace PlanningPoker.Client.Connections
{
    public sealed class PlanningConnectionFactory
    {
        private IOptions<ConnectionSettings> _connectionSettings;
        private IResponseMessageParser _responseMessageParser;
        private IPokerConnection _pokerConnection;
        private UserCacheProvider _userCacheProvider;
        internal PlanningConnectionFactory(IOptions<ConnectionSettings> connectionSettings,
            IResponseMessageParser responseMessageParser, IPokerConnection pokerConnection,
            UserCacheProvider userCacheProvider)
        {
            _connectionSettings = connectionSettings;
            _responseMessageParser = responseMessageParser;
            _pokerConnection = pokerConnection;
            _userCacheProvider = userCacheProvider;
        }
        public PlanningPokerConnection NewConnection()
        {
            return new PlanningPokerConnection(_connectionSettings, _responseMessageParser, _pokerConnection, _userCacheProvider);
        }
    }
}