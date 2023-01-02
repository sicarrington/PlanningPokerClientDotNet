using System;
using Microsoft.Extensions.DependencyInjection;
using PlanningPoker.Client.Connections;

namespace PlanningPoker.Client.Tests.Integration
{
	public class SubscribeSessionTests
	{
        private IServiceProvider _serviceProvider;
        private readonly IPlanningConnectionFactory _planningConnectionFactory;

        public SubscribeSessionTests()
        {
            _serviceProvider = TestHelper.GetServiceProvider();
            _planningConnectionFactory = _serviceProvider.GetRequiredService<IPlanningConnectionFactory>();
        }

        [Fact]
        public async Task SubscribingToASessionSucceeds()
        {
            var connection = _planningConnectionFactory.NewConnection();
            string? userId = null;
            string? sessionId = null;

            connection.OnSessionSuccesfullyCreated(information =>
            {
                userId = information.userId;
                sessionId = information.sessionId;
            });

            await connection.Start(CancellationToken.None);
            await connection.CreateSession("TestOne");

            await TestHelper.AwaitExpectation(() => (sessionId == null || userId == null));

            bool expectationMet = false;
            var secondConnection = _planningConnectionFactory.NewConnection();
            secondConnection.OnSessionSubscribeSucceeded(() =>
            {
                expectationMet = true;
            });

            await secondConnection.Start(CancellationToken.None);
            await secondConnection.SubscribeSession(userId, sessionId);

            await TestHelper.AwaitExpectation(() => expectationMet == false);

            Assert.True(expectationMet);
        }
    }
}

