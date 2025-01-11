using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VehicleKhatabook.SchemaBuilder.Migrations
{
    /// <inheritdoc />
    public partial class SubscriptionMaster : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SubscriptionMaster",
                columns: table => new
                {
                    SubscriptionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubscriptionName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubscriptionAmount = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubscriptionDurationDays = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubscriptionIsRenewable = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubscriptionRenewalReminderDaysBefore = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubscriptionTrialPeriodDays = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubscriptionMaster", x => x.SubscriptionId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SubscriptionMaster");
        }
    }
}
