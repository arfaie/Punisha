using Microsoft.EntityFrameworkCore.Migrations;

namespace ECommerce.Migrations
{
    public partial class Maker : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MakerId",
                table: "Cars",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Makers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false, defaultValueSql: "NEWID()"),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Makers", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cars_MakerId",
                table: "Cars",
                column: "MakerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cars_Makers_MakerId",
                table: "Cars",
                column: "MakerId",
                principalTable: "Makers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cars_Makers_MakerId",
                table: "Cars");

            migrationBuilder.DropTable(
                name: "Makers");

            migrationBuilder.DropIndex(
                name: "IX_Cars_MakerId",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "MakerId",
                table: "Cars");
        }
    }
}
