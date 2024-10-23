using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VehicleKhatabook.SchemaBuilder.Migrations
{
    /// <inheritdoc />
    public partial class AddSMSProviderConfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SMSProviderConfigs",
                columns: table => new
                {
                    ProviderID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProviderName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    APIUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    AuthKey = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ClientID = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SenderID = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Timeout = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    UserID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserID1 = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SMSProviderConfigs", x => x.ProviderID);
                    table.ForeignKey(
                        name: "FK_SMSProviderConfigs_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID");
                    table.ForeignKey(
                        name: "FK_SMSProviderConfigs_Users_UserID1",
                        column: x => x.UserID1,
                        principalTable: "Users",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_SMSProviderConfigs_UserID",
                table: "SMSProviderConfigs",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_SMSProviderConfigs_UserID1",
                table: "SMSProviderConfigs",
                column: "UserID1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SMSProviderConfigs");
        }
    }
}
