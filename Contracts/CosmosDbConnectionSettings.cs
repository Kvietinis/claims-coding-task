namespace Claims.Contracts
{
    public class CosmosDbConnectionSettings
    {
        public string Account { get; set; }

        public string Key { get; set; }

        public string Database { get; set; }

        public string Container { get; set; }
    }
}
