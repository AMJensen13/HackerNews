using HackerNews.Domain.Exceptions;
using HackerNews.Domain.Interfaces;
using HackerNews.Domain.Models.CosmosDb;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Extensions.Options;

namespace HackerNews.Data.Services
{
    public class CosmosDbService : ICosmosDbService
    {
        private readonly CosmosDbConfig _config;
        private readonly CosmosClient _client;

        public CosmosDbService(IOptions<CosmosDbConfig> _configOptions)
        {
            _config = _configOptions.Value;

            var clientBuilder = new CosmosClientBuilder(_config.ConnectionString);
            _client = clientBuilder.WithConnectionModeDirect()
                                   .Build();

            var database = _client.GetDatabase(_config.DatabaseName);

            if (database == null)
            {
                throw new CosmosDbException($"Database '{_config.DatabaseName}' does not exist.");
            }

            if (database.GetContainer(_config.ContainerName) == null)
            {
                throw new CosmosDbException($"Container '{_config.ContainerName}' does not exist.");
            }
        }



    }
}
