using EnsureThat;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Claims.Persistance.CosmosDb
{
    public class CosmosDatabaseCreationService : IHostedService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public CosmosDatabaseCreationService(IServiceScopeFactory scopeFactory)
        {
            Ensure.That(scopeFactory, nameof(scopeFactory)).IsNotNull();

            _scopeFactory = scopeFactory;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ClaimsCosmosContext>();

            await context.CreateDatabase().ConfigureAwait(false);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
