using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ParkingControlWeb.Migrations
{
    /// <inheritdoc />
    public partial class AddStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropColumn(
                name: "ImageLink",
                table: "RenewalRequests");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "RenewalRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "RenewalRequests");

            migrationBuilder.AddColumn<string>(
                name: "ImageLink",
                table: "RenewalRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "MetaDatas",
                columns: new[] { "Id", "Key", "Value" },
                values: new object[,]
                {
                    { "0daa70b0-d996-4723-9342-e51f5318bf6a", "RenewalCardNumber", "585983119387" },
                    { "28278b37-71da-4826-97d8-43cd12f1265c", "RenewalCardName", "کامیاب محمدی تبار" }
                });
        }
    }
}
