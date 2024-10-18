using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VehicleKhatabook.SchemaBuilder.Migrations
{
    /// <inheritdoc />
    public partial class fueltTraking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FuelTrackings_Users_DriverID",
                table: "FuelTrackings");

            migrationBuilder.RenameColumn(
                name: "DriverID",
                table: "FuelTrackings",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_FuelTrackings_DriverID",
                table: "FuelTrackings",
                newName: "IX_FuelTrackings_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_FuelTrackings_Users_UserId",
                table: "FuelTrackings",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FuelTrackings_Users_UserId",
                table: "FuelTrackings");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "FuelTrackings",
                newName: "DriverID");

            migrationBuilder.RenameIndex(
                name: "IX_FuelTrackings_UserId",
                table: "FuelTrackings",
                newName: "IX_FuelTrackings_DriverID");

            migrationBuilder.AddForeignKey(
                name: "FK_FuelTrackings_Users_DriverID",
                table: "FuelTrackings",
                column: "DriverID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
