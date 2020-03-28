using Microsoft.EntityFrameworkCore.Migrations;

namespace ECommerce.Migrations
{
    public partial class BrandCar : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                "BrandId",
                "Products",
                nullable: true);

            migrationBuilder.CreateIndex(
                "IX_Products_BrandId",
                "Products",
                "BrandId");

            migrationBuilder.AddForeignKey(
                "FK_Products_Brands_BrandId",
                "Products",
                "BrandId",
                "Brands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                "FK_Products_Brands_BrandId",
                "Products");

            migrationBuilder.DropIndex(
                "IX_Products_BrandId",
                "Products");

            migrationBuilder.DropColumn(
                "BrandId",
                "Products");
        }
    }
}
