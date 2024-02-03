using Claims.Persistance.Abstractions.Models;

namespace Claims.Persistance.Abstractions
{
    public interface ICoversRepository
    {
        Task<Cover[]> Get();

        Task<Cover> Get(string id);

        Task Create(Cover cover);

        Task Delete(string id);
    }
}
