using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ParkingControlWeb.Migrations
{
    /// <inheritdoc />
    public partial class ModifRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2d9a2d5b-7008-4291-8cf7-640474892d63", null, "Driver", null },
                    { "4aed93ce-336f-4cd5-9ba1-a93c60673672", null, "Expert", null },
                    { "b22b46ca-5cd8-4396-a41f-26ddb6c53bfc", null, "GlobalAdmin", null },
                    { "fe494481-663d-42b5-a744-7acf2af83082", null, "SystemAdmin", null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "790f7260-a8e6-49a9-a087-d39301a9e991");

            migrationBuilder.DeleteData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "8c7a7a3c-713a-414f-9ff8-c825017592fa");

            migrationBuilder.DeleteData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "91c5290a-b043-4751-bb34-881e870109b1");

            migrationBuilder.DeleteData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "b1d9991e-29d4-4244-8012-4a25639526e0");

            migrationBuilder.InsertData(
                table: "IdentityRole",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2d9a2d5b-7008-4291-8cf7-640474892d63", null, "Driver", null },
                    { "4aed93ce-336f-4cd5-9ba1-a93c60673672", null, "Expert", null },
                    { "b22b46ca-5cd8-4396-a41f-26ddb6c53bfc", null, "GlobalAdmin", null },
                    { "fe494481-663d-42b5-a744-7acf2af83082", null, "SystemAdmin", null }
                });
        }
    }
}
