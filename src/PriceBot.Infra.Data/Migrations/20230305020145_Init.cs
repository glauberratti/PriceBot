using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PriceBot.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    BRLValue = table.Column<decimal>(type: "TEXT", precision: 28, scale: 3, nullable: false, defaultValue: 0m),
                    USDValue = table.Column<decimal>(type: "TEXT", precision: 28, scale: 3, nullable: false, defaultValue: 0m),
                    EURValue = table.Column<decimal>(type: "TEXT", precision: 28, scale: 3, nullable: false, defaultValue: 0m)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
