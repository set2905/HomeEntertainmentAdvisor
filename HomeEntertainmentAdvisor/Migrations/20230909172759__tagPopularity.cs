using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeEntertainmentAdvisor.Migrations
{
    /// <inheritdoc />
    public partial class _tagPopularity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Popularity",
                table: "Tags",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Popularity",
                table: "Tags");
        }
    }
}
