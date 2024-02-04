using Claims.Auditing.Abstractions;
using Claims.Auditing.Implementations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Claims.Auditing.IoC
{
    public static class IoCExtensions
    {
        public static IServiceCollection ConfigureAuditingServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IAuditer, Auditer>();

            return services;
        }
    }
}
