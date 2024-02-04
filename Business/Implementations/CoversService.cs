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

        public async Task<CoverDto> Create(CoverDto claim)
        {
            var repository = _unitOfWork.GetCovers();
            var entity = ToEntity(claim);

            entity.Id = Guid.NewGuid().ToString();
            entity.Premium = ComputePremium(entity.StartDate, entity.EndDate, entity.Type);

            await repository.Create(entity).ConfigureAwait(false);

            var result = ToDto(entity);

            return result;
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

        public decimal ComputePremium(DateOnly startDate, DateOnly endDate, CoverType coverType)
        {
            var multiplier = 1.3m;

            if (coverType == CoverType.Yacht)
            {
                multiplier = 1.1m;
            }

            if (coverType == CoverType.PassengerShip)
            {
                multiplier = 1.2m;
            }

            if (coverType == CoverType.Tanker)
            {
                multiplier = 1.5m;
            }

            var premiumPerDay = 1250 * multiplier;
            var insuranceLength = endDate.DayNumber - startDate.DayNumber;
            var totalPremium = 0m;

            for (var i = 0; i < insuranceLength; i++)
            {
                if (i < 30)
                {
                    totalPremium += premiumPerDay;
                }

                if (i < 180 && coverType == CoverType.Yacht)
                {
                    totalPremium += premiumPerDay - premiumPerDay * 0.05m;
                }
                else if (i < 180)
                {
                    totalPremium += premiumPerDay - premiumPerDay * 0.02m;
                }

                if (i < 365 && coverType != CoverType.Yacht)
                {
                    totalPremium += premiumPerDay - premiumPerDay * 0.03m;
                }
                else if (i < 365)
                {
                    totalPremium += premiumPerDay - premiumPerDay * 0.08m;
                }
            }

            return totalPremium;
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
