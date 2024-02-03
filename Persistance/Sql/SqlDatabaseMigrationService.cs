using EnsureThat;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Claims.Persistance.Sql
{
    public class SqlDatabaseMigrationService : IHostedService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public SqlDatabaseMigrationService(IServiceScopeFactory scopeFactory)
        {
            Ensure.That(scopeFactory, nameof(scopeFactory)).IsNotNull();

            _scopeFactory = scopeFactory;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _scopeFactory.CreateScope();
            await scope.ServiceProvider.GetRequiredService<ClaimsSqlDbContext>().Database.MigrateAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
