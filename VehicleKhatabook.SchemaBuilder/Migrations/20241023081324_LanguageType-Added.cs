using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VehicleKhatabook.SchemaBuilder.Migrations
{
    /// <inheritdoc />
    public partial class LanguageTypeAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Language",
                table: "Users");

            migrationBuilder.AddColumn<int>(
                name: "LanguageTypeId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "LanguageTypes",
                columns: table => new
                {
                    LanguageTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LanguageName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LanguageTypes", x => x.LanguageTypeId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_LanguageTypeId",
                table: "Users",
                column: "LanguageTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_LanguageTypes_LanguageTypeId",
                table: "Users",
                column: "LanguageTypeId",
                principalTable: "LanguageTypes",
                principalColumn: "LanguageTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_LanguageTypes_LanguageTypeId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "LanguageTypes");

            migrationBuilder.DropIndex(
                name: "IX_Users_LanguageTypeId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LanguageTypeId",
                table: "Users");

            migrationBuilder.AddColumn<string>(
                name: "Language",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
