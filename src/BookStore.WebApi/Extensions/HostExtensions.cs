namespace BookStore.WebApi.Extensions
{
    public static class HostExtension
    {
        public static void AddApplicationConfigurations(this WebApplicationBuilder builder)
        {
            var env = builder.Environment;

            builder.Configuration
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();
        }
    }
}
