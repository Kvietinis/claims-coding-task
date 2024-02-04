using Claims.Auditing.Abstractions;
using Claims.Business.Abstractions;
using Claims.Contracts;
using EnsureThat;
using Microsoft.AspNetCore.Mvc;

namespace Claims.Controllers;

[ApiController]
[Route("[controller]")]
public class CoversController : ControllerBase
{
    private readonly IAuditer _auditer;
    private readonly ICoversService _coversService;

    public CoversController(ICoversService coversService, IAuditer auditer)
    {
        Ensure.That(coversService, nameof(coversService)).IsNotNull();
        Ensure.That(auditer, nameof(auditer)).IsNotNull();

        _coversService = coversService;
        _auditer = auditer;
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
    public async Task<ActionResult<CoverDto>> CreateAsync(CoverDto cover)
    {
        var result = await _coversService.Create(cover);
        var url = $"{nameof(CoversController)}/{{{result.Id}}}";

        _auditer.AuditCover(cover.Id, "POST");

        return Created(url, result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAsync(string id)
    {
        await _coversService.Delete(id);

        _auditer.AuditCover(id, "DELETE");

        return NoContent();
    }
}