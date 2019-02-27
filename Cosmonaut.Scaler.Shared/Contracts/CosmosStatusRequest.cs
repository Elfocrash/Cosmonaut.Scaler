namespace Cosmonaut.Scaler.Shared.Contracts
{
    public class CosmosStatusRequest
    {
        public string AccountUri { get; set; }

        public string AccountKey { get; set; }

        public CosmosStatusRequest(string accountUri, string accountKey)
        {
            AccountUri = accountUri;
            AccountKey = accountKey;
        }
    }
}