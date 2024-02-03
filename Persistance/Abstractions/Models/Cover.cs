using Claims.Contracts;

namespace Claims.Persistance.Abstractions.Models
{
    public class Cover : BaseStringIdModel
    {
        public DateOnly StartDate { get; set; }

        public DateOnly EndDate { get; set; }

        public CoverType Type { get; set; }

        public decimal Premium { get; set; }
    }
}
