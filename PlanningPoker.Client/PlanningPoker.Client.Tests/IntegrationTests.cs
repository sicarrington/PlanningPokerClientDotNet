using System;
using System.IO;
using System.Threading;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;
using PlanningPoker.Client.Connections;
using PlanningPoker.Client.MessageFactories;
using PlanningPoker.Client.Services;
using PlanningPoker.Client.Utilities;
using Xunit;

namespace PlanningPoker.Client.Tests
{
    public class IntegrationTests
    {
        //[Fact]
        public async void Test1()
        {
            var connectionSettings = new ConnectionSettings
            {
                PlanningSocketUri = new Uri("wss://planningpokercore.azurewebsites.net/ws"),
                PlanningApiUri = new Uri("https://sicarringtonplanningpokerapinew.azurewebsites.net/api")
            };
            var optionsMock = new Mock<IOptions<ConnectionSettings>>();
            optionsMock.Setup(x => x.Value).Returns(connectionSettings);


            var serviceCollection = new ServiceCollection();
            serviceCollection.AddOptions();

            serviceCollection.AddPlanningPokerClient();
            var serviceProvider = serviceCollection.BuildServiceProvider();
            var responseMessageParser = serviceProvider.GetService<IResponseMessageParser>();
            var pokerConnection = serviceProvider.GetService<IPokerConnection>();
            var userCacheProvider = serviceProvider.GetService<UserCacheProvider>();
            var planningPokerService = serviceProvider.GetService<IPlanningPokerService>();

            var planningConnection = new PlanningPokerConnection(optionsMock.Object, responseMessageParser, pokerConnection, userCacheProvider, planningPokerService);
            await planningConnection.Start(CancellationToken.None);
            await planningConnection.CreateSession("Simon");

            while (true)
            {
                System.Threading.Thread.Sleep(2000);
            }
        }
    }
}
