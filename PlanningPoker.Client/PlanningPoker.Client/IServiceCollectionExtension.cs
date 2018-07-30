using System.Collections.Generic;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using PlanningPoker.Client.Connections;
using PlanningPoker.Client.MessageFactories;
using PlanningPoker.Client.Services;
using PlanningPoker.Client.Utilities;

namespace PlanningPoker.Client
{
    public static class IServiceCollectionExtension
    {
        public static IServiceCollection AddPlanningPokerClient(this IServiceCollection services)
        {
            var connectionSettings = new ConnectionSettings();
            services.AddSingleton(typeof(ConnectionSettings), connectionSettings);
            var connectionSettingsOptions = Options.Create(connectionSettings);

            var messageParser = new MessageParser();
            services.AddSingleton(typeof(MessageParser), messageParser);

            var responseMessageFactories = new List<IResponseMessageFactory>();
            responseMessageFactories.Add(new NewSessionResponseMessageFactory(messageParser));
            responseMessageFactories.Add(new SubscribeSessionResponseMessageFactory(messageParser));
            responseMessageFactories.Add(new JoinSessionResponseMessageFactory(messageParser));
            responseMessageFactories.Add(new RefreshSessionMessageFactory(messageParser));

            var responseMessageParser = new ResponseMessageParser(messageParser, responseMessageFactories);
            services.AddSingleton(typeof(IResponseMessageParser), responseMessageParser);

            var planningPokerSocket = new PlanningPokerSocket(connectionSettingsOptions);
            services.AddSingleton(typeof(IPokerConnection), planningPokerSocket);

            var userCacheProvider = new UserCacheProvider();
            services.AddSingleton(typeof(UserCacheProvider), userCacheProvider);

            var httpClient = new HttpClient();
            services.AddSingleton(typeof(HttpClient), httpClient);
            var planningPokerApiService = new PlanningPokerApiService(httpClient, connectionSettingsOptions);
            services.AddSingleton(typeof(IPlanningPokerService), planningPokerApiService);

            var planningPokerConnectionFactory = new PlanningConnectionFactory(connectionSettingsOptions,
                responseMessageParser, planningPokerSocket, userCacheProvider, planningPokerApiService);
            services.AddSingleton(typeof(PlanningConnectionFactory), planningPokerConnectionFactory);
            services = AddResponseMessageFactories(services);



            return services;
        }
        private static IServiceCollection AddResponseMessageFactories(IServiceCollection services)
        {
            services.AddSingleton<IResponseMessageFactory, NewSessionResponseMessageFactory>();
            services.AddSingleton<IResponseMessageFactory, SubscribeSessionResponseMessageFactory>();
            return services;
        }
    }
}