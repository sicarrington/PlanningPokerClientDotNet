using System;
using Microsoft.Extensions.Options;
using PlanningPoker.Client.MessageFactories;
using PlanningPoker.Client.Services;
using PlanningPoker.Client.Utilities;

namespace PlanningPoker.Client.Connections
{
    public sealed class PlanningConnectionFactory : IPlanningConnectionFactory
    {
        private IOptions<PokerConnectionSettings> _connectionSettings;
        private IResponseMessageParser _responseMessageParser;
        private IPokerConnection _pokerConnection;
        private UserCacheProvider _userCacheProvider;
        private IPlanningPokerService _planningPokerService;
        internal PlanningConnectionFactory(IOptions<PokerConnectionSettings> connectionSettings,
            IResponseMessageParser responseMessageParser, IPokerConnection pokerConnection,
            UserCacheProvider userCacheProvider, IPlanningPokerService planningPokerService)
        {
            _connectionSettings = connectionSettings ?? throw new ArgumentNullException(nameof(connectionSettings));
            _responseMessageParser = responseMessageParser ?? throw new ArgumentNullException(nameof(responseMessageParser));
            _pokerConnection = pokerConnection ?? throw new ArgumentNullException(nameof(pokerConnection));
            _userCacheProvider = userCacheProvider ?? throw new ArgumentNullException(nameof(userCacheProvider));
            _planningPokerService = planningPokerService ?? throw new ArgumentNullException(nameof(planningPokerService));
        }
        public IPlanningPokerConnection NewConnection()
        {
            return new PlanningPokerConnection(_connectionSettings, _responseMessageParser, _pokerConnection, _userCacheProvider, _planningPokerService);
        }
    }
}