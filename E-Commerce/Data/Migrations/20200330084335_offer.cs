using Microsoft.EntityFrameworkCore.Migrations;

namespace ECommerce.Migrations
{
    public partial class offer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CarNames",
                table: "ProductFields");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "ProductFields");

            migrationBuilder.DropColumn(
                name: "Starts",
                table: "CommentAndStars");

            migrationBuilder.AddColumn<string>(
                name: "OfferId",
                table: "Offers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "idUserGroup",
                table: "Offers",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "CommentAndStars",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Stars",
                table: "CommentAndStars",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Offers_OfferId",
                table: "Offers",
                column: "OfferId");

            migrationBuilder.CreateIndex(
                name: "IX_Offers_idUserGroup",
                table: "Offers",
                column: "idUserGroup");

            migrationBuilder.AddForeignKey(
                name: "FK_Offers_Offers_OfferId",
                table: "Offers",
                column: "OfferId",
                principalTable: "Offers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Offers_UserGroups_idUserGroup",
                table: "Offers",
                column: "idUserGroup",
                principalTable: "UserGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Offers_Offers_OfferId",
                table: "Offers");

            migrationBuilder.DropForeignKey(
                name: "FK_Offers_UserGroups_idUserGroup",
                table: "Offers");

            migrationBuilder.DropIndex(
                name: "IX_Offers_OfferId",
                table: "Offers");

            migrationBuilder.DropIndex(
                name: "IX_Offers_idUserGroup",
                table: "Offers");

            migrationBuilder.DropColumn(
                name: "OfferId",
                table: "Offers");

            migrationBuilder.DropColumn(
                name: "idUserGroup",
                table: "Offers");

            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "CommentAndStars");

            migrationBuilder.DropColumn(
                name: "Stars",
                table: "CommentAndStars");

            migrationBuilder.AddColumn<string>(
                name: "CarNames",
                table: "ProductFields",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "ProductFields",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Starts",
                table: "CommentAndStars",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
