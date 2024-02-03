using Claims.Persistance.Abstractions;
using EnsureThat;
using Microsoft.EntityFrameworkCore;

namespace Claims.Persistance.Sql
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ClaimsSqlDbContext _context;
        private readonly IClaimsRepository _claimsRepository;
        private readonly ICoversRepository _coversRepository;

        public UnitOfWork(ClaimsSqlDbContext context)
        {
            Ensure.That(context, nameof(context)).IsNotNull();

            _context = context;
            _claimsRepository = new ClaimsRepository(context);
            _coversRepository = new CoversRepository(context);
        }

        public IClaimsRepository GetClaims()
        {
            return _claimsRepository;
        }

        public ICoversRepository GetCovers()
        {
            return _coversRepository;
        }

        public async Task Commit()
        {
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        public void Dispose()
        {
            _context?.Dispose();
        }

        public async Task Reject()
        {

            foreach (var item in _context.ChangeTracker.Entries().Where(e => e.State != EntityState.Unchanged))
            {
                switch (item.State)
                {
                    case EntityState.Added:
                        item.State = EntityState.Detached;
                        break;
                    case EntityState.Deleted:
                    case EntityState.Modified:
                        await item.ReloadAsync().ConfigureAwait(false);
                        break;
                }
            }
        }
    }
}
