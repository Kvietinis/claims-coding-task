using Claims.Business.Abstractions;
using Claims.Contracts;
using Claims.Persistance.Abstractions;
using Claims.Persistance.Abstractions.Models;
using EnsureThat;

namespace Claims.Business.Implementations
{
    public class CoversService : ICoversService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CoversService(IUnitOfWork unitOfWork)
        {
            Ensure.That(unitOfWork, nameof(unitOfWork)).IsNotNull();

            _unitOfWork = unitOfWork;
        }

        public async Task Create(CoverDto claim)
        {
            var repository = _unitOfWork.GetCovers();
            var entity = ToEntity(claim);

            await repository.Create(entity).ConfigureAwait(false);
        }

        public async Task Delete(string id)
        {
            var repository = _unitOfWork.GetCovers();

            await repository.Delete(id).ConfigureAwait(false);
        }

        public async Task<CoverDto[]> Get()
        {
            var repository = _unitOfWork.GetCovers();
            var entities = await repository.Get().ConfigureAwait(false);
            var result = entities.Select(ToDto).ToArray();

            return result;
        }

        public async Task<CoverDto> Get(string id)
        {
            var repository = _unitOfWork.GetCovers();
            var entity = await repository.Get(id).ConfigureAwait(false);
            var result = ToDto(entity);

            return result;
        }

        private static Cover ToEntity(CoverDto cover)
        {
            var result = new Cover
            {
                Id = cover.Id,
                EndDate = cover.EndDate,
                Premium = cover.Premium,
                StartDate = cover.StartDate,
                Type = cover.Type
            };

            return result;
        }

        private static CoverDto ToDto(Cover cover)
        {
            var result = new CoverDto
            {
                Id = cover.Id,
                EndDate = cover.EndDate,
                Premium = cover.Premium,
                StartDate = cover.StartDate,
                Type = cover.Type
            };

            return result;
        }
    }
}
