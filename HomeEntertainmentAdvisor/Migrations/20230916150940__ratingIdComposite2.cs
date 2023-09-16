using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeEntertainmentAdvisor.Migrations
{
    /// <inheritdoc />
    public partial class _ratingIdComposite2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_AspNetUsers_AuthorId",
                table: "Ratings");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Ratings_RatingId",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_RatingId",
                table: "Reviews");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Ratings",
                table: "Ratings");

            migrationBuilder.DropIndex(
                name: "IX_Ratings_AuthorId",
                table: "Ratings");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Ratings");

            migrationBuilder.RenameColumn(
                name: "RatingId",
                table: "Reviews",
                newName: "RatingMediaPieceId");

            migrationBuilder.AddColumn<string>(
                name: "RatingAuthorId",
                table: "Reviews",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AuthorId",
                table: "Ratings",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "MediaPieces",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ratings",
                table: "Ratings",
                columns: new[] { "AuthorId", "MediaPieceId" });

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_RatingAuthorId_RatingMediaPieceId",
                table: "Reviews",
                columns: new[] { "RatingAuthorId", "RatingMediaPieceId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_AspNetUsers_AuthorId",
                table: "Ratings",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Ratings_RatingAuthorId_RatingMediaPieceId",
                table: "Reviews",
                columns: new[] { "RatingAuthorId", "RatingMediaPieceId" },
                principalTable: "Ratings",
                principalColumns: new[] { "AuthorId", "MediaPieceId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_AspNetUsers_AuthorId",
                table: "Ratings");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Ratings_RatingAuthorId_RatingMediaPieceId",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_RatingAuthorId_RatingMediaPieceId",
                table: "Reviews");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Ratings",
                table: "Ratings");

            migrationBuilder.DropColumn(
                name: "RatingAuthorId",
                table: "Reviews");

            migrationBuilder.RenameColumn(
                name: "RatingMediaPieceId",
                table: "Reviews",
                newName: "RatingId");

            migrationBuilder.AlterColumn<string>(
                name: "AuthorId",
                table: "Ratings",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "Ratings",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "MediaPieces",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ratings",
                table: "Ratings",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_RatingId",
                table: "Reviews",
                column: "RatingId");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_AuthorId",
                table: "Ratings",
                column: "AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_AspNetUsers_AuthorId",
                table: "Ratings",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Ratings_RatingId",
                table: "Reviews",
                column: "RatingId",
                principalTable: "Ratings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
