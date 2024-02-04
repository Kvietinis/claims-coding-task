using Claims.Business.Abstractions;
using Claims.Contracts;
using EnsureThat;
using Microsoft.AspNetCore.Mvc;

namespace Claims.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ComputationsController : ControllerBase
    {
        private readonly ICoversService _coversService;

        public ComputationsController(ICoversService coversService)
        {
            Ensure.That(coversService, nameof(coversService)).IsNotNull();

            _coversService = coversService;
        }

        [HttpPost]
        public ActionResult<decimal> ComputePremiumAsync([FromQuery] DateOnly startDate, [FromQuery] DateOnly endDate, [FromQuery] CoverType coverType)
        {
            var result = _coversService.ComputePremium(startDate, endDate, coverType);

            return Ok(result);
        }
    }
}
