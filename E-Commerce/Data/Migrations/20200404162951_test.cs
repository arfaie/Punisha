using Microsoft.EntityFrameworkCore.Migrations;

namespace ECommerce.Migrations
{
    public partial class test : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Offers_Offers_OfferId",
                table: "Offers");

            migrationBuilder.DropIndex(
                name: "IX_Offers_OfferId",
                table: "Offers");

            migrationBuilder.DropColumn(
                name: "OfferId",
                table: "Offers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OfferId",
                table: "Offers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Offers_OfferId",
                table: "Offers",
                column: "OfferId");

            migrationBuilder.AddForeignKey(
                name: "FK_Offers_Offers_OfferId",
                table: "Offers",
                column: "OfferId",
                principalTable: "Offers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
