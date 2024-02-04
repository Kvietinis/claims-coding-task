using Claims.ActionFilters;
using Claims.Business.Abstractions;
using Claims.Contracts;
using EnsureThat;
using Microsoft.AspNetCore.Mvc;

namespace Claims.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClaimsController : ControllerBase
    {
        private readonly IClaimsService _claimsService;

        public ClaimsController(IClaimsService claimsService)
        {
            Ensure.That(claimsService, nameof(claimsService)).IsNotNull();

            _claimsService = claimsService;
        }

        [HttpGet]
        public async Task<ActionResult<ClaimDto[]>> GetAsync()
        {
            var result = await _claimsService.Get();

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ClaimDto>> GetAsync(string id)
        {
            var result = await _claimsService.Get(id);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPost]
        [ServiceFilter(typeof(AuditingActionFilterAttribute))]
        public async Task<ActionResult<ClaimDto>> CreateAsync(ClaimDto claim)
        {
            var result = await _claimsService.Create(claim);
            var url = $"{nameof(ClaimsController)}/{{{result.Id}}}";

            return Created(url, result);
        }

        [HttpDelete("{id}")]
        [ServiceFilter(typeof(AuditingActionFilterAttribute))]
        public async Task<ActionResult> DeleteAsync(string id)
        {
            await _claimsService.Delete(id);

            return NoContent();
        }
    }
}