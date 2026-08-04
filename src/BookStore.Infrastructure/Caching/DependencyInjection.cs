using BookStore.Application.Registration.Interfaces;
using BookStore.Application.Registrations.Interfaces;
using BookStore.Infrastructure.Caching.Options;
using BookStore.Infrastructure.Caching.Registration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace BookStore.Infrastructure.Caching
{
    internal static class DependencyInjection
    {
        public static IServiceCollection AddCaching(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<RedisCacheOptions>(
                configuration.GetSection(RedisCacheOptions.SectionName));

            services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                RedisCacheOptions options = sp.GetRequiredService<IOptions<RedisCacheOptions>>().Value;

                ConfigurationOptions configurationOptions =
                    ConfigurationOptions.Parse(options.ConnectionString);

                configurationOptions.AbortOnConnectFail = false;

                return ConnectionMultiplexer.Connect(configurationOptions);
            });

            services.AddScoped<IPendingRegistrationStore, RedisPendingRegistrationStore>();

            services.AddSingleton<IRegistrationSettings, RegistrationSettings>();

            services.AddOptions<RedisCacheOptions>().Bind(configuration.GetSection(RedisCacheOptions.SectionName)).ValidateOnStart();

            return services;
        }
    }
}
