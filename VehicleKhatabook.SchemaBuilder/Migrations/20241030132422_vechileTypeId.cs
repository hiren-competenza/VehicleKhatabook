using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VehicleKhatabook.SchemaBuilder.Migrations
{
    /// <inheritdoc />
    public partial class vechileTypeId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VehicleType",
                table: "Vehicles");

            migrationBuilder.AddColumn<int>(
                name: "VehicleTypeId",
                table: "Vehicles",
                type: "int",
                maxLength: 50,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VehicleTypeId",
                table: "Vehicles");

            migrationBuilder.AddColumn<string>(
                name: "VehicleType",
                table: "Vehicles",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }
    }
}
