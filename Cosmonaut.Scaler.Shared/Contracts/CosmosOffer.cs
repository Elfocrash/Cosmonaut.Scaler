namespace Cosmonaut.Scaler.Shared.Contracts
{
    public class CosmosOffer
    {
        public string Id { get; set; }

        public string ResourceId { get; set; }

        public int Throughput { get; set; }
    }
}