using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VehicleKhatabook.SchemaBuilder.Migrations
{
    /// <inheritdoc />
    public partial class OwnerDriverManaged_Fixed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DriverOwnerUser_Users_UserID",
                table: "DriverOwnerUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DriverOwnerUser",
                table: "DriverOwnerUser");

            migrationBuilder.RenameTable(
                name: "DriverOwnerUser",
                newName: "DriverOwnerUsers");

            migrationBuilder.RenameIndex(
                name: "IX_DriverOwnerUser_UserID",
                table: "DriverOwnerUsers",
                newName: "IX_DriverOwnerUsers_UserID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DriverOwnerUsers",
                table: "DriverOwnerUsers",
                column: "DriverOwnerUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_DriverOwnerUsers_Users_UserID",
                table: "DriverOwnerUsers",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DriverOwnerUsers_Users_UserID",
                table: "DriverOwnerUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DriverOwnerUsers",
                table: "DriverOwnerUsers");

            migrationBuilder.RenameTable(
                name: "DriverOwnerUsers",
                newName: "DriverOwnerUser");

            migrationBuilder.RenameIndex(
                name: "IX_DriverOwnerUsers_UserID",
                table: "DriverOwnerUser",
                newName: "IX_DriverOwnerUser_UserID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DriverOwnerUser",
                table: "DriverOwnerUser",
                column: "DriverOwnerUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_DriverOwnerUser_Users_UserID",
                table: "DriverOwnerUser",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
