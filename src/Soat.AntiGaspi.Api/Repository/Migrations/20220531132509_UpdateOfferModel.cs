﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Soat.AntiGaspi.Api.Migrations
{
    public partial class UpdateOfferModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                schema: "antigaspi",
                table: "Offers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "Availability",
                schema: "antigaspi",
                table: "Offers",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyName",
                schema: "antigaspi",
                table: "Offers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                schema: "antigaspi",
                table: "Offers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                schema: "antigaspi",
                table: "Offers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "Expiration",
                schema: "antigaspi",
                table: "Offers",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                schema: "antigaspi",
                table: "Offers",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                schema: "antigaspi",
                table: "Offers",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                schema: "antigaspi",
                table: "Offers");

            migrationBuilder.DropColumn(
                name: "Availability",
                schema: "antigaspi",
                table: "Offers");

            migrationBuilder.DropColumn(
                name: "CompanyName",
                schema: "antigaspi",
                table: "Offers");

            migrationBuilder.DropColumn(
                name: "Description",
                schema: "antigaspi",
                table: "Offers");

            migrationBuilder.DropColumn(
                name: "Email",
                schema: "antigaspi",
                table: "Offers");

            migrationBuilder.DropColumn(
                name: "Expiration",
                schema: "antigaspi",
                table: "Offers");

            migrationBuilder.DropColumn(
                name: "Status",
                schema: "antigaspi",
                table: "Offers");

            migrationBuilder.DropColumn(
                name: "Title",
                schema: "antigaspi",
                table: "Offers");
        }
    }
}
