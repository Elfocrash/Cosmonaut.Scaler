using System.Collections.Generic;
using System.Threading.Tasks;
using Cosmonaut.Scaler.Server.Data.Models;

namespace Cosmonaut.Scaler.Server.Services
{
    public interface ICosmosService
    {
        Task<List<CosmosAccount>> GetAllCosmosAccounts();

        Task<CosmosAccount> GetCosmosAccount(string accountUri);

        Task AddCosmosAccount(string accountUri, string accountKey);
    }
}