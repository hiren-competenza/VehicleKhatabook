using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VehicleKhatabook.SchemaBuilder.Migrations
{
    /// <inheritdoc />
    public partial class PremiumUserDatesAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TypeName",
                table: "VehicleTypes",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "VehicleTypes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "VehicleTypes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "VehicleTypes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedOn",
                table: "VehicleTypes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedBy",
                table: "VehicleTypes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VehicleTypeLanguageJson",
                table: "VehicleTypes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PremiumExpiryDate",
                table: "Users",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PremiumStartDate",
                table: "Users",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReferCodeCount",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IncomeCategoryLanguageJson",
                table: "IncomeCategories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExpenseCategoryLanguageJson",
                table: "ExpenseCategories",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "VehicleTypes");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "VehicleTypes");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "VehicleTypes");

            migrationBuilder.DropColumn(
                name: "LastModifiedOn",
                table: "VehicleTypes");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "VehicleTypes");

            migrationBuilder.DropColumn(
                name: "VehicleTypeLanguageJson",
                table: "VehicleTypes");

            migrationBuilder.DropColumn(
                name: "PremiumExpiryDate",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PremiumStartDate",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ReferCodeCount",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IncomeCategoryLanguageJson",
                table: "IncomeCategories");

            migrationBuilder.DropColumn(
                name: "ExpenseCategoryLanguageJson",
                table: "ExpenseCategories");

            migrationBuilder.AlterColumn<string>(
                name: "TypeName",
                table: "VehicleTypes",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);
        }
    }
}
