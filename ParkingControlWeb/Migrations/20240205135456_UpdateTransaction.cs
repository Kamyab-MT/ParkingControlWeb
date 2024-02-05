using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ParkingControlWeb.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTransaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<string>(
                name: "CarId",
                table: "Transactions",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CardNumber",
                table: "Transactions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OwnerName",
                table: "Transactions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ParkingId",
                table: "Transactions",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TrackingCode",
                table: "Transactions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_CarId",
                table: "Transactions",
                column: "CarId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_ParkingId",
                table: "Transactions",
                column: "ParkingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Cars_CarId",
                table: "Transactions",
                column: "CarId",
                principalTable: "Cars",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Parkings_ParkingId",
                table: "Transactions",
                column: "ParkingId",
                principalTable: "Parkings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Cars_CarId",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Parkings_ParkingId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_CarId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_ParkingId",
                table: "Transactions");

            migrationBuilder.DeleteData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "0e7589b6-5e48-4a04-8e72-97c610fc4d69");

            migrationBuilder.DeleteData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "8482dcee-cdd8-4da4-a710-dfb522f708a4");

            migrationBuilder.DeleteData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "be8ae3c1-0b72-4b9a-8caf-c5670380e70e");

            migrationBuilder.DeleteData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "c926999f-8e7d-401e-9bcf-78e79f5caa77");

            migrationBuilder.DeleteData(
                table: "Pricings",
                keyColumn: "Id",
                keyValue: "6d296c17-99a1-4c87-bb5e-f470bb016010");

            migrationBuilder.DeleteData(
                table: "Pricings",
                keyColumn: "Id",
                keyValue: "84306cd4-5e58-4574-94cb-c37ce8c8e89d");

            migrationBuilder.DeleteData(
                table: "Pricings",
                keyColumn: "Id",
                keyValue: "a526b004-16e2-4f71-9d92-9c7bfb54ed15");

            migrationBuilder.DeleteData(
                table: "Pricings",
                keyColumn: "Id",
                keyValue: "bc403c09-7728-4b18-9b6e-efef04502d15");

            migrationBuilder.DropColumn(
                name: "CarId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "CardNumber",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "OwnerName",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "ParkingId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "TrackingCode",
                table: "Transactions");

            migrationBuilder.AlterColumn<int>(
                name: "Ballance",
                table: "AspNetUsers",
                type: "int",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "real",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "IdentityRole",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "09c5f212-8b32-4921-9bd9-df055982337d", null, "Expert", "EXPERT" },
                    { "6daf8841-f920-4cac-99f4-938b3e2a3aea", null, "Driver", "DRIVER" },
                    { "bbac2f72-c6d9-4808-a8a7-5fee6c455896", null, "GlobalAdmin", "GLOBALADMIN" },
                    { "df022034-4b47-4367-8580-fe8f21192372", null, "SystemAdmin", "SYSTEMADMIN" }
                });

            migrationBuilder.InsertData(
                table: "Pricings",
                columns: new[] { "Id", "Price", "Title" },
                values: new object[,]
                {
                    { "1b305532-7704-401d-a602-19cde7667ede", 16000, "OneYear" },
                    { "9095824b-405c-44f5-8a1d-7f96f5e574b6", 9000, "SixMonth" },
                    { "a6af8202-429e-4516-9c12-9adea7323ac5", 2000, "OneMonth" },
                    { "a7f8b91c-a91c-4255-98c8-f9059fbf2e8f", 5000, "ThreeMonth" }
                });
        }
    }
}
