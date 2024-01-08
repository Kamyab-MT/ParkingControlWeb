using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParkingControlWeb.Migrations
{
    /// <inheritdoc />
    public partial class EditUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Active",
                table: "AspNetUsers",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Active",
                table: "AspNetUsers");
        }
    }
}
