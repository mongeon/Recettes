using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Recettes.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddIngredientsUnit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Quantity",
                table: "Ingredients",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Unit",
                table: "Ingredients",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "Ingredients");

            migrationBuilder.DropColumn(
                name: "Unit",
                table: "Ingredients");
        }
    }
}
