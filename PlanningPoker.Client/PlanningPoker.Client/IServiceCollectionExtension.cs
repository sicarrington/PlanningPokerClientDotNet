using Microsoft.Extensions.DependencyInjection;
using PlanningPoker.Client.Connections;
using PlanningPoker.Client.MessageFactories;
using PlanningPoker.Client.Utilities;

namespace PlanningPoker.Client
{
    public static class IServiceCollectionExtension
    {
        public static IServiceCollection AddPlanningPokerClient(this IServiceCollection services)
        {
            services.AddSingleton<IResponseMessageParser, ResponseMessageParser>();
            services.AddSingleton<MessageParser, MessageParser>();
            services.AddTransient<IPokerConnection, PlanningPokerSocket>();
            services.AddSingleton<UserCacheProvider, UserCacheProvider>();

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