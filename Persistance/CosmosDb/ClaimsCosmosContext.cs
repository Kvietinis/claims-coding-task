using Claims.Contracts;
using Claims.Persistance.Abstractions.Models;
using EnsureThat;
using Microsoft.Azure.Cosmos;

namespace Claims.Persistance.CosmosDb
{
    public class ClaimsCosmosContext
    {
        private readonly CosmosDbConnectionSettings _settings;
        private readonly CosmosClient _client;
        private readonly Container _container;

        public ClaimsCosmosContext(CosmosDbConnectionSettings settings)
        {
            Ensure.That(settings, nameof(settings)).IsNotNull();

            _settings = settings;
            _client = new CosmosClient(_settings.Account, _settings.Key);

            _container = _client.GetContainer(_settings.Database, _settings.Container);
        }

        public async Task CreateDatabase()
        {
            var db = await _client.CreateDatabaseIfNotExistsAsync(_settings.Database).ConfigureAwait(false);

            await db.Database.CreateContainerIfNotExistsAsync(_settings.Container, "/id").ConfigureAwait(false);
        }

        public async Task<T[]> Get<T>() where T : BaseStringIdModel
        {
            var query = _container.GetItemQueryIterator<T>(new QueryDefinition("SELECT * FROM c"));
            var results = new List<T>();

            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync().ConfigureAwait(false);

                results.AddRange(response.ToList());
            }

            return results.ToArray();
        }

        public async Task<T> Get<T>(string id) where T : BaseStringIdModel
        {
            try
            {
                var response = await _container.ReadItemAsync<T>(id, new PartitionKey(id)).ConfigureAwait(false);
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        public async Task Create<T>(T item) where T : BaseStringIdModel
        {
            await _container.CreateItemAsync(item, new PartitionKey(item.Id)).ConfigureAwait(false);
        }

        public async Task Delete<T>(string id) where T : BaseStringIdModel
        {
            await _container.DeleteItemAsync<T>(id, new PartitionKey(id)).ConfigureAwait(false);
        }
    }
}
