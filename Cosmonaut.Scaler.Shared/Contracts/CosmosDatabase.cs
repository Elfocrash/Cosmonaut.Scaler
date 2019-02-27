using System.Collections.Generic;

namespace Cosmonaut.Scaler.Shared.Contracts
{
    public class CosmosDatabase
    {
        public string Id { get; set; }

        public string ResourceId { get; set; }

        public List<CosmosCollection> Collections { get; set; }

        public CosmosOffer Offer { get; set; }

        public bool HasOffer => Offer != null;
    }
}