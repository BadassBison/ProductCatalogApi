using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Product_Catalog_Api.Migrations
{
    public partial class MyFirstMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "products",
                columns: table => new
                {
                    productId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(nullable: true),
                    price = table.Column<double>(nullable: false),
                    cost = table.Column<double>(nullable: false),
                    quantity = table.Column<int>(nullable: false),
                    description = table.Column<string>(nullable: true),
                    manufacturerId = table.Column<int>(nullable: false),
                    createdDate = table.Column<DateTime>(nullable: false),
                    lastUpdatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_products", x => x.productId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "products");
        }
    }
}
