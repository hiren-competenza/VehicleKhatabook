using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VehicleKhatabook.SchemaBuilder.Migrations
{
    /// <inheritdoc />
    public partial class updateincomeexpense : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_Users_DriverID",
                table: "Expenses");

            migrationBuilder.DropForeignKey(
                name: "FK_Incomes_Users_DriverID",
                table: "Incomes");

            migrationBuilder.RenameColumn(
                name: "DriverID",
                table: "Incomes",
                newName: "UserID");

            migrationBuilder.RenameIndex(
                name: "IX_Incomes_DriverID",
                table: "Incomes",
                newName: "IX_Incomes_UserID");

            migrationBuilder.RenameColumn(
                name: "DriverID",
                table: "Expenses",
                newName: "UserID");

            migrationBuilder.RenameIndex(
                name: "IX_Expenses_DriverID",
                table: "Expenses",
                newName: "IX_Expenses_UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_Users_UserID",
                table: "Expenses",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Incomes_Users_UserID",
                table: "Incomes",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_Users_UserID",
                table: "Expenses");

            migrationBuilder.DropForeignKey(
                name: "FK_Incomes_Users_UserID",
                table: "Incomes");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "Incomes",
                newName: "DriverID");

            migrationBuilder.RenameIndex(
                name: "IX_Incomes_UserID",
                table: "Incomes",
                newName: "IX_Incomes_DriverID");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "Expenses",
                newName: "DriverID");

            migrationBuilder.RenameIndex(
                name: "IX_Expenses_UserID",
                table: "Expenses",
                newName: "IX_Expenses_DriverID");

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_Users_DriverID",
                table: "Expenses",
                column: "DriverID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Incomes_Users_DriverID",
                table: "Incomes",
                column: "DriverID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
