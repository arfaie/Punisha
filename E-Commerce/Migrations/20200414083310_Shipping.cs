using Microsoft.EntityFrameworkCore.Migrations;

namespace ECommerce.Migrations
{
    public partial class Shipping : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FinalPrice",
                table: "Factors");

            migrationBuilder.AddColumn<int>(
                name: "Weight",
                table: "Products",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ShippingCost",
                table: "Factors",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ShippingState",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    StateId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShippingState", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "ShippingState",
                columns: new[] { "Id", "Name", "StateId" },
                values: new object[,]
                {
                    { "6f9c65d6-6596-4fd0-8193-7c32dafcec01", "تهران", 1 },
                    { "6f9c65d6-6596-4fd0-8193-7c32dafcec29", "خراسان شمالی", 29 },
                    { "6f9c65d6-6596-4fd0-8193-7c32dafcec28", "کهگلویه و بویراحمد", 28 },
                    { "6f9c65d6-6596-4fd0-8193-7c32dafcec27", "ایلام", 27 },
                    { "6f9c65d6-6596-4fd0-8193-7c32dafcec26", "سیستان و بلوچستان", 26 },
                    { "6f9c65d6-6596-4fd0-8193-7c32dafcec25", "یزد", 25 },
                    { "6f9c65d6-6596-4fd0-8193-7c32dafcec24", "چهارمحال و بختیاری", 24 },
                    { "6f9c65d6-6596-4fd0-8193-7c32dafcec23", "هرمزگان", 23 },
                    { "6f9c65d6-6596-4fd0-8193-7c32dafcec22", "کرمان", 22 },
                    { "6f9c65d6-6596-4fd0-8193-7c32dafcec21", "بوشهر", 21 },
                    { "6f9c65d6-6596-4fd0-8193-7c32dafcec20", "لرستان", 20 },
                    { "6f9c65d6-6596-4fd0-8193-7c32dafcec19", "کرمانشاه", 19 },
                    { "6f9c65d6-6596-4fd0-8193-7c32dafcec18", "کردستان", 18 },
                    { "6f9c65d6-6596-4fd0-8193-7c32dafcec17", "همدان", 17 },
                    { "6f9c65d6-6596-4fd0-8193-7c32dafcec30", "خراسان جنوبی", 30 },
                    { "6f9c65d6-6596-4fd0-8193-7c32dafcec16", "آذربایجان غربی", 16 },
                    { "6f9c65d6-6596-4fd0-8193-7c32dafcec14", "گلستان", 14 },
                    { "6f9c65d6-6596-4fd0-8193-7c32dafcec13", "مازندران", 13 },
                    { "6f9c65d6-6596-4fd0-8193-7c32dafcec12", "زنجان", 12 },
                    { "6f9c65d6-6596-4fd0-8193-7c32dafcec11", "مرکزی", 11 },
                    { "6f9c65d6-6596-4fd0-8193-7c32dafcec10", "قم", 10 },
                    { "6f9c65d6-6596-4fd0-8193-7c32dafcec09", "سمنان", 9 },
                    { "6f9c65d6-6596-4fd0-8193-7c32dafcec08", "قزوین", 8 },
                    { "6f9c65d6-6596-4fd0-8193-7c32dafcec07", "خراسان رضوی", 7 },
                    { "6f9c65d6-6596-4fd0-8193-7c32dafcec06", "اصفهان", 6 },
                    { "6f9c65d6-6596-4fd0-8193-7c32dafcec05", "فارس", 5 },
                    { "6f9c65d6-6596-4fd0-8193-7c32dafcec04", "خوزستان", 4 },
                    { "6f9c65d6-6596-4fd0-8193-7c32dafcec03", "آذربایجان شرقی", 3 },
                    { "6f9c65d6-6596-4fd0-8193-7c32dafcec02", "گیلان", 2 },
                    { "6f9c65d6-6596-4fd0-8193-7c32dafcec15", "اردبیل", 15 },
                    { "6f9c65d6-6596-4fd0-8193-7c32dafcec31", "البرز", 31 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShippingState");

            migrationBuilder.DropColumn(
                name: "Weight",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ShippingCost",
                table: "Factors");

            migrationBuilder.AddColumn<int>(
                name: "FinalPrice",
                table: "Factors",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
