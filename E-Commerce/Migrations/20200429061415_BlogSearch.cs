using Microsoft.EntityFrameworkCore.Migrations;

namespace ECommerce.Migrations
{
    public partial class BlogSearch : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NewsId",
                table: "Tags",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tags_NewsId",
                table: "Tags",
                column: "NewsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_Newses_NewsId",
                table: "Tags",
                column: "NewsId",
                principalTable: "Newses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tags_Newses_NewsId",
                table: "Tags");

            migrationBuilder.DropIndex(
                name: "IX_Tags_NewsId",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "NewsId",
                table: "Tags");
        }
    }
}
