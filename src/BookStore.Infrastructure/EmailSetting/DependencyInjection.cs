using BookStore.Application.Registrations.Interfaces;
using BookStore.Infrastructure.EmailSetting.Templates;
using BookStore.Infrastructure.Security.Otp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BookStore.Infrastructure.EmailSetting
{
    internal static class DependencyInjection
    {
        public static IServiceCollection AddEmail(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<OtpHashOptions>(configuration.GetSection(OtpHashOptions.SectionName));

            services.AddSingleton<IOtpGenerator, RandomOtpGenerator>();

            services.AddSingleton<IOtpHasher, HmacOtpHasher>();

            services.AddScoped<IEmailSender, FakeEmailSender>();

            services.AddScoped<IRegistrationEmailTemplateProvider, RegistrationEmailTemplateProvider>();

            return services;
        }
    }
}
