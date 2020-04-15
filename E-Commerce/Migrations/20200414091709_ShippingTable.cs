using Microsoft.EntityFrameworkCore.Migrations;

namespace ECommerce.Migrations
{
    public partial class ShippingTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FactorItems_Orders_OrderId",
                table: "FactorItems");

            migrationBuilder.DropIndex(
                name: "IX_FactorItems_OrderId",
                table: "FactorItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShippingState",
                table: "ShippingState");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "FactorItems");

            migrationBuilder.RenameTable(
                name: "ShippingState",
                newName: "ShippingStates");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShippingStates",
                table: "ShippingStates",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ShippingStates",
                table: "ShippingStates");

            migrationBuilder.RenameTable(
                name: "ShippingStates",
                newName: "ShippingState");

            migrationBuilder.AddColumn<string>(
                name: "OrderId",
                table: "FactorItems",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShippingState",
                table: "ShippingState",
                column: "Id");

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
    }
}
