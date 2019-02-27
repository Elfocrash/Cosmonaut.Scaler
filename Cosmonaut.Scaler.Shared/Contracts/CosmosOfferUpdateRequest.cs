namespace Cosmonaut.Scaler.Shared.Contracts
{
    public class CosmosOfferUpdateRequest
    {
        public string AccountKey { get; set; }

        public string OfferId { get; set; }

        public int Throughput { get; set; }

        public CosmosOfferUpdateRequest(string accountKey, string offerId, int throughput)
        {
            AccountKey = accountKey;
            OfferId = offerId;
            Throughput = throughput;
        }
    }
}