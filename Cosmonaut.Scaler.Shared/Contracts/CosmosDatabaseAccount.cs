using System.Collections.Generic;
using System.Linq;

namespace Cosmonaut.Scaler.Shared.Contracts
{
    public class CosmosDatabaseAccount
    {
        public string ServiceUrl { get; set; }

        public int DatabaseCount => Databases.Count;

        public int TotalThroughput => Databases.Where(x => x.HasOffer).Select(x=>x.Offer.Throughput).Sum() + Databases.SelectMany(x => x.Collections).Where(x => x.HasOffer)
            .Select(x => x.Offer.Throughput).Sum();

        public List<CosmosDatabase> Databases { get; set; } = new List<CosmosDatabase>();
    }
}