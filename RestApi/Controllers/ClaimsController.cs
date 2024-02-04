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
        
        //private readonly ILogger<ClaimsController> _logger;
        //private readonly CosmosDbService _cosmosDbService;
        private readonly IAuditer _auditer;
        private readonly IClaimsService _claimsService;

        //public ClaimsController(ILogger<ClaimsController> logger, CosmosDbService cosmosDbService, AuditContext auditContext)
        //{
        //    _logger = logger;
        //    _cosmosDbService = cosmosDbService;
        //    _auditer = new Auditer(auditContext);
        //}

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

    //public class CosmosDbService
    //{
    //    private readonly Container _container;

    //    public CosmosDbService(CosmosClient dbClient,
    //        string databaseName,
    //        string containerName)
    //    {
    //        if (dbClient == null) throw new ArgumentNullException(nameof(dbClient));
    //        _container = dbClient.GetContainer(databaseName, containerName);
    //    }

    //    public async Task<IEnumerable<Claim>> GetClaimsAsync()
    //    {
    //        var query = _container.GetItemQueryIterator<Claim>(new QueryDefinition("SELECT * FROM c"));
    //        var results = new List<Claim>();
    //        while (query.HasMoreResults)
    //        {
    //            var response = await query.ReadNextAsync();

    //            results.AddRange(response.ToList());
    //        }
    //        return results;
    //    }

    //    public async Task<Claim> GetClaimAsync(string id)
    //    {
    //        try
    //        {
    //            var response = await _container.ReadItemAsync<Claim>(id, new PartitionKey(id));
    //            return response.Resource;
    //        }
    //        catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
    //        {
    //            return null;
    //        }
    //    }

    //    public Task AddItemAsync(Claim item)
    //    {
    //        return _container.CreateItemAsync(item, new PartitionKey(item.Id));
    //    }

    //    public Task DeleteItemAsync(string id)
    //    {
    //        return _container.DeleteItemAsync<Claim>(id, new PartitionKey(id));
    //    }
    //}
}