using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VehicleKhatabook.SchemaBuilder.Migrations
{
    /// <inheritdoc />
    public partial class OTPforAnonymousUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OtpRequests_Users_UserID",
                table: "OtpRequests");

            migrationBuilder.DropIndex(
                name: "IX_OtpRequests_UserID",
                table: "OtpRequests");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_OtpRequests_UserID",
                table: "OtpRequests",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_OtpRequests_Users_UserID",
                table: "OtpRequests",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
