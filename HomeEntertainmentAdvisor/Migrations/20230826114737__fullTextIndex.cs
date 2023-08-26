using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeEntertainmentAdvisor.Migrations
{
    /// <inheritdoc />
    public partial class _fullTextIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rating_AspNetUsers_AuthorId",
                table: "Rating");

            migrationBuilder.DropForeignKey(
                name: "FK_Rating_MediaPieces_MediaPieceId",
                table: "Rating");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Rating_RatingId",
                table: "Reviews");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Rating",
                table: "Rating");

            migrationBuilder.RenameTable(
                name: "Rating",
                newName: "Ratings");

            migrationBuilder.RenameIndex(
                name: "IX_Rating_MediaPieceId",
                table: "Ratings",
                newName: "IX_Ratings_MediaPieceId");

            migrationBuilder.RenameIndex(
                name: "IX_Rating_AuthorId",
                table: "Ratings",
                newName: "IX_Ratings_AuthorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ratings",
                table: "Ratings",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_AspNetUsers_AuthorId",
                table: "Ratings",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_MediaPieces_MediaPieceId",
                table: "Ratings",
                column: "MediaPieceId",
                principalTable: "MediaPieces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Ratings_RatingId",
                table: "Reviews",
                column: "RatingId",
                principalTable: "Ratings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_AspNetUsers_AuthorId",
                table: "Ratings");

            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_MediaPieces_MediaPieceId",
                table: "Ratings");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Ratings_RatingId",
                table: "Reviews");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Ratings",
                table: "Ratings");

            migrationBuilder.RenameTable(
                name: "Ratings",
                newName: "Rating");

            migrationBuilder.RenameIndex(
                name: "IX_Ratings_MediaPieceId",
                table: "Rating",
                newName: "IX_Rating_MediaPieceId");

            migrationBuilder.RenameIndex(
                name: "IX_Ratings_AuthorId",
                table: "Rating",
                newName: "IX_Rating_AuthorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Rating",
                table: "Rating",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Rating_AspNetUsers_AuthorId",
                table: "Rating",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Rating_MediaPieces_MediaPieceId",
                table: "Rating",
                column: "MediaPieceId",
                principalTable: "MediaPieces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Rating_RatingId",
                table: "Reviews",
                column: "RatingId",
                principalTable: "Rating",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
