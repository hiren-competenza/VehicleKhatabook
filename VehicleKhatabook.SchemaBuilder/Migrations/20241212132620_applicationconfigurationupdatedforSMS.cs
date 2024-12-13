using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VehicleKhatabook.SchemaBuilder.Migrations
{
    /// <inheritdoc />
    public partial class applicationconfigurationupdatedforSMS : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SMSApiUrl",
                table: "ApplicationConfigurations",
                newName: "SmsApiUrl");

            migrationBuilder.RenameColumn(
                name: "TrialPeriodDays",
                table: "ApplicationConfigurations",
                newName: "SubscriptionTrialPeriodDays");

            migrationBuilder.RenameColumn(
                name: "SMSSenderId",
                table: "ApplicationConfigurations",
                newName: "SmsUser");

            migrationBuilder.RenameColumn(
                name: "SMSApiKey",
                table: "ApplicationConfigurations",
                newName: "SmsStype");

            migrationBuilder.RenameColumn(
                name: "RenewalReminderDaysBefore",
                table: "ApplicationConfigurations",
                newName: "SubscriptionRenewalReminderDaysBefore");

            migrationBuilder.RenameColumn(
                name: "IsRenewable",
                table: "ApplicationConfigurations",
                newName: "SubscriptionIsRenewable");

            migrationBuilder.AddColumn<string>(
                name: "SmsPassword",
                table: "ApplicationConfigurations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SmsPriority",
                table: "ApplicationConfigurations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SmsSender",
                table: "ApplicationConfigurations",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SmsPassword",
                table: "ApplicationConfigurations");

            migrationBuilder.DropColumn(
                name: "SmsPriority",
                table: "ApplicationConfigurations");

            migrationBuilder.DropColumn(
                name: "SmsSender",
                table: "ApplicationConfigurations");

            migrationBuilder.RenameColumn(
                name: "SmsApiUrl",
                table: "ApplicationConfigurations",
                newName: "SMSApiUrl");

            migrationBuilder.RenameColumn(
                name: "SubscriptionTrialPeriodDays",
                table: "ApplicationConfigurations",
                newName: "TrialPeriodDays");

            migrationBuilder.RenameColumn(
                name: "SubscriptionRenewalReminderDaysBefore",
                table: "ApplicationConfigurations",
                newName: "RenewalReminderDaysBefore");

            migrationBuilder.RenameColumn(
                name: "SubscriptionIsRenewable",
                table: "ApplicationConfigurations",
                newName: "IsRenewable");

            migrationBuilder.RenameColumn(
                name: "SmsUser",
                table: "ApplicationConfigurations",
                newName: "SMSSenderId");

            migrationBuilder.RenameColumn(
                name: "SmsStype",
                table: "ApplicationConfigurations",
                newName: "SMSApiKey");
        }
    }
}
