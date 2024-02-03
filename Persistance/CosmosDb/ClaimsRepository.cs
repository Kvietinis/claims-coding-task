using Claims.Persistance.Abstractions;
using Claims.Persistance.Abstractions.Models;
using EnsureThat;

namespace Claims.Persistance.CosmosDb
{
    public class ClaimsRepository : IClaimsRepository
    {
        private readonly ClaimsCosmosContext _context;

        public ClaimsRepository(ClaimsCosmosContext context)
        {
            Ensure.That(context, nameof(context)).IsNotNull();

            _context = context;
        }

        public async Task Create(Claim claim)
        {
            await _context.Create(claim).ConfigureAwait(false);
        }

        public async Task Delete(string id)
        {
            await _context.Delete<Claim>(id).ConfigureAwait(false);
        }

        public async Task<Claim[]> Get()
        {
            var result = await _context.Get<Claim>().ConfigureAwait(false);

            return result;
        }

        public async Task<Claim> Get(string id)
        {
            var result = await _context.Get<Claim>(id).ConfigureAwait(false);

            return result;
        }
    }
}
