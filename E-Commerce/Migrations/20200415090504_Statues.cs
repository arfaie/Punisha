using Microsoft.EntityFrameworkCore.Migrations;

namespace ECommerce.Migrations
{
    public partial class Statues : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Statuses",
                columns: new[] { "Id", "Title" },
                values: new object[,]
                {
                    { "6f9c65d681937c32dafcec01", "پرداخت نشده" },
                    { "6f9c65d681937c32dafcec03", "در صف بررسی" },
                    { "6f9c65d681937c32dafcec04", "آماده سازی سفارش" },
                    { "6f9c65d681937c32dafcec05", "تحویل به پسب" },
                    { "6f9c65d681937c32dafcec06", "تحویل به مشتری" },
                    { "6f9c65d681937c32dafcec07", "درخواست مرجوعی" },
                    { "6f9c65d681937c32dafcec08", "مرجوع شده" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Statuses",
                keyColumn: "Id",
                keyValue: "6f9c65d681937c32dafcec01");

            migrationBuilder.DeleteData(
                table: "Statuses",
                keyColumn: "Id",
                keyValue: "6f9c65d681937c32dafcec03");

            migrationBuilder.DeleteData(
                table: "Statuses",
                keyColumn: "Id",
                keyValue: "6f9c65d681937c32dafcec04");

            migrationBuilder.DeleteData(
                table: "Statuses",
                keyColumn: "Id",
                keyValue: "6f9c65d681937c32dafcec05");

            migrationBuilder.DeleteData(
                table: "Statuses",
                keyColumn: "Id",
                keyValue: "6f9c65d681937c32dafcec06");

            migrationBuilder.DeleteData(
                table: "Statuses",
                keyColumn: "Id",
                keyValue: "6f9c65d681937c32dafcec07");

            migrationBuilder.DeleteData(
                table: "Statuses",
                keyColumn: "Id",
                keyValue: "6f9c65d681937c32dafcec08");
        }
    }
}
