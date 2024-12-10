using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VehicleKhatabook.SchemaBuilder.Migrations
{
    /// <inheritdoc />
    public partial class UserLanguageType_Reference : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OwnerKhataCredits_DriverOwnerUsers_DriverOwnerId",
                table: "OwnerKhataCredits");

            migrationBuilder.DropForeignKey(
                name: "FK_OwnerKhataDebits_DriverOwnerUsers_DriverOwnerId",
                table: "OwnerKhataDebits");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_LanguageTypes_LanguageTypeId",
                table: "Users");

            migrationBuilder.AddForeignKey(
                name: "FK_OwnerKhataCredits_DriverOwnerUsers_DriverOwnerId",
                table: "OwnerKhataCredits",
                column: "DriverOwnerId",
                principalTable: "DriverOwnerUsers",
                principalColumn: "DriverOwnerUserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OwnerKhataDebits_DriverOwnerUsers_DriverOwnerId",
                table: "OwnerKhataDebits",
                column: "DriverOwnerId",
                principalTable: "DriverOwnerUsers",
                principalColumn: "DriverOwnerUserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_LanguageTypes_LanguageTypeId",
                table: "Users",
                column: "LanguageTypeId",
                principalTable: "LanguageTypes",
                principalColumn: "LanguageTypeId",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OwnerKhataCredits_DriverOwnerUsers_DriverOwnerId",
                table: "OwnerKhataCredits");

            migrationBuilder.DropForeignKey(
                name: "FK_OwnerKhataDebits_DriverOwnerUsers_DriverOwnerId",
                table: "OwnerKhataDebits");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_LanguageTypes_LanguageTypeId",
                table: "Users");

            migrationBuilder.AddForeignKey(
                name: "FK_OwnerKhataCredits_DriverOwnerUsers_DriverOwnerId",
                table: "OwnerKhataCredits",
                column: "DriverOwnerId",
                principalTable: "DriverOwnerUsers",
                principalColumn: "DriverOwnerUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_OwnerKhataDebits_DriverOwnerUsers_DriverOwnerId",
                table: "OwnerKhataDebits",
                column: "DriverOwnerId",
                principalTable: "DriverOwnerUsers",
                principalColumn: "DriverOwnerUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_LanguageTypes_LanguageTypeId",
                table: "Users",
                column: "LanguageTypeId",
                principalTable: "LanguageTypes",
                principalColumn: "LanguageTypeId");
        }
    }
}
