using Claims.Contracts;

namespace Claims.Business.Abstractions
{
    public interface IClaimsService
    {
        Task<ClaimDto[]> Get();

        Task<ClaimDto> Get(string id);

        Task Create(ClaimDto claim);

        Task Delete(string id);
    }
}
