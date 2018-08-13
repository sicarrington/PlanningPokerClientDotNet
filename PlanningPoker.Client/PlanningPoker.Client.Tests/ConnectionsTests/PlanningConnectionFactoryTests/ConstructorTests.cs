using System;
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
        private Mock<IOptions<PokerConnectionSettings>> _connectionSettings;
        private Mock<IResponseMessageParser> _responseMessageParser;
        private Mock<IPokerConnection> _pokerConnection;
        private Mock<UserCacheProvider> _userCacheProvider;
        private Mock<IPlanningPokerService> _planningPokerService;
        public ConstructorTests()
        {
            _connectionSettings = new Mock<IOptions<PokerConnectionSettings>>();
            _responseMessageParser = new Mock<IResponseMessageParser>();
            _pokerConnection = new Mock<IPokerConnection>();
            _userCacheProvider = new Mock<UserCacheProvider>();
            _planningPokerService = new Mock<IPlanningPokerService>();
        }
        [Fact]
        public void GivenConstuctorIsCalled_WhenConnectionSettingsPassedIsNull_ThenExceptionIsThrown()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new PlanningConnectionFactory(null, _responseMessageParser.Object, _pokerConnection.Object, _userCacheProvider.Object, _planningPokerService.Object);
            });
        }
        [Fact]
        public void GivenConstuctorIsCalled_WhenResponseMessageParserPassedIsNull_ThenExceptionIsThrown()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new PlanningConnectionFactory(_connectionSettings.Object, null, _pokerConnection.Object, _userCacheProvider.Object, _planningPokerService.Object);
            });
        }
        [Fact]
        public void GivenConstuctorIsCalled_WhenPokerConnectionPassedIsNull_ThenExceptionIsThrown()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new PlanningConnectionFactory(_connectionSettings.Object, _responseMessageParser.Object, null, _userCacheProvider.Object, _planningPokerService.Object);
            });
        }
        [Fact]
        public void GivenConstuctorIsCalled_WhenUserCacheProviderPassedIsNull_ThenExceptionIsThrown()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new PlanningConnectionFactory(_connectionSettings.Object, _responseMessageParser.Object, _pokerConnection.Object, null, _planningPokerService.Object);
            });
        }
        [Fact]
        public void GivenConstuctorIsCalled_WhenPlanningPokerServicePassedIsNull_ThenExceptionIsThrown()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new PlanningConnectionFactory(_connectionSettings.Object, _responseMessageParser.Object, _pokerConnection.Object, _userCacheProvider.Object, null);
            });
        }
        [Fact]
        public void GivenConstructorIsCalled_WhenValidParametersArePassed_ThenConsturctionSuceeds()
        {
            var result = new PlanningConnectionFactory(_connectionSettings.Object, _responseMessageParser.Object, _pokerConnection.Object, _userCacheProvider.Object, _planningPokerService.Object);

            Assert.NotNull(result);
        }
    }
}