using Microsoft.Extensions.Options;
using PlanningPoker.Client.MessageFactories;

namespace PlanningPoker.Client.Connections
{
    public sealed class PlanningConnectionFactory
    {
        private IOptions<ConnectionSettings> _connectionSettings;
        private IResponseMessageParser _responseMessageParser;
        private IPokerConnection _pokerConnection;
        internal PlanningConnectionFactory(IOptions<ConnectionSettings> connectionSettings,
            IResponseMessageParser responseMessageParser, IPokerConnection pokerConnection)
        {
            _connectionSettings = connectionSettings;
            _responseMessageParser = responseMessageParser;
            _pokerConnection = pokerConnection;
        }
        public PlanningPokerConnection NewConnection()
        {
            return new PlanningPokerConnection(_connectionSettings, _responseMessageParser, _pokerConnection);
        }
    }
}