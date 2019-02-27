using System.Collections.Concurrent;
using System.Collections.Generic;
using Microsoft.Azure.Documents.Client;

namespace Cosmonaut.Scaler.Server
{
    public class CosmonautClientHolder
    {
        public IDictionary<string, ICosmonautClient> CosmonautClients { get; } = new Dictionary<string, ICosmonautClient>();

        public void AddClient(ICosmonautClient client)
        {
            var cosmosAccountEndpoint = client.DocumentClient.ServiceEndpoint.OriginalString.TrimEnd('/');
            if (!CosmonautClients.ContainsKey(cosmosAccountEndpoint))
                CosmonautClients.Add(cosmosAccountEndpoint, client);
        }

        public void AddClient(string serviceEndpoint, string key)
        {
            var cosmosAccountEndpoint = serviceEndpoint.TrimEnd('/');
            if (CosmonautClients.ContainsKey(cosmosAccountEndpoint))
                return;
            
            var cosmosClient = new CosmonautClient(cosmosAccountEndpoint, key, new ConnectionPolicy{ConnectionProtocol = Protocol.Tcp, ConnectionMode = ConnectionMode.Direct});
            CosmonautClients.Add(cosmosAccountEndpoint, cosmosClient);
        }

        public ICosmonautClient GetClient(string uri)
        {
            return CosmonautClients[uri.TrimEnd('/')];
        }
    }
}