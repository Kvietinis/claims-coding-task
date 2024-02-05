using Claims.Auditing.Abstractions;
using EnsureThat;
using Microsoft.Extensions.DependencyInjection;

namespace Claims.Auditing.Implementations
{
    public class Auditer : IAuditer
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public Auditer(IServiceScopeFactory scopeFactory)
        {
            Ensure.That(scopeFactory, nameof(scopeFactory)).IsNotNull();

            _scopeFactory = scopeFactory;
        }

        public async Task AuditClaim(string id, string httpRequestType)
        {
            var claimAudit = new ClaimAudit()
            {
                Created = DateTime.Now,
                HttpRequestType = httpRequestType,
                ClaimId = id
            };

            using var scope = _scopeFactory.CreateScope();
            using var context = scope.ServiceProvider.GetRequiredService<AuditContext>();

            await context.AddAsync(claimAudit).ConfigureAwait(false);
            await context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task AuditCover(string id, string httpRequestType)
        {
            var coverAudit = new CoverAudit()
            {
                Created = DateTime.Now,
                HttpRequestType = httpRequestType,
                CoverId = id
            };

            using var scope = _scopeFactory.CreateScope();
            using var context = scope.ServiceProvider.GetRequiredService<AuditContext>();

            await context.AddAsync(coverAudit).ConfigureAwait(false);
            await context.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
