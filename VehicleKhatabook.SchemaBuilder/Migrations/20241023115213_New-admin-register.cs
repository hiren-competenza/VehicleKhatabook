using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VehicleKhatabook.SchemaBuilder.Migrations
{
    /// <inheritdoc />
    public partial class Newadminregister : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdminUsers",
                columns: table => new
                {
                    AdminID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Role = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SecurityQuestion = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    SecurityAnswerHash = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    MobileNumber = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    LastLogin = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedByAdminAdminID = table.Column<int>(type: "int", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminUsers", x => x.AdminID);
                    table.ForeignKey(
                        name: "FK_AdminUsers_AdminUsers_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "AdminUsers",
                        principalColumn: "AdminID");
                    table.ForeignKey(
                        name: "FK_AdminUsers_AdminUsers_ModifiedByAdminAdminID",
                        column: x => x.ModifiedByAdminAdminID,
                        principalTable: "AdminUsers",
                        principalColumn: "AdminID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdminUsers_ModifiedBy",
                table: "AdminUsers",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_AdminUsers_ModifiedByAdminAdminID",
                table: "AdminUsers",
                column: "ModifiedByAdminAdminID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdminUsers");
        }
    }
}
