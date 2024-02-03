using Claims.Persistance.Abstractions;
using Claims.Persistance.Abstractions.Models;
using EnsureThat;
using Microsoft.EntityFrameworkCore;

namespace Claims.Persistance.Sql
{
    public class ClaimsRepository : IClaimsRepository
    {
        private readonly ClaimsSqlDbContext _context;

        public ClaimsRepository(ClaimsSqlDbContext context)
        {
            Ensure.That(context, nameof(context)).IsNotNull();

            _context = context;
        }

        public async Task Create(Claim claim)
        {
            await _context.Set<Claim>().AddAsync(claim).ConfigureAwait(false);
        }

        public async Task Delete(string id)
        {
            var entity = await _context.FindAsync<Claim>(new object[1] { id }).ConfigureAwait(false);

            if (entity != null)
            {
                _context.Remove(entity);
            }
        }

        public async Task<Claim[]> Get()
        {
            var result = await _context.Set<Claim>().ToArrayAsync().ConfigureAwait(false);

            return result;
        }

        public async Task<Claim> Get(string id)
        {
            var result = await _context.FindAsync<Claim>(new object[1] { id }).ConfigureAwait(false);

            return result;
        }
    }
}
