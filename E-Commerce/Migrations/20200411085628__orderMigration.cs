using Microsoft.EntityFrameworkCore.Migrations;

namespace ECommerce.Migrations
{
    public partial class _orderMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OrderId",
                table: "FactorItems",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FactorItems_OrderId",
                table: "FactorItems",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_FactorItems_Orders_OrderId",
                table: "FactorItems",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FactorItems_Orders_OrderId",
                table: "FactorItems");

            migrationBuilder.DropIndex(
                name: "IX_FactorItems_OrderId",
                table: "FactorItems");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "FactorItems");
        }
    }
}
