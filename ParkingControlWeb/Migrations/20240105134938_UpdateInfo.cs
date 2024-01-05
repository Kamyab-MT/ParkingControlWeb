using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParkingControlWeb.Migrations
{
    /// <inheritdoc />
    public partial class UpdateInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "RegisterDate",
                table: "Info",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Info",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Info_UserId",
                table: "Info",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Info_AspNetUsers_UserId",
                table: "Info",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Info_AspNetUsers_UserId",
                table: "Info");

            migrationBuilder.DropIndex(
                name: "IX_Info_UserId",
                table: "Info");

            migrationBuilder.DropColumn(
                name: "RegisterDate",
                table: "Info");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Info");
        }
    }
}
