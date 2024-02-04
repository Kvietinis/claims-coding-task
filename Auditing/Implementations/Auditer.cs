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

        public void AuditClaim(string id, string httpRequestType)
        {
            var claimAudit = new ClaimAudit()
            {
                Created = DateTime.Now,
                HttpRequestType = httpRequestType,
                ClaimId = id
            };

            _context.Add(claimAudit);
            _context.SaveChanges();
        }

        public void AuditCover(string id, string httpRequestType)
        {
            var coverAudit = new CoverAudit()
            {
                Created = DateTime.Now,
                HttpRequestType = httpRequestType,
                CoverId = id
            };

            _context.Add(coverAudit);
            _context.SaveChanges();
        }
    }
}
