using BookStore.Domain.Common.Services;
using BookStore.Infrastructure.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BookStore.Infrastructure.Security;

internal static class DependencyInjection
{
    public static IServiceCollection AddSecurity(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<PasswordHashOptions>(
            configuration.GetSection(PasswordHashOptions.SectionName));

        services.AddSingleton<IPasswordHasher, BcryptPasswordHasher>();

        services.AddSingleton<ISystemClock, SystemClock>();

        return services;
    }
}