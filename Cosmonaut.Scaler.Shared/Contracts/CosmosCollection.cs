namespace Cosmonaut.Scaler.Shared.Contracts
{
    public class CosmosCollection
    {
        public string Id { get; set; }

        public string ResourceId { get; set; }

        public bool IsPartitioned => !string.IsNullOrEmpty(PartitionKey);

        public string PartitionKey { get; set; }

        public CosmosOffer Offer { get; set; }

        public bool HasOffer => Offer != null;

        public bool HasDedicatedThroughput => DatabaseHasOffer && HasOffer;

        public bool DatabaseHasOffer { get; set; }
    }
}