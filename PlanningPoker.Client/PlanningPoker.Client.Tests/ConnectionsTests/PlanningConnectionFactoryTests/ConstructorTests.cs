using System;
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
    public class ConstructorTests
    {
        private readonly Mock<IOptions<PokerConnectionSettings>> _connectionSettings;
        private readonly Mock<IResponseMessageParser> _responseMessageParser;
        private readonly Mock<IPokerConnection> _pokerConnection;
        private readonly Mock<UserCacheProvider> _userCacheProvider;
        private readonly Mock<IPlanningPokerService> _planningPokerService;
        private readonly Mock<ILogger<PlanningPokerConnection>> _logger;

        public ConstructorTests()
        {
            _connectionSettings = new Mock<IOptions<PokerConnectionSettings>>();
            _responseMessageParser = new Mock<IResponseMessageParser>();
            _pokerConnection = new Mock<IPokerConnection>();
            _userCacheProvider = new Mock<UserCacheProvider>();
            _planningPokerService = new Mock<IPlanningPokerService>();
            _logger = new Mock<ILogger<PlanningPokerConnection>>();
        }
        [Fact]
        public void GivenConstuctorIsCalled_WhenConnectionSettingsPassedIsNull_ThenExceptionIsThrown()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new PlanningConnectionFactory(null, _responseMessageParser.Object, _pokerConnection.Object, _userCacheProvider.Object, _planningPokerService.Object, _logger.Object);
            });
        }
        [Fact]
        public void GivenConstuctorIsCalled_WhenResponseMessageParserPassedIsNull_ThenExceptionIsThrown()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new PlanningConnectionFactory(_connectionSettings.Object, null, _pokerConnection.Object, _userCacheProvider.Object, _planningPokerService.Object, _logger.Object);
            });
        }
        [Fact]
        public void GivenConstuctorIsCalled_WhenPokerConnectionPassedIsNull_ThenExceptionIsThrown()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new PlanningConnectionFactory(_connectionSettings.Object, _responseMessageParser.Object, null, _userCacheProvider.Object, _planningPokerService.Object, _logger.Object);
            });
        }
        [Fact]
        public void GivenConstuctorIsCalled_WhenUserCacheProviderPassedIsNull_ThenExceptionIsThrown()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new PlanningConnectionFactory(_connectionSettings.Object, _responseMessageParser.Object, _pokerConnection.Object, null, _planningPokerService.Object, _logger.Object);
            });
        }
        [Fact]
        public void GivenConstuctorIsCalled_WhenPlanningPokerServicePassedIsNull_ThenExceptionIsThrown()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new PlanningConnectionFactory(_connectionSettings.Object, _responseMessageParser.Object, _pokerConnection.Object, _userCacheProvider.Object, null, _logger.Object);
            });
        }
        [Fact]
        public void GivenConstuctorIsCalled_WhenLoggerIsNull_ThenExceptionIsThrown()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new PlanningConnectionFactory(_connectionSettings.Object, _responseMessageParser.Object, _pokerConnection.Object, _userCacheProvider.Object, _planningPokerService.Object, null);
            });
        }
        [Fact]
        public void GivenConstructorIsCalled_WhenValidParametersArePassed_ThenConsturctionSuceeds()
        {
            var result = new PlanningConnectionFactory(_connectionSettings.Object, _responseMessageParser.Object, _pokerConnection.Object, _userCacheProvider.Object, _planningPokerService.Object, _logger.Object);

            Assert.NotNull(result);
        }
    }
}