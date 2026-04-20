using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiaMate.Migrations
{
    /// <inheritdoc />
    public partial class addFood : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Foods",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CaloriesPer100g = table.Column<double>(type: "float", nullable: false),
                    Carbs_G = table.Column<double>(type: "float", nullable: false),
                    Sugar_G = table.Column<double>(type: "float", nullable: false),
                    Fiber_G = table.Column<double>(type: "float", nullable: false),
                    Protein_G = table.Column<double>(type: "float", nullable: false),
                    Fat_G = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Foods", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Foods");
        }
    }
}
