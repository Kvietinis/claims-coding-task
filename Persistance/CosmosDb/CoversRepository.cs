using Claims.Persistance.Abstractions;
using Claims.Persistance.Abstractions.Models;
using EnsureThat;

namespace Claims.Persistance.CosmosDb
{
    public class CoversRepository : ICoversRepository
    {
        private readonly ClaimsCosmosContext _context;

        public CoversRepository(ClaimsCosmosContext context)
        {
            Ensure.That(context, nameof(context)).IsNotNull();

            _context = context;
        }

        public async Task Create(Cover cover)
        {
            await _context.Create(cover).ConfigureAwait(false);
        }

        public async Task Delete(string id)
        {
            await _context.Delete<Cover>(id).ConfigureAwait(false);
        }

        public async Task<Cover[]> Get()
        {
            var result = await _context.Get<Cover>().ConfigureAwait(false);

            return result;
        }

        public async Task<Cover> Get(string id)
        {
            var result = await _context.Get<Cover>(id).ConfigureAwait(false);

            return result;
        }
    }
}
