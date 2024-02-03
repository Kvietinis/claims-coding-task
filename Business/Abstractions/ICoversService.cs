using Claims.Contracts;

namespace Claims.Business.Abstractions
{
    public interface ICoversService
    {
        Task<CoverDto[]> Get();

        Task<CoverDto> Get(string id);

        Task Create(CoverDto claim);

        Task Delete(string id);
    }
}
