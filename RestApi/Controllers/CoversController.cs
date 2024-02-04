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
    //private readonly ILogger<CoversController> _logger;
    //private readonly Container _container;
    private readonly IAuditer _auditer;
    private readonly ICoversService _coversService;

    //public CoversController(CosmosClient cosmosClient, AuditContext auditContext, ILogger<CoversController> logger)
    //{
    //    _logger = logger;
    //    _auditer = new Auditer(auditContext);
    //    _container = cosmosClient?.GetContainer("ClaimDb", "Cover")
    //                 ?? throw new ArgumentNullException(nameof(cosmosClient));
    //}

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

    //private decimal ComputePremium(DateOnly startDate, DateOnly endDate, CoverType coverType)
    //{
    //    var multiplier = 1.3m;
    //    if (coverType == CoverType.Yacht)
    //    {
    //        multiplier = 1.1m;
    //    }

    //    if (coverType == CoverType.PassengerShip)
    //    {
    //        multiplier = 1.2m;
    //    }

    //    if (coverType == CoverType.Tanker)
    //    {
    //        multiplier = 1.5m;
    //    }

    //    var premiumPerDay = 1250 * multiplier;
    //    var insuranceLength = endDate.DayNumber - startDate.DayNumber;
    //    var totalPremium = 0m;

    //    for (var i = 0; i < insuranceLength; i++)
    //    {
    //        if (i < 30) totalPremium += premiumPerDay;
    //        if (i < 180 && coverType == CoverType.Yacht) totalPremium += premiumPerDay - premiumPerDay * 0.05m;
    //        else if (i < 180) totalPremium += premiumPerDay - premiumPerDay * 0.02m;
    //        if (i < 365 && coverType != CoverType.Yacht) totalPremium += premiumPerDay - premiumPerDay * 0.03m;
    //        else if (i < 365) totalPremium += premiumPerDay - premiumPerDay * 0.08m;
    //    }

    //    return totalPremium;
    //}
}