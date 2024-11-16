using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VehicleKhatabook.SchemaBuilder.Migrations
{
    /// <inheritdoc />
    public partial class OwnerDriverManaged : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OwnerKhataCredits_Users_UserId",
                table: "OwnerKhataCredits");

            migrationBuilder.DropForeignKey(
                name: "FK_UserExpenses_Users_UserID",
                table: "UserExpenses");

            migrationBuilder.DropForeignKey(
                name: "FK_UserIncomes_Users_UserID",
                table: "UserIncomes");

            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_Users_UserID",
                table: "Vehicles");

            migrationBuilder.DropIndex(
                name: "IX_UserIncomes_UserID",
                table: "UserIncomes");

            migrationBuilder.DropIndex(
                name: "IX_UserExpenses_UserID",
                table: "UserExpenses");

            migrationBuilder.AddColumn<Guid>(
                name: "IncomeVehicleId",
                table: "UserIncomes",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "VehicleID",
                table: "UserIncomes",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ExpenseVehicleId",
                table: "UserExpenses",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "VehicleID",
                table: "UserExpenses",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "DriverOwnerId",
                table: "OwnerKhataDebits",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "DriverOwnerId",
                table: "OwnerKhataCredits",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "DriverOwnerUser",
                columns: table => new
                {
                    DriverOwnerUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    MobileNumber = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    UserType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DriverOwnerUser", x => x.DriverOwnerUserId);
                    table.ForeignKey(
                        name: "FK_DriverOwnerUser_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserIncomes_IncomeVehicleId",
                table: "UserIncomes",
                column: "IncomeVehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserExpenses_ExpenseVehicleId",
                table: "UserExpenses",
                column: "ExpenseVehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_DriverOwnerUser_UserID",
                table: "DriverOwnerUser",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_OwnerKhataCredits_Users_UserId",
                table: "OwnerKhataCredits",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_UserExpense_ExpenseVehicle",
                table: "UserExpenses",
                column: "ExpenseVehicleId",
                principalTable: "Vehicles",
                principalColumn: "VehicleID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserIncome_IncomeVehicle",
                table: "UserIncomes",
                column: "IncomeVehicleId",
                principalTable: "Vehicles",
                principalColumn: "VehicleID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_User_Vehicle",
                table: "Vehicles",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OwnerKhataCredits_Users_UserId",
                table: "OwnerKhataCredits");

            migrationBuilder.DropForeignKey(
                name: "FK_UserExpense_ExpenseVehicle",
                table: "UserExpenses");

            migrationBuilder.DropForeignKey(
                name: "FK_UserIncome_IncomeVehicle",
                table: "UserIncomes");

            migrationBuilder.DropForeignKey(
                name: "FK_User_Vehicle",
                table: "Vehicles");

            migrationBuilder.DropTable(
                name: "DriverOwnerUser");

            migrationBuilder.DropIndex(
                name: "IX_UserIncomes_IncomeVehicleId",
                table: "UserIncomes");

            migrationBuilder.DropIndex(
                name: "IX_UserExpenses_ExpenseVehicleId",
                table: "UserExpenses");

            migrationBuilder.DropColumn(
                name: "IncomeVehicleId",
                table: "UserIncomes");

            migrationBuilder.DropColumn(
                name: "VehicleID",
                table: "UserIncomes");

            migrationBuilder.DropColumn(
                name: "ExpenseVehicleId",
                table: "UserExpenses");

            migrationBuilder.DropColumn(
                name: "VehicleID",
                table: "UserExpenses");

            migrationBuilder.DropColumn(
                name: "DriverOwnerId",
                table: "OwnerKhataDebits");

            migrationBuilder.DropColumn(
                name: "DriverOwnerId",
                table: "OwnerKhataCredits");

            migrationBuilder.CreateIndex(
                name: "IX_UserIncomes_UserID",
                table: "UserIncomes",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_UserExpenses_UserID",
                table: "UserExpenses",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_OwnerKhataCredits_Users_UserId",
                table: "OwnerKhataCredits",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserExpenses_Users_UserID",
                table: "UserExpenses",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserIncomes_Users_UserID",
                table: "UserIncomes",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_Users_UserID",
                table: "Vehicles",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
