using System;
using Microsoft.Extensions.DependencyInjection;
using PlanningPoker.Client.Connections;

namespace PlanningPoker.Client.Tests.Integration
{
	public class PlaceVoteTests
	{
        private IServiceProvider _serviceProvider;
        private readonly IPlanningConnectionFactory _planningConnectionFactory;

        public PlaceVoteTests()
        {
            _serviceProvider = TestHelper.GetServiceProvider();
            _planningConnectionFactory = _serviceProvider.GetRequiredService<IPlanningConnectionFactory>();
        }

        [Fact]
        public async Task PlaceVoteSucceeds()
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

            Model.StoryPoint updatedStoryPoint = Model.StoryPoint.NotVoted;
            connection.OnSessionInformationUpdated(information => {
                updatedStoryPoint = information.Participants[0].CurrentVote;
            });
            await connection.PlaceVote(sessionId, userId, Model.StoryPoint.Four);

            await TestHelper.AwaitExpectation(() => updatedStoryPoint != Model.StoryPoint.NotVoted);

            Assert.Equal(Model.StoryPoint.Four, updatedStoryPoint);
        }
    }
}

