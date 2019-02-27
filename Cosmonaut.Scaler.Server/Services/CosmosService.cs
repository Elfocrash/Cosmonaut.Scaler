using System.Collections.Generic;
using System.Threading.Tasks;
using Cosmonaut.Scaler.Server.Data;
using Cosmonaut.Scaler.Server.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Cosmonaut.Scaler.Server.Services
{
    public class CosmosService : ICosmosService
    {
        private readonly DataContext _dataContext;

        public CosmosService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<List<CosmosAccount>> GetAllCosmosAccounts()
        {
            return await _dataContext.CosmosAccounts.ToListAsync();
        }

        public async Task<CosmosAccount> GetCosmosAccount(string accountUri)
        {
            return await _dataContext.CosmosAccounts.SingleOrDefaultAsync(x => x.Endpoint == accountUri.TrimEnd('/'));
        }

        public async Task AddCosmosAccount(string accountUri, string accountKey)
        {
            var account = new CosmosAccount
            {
                Endpoint = accountUri.TrimEnd('/'),
                MasterKey = accountKey
            };
            await _dataContext.AddAsync(account);
            await _dataContext.SaveChangesAsync();
        }
    }
}