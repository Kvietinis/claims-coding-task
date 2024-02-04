using Claims.Auditing.Abstractions;
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
        private readonly IAuditer _auditer;
        private readonly IClaimsService _claimsService;

        public ClaimsController(IClaimsService claimsService, IAuditer auditer)
        {
            Ensure.That(claimsService, nameof(claimsService)).IsNotNull();
            Ensure.That(auditer, nameof(auditer)).IsNotNull();

            _claimsService = claimsService;
            _auditer = auditer;
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
        public async Task<ActionResult<ClaimDto>> CreateAsync(ClaimDto claim)
        {
            var result = await _claimsService.Create(claim);
            var url = $"{nameof(ClaimsController)}/{{{result.Id}}}";

            _auditer.AuditClaim(claim.Id, "POST");

            return Created(url, result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(string id)
        {
            await _claimsService.Delete(id);

            _auditer.AuditClaim(id, "DELETE");

            return NoContent();
        }
    }
}