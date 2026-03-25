using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiaMate.Migrations
{
    /// <inheritdoc />
    public partial class addMeal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Meals",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "carbs",
                table: "Meals",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "fats",
                table: "Meals",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "protein",
                table: "Meals",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Meals");

            migrationBuilder.DropColumn(
                name: "carbs",
                table: "Meals");

            migrationBuilder.DropColumn(
                name: "fats",
                table: "Meals");

            migrationBuilder.DropColumn(
                name: "protein",
                table: "Meals");
        }
    }
}
