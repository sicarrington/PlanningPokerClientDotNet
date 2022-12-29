using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace PlanningPoker.Client.Tests.Integration
{
	public class TestHelper
	{
        public static IConfigurationRoot GetIConfigurationRoot()
        {
            return new ConfigurationBuilder()
                //.SetBasePath(outputPath)
                .AddJsonFile("appsettings.json", optional: false)
                .Build();
        }

        public static IServiceProvider GetServiceProvider()
        {
            return new ServiceCollection()
                .AddOptions()
                .AddPlanningPokerClient(GetIConfigurationRoot())
                .BuildServiceProvider();
        }

        public static async Task AwaitExpectation(Func<bool> test, int maxTries = 5)
        {
            var count = 0;
            do
            {
                await Task.Delay(TimeSpan.FromSeconds(1));
                count++;
            }
            while (test() && count < maxTries);
        }
    }
}

