using Claims.Auditing.Abstractions;
using EnsureThat;

namespace Claims.Auditing.Implementations
{
    public class Auditer : IAuditer
    {
        private readonly AuditContext _context;

        public Auditer(AuditContext context)
        {
            Ensure.That(context, nameof(context)).IsNotNull();

            _context = context;
        }

        public async Task AuditClaim(string id, string httpRequestType)
        {
            var claimAudit = new ClaimAudit()
            {
                Created = DateTime.Now,
                HttpRequestType = httpRequestType,
                ClaimId = id
            };

            await _context.AddAsync(claimAudit).ConfigureAwait(false);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task AuditCover(string id, string httpRequestType)
        {
            var coverAudit = new CoverAudit()
            {
                Created = DateTime.Now,
                HttpRequestType = httpRequestType,
                CoverId = id
            };

            await _context.AddAsync(coverAudit).ConfigureAwait(false);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
