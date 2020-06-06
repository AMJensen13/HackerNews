namespace HackerNews.Domain.Models.CosmosDb
{
    public class CosmosDbConfig
    {
        public string DatabaseName { get; set; }
        public string ContainerName { get; set; }
        public string ConnectionString { get; set; }
        public string PartitionKey { get; set; }
    }
}
