
using Claims.Persistance.Abstractions.Models;

namespace Claims.Persistance.Abstractions
{
    public interface IClaimsRepository
    {
        Task<Claim[]> Get();

        Task<Claim> Get(string id);

        Task Create(Claim claim);

        Task Delete(string id);
    }
}
