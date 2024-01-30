using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ParkingControlWeb.Migrations
{
    /// <inheritdoc />
    public partial class AddPricing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Pricings",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pricings", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Pricings",
                columns: new[] { "Id", "Price", "Title" },
                values: new object[,]
                {
                    { "03394e23-d768-4404-8b03-c7f111bb2d91", 5000f, "ThreeMonth" },
                    { "9bff3efd-a59c-47ad-8330-b8460a235765", 2000f, "OneMonth" },
                    { "c28b2f3d-9d52-4dc3-8d4a-a477a02fa168", 9000f, "SixMonth" },
                    { "e5a08920-b0e3-44aa-9ac6-8647970eaf5e", 16000f, "OneYear" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Pricings");

            migrationBuilder.DeleteData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "51a987ec-adb0-4843-932a-bbf93b1918cf");

            migrationBuilder.DeleteData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "b969abc5-af9f-4546-a739-12d8380477d6");

            migrationBuilder.DeleteData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "d5e1ff8e-fe46-495f-9d78-a2fd902ab16a");

            migrationBuilder.DeleteData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "db8889f2-7c49-4a7b-85d0-9d2cd0fe3784");

            migrationBuilder.InsertData(
                table: "IdentityRole",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "790f7260-a8e6-49a9-a087-d39301a9e991", null, "Driver", null },
                    { "8c7a7a3c-713a-414f-9ff8-c825017592fa", null, "GlobalAdmin", null },
                    { "91c5290a-b043-4751-bb34-881e870109b1", null, "SystemAdmin", null },
                    { "b1d9991e-29d4-4244-8012-4a25639526e0", null, "Expert", null }
                });
        }
    }
}
