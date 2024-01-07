using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParkingControlWeb.Migrations
{
    /// <inheritdoc />
    public partial class AddParkingToInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ParkingId",
                table: "Info",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Info_ParkingId",
                table: "Info",
                column: "ParkingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Info_Parkings_ParkingId",
                table: "Info",
                column: "ParkingId",
                principalTable: "Parkings",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Info_Parkings_ParkingId",
                table: "Info");

            migrationBuilder.DropIndex(
                name: "IX_Info_ParkingId",
                table: "Info");

            migrationBuilder.DropColumn(
                name: "ParkingId",
                table: "Info");
        }
    }
}
