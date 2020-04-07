using Microsoft.EntityFrameworkCore.Migrations;

namespace ECommerce.Migrations
{
    public partial class CarProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fields_SelectGroups_SelectGroupId",
                table: "Fields");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Cars_CarId",
                table: "Products");

            migrationBuilder.DropTable(
                name: "ProductSelectedItems");

            migrationBuilder.DropIndex(
                name: "IX_Products_CarId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Fields_SelectGroupId",
                table: "Fields");

            migrationBuilder.DropColumn(
                name: "CarId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "SelectGroupId",
                table: "Fields");

            migrationBuilder.AddColumn<string>(
                name: "CategoryId",
                table: "Fields",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CarProducts",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false, defaultValueSql: "NEWID()"),
                    CarId = table.Column<string>(nullable: true),
                    ProductId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CarProducts_Cars_CarId",
                        column: x => x.CarId,
                        principalTable: "Cars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CarProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Fields_CategoryId",
                table: "Fields",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CarProducts_CarId",
                table: "CarProducts",
                column: "CarId");

            migrationBuilder.CreateIndex(
                name: "IX_CarProducts_ProductId",
                table: "CarProducts",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Fields_Categories_CategoryId",
                table: "Fields",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fields_Categories_CategoryId",
                table: "Fields");

            migrationBuilder.DropTable(
                name: "CarProducts");

            migrationBuilder.DropIndex(
                name: "IX_Fields_CategoryId",
                table: "Fields");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Fields");

            migrationBuilder.AddColumn<string>(
                name: "CarId",
                table: "Products",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SelectGroupId",
                table: "Fields",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ProductSelectedItems",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false, defaultValueSql: "NEWID()"),
                    ProductFieldId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    SelectItemId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductSelectedItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductSelectedItems_ProductFields_ProductFieldId",
                        column: x => x.ProductFieldId,
                        principalTable: "ProductFields",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductSelectedItems_SelectItems_SelectItemId",
                        column: x => x.SelectItemId,
                        principalTable: "SelectItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_CarId",
                table: "Products",
                column: "CarId");

            migrationBuilder.CreateIndex(
                name: "IX_Fields_SelectGroupId",
                table: "Fields",
                column: "SelectGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductSelectedItems_ProductFieldId",
                table: "ProductSelectedItems",
                column: "ProductFieldId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductSelectedItems_SelectItemId",
                table: "ProductSelectedItems",
                column: "SelectItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Fields_SelectGroups_SelectGroupId",
                table: "Fields",
                column: "SelectGroupId",
                principalTable: "SelectGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Cars_CarId",
                table: "Products",
                column: "CarId",
                principalTable: "Cars",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
