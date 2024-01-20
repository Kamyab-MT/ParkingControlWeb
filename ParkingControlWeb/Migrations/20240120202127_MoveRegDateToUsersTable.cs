using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParkingControlWeb.Migrations
{
    /// <inheritdoc />
    public partial class MoveRegDateToUsersTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RegisterDate",
                table: "Info");

            migrationBuilder.AddColumn<DateTime>(
                name: "RegisterDate",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RegisterDate",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<DateTime>(
                name: "RegisterDate",
                table: "Info",
                type: "datetime2",
                nullable: true);
        }
    }
}
