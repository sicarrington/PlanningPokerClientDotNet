using System;
using Microsoft.Extensions.DependencyInjection;
using PlanningPoker.Client.Connections;

namespace PlanningPoker.Client.Tests.Integration
{
	public class StopTests
	{
        private IServiceProvider _serviceProvider;
        private readonly IPlanningConnectionFactory _planningConnectionFactory;

        public StopTests()
        {
            _serviceProvider = TestHelper.GetServiceProvider();
            _planningConnectionFactory = _serviceProvider.GetRequiredService<IPlanningConnectionFactory>();
        }

        [Fact]
        public async Task StopWorks()
        {
            string? sessionId = null;
            string? userId = null;

            var connection = _planningConnectionFactory.NewConnection();

            connection.OnSessionSuccesfullyCreated(information =>
            {
                sessionId = information.sessionId;
                userId = information.userId;
            });

            await connection.Start(CancellationToken.None);
            await connection.CreateSession("TestOne");

            await TestHelper.AwaitExpectation(() => sessionId != null && userId != null);

            await connection.Disconnect();
        }
    }
}

