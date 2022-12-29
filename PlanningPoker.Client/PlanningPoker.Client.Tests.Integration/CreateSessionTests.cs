using System;
using Microsoft.Extensions.DependencyInjection;
using PlanningPoker.Client.Connections;

namespace PlanningPoker.Client.Tests.Integration;

public class CreateSessionTests
{
    private IServiceProvider _serviceProvider;
    private readonly IPlanningConnectionFactory _planningConnectionFactory;
    public CreateSessionTests()
    {
        _serviceProvider = TestHelper.GetServiceProvider();
        _planningConnectionFactory = _serviceProvider.GetRequiredService<IPlanningConnectionFactory>();
    }

    [Fact]
    public async Task CreateSessionSucceeds()
    {

        var expectationMet = false;

        var connection = _planningConnectionFactory.NewConnection();

        connection.OnSessionSuccesfullyCreated(information =>
        {
            expectationMet = true;
        });

        await connection.Start(CancellationToken.None);
        await connection.CreateSession("TestOne");

        var count = 0;
        do
        {
            await Task.Delay(TimeSpan.FromSeconds(1));
            count++;
        }
        while (expectationMet == false && count < 5);

        Assert.True(expectationMet);
    }
}