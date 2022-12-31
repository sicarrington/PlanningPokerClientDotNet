using Castle.Core.Logging;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using PlanningPoker.Client.Connections;
using PlanningPoker.Client.MessageFactories;
using PlanningPoker.Client.Services;
using PlanningPoker.Client.Utilities;
using Xunit;

namespace PlanningPoker.Client.Tests.ConnectionsTests.PlanningPokerConnectionFactoryTests
{
    public class NewConnectionTests
    {
        private Mock<IOptions<PokerConnectionSettings>> _connectionSettings;
        private Mock<IResponseMessageParser> _responseMessageParser;
        private Mock<IPokerConnection> _pokerConnection;
        private Mock<UserCacheProvider> _userCacheProvider;
        private Mock<IPlanningPokerService> _planningPokerService;
        private PlanningConnectionFactory _planningConnectionFactory;
        private readonly Mock<ILogger<PlanningPokerConnection>> _logger;


        public NewConnectionTests()
        {
            _connectionSettings = new Mock<IOptions<PokerConnectionSettings>>();
            _responseMessageParser = new Mock<IResponseMessageParser>();
            _pokerConnection = new Mock<IPokerConnection>();
            _userCacheProvider = new Mock<UserCacheProvider>();
            _planningPokerService = new Mock<IPlanningPokerService>();
            _logger = new Mock<ILogger<PlanningPokerConnection>>();

            _planningConnectionFactory = new PlanningConnectionFactory(_connectionSettings.Object, _responseMessageParser.Object, _pokerConnection.Object,
                _userCacheProvider.Object, _planningPokerService.Object, _logger.Object);
        }
        [Fact]
        public void GivenNewConnectionIsCalled_ThenNewInstanceOfPlanningPokerConnectionIsReturned()
        {
            var result = _planningConnectionFactory.NewConnection();

            Assert.NotNull(result);
        }
    }
}