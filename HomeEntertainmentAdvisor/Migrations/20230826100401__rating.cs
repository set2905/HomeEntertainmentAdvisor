using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeEntertainmentAdvisor.Migrations
{
    /// <inheritdoc />
    public partial class _rating : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_AspNetUsers_AuthorId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_MediaPieces_MediaPieceId",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_AuthorId",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "Reviews");


            migrationBuilder.RenameColumn(
                name: "MediaPieceId",
                table: "Reviews",
                newName: "RatingId");

            migrationBuilder.RenameIndex(
                name: "IX_Reviews_MediaPieceId",
                table: "Reviews",
                newName: "IX_Reviews_RatingId");

            migrationBuilder.CreateTable(
                name: "Rating",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AuthorId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    MediaPieceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Grade = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rating", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rating_AspNetUsers_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Rating_MediaPieces_MediaPieceId",
                        column: x => x.MediaPieceId,
                        principalTable: "MediaPieces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Rating_AuthorId",
                table: "Rating",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Rating_MediaPieceId",
                table: "Rating",
                column: "MediaPieceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Rating_RatingId",
                table: "Reviews",
                column: "RatingId",
                principalTable: "Rating",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Rating_RatingId",
                table: "Reviews");

            migrationBuilder.DropTable(
                name: "Rating");

            migrationBuilder.RenameColumn(
                name: "RatingId",
                table: "Reviews",
                newName: "MediaPieceId");

            migrationBuilder.RenameIndex(
                name: "IX_Reviews_RatingId",
                table: "Reviews",
                newName: "IX_Reviews_MediaPieceId");

            migrationBuilder.AddColumn<string>(
                name: "AuthorId",
                table: "Reviews",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_AuthorId",
                table: "Reviews",
                column: "AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_AspNetUsers_AuthorId",
                table: "Reviews",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_MediaPieces_MediaPieceId",
                table: "Reviews",
                column: "MediaPieceId",
                principalTable: "MediaPieces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
