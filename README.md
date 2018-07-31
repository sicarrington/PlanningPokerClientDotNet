# Planning Poker dotnet Client Library

[See the planning poker site here](https://scrumplanningpoker.azurewebsites.net)

[See the nuget pacakge](https://www.nuget.org/packages/PlanningPoker.Client)

## Example Usage

Use ```AddPlanningPokerClient()``` method to add dependencies into DI container:

```csharp
    var serviceProvider = new ServiceCollection()
        .AddOptions()
        .AddPlanningPokerClient()
        .BuildServiceProvider();
```

In order to make a connection, use ```PlanningConnectionFactory``` to create a new instace. 
Start the connection by calling the ```Start()``` method.
```csharp
    var connectionFactor = serviceProvider.GetService<PlanningConnectionFactory>();
    var connection = connectionFactor.NewConnection();

    //Set up handling for events on the connection:
    connection.OnSessionSuccesfullyCreated((sessionDetails) =>
    {
        Console.WriteLine($"SessionId: {sessionDetails.sessionId}");
        Console.WriteLine($"UserId: {sessionDetails.userId}");
    });
    connection.OnSessionCreationFailed(() =>
    {
        Console.WriteLine("Failed to create session");
    });

    try
    {
        await connection.Start(CancellationToken.None);

        await connection.CreateSession("Simon");
    }
    catch(InvalidOperationException ex)
    {
        //error creating session
    }
```