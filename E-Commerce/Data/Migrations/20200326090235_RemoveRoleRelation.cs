using Microsoft.EntityFrameworkCore.Migrations;

namespace ECommerce.Migrations
{
    public partial class RemoveRoleRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                "FK_AspNetRoles_AspNetUsers_ApplicationUserId",
                "AspNetRoles");

            migrationBuilder.DropIndex(
                "IX_AspNetRoles_ApplicationUserId",
                "AspNetRoles");

            migrationBuilder.DropColumn(
                "ApplicationUserId",
                "AspNetRoles");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                "ApplicationUserId",
                "AspNetRoles",
                "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                "IX_AspNetRoles_ApplicationUserId",
                "AspNetRoles",
                "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                "FK_AspNetRoles_AspNetUsers_ApplicationUserId",
                "AspNetRoles",
                "ApplicationUserId",
                "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
