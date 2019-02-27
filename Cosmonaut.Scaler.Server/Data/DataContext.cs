using Cosmonaut.Scaler.Server.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Cosmonaut.Scaler.Server.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
            base.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public DbSet<CosmosAccount> CosmosAccounts { get; set; }
    }
}