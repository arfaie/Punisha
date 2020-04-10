using Microsoft.EntityFrameworkCore.Migrations;

namespace ECommerce.Migrations
{
    public partial class AddressFactorForeignKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AddressId",
                table: "Factors",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Factors_AddressId",
                table: "Factors",
                column: "AddressId");

            migrationBuilder.AddForeignKey(
                name: "FK_Factors_Addresses_AddressId",
                table: "Factors",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Factors_Addresses_AddressId",
                table: "Factors");

            migrationBuilder.DropIndex(
                name: "IX_Factors_AddressId",
                table: "Factors");

            migrationBuilder.DropColumn(
                name: "AddressId",
                table: "Factors");
        }
    }
}
