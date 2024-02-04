using EnsureThat;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Claims.Infrastructure.Sql
{
    public abstract class BaseMigrationService<T> : IHostedService where T : DbContext
    {
        private readonly IServiceScopeFactory _scopeFactory;

        protected BaseMigrationService(IServiceScopeFactory scopeFactory)
        {
            Ensure.That(scopeFactory, nameof(scopeFactory)).IsNotNull();

            _scopeFactory = scopeFactory;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _scopeFactory.CreateScope();
            using var context = scope.ServiceProvider.GetRequiredService<T>();

            await context.Database.MigrateAsync().ConfigureAwait(false);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
