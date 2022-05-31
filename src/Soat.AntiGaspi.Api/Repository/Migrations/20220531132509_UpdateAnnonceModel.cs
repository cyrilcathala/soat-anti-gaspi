using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Soat.AntiGaspi.Api.Migrations
{
    public partial class UpdateAnnonceModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                schema: "antigaspi",
                table: "Annonces",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "Availability",
                schema: "antigaspi",
                table: "Annonces",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyName",
                schema: "antigaspi",
                table: "Annonces",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                schema: "antigaspi",
                table: "Annonces",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                schema: "antigaspi",
                table: "Annonces",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "Expiration",
                schema: "antigaspi",
                table: "Annonces",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                schema: "antigaspi",
                table: "Annonces",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                schema: "antigaspi",
                table: "Annonces",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                schema: "antigaspi",
                table: "Annonces");

            migrationBuilder.DropColumn(
                name: "Availability",
                schema: "antigaspi",
                table: "Annonces");

            migrationBuilder.DropColumn(
                name: "CompanyName",
                schema: "antigaspi",
                table: "Annonces");

            migrationBuilder.DropColumn(
                name: "Description",
                schema: "antigaspi",
                table: "Annonces");

            migrationBuilder.DropColumn(
                name: "Email",
                schema: "antigaspi",
                table: "Annonces");

            migrationBuilder.DropColumn(
                name: "Expiration",
                schema: "antigaspi",
                table: "Annonces");

            migrationBuilder.DropColumn(
                name: "Status",
                schema: "antigaspi",
                table: "Annonces");

            migrationBuilder.DropColumn(
                name: "Title",
                schema: "antigaspi",
                table: "Annonces");
        }
    }
}
