using Microsoft.EntityFrameworkCore.Migrations;

namespace Cosmonaut.Scaler.Server.Migrations
{
    public partial class Added_Cosmos_Accounts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CosmosAccounts",
                columns: table => new
                {
                    Endpoint = table.Column<string>(nullable: false),
                    MasterKey = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CosmosAccounts", x => x.Endpoint);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CosmosAccounts");
        }
    }
}
