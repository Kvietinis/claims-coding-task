using Claims.Business.Abstractions;
using Claims.Business.Implementations;
using Claims.Persistance.IoC;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Claims.Business.IoC
{
    public static class IoCExtensions
    {
        public static IServiceCollection ConfigureBusinessServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IClaimsService, ClaimsService>();
            services.AddTransient<ICoversService, CoversService>();

            services.ConfigurePersistanceServices(configuration);

            return services;
        }
    }
}
