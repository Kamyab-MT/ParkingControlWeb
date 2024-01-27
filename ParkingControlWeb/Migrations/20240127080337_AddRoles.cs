using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ParkingControlWeb.Migrations
{
    /// <inheritdoc />
    public partial class AddRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropTable(
                name: "AspNetRoles");

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
        }
    }
}
