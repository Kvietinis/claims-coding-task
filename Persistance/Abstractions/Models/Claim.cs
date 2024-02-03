using Claims.Contracts;

namespace Claims.Persistance.Abstractions.Models
{
    public class Claim : BaseStringIdModel
    {
        public string CoverId { get; set; }

        public DateTime Created { get; set; }

        public string Name { get; set; }

        public ClaimType Type { get; set; }

        public decimal DamageCost { get; set; }
    }
}
