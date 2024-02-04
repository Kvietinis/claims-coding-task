using Claims.Business.Abstractions;
using Claims.Contracts;
using Claims.Persistance.Abstractions;
using Claims.Persistance.Abstractions.Models;
using EnsureThat;

namespace Claims.Business.Implementations
{
    public class ClaimsService : IClaimsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ClaimsService(IUnitOfWork unitOfWork)
        {
            Ensure.That(unitOfWork, nameof(unitOfWork)).IsNotNull();

            _unitOfWork = unitOfWork;
        }

        public async Task<ClaimDto> Create(ClaimDto claim)
        {
            var repository = _unitOfWork.GetClaims();
            var entity = ToEntity(claim);

            entity.Id = Guid.NewGuid().ToString();

            await repository.Create(entity).ConfigureAwait(false);
            await _unitOfWork.Commit().ConfigureAwait(false);

            var result = ToDto(entity);

            return result;
        }

        public async Task Delete(string id)
        {
            var repository = _unitOfWork.GetClaims();

            await repository.Delete(id).ConfigureAwait(false);
            await _unitOfWork.Commit().ConfigureAwait(false);
        }

        public async Task<ClaimDto[]> Get()
        {
            var repository = _unitOfWork.GetClaims();
            var entities = await repository.Get().ConfigureAwait(false);
            var result = entities.Select(ToDto).ToArray();

            return result;
        }

        public async Task<ClaimDto> Get(string id)
        {
            var repository = _unitOfWork.GetClaims();
            var entity = await repository.Get(id).ConfigureAwait(false);
            var result = ToDto(entity);

            return result;
        }

        private static Claim ToEntity(ClaimDto claim)
        {
            var result = new Claim
            {
                Id = claim.Id,
                CoverId = claim.CoverId,
                Created = claim.Created,
                DamageCost = claim.DamageCost,
                Name = claim.Name,
                Type = claim.Type
            };

            return result;
        }

        private static ClaimDto ToDto(Claim claim)
        {
            var result = new ClaimDto
            {
                Id = claim.Id,
                CoverId = claim.CoverId,
                Created = claim.Created,
                DamageCost = claim.DamageCost,
                Name = claim.Name,
                Type = claim.Type
            };

            return result;
        }
    }
}
