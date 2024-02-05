using Claims.Auditing.Abstractions;
using EnsureThat;
using Microsoft.Extensions.DependencyInjection;

namespace Claims.Auditing.Implementations
{
    public class Auditer : IAuditer
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IBackgroundTaskQueue _taskQueue;

        public Auditer(IServiceScopeFactory scopeFactory, IBackgroundTaskQueue taskQueue)
        {
            Ensure.That(scopeFactory, nameof(scopeFactory)).IsNotNull();
            Ensure.That(taskQueue, nameof(taskQueue)).IsNotNull();

            _scopeFactory = scopeFactory;
            _taskQueue = taskQueue;
        }

        public async Task AuditClaim(string id, string httpRequestType)
        {
            var claimAudit = new ClaimAudit()
            {
                Created = DateTime.Now,
                HttpRequestType = httpRequestType,
                ClaimId = id
            };

            await _taskQueue.Queue(async () =>
            {
                using var scope = _scopeFactory.CreateScope();
                using var context = scope.ServiceProvider.GetRequiredService<AuditContext>();

                await context.AddAsync(claimAudit).ConfigureAwait(false);
                await context.SaveChangesAsync().ConfigureAwait(false);
            })
            .ConfigureAwait(false);
        }

        public async Task AuditCover(string id, string httpRequestType)
        {
            var coverAudit = new CoverAudit()
            {
                Created = DateTime.Now,
                HttpRequestType = httpRequestType,
                CoverId = id
            };

            await _taskQueue.Queue(async () =>
            {
                using var scope = _scopeFactory.CreateScope();
                using var context = scope.ServiceProvider.GetRequiredService<AuditContext>();

                await context.AddAsync(coverAudit).ConfigureAwait(false);
                await context.SaveChangesAsync().ConfigureAwait(false);
            })
            .ConfigureAwait(false);
        }
    }
}
