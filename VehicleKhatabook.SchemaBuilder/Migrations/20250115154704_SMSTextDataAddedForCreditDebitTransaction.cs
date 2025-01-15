using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VehicleKhatabook.SchemaBuilder.Migrations
{
    /// <inheritdoc />
    public partial class SMSTextDataAddedForCreditDebitTransaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreditTransactionSmsText",
                table: "ApplicationConfigurations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DebitTransactionSmsText",
                table: "ApplicationConfigurations",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreditTransactionSmsText",
                table: "ApplicationConfigurations");

            migrationBuilder.DropColumn(
                name: "DebitTransactionSmsText",
                table: "ApplicationConfigurations");
        }
    }
}
