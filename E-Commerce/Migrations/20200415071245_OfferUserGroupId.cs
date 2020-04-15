using Microsoft.EntityFrameworkCore.Migrations;

namespace ECommerce.Migrations
{
    public partial class OfferUserGroupId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Offers_UserGroups_idUserGroup",
                table: "Offers");

            migrationBuilder.DropIndex(
                name: "IX_Offers_idUserGroup",
                table: "Offers");

            migrationBuilder.DropColumn(
                name: "idUserGroup",
                table: "Offers");

            migrationBuilder.AddColumn<string>(
                name: "UserGroupId",
                table: "Offers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Offers_UserGroupId",
                table: "Offers",
                column: "UserGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Offers_UserGroups_UserGroupId",
                table: "Offers",
                column: "UserGroupId",
                principalTable: "UserGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Offers_UserGroups_UserGroupId",
                table: "Offers");

            migrationBuilder.DropIndex(
                name: "IX_Offers_UserGroupId",
                table: "Offers");

            migrationBuilder.DropColumn(
                name: "UserGroupId",
                table: "Offers");

            migrationBuilder.AddColumn<string>(
                name: "idUserGroup",
                table: "Offers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Offers_idUserGroup",
                table: "Offers",
                column: "idUserGroup");

            migrationBuilder.AddForeignKey(
                name: "FK_Offers_UserGroups_idUserGroup",
                table: "Offers",
                column: "idUserGroup",
                principalTable: "UserGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
