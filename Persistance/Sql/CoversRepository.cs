using Claims.Persistance.Abstractions;
using Claims.Persistance.Abstractions.Models;
using EnsureThat;
using Microsoft.EntityFrameworkCore;

namespace Claims.Persistance.Sql
{
    public class CoversRepository : ICoversRepository
    {
        private readonly ClaimsSqlDbContext _context;

        public CoversRepository(ClaimsSqlDbContext context)
        {
            Ensure.That(context, nameof(context)).IsNotNull();

            _context = context;
        }

        public async Task Create(Cover cover)
        {
            await _context.Set<Cover>().AddAsync(cover).ConfigureAwait(false);
        }

        public async Task Delete(string id)
        {
            var entity = await _context.FindAsync<Cover>(new object[1] { id }).ConfigureAwait(false);

            if (entity != null)
            {
                _context.Remove(entity);
            }
        }

        public async Task<Cover[]> Get()
        {
            var result = await _context.Set<Cover>().ToArrayAsync().ConfigureAwait(false);

            return result;
        }

        public async Task<Cover> Get(string id)
        {
            var result = await _context.FindAsync<Cover>(new object[1] { id }).ConfigureAwait(false);

            return result;
        }
    }
}
