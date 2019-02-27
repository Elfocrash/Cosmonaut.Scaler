using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace Cosmonaut.Scaler.Server.Data.Models
{
    public class CosmosAccount
    {
        [Key]
        public string Endpoint { get; set; }

        public string MasterKey { get; set; }
    }
}