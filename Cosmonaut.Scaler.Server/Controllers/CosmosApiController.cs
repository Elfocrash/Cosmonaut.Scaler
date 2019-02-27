using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Cosmonaut.Scaler.Server.Data.Models;
using Cosmonaut.Scaler.Server.Services;
using Cosmonaut.Scaler.Shared;
using Cosmonaut.Scaler.Shared.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;

namespace Cosmonaut.Scaler.Server.Controllers
{
    public class CosmosApiController : Controller
    {
        private readonly CosmonautClientHolder _clientHolder;
        private readonly ICosmosService _cosmosService;

        public CosmosApiController(CosmonautClientHolder clientHolder, ICosmosService cosmosService)
        {
            _clientHolder = clientHolder;
            _cosmosService = cosmosService;
        }

        [HttpGet("api/cosmos")]
        public async Task<IActionResult> LoadAll()
        {
            var cosmosAccounts = await _cosmosService.GetAllCosmosAccounts();
            var response = new List<CosmosDatabaseAccount>();
           
            foreach (var account in cosmosAccounts)
            {
                var databases = new List<CosmosDatabase>();
                var cosmosClient = _clientHolder.GetClient(account.Endpoint);
                var allOffers = (await cosmosClient.QueryOffersV2Async()).ToList();
                var dbs = await cosmosClient.QueryDatabasesAsync();
                foreach (var db in dbs)
                {
                    var databaseOffer = allOffers.SingleOrDefault(x => x.ResourceLink == db.SelfLink);
                    var database = new CosmosDatabase
                    {
                        Id = db.Id,
                        ResourceId = db.ResourceId,
                        Collections = new List<CosmosCollection>(),
                        Offer = databaseOffer != null ? new CosmosOffer
                        {
                            Id = databaseOffer.Id,
                            ResourceId = databaseOffer.ResourceId,
                            Throughput = databaseOffer.Content.OfferThroughput
                        } : null
                    };

                    var collections = new List<CosmosCollection>();
                    var documentCollections = await cosmosClient.QueryCollectionsAsync(database.Id);
                    foreach (var documentCollection in documentCollections)
                    {
                        var collectionOffer = allOffers.SingleOrDefault(x => x.ResourceLink == documentCollection.SelfLink);

                        var collection = new CosmosCollection
                        {
                            Id = documentCollection.Id,
                            ResourceId = documentCollection.ResourceId,
                            PartitionKey = documentCollection.PartitionKey?.Paths?.FirstOrDefault() ?? "",
                            DatabaseHasOffer = database.HasOffer,
                            Offer = collectionOffer != null ? new CosmosOffer
                            {
                                Id = collectionOffer.Id,
                                ResourceId = collectionOffer.ResourceId,
                                Throughput = collectionOffer.Content.OfferThroughput
                            } : null
                        };
                        collections.Add(collection);
                    }

                    database.Collections = collections;
                    databases.Add(database);
                }

                var databaseAccount = new CosmosDatabaseAccount
                {
                    Databases = databases,
                    ServiceUrl = account.Endpoint
                };
                response.Add(databaseAccount);
            }

            return Ok(response);
        }

        [HttpPost("api/cosmos/dbaccounts")]
        public async Task<IActionResult> CheckAccountStatus([FromBody] CosmosStatusRequest request)
        {
            try
            {
                var pingClient = new CosmonautClient(request.AccountUri, request.AccountKey);
                await pingClient.DocumentClient.GetDatabaseAccountAsync();

                var cosmosAccount = await _cosmosService.GetCosmosAccount(request.AccountUri);
                if (cosmosAccount != null)
                    return Ok(new StatusResponse("It looks like this account already exists."));

                await _cosmosService.AddCosmosAccount(request.AccountUri, request.AccountKey);
                _clientHolder.AddClient(request.AccountUri, request.AccountKey);
                return Ok(new StatusResponse(true));
            }
            catch
            {
                return Ok(new StatusResponse("It looks like the Uri or the Master Key are invalid."));
            }
        }

        [HttpPost("api/cosmos/offers")]
        public async Task<IActionResult> UpdateOffer([FromBody] CosmosOfferUpdateRequest request)
        {
            try
            {
                var client = _clientHolder.GetClient(request.AccountKey);
                var offers = (await client.QueryOffersV2Async(x => x.Id == request.OfferId)).ToList();
                if (!offers.Any())
                    return Ok(new OfferUpdateResponse { Message = "This offer doesn't exist!" });

                var offer = offers.Single();
                var updated = await client.UpdateOfferAsync(new OfferV2(offer, request.Throughput));

                return Ok(new OfferUpdateResponse {IsSuccess = true, Message = "Successfully updated the throughput."});
            }
            catch(DocumentClientException exception)
            {
                return Ok(new OfferUpdateResponse { Message = exception.Error.Message });
            }
        }
    }
}