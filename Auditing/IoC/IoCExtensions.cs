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
            services.AddScoped<IAuditer>(s =>
            {
                var factory = s.GetRequiredService<IServiceScopeFactory>();
                var taskQueue = s.GetRequiredService<IBackgroundTaskQueue>();

                var service = new Auditer(factory);
                var result = new AuditerQueueDecorator(service, taskQueue);

                return result;
            });

            services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();

            services.AddHostedService<QueueHostedService>();
            services.AddHostedService<SqlAuditingMigrationService>();

            return services;
        }
    }
}
