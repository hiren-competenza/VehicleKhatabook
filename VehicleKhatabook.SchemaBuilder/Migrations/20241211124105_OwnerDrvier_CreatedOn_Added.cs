using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VehicleKhatabook.SchemaBuilder.Migrations
{
    /// <inheritdoc />
    public partial class OwnerDrvier_CreatedOn_Added : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "OwnerKhataDebits",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "OwnerKhataDebits",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedOn",
                table: "OwnerKhataDebits",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedBy",
                table: "OwnerKhataDebits",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "OwnerKhataCredits",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "OwnerKhataCredits",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedOn",
                table: "OwnerKhataCredits",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedBy",
                table: "OwnerKhataCredits",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "OwnerKhataDebits");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "OwnerKhataDebits");

            migrationBuilder.DropColumn(
                name: "LastModifiedOn",
                table: "OwnerKhataDebits");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "OwnerKhataDebits");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "OwnerKhataCredits");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "OwnerKhataCredits");

            migrationBuilder.DropColumn(
                name: "LastModifiedOn",
                table: "OwnerKhataCredits");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "OwnerKhataCredits");
        }
    }
}
