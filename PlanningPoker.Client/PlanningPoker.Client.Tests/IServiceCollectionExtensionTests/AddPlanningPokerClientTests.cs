using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;
using PlanningPoker.Client.Connections;
using PlanningPoker.Client.Exceptions;
using PlanningPoker.Client.MessageFactories;
using PlanningPoker.Client.Services;
using PlanningPoker.Client.Utilities;
using Xunit;

namespace PlanningPoker.Client.Tests.IServiceCollectionExtensionTests
{
    public class AddPlanningPokerClientTests
    {
        string _expectedPlanningSocketUri = "wss://scrumplanningpoker.azurewebsites.net/";
        string _expectedPlanningApiUri = "https://scrumplanningpoker.azurewebsites.net/api";
        string _expectedPlanningApiKey = "12345";
        IConfigurationRoot _expectedFullConfiguration;
        ServiceProvider _configuredServiceProvider;
        public AddPlanningPokerClientTests()
        {
            var builder = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string> {
                    { "PokerConnectionSettings:PlanningSocketUri", _expectedPlanningSocketUri },
                    { "PokerConnectionSettings:PlanningApiUri", _expectedPlanningApiUri } ,
                    { "PokerConnectionSettings:ApiKey", _expectedPlanningApiKey } });
            _expectedFullConfiguration = builder.Build();

            _configuredServiceProvider = new ServiceCollection()
                .AddPlanningPokerClient(_expectedFullConfiguration)
                .BuildServiceProvider();
        }
        [Fact]
        public void GivenAddPlanningPokerClientIsCalled_WhenPlanningSocketUriIsMissingFromSettings_ThenExceptionIsThrown()
        {
            var builder = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string> {
                    { "PokerConnectionSettings:PlanningApiUri", _expectedPlanningApiUri },
                    { "PokerConnectionSettings:ApiKey", _expectedPlanningApiKey } });
            var configuration = builder.Build();

            var serviceProvider = new ServiceCollection();

            var exception = Assert.Throws<ConfigurationException>(() => { serviceProvider.AddPlanningPokerClient(configuration); });
            Assert.Equal(exception.Message, "Setting PokerConnectionSettings:PlanningSocketUri was not loaded in configuration");
        }
        [Fact]
        public void GivenAddPlanningPokerClientIsCalled_WhenPlanningSocketUriIsPresentInSettingsButIsNotValidFormatUri_ThenExceptionIsThrown()
        {
            var planningSocketUri = "NotAUri";
            var builder = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string> {
                    { "PokerConnectionSettings:PlanningSocketUri", planningSocketUri },
                    { "PokerConnectionSettings:PlanningApiUri", _expectedPlanningApiUri },
                    { "PokerConnectionSettings:ApiKey", _expectedPlanningApiKey } });
            var configuration = builder.Build();

            var serviceProvider = new ServiceCollection();

            var exception = Assert.Throws<ConfigurationException>(() => { serviceProvider.AddPlanningPokerClient(configuration); });
            Assert.Equal(exception.Message, $"Setting PokerConnectionSettings:PlanningSocketUri is not a valid uri. The value supplied must be an absolute url. Value was: {planningSocketUri}");
        }
        [Fact]
        public void GivenAddPlanningPokerClientIsCalled_WhenPlanningApiUriIsMissingFromSettings_ThenExceptionIsThrown()
        {
            var builder = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string> {
                    { "PokerConnectionSettings:PlanningSocketUri", _expectedPlanningSocketUri },
                    { "PokerConnectionSettings:ApiKey", _expectedPlanningApiKey } });
            var configuration = builder.Build();

            var serviceProvider = new ServiceCollection();

            var exception = Assert.Throws<ConfigurationException>(() => { serviceProvider.AddPlanningPokerClient(configuration); });
            Assert.Equal(exception.Message, "Setting PokerConnectionSettings:PlanningApiUri was not loaded in configuration");
        }
        [Fact]
        public void GivenAddPlanningPokerClientIsCalled_WhenPlanningApiUriIsPresentInSettingsButIsNotValidFormatUri_ThenExceptionIsThrown()
        {
            var planningApiUri = "NotAUri";
            var builder = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string> {
                    { "PokerConnectionSettings:PlanningSocketUri", _expectedPlanningSocketUri },
                    { "PokerConnectionSettings:PlanningApiUri", planningApiUri },
                    { "PokerConnectionSettings:ApiKey", _expectedPlanningApiKey } });
            var configuration = builder.Build();

            var serviceProvider = new ServiceCollection();

            var exception = Assert.Throws<ConfigurationException>(() => { serviceProvider.AddPlanningPokerClient(configuration); });
            Assert.Equal(exception.Message, $"Setting PokerConnectionSettings:PlanningApiUri is not a valid uri. The value supplied must be an absolute url. Value was: {planningApiUri}");
        }
        [Fact]
        public void GivenAddPlanningPokerClientIsCalled_WhenApiKeyIsMissingFromSettings_ThenExceptionIsThrown()
        {
            var builder = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string> {
                    { "PokerConnectionSettings:PlanningSocketUri", _expectedPlanningSocketUri },
                    { "PokerConnectionSettings:PlanningApiUri", _expectedPlanningApiUri } });
            var configuration = builder.Build();

            var serviceProvider = new ServiceCollection();

            var exception = Assert.Throws<ConfigurationException>(() => { serviceProvider.AddPlanningPokerClient(configuration); });
            Assert.Equal(exception.Message, "Setting PokerConnectionSettings:ApiKey was not loaded in configuration");
        }
        [Fact]
        public void GivenAddPlanningPokerIsCalled_WhenCollectionIsReturned_ThenPokerConnectionSettingsIsConfigured()
        {
            var connectionSettingsOptions = (IOptions<PokerConnectionSettings>)_configuredServiceProvider.GetService(typeof(IOptions<PokerConnectionSettings>));

            Assert.NotNull(connectionSettingsOptions);
            Assert.NotNull(connectionSettingsOptions.Value);
        }
        [Fact]
        public void GivenAddPlanningPokerIsCalled_WhenCollectionIsReturned_ThenPokerConnectionSettingsPlanningSocketUriIsConfiguredCorrectly()
        {
            var connectionSettingsOptions = (IOptions<PokerConnectionSettings>)_configuredServiceProvider.GetService(typeof(IOptions<PokerConnectionSettings>));

            Assert.Equal(connectionSettingsOptions.Value.PlanningSocketUri.AbsoluteUri, _expectedPlanningSocketUri);
        }
        [Fact]
        public void GivenAddPlanningPokerIsCalled_WhenCollectionIsReturned_ThenPokerConnectionSettingsPlanningApiUrIsConfiguredCorrectly()
        {
            var connectionSettingsOptions = (IOptions<PokerConnectionSettings>)_configuredServiceProvider.GetService(typeof(IOptions<PokerConnectionSettings>));

            Assert.Equal(connectionSettingsOptions.Value.PlanningApiUri.AbsoluteUri, _expectedPlanningApiUri);
        }
        [Fact]
        public void GivenAddPlanningPokerIsCalled_WhenCollectionIsReturned_ThenPokerConnectionSettingsPlanningApiKeyConfiguredCorrectly()
        {
            var connectionSettingsOptions = (IOptions<PokerConnectionSettings>)_configuredServiceProvider.GetService(typeof(IOptions<PokerConnectionSettings>));

            Assert.Equal(connectionSettingsOptions.Value.ApiKey, _expectedPlanningApiKey);
        }
        [Fact]
        public void GivenAddPlanningPokerClientIsCalled_WhenServiceCollectionIsReturned_ThenMessageParserIsConfigured()
        {
            var service = (MessageParser)_configuredServiceProvider.GetService(typeof(MessageParser));

            Assert.NotNull(service);
        }
        [Fact]
        public void GivenAddPlanningPokerClientIsCalled_WhenServiceCollectionIsReturned_ThenResponseMessageParserIsConfigured()
        {
            var service = (IResponseMessageParser)_configuredServiceProvider.GetService(typeof(IResponseMessageParser));

            Assert.NotNull(service);
        }
        [Fact]
        public void GivenAddPlanningPokerClientIsCalled_WhenServiceCollectionIsReturned_ThenPokerConnectionIsConfigured()
        {
            var service = (IPokerConnection)_configuredServiceProvider.GetService(typeof(IPokerConnection));

            Assert.NotNull(service);
        }
        [Fact]
        public void GivenAddPlanningPokerClientIsCalled_WhenServiceCollectionIsReturned_ThenUserCacheProviderIsConfigured()
        {
            var service = (UserCacheProvider)_configuredServiceProvider.GetService(typeof(UserCacheProvider));

            Assert.NotNull(service);
        }
        [Fact]
        public void GivenAddPlanningPokerClientIsCalled_WhenServiceCollectionIsReturned_ThenHttpClientIsConfigured()
        {
            var service = (HttpClient)_configuredServiceProvider.GetService(typeof(HttpClient));

            Assert.NotNull(service);
        }
        [Fact]
        public void GivenAddPlanningPokerClientIsCalled_WhenServiceCollectionIsReturned_ThenPlanningPokerServiceIsConfigured()
        {
            var service = (IPlanningPokerService)_configuredServiceProvider.GetService(typeof(IPlanningPokerService));

            Assert.NotNull(service);
        }
        [Fact]
        public void GivenAddPlanningPokerClientIsCalled_WhenServiceCollectionIsReturned_ThenPlanningConnectionFactoryIsConfigured()
        {
            var service = (PlanningConnectionFactory)_configuredServiceProvider.GetService(typeof(PlanningConnectionFactory));

            Assert.NotNull(service);
        }
        [Fact]
        public void GivenAddPlanningPokerClientIsCalled_WhenServiceCollectionIsReturned_ThenNewSessionResponseMessageFactoryIsConfigured()
        {
            var services = (IEnumerable<IResponseMessageFactory>)_configuredServiceProvider.GetServices(typeof(IResponseMessageFactory));

            Assert.True(services.Any(x => x is NewSessionResponseMessageFactory));
        }
        [Fact]
        public void GivenAddPlanningPokerClientIsCalled_WhenServiceCollectionIsReturned_ThenSubscribeSessionResponseMessageFactoryIsConfigured()
        {
            var services = (IEnumerable<IResponseMessageFactory>)_configuredServiceProvider.GetServices(typeof(IResponseMessageFactory));

            Assert.True(services.Any(x => x is SubscribeSessionResponseMessageFactory));
        }
        [Fact]
        public void GivenAddPlanningPokerClientIsCalled_WhenServiceCollectionIsReturned_ThenJoinSessionResponseMessageFactoryIsConfigured()
        {
            var services = (IEnumerable<IResponseMessageFactory>)_configuredServiceProvider.GetServices(typeof(IResponseMessageFactory));

            Assert.True(services.Any(x => x is JoinSessionResponseMessageFactory));
        }
        [Fact]
        public void GivenAddPlanningPokerClientIsCalled_WhenServiceCollectionIsReturned_ThenRefreshSessionMessageFactoryIsConfigured()
        {
            var services = (IEnumerable<IResponseMessageFactory>)_configuredServiceProvider.GetServices(typeof(IResponseMessageFactory));

            Assert.True(services.Any(x => x is RefreshSessionMessageFactory));
        }
    }
}