﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace ECommerce.Migrations
{
    public partial class _newsMigrationUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NewsTagsId",
                table: "Tags",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tags_NewsTagsId",
                table: "Tags",
                column: "NewsTagsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_NewsTagses_NewsTagsId",
                table: "Tags",
                column: "NewsTagsId",
                principalTable: "NewsTagses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tags_NewsTagses_NewsTagsId",
                table: "Tags");

            migrationBuilder.DropIndex(
                name: "IX_Tags_NewsTagsId",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "NewsTagsId",
                table: "Tags");
        }
    }
}
