using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ParkingControlWeb.Migrations
{
    /// <inheritdoc />
    public partial class EditBallanceTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DeleteData(
                table: "Pricings",
                keyColumn: "Id",
                keyValue: "297a90c8-cc0f-40fe-9d68-99adf0d83e73");

            migrationBuilder.DeleteData(
                table: "Pricings",
                keyColumn: "Id",
                keyValue: "a4cba00f-db74-45dd-8ea1-6ceb0142c2c8");

            migrationBuilder.DeleteData(
                table: "Pricings",
                keyColumn: "Id",
                keyValue: "e27a2f5d-f103-4bd5-8b27-2f94781ca175");

            migrationBuilder.DeleteData(
                table: "Pricings",
                keyColumn: "Id",
                keyValue: "e79d75ef-89f1-4f3d-9607-562d7443af0f");

            migrationBuilder.AlterColumn<int>(
                name: "Price",
                table: "Pricings",
                type: "int",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.InsertData(
                table: "Pricings",
                columns: new[] { "Id", "Price", "Title" },
                values: new object[,]
                {
                    { "1b305532-7704-401d-a602-19cde7667ede", 16000, "OneYear" },
                    { "9095824b-405c-44f5-8a1d-7f96f5e574b6", 9000, "SixMonth" },
                    { "a6af8202-429e-4516-9c12-9adea7323ac5", 2000, "OneMonth" },
                    { "a7f8b91c-a91c-4255-98c8-f9059fbf2e8f", 5000, "ThreeMonth" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "09c5f212-8b32-4921-9bd9-df055982337d");

            migrationBuilder.DeleteData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "6daf8841-f920-4cac-99f4-938b3e2a3aea");

            migrationBuilder.DeleteData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "bbac2f72-c6d9-4808-a8a7-5fee6c455896");

            migrationBuilder.DeleteData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "df022034-4b47-4367-8580-fe8f21192372");

            migrationBuilder.DeleteData(
                table: "Pricings",
                keyColumn: "Id",
                keyValue: "1b305532-7704-401d-a602-19cde7667ede");

            migrationBuilder.DeleteData(
                table: "Pricings",
                keyColumn: "Id",
                keyValue: "9095824b-405c-44f5-8a1d-7f96f5e574b6");

            migrationBuilder.DeleteData(
                table: "Pricings",
                keyColumn: "Id",
                keyValue: "a6af8202-429e-4516-9c12-9adea7323ac5");

            migrationBuilder.DeleteData(
                table: "Pricings",
                keyColumn: "Id",
                keyValue: "a7f8b91c-a91c-4255-98c8-f9059fbf2e8f");

            migrationBuilder.AlterColumn<float>(
                name: "Price",
                table: "Pricings",
                type: "real",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.InsertData(
                table: "IdentityRole",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "13899aa4-3cb6-44be-844b-f87612558602", null, "GlobalAdmin", "GLOBALADMIN" },
                    { "6d028beb-7a69-437f-80e5-87465d53af29", null, "Expert", "EXPERT" },
                    { "a52c940a-6aa1-4282-8605-d3ac184fde53", null, "Driver", "DRIVER" },
                    { "b6896bee-eb49-4db0-abb5-37a0e3549b02", null, "SystemAdmin", "SYSTEMADMIN" }
                });

            migrationBuilder.InsertData(
                table: "Pricings",
                columns: new[] { "Id", "Price", "Title" },
                values: new object[,]
                {
                    { "297a90c8-cc0f-40fe-9d68-99adf0d83e73", 9000f, "SixMonth" },
                    { "a4cba00f-db74-45dd-8ea1-6ceb0142c2c8", 2000f, "OneMonth" },
                    { "e27a2f5d-f103-4bd5-8b27-2f94781ca175", 16000f, "OneYear" },
                    { "e79d75ef-89f1-4f3d-9607-562d7443af0f", 5000f, "ThreeMonth" }
                });
        }
    }
}
