using BookStore.Application.Common.Interfaces;
using BookStore.Application.Users.Interfaces;
using BookStore.Domain.Common.Services;
using BookStore.Infrastructure.Common;
using BookStore.Infrastructure.Persistence;
using BookStore.Infrastructure.Persistence.Interceptors;
using BookStore.Infrastructure.Persistence.Repositories;
using BookStore.Infrastructure.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BookStore.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<PasswordHashOptions>(
            configuration.GetSection(PasswordHashOptions.SectionName));

        services.AddScoped<ConvertDomainEventsToOutboxMessagesInterceptor>();

        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            var interceptor = sp.GetRequiredService<ConvertDomainEventsToOutboxMessagesInterceptor>();
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
                   .AddInterceptors(interceptor);
        });

        services.AddScoped<IUserRepository, UserRepository>();

        services.AddScoped<IUnitOfWork>(
            sp => sp.GetRequiredService<ApplicationDbContext>());

        services.AddSingleton<IPasswordHasher, BcryptPasswordHasher>();
        services.AddSingleton<ISystemClock, SystemClock>();

        return services;
    }
}
