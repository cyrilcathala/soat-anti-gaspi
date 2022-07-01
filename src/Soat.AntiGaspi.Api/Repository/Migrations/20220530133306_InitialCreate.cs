using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Soat.AntiGaspi.Api.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "antigaspi");

            migrationBuilder.CreateTable(
                name: "Offers",
                schema: "antigaspi",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Offers", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Offers",
                schema: "antigaspi");
        }
    }
}
