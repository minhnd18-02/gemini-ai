using Microsoft.Azure.Mobile.Server.Config;

namespace GeminiAI
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApiKeyConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AppConfiguration>(configuration.GetSection("GeminiSettings"));

            return services;
        }
    }
}
