using Claims.Contracts;
using Claims.Persistance.Abstractions;
using Claims.Persistance.CosmosDb;
using Claims.Persistance.Sql;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using CosmosUnitOfWork = Claims.Persistance.CosmosDb.UnitOfWork;
using SqlUnitOfWork = Claims.Persistance.Sql.UnitOfWork;

namespace Claims.Persistance.IoC
{
    public static class IoCExtensions
    {
        public static IServiceCollection ConfigurePersistanceServices(this IServiceCollection services, IConfiguration configuration)
        {
            var persistanceProvider = configuration["PersistanceSettings:Provider"];

            if (string.Equals("sql", persistanceProvider, StringComparison.OrdinalIgnoreCase))
            {
                services.AddDbContext<ClaimsSqlDbContext>(options => options.UseSqlServer(configuration["ConnectionStrings:ClaimsConnection"]));
                services.AddScoped<IUnitOfWork, SqlUnitOfWork>();

                services.AddHostedService<SqlDatabaseMigrationService>();
            }
            else
            {
                services.AddSingleton(s =>
                {
                    var options = s.GetRequiredService<IOptions<CosmosDbConnectionSettings>>();
                    var result = new ClaimsCosmosContext(options.Value);

                    return result;
                });
                services.AddScoped<IUnitOfWork, CosmosUnitOfWork>();

                services.AddHostedService<CosmosDatabaseCreationService>();
            }

            return services;
        }
    }
}
