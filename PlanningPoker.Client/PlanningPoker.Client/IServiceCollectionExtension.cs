using System;
using System.Collections.Generic;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PlanningPoker.Client.Connections;
using PlanningPoker.Client.Exceptions;
using PlanningPoker.Client.MessageFactories;
using PlanningPoker.Client.Services;
using PlanningPoker.Client.Utilities;

namespace PlanningPoker.Client
{
    public static class IServiceCollectionExtension
    {
        private static (Uri PlanningSocketUri, Uri PlanningApiUri, string PlanningApiKey) ValidateAndRetrieveConfiguration(IConfigurationRoot configuration)
        {
            var planningSocketPath = configuration.GetValue<string>("PokerConnectionSettings:PlanningSocketUri");
            if (string.IsNullOrEmpty(planningSocketPath))
            {
                throw new ConfigurationException("Setting PokerConnectionSettings:PlanningSocketUri was not loaded in configuration");
            }
            Uri planningSocketUri;
            if (!Uri.TryCreate(planningSocketPath, UriKind.Absolute, out planningSocketUri))
            {
                throw new ConfigurationException($"Setting PokerConnectionSettings:PlanningSocketUri is not a valid uri. The value supplied must be an absolute url. Value was: {planningSocketPath}");
            }

            var planningApiPath = configuration.GetValue<string>("PokerConnectionSettings:PlanningApiUri");
            if (string.IsNullOrEmpty(planningApiPath))
            {
                throw new ConfigurationException("Setting PokerConnectionSettings:PlanningApiUri was not loaded in configuration");
            }
            Uri planningApiUri;
            if (!Uri.TryCreate(planningApiPath, UriKind.Absolute, out planningApiUri))
            {
                throw new ConfigurationException($"Setting PokerConnectionSettings:PlanningApiUri is not a valid uri. The value supplied must be an absolute url. Value was: {planningApiPath}");
            }

            var planningApiKey = configuration.GetValue<string>("PokerConnectionSettings:ApiKey");
            if (string.IsNullOrEmpty(planningApiKey))
            {
                throw new ConfigurationException("Setting PokerConnectionSettings:ApiKey was not loaded in configuration");
            }

            return (planningSocketUri, planningApiUri, planningApiKey);
        }

        public static IServiceCollection AddPlanningPokerClient(this IServiceCollection services, IConfigurationRoot configuration)
        {
            services.Configure<PokerConnectionSettings>(options => configuration.GetSection("PokerConnectionSettings").Bind(options));

            var validatedConfiguration = IServiceCollectionExtension.ValidateAndRetrieveConfiguration(configuration);

            var connectionSettings = new PokerConnectionSettings(
                validatedConfiguration.PlanningSocketUri,
                validatedConfiguration.PlanningApiUri,
                validatedConfiguration.PlanningApiKey
            );
            var connectionSettingsOptions = Options.Create(connectionSettings);

            return services.AddSingleton<MessageParser>()
                .AddSingleton<IResponseMessageParser, ResponseMessageParser>()
                .AddTransient<IPokerConnection, PlanningPokerSocket>()
                .AddSingleton<UserCacheProvider>()
                .AddHttpClient()
                .AddSingleton<IPlanningPokerService, PlanningPokerApiService>()
                .AddSingleton<IPlanningConnectionFactory>(provider =>
                    new PlanningConnectionFactory(connectionSettingsOptions,
                        provider.GetRequiredService<IResponseMessageParser>(),
                        provider.GetRequiredService<IPokerConnection>(),
                        provider.GetRequiredService<UserCacheProvider>(),
                        provider.GetRequiredService<IPlanningPokerService>(),
                        provider.GetRequiredService<ILogger<PlanningPokerConnection>>()))
                .AddResponseMessageFactories();
        }

        private static IServiceCollection AddResponseMessageFactories(this IServiceCollection services)
        {
            services.AddSingleton<IResponseMessageFactory, NewSessionResponseMessageFactory>();
            services.AddSingleton<IResponseMessageFactory, SubscribeSessionResponseMessageFactory>();
            services.AddSingleton<IResponseMessageFactory, JoinSessionResponseMessageFactory>();
            services.AddSingleton<IResponseMessageFactory, RefreshSessionMessageFactory>();
            services.AddSingleton<IResponseMessageFactory, EndSessionClientMessageFactory>();

            return services;
        }
    }
}