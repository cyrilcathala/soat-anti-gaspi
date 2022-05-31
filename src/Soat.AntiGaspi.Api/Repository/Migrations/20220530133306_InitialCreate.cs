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
                name: "Annonces",
                schema: "antigaspi",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Annonces", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Annonces",
                schema: "antigaspi");
        }
    }
}
