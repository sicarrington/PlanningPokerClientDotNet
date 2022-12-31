using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PlanningPoker.Client.MessageFactories;
using PlanningPoker.Client.Services;
using PlanningPoker.Client.Utilities;

namespace PlanningPoker.Client.Connections
{
    public sealed class PlanningConnectionFactory : IPlanningConnectionFactory
    {
        private readonly IOptions<PokerConnectionSettings> _connectionSettings;
        private readonly IResponseMessageParser _responseMessageParser;
        private readonly IPokerConnection _pokerConnection;
        private readonly UserCacheProvider _userCacheProvider;
        private readonly IPlanningPokerService _planningPokerService;
        private readonly ILogger<PlanningPokerConnection> _logger;

        internal PlanningConnectionFactory(IOptions<PokerConnectionSettings> connectionSettings,
            IResponseMessageParser responseMessageParser, IPokerConnection pokerConnection,
            UserCacheProvider userCacheProvider, IPlanningPokerService planningPokerService,
            ILogger<PlanningPokerConnection> logger)
        {
            _connectionSettings = connectionSettings ?? throw new ArgumentNullException(nameof(connectionSettings));
            _responseMessageParser = responseMessageParser ?? throw new ArgumentNullException(nameof(responseMessageParser));
            _pokerConnection = pokerConnection ?? throw new ArgumentNullException(nameof(pokerConnection));
            _userCacheProvider = userCacheProvider ?? throw new ArgumentNullException(nameof(userCacheProvider));
            _planningPokerService = planningPokerService ?? throw new ArgumentNullException(nameof(planningPokerService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public IPlanningPokerConnection NewConnection()
        {
            return new PlanningPokerConnection(_connectionSettings, _responseMessageParser, _pokerConnection, _userCacheProvider, _planningPokerService, _logger);
        }
    }
}