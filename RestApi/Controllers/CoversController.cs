using Claims.ActionFilters;
using Claims.Business.Abstractions;
using Claims.Contracts;
using EnsureThat;
using Microsoft.AspNetCore.Mvc;

namespace Claims.Controllers;

[ApiController]
[Route("[controller]")]
public class CoversController : ControllerBase
{
    private readonly ICoversService _coversService;

    public CoversController(ICoversService coversService)
    {
        Ensure.That(coversService, nameof(coversService)).IsNotNull();

        _coversService = coversService;
    }

    [HttpGet]
    public async Task<ActionResult<CoverDto[]>> GetAsync()
    {
        var result = await _coversService.Get();

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CoverDto>> GetAsync(string id)
    {
        var result = await _coversService.Get(id);

        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpPost]
    [ServiceFilter(typeof(AuditingActionFilterAttribute))]
    public async Task<ActionResult<CoverDto>> CreateAsync(CoverDto cover)
    {
        var result = await _coversService.Create(cover);
        var url = $"{nameof(CoversController)}/{{{result.Id}}}";

        return Created(url, result);
    }

    [HttpDelete("{id}")]
    [ServiceFilter(typeof(AuditingActionFilterAttribute))]
    public async Task<ActionResult> DeleteAsync(string id)
    {
        await _coversService.Delete(id);

        return NoContent();
    }
}