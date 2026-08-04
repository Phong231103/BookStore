using BookStore.Infrastructure.Caching;
using BookStore.Infrastructure.EmailSetting;
using BookStore.Infrastructure.Persistence;
using BookStore.Infrastructure.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BookStore.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddPersistence(configuration)
            .AddSecurity(configuration)
            .AddCaching(configuration)
            .AddEmail(configuration); ;

        return services;
    }
}
