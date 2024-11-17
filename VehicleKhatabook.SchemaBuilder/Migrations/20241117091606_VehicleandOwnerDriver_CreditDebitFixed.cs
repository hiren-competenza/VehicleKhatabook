using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VehicleKhatabook.SchemaBuilder.Migrations
{
    /// <inheritdoc />
    public partial class VehicleandOwnerDriver_CreditDebitFixed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OwnerKhataCredits_Users_UserId",
                table: "OwnerKhataCredits");

            migrationBuilder.DropForeignKey(
                name: "FK_OwnerKhataDebits_Users_UserId",
                table: "OwnerKhataDebits");

            migrationBuilder.DropIndex(
                name: "IX_OwnerKhataDebits_UserId",
                table: "OwnerKhataDebits");

            migrationBuilder.DropIndex(
                name: "IX_OwnerKhataCredits_UserId",
                table: "OwnerKhataCredits");

            migrationBuilder.DropColumn(
                name: "UserID",
                table: "UserIncomes");

            migrationBuilder.DropColumn(
                name: "UserID",
                table: "UserExpenses");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "OwnerKhataDebits");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "OwnerKhataCredits");

            migrationBuilder.CreateIndex(
                name: "IX_OwnerKhataDebits_DriverOwnerId",
                table: "OwnerKhataDebits",
                column: "DriverOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_OwnerKhataCredits_DriverOwnerId",
                table: "OwnerKhataCredits",
                column: "DriverOwnerId");

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

            migrationBuilder.DropIndex(
                name: "IX_OwnerKhataDebits_DriverOwnerId",
                table: "OwnerKhataDebits");

            migrationBuilder.DropIndex(
                name: "IX_OwnerKhataCredits_DriverOwnerId",
                table: "OwnerKhataCredits");

            migrationBuilder.AddColumn<Guid>(
                name: "UserID",
                table: "UserIncomes",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "UserID",
                table: "UserExpenses",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "OwnerKhataDebits",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "OwnerKhataCredits",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_OwnerKhataDebits_UserId",
                table: "OwnerKhataDebits",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_OwnerKhataCredits_UserId",
                table: "OwnerKhataCredits",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_OwnerKhataCredits_Users_UserId",
                table: "OwnerKhataCredits",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_OwnerKhataDebits_Users_UserId",
                table: "OwnerKhataDebits",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
