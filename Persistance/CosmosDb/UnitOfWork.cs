using Claims.Persistance.Abstractions;
using EnsureThat;

namespace Claims.Persistance.CosmosDb
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IClaimsRepository _claimsRepository;
        private readonly ICoversRepository _coversRepository;

        public UnitOfWork(ClaimsCosmosContext context)
        {
            Ensure.That(context, nameof(context)).IsNotNull();

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

        public Task Commit()
        {
            return Task.CompletedTask;
        }

        public Task Reject()
        {
            return Task.CompletedTask;
        }

        public void Dispose()
        {
        }
    }
}
