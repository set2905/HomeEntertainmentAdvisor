using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeEntertainmentAdvisor.Migrations
{
    /// <inheritdoc />
    public partial class _mediaGroup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MediaGroupId",
                table: "MediaPieces",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MediaPieces_MediaGroupId",
                table: "MediaPieces",
                column: "MediaGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_MediaPieces_MediaGroups_MediaGroupId",
                table: "MediaPieces",
                column: "MediaGroupId",
                principalTable: "MediaGroups",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MediaPieces_MediaGroups_MediaGroupId",
                table: "MediaPieces");

            migrationBuilder.DropIndex(
                name: "IX_MediaPieces_MediaGroupId",
                table: "MediaPieces");

            migrationBuilder.DropColumn(
                name: "MediaGroupId",
                table: "MediaPieces");
        }
    }
}
