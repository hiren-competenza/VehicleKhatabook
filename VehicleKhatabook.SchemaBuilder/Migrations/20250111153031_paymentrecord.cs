using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VehicleKhatabook.SchemaBuilder.Migrations
{
    /// <inheritdoc />
    public partial class paymentrecord : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PackageId",
                table: "PaymentHistory",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Validity",
                table: "PaymentHistory",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PaymentHistory_PackageId",
                table: "PaymentHistory",
                column: "PackageId");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentHistory_SubscriptionMaster_PackageId",
                table: "PaymentHistory",
                column: "PackageId",
                principalTable: "SubscriptionMaster",
                principalColumn: "SubscriptionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentHistory_SubscriptionMaster_PackageId",
                table: "PaymentHistory");

            migrationBuilder.DropIndex(
                name: "IX_PaymentHistory_PackageId",
                table: "PaymentHistory");

            migrationBuilder.DropColumn(
                name: "PackageId",
                table: "PaymentHistory");

            migrationBuilder.DropColumn(
                name: "Validity",
                table: "PaymentHistory");
        }
    }
}
