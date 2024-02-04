using Claims.Auditing.Abstractions;
using Claims.Auditing.Implementations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Claims.Auditing.IoC
{
    public static class IoCExtensions
    {
        public static IServiceCollection ConfigureAuditingServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AuditContext>(options => options.UseSqlServer(configuration["ConnectionStrings:DefaultConnection"]));
            services.AddTransient<IAuditer, Auditer>();

            services.AddHostedService<SqlAuditingMigrationService>();

            return services;
        }
    }
}
