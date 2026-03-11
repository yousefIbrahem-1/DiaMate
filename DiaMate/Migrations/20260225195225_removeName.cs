using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiaMate.Migrations
{
    /// <inheritdoc />
    public partial class removeName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SecondName",
                table: "Persons");

            migrationBuilder.DropColumn(
                name: "ThirdName",
                table: "Persons");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SecondName",
                table: "Persons",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ThirdName",
                table: "Persons",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }
    }
}
