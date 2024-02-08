using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParkingControlWeb.Migrations
{
    /// <inheritdoc />
    public partial class RemoveBallanceFromUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ballance",
                table: "AspNetUsers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "Ballance",
                table: "AspNetUsers",
                type: "real",
                nullable: true);
        }
    }
}
