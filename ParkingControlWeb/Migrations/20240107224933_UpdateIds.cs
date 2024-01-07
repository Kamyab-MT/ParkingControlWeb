using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParkingControlWeb.Migrations
{
    /// <inheritdoc />
    public partial class UpdateIds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<string>(
                name: "ParkingId",
                table: "AspNetUsers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ParkingId",
                table: "AspNetUsers",
                column: "ParkingId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Parkings_ParkingId",
                table: "AspNetUsers",
                column: "ParkingId",
                principalTable: "Parkings",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Parkings_ParkingId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ParkingId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ParkingId",
                table: "AspNetUsers");

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
    }
}
