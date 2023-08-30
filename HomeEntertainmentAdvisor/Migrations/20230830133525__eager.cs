using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeEntertainmentAdvisor.Migrations
{
    /// <inheritdoc />
    public partial class _eager : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: new Guid("ddd4edd3-bd3b-4bc9-8474-a6820a313483"));

            migrationBuilder.DeleteData(
                table: "Ratings",
                keyColumn: "Id",
                keyValue: new Guid("bbd5efd3-bd3b-4bc9-8474-a6820a313483"));

            migrationBuilder.DeleteData(
                table: "MediaPieces",
                keyColumn: "Id",
                keyValue: new Guid("706c2e99-6f6c-4472-81a5-43c56e11637c"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "MediaPieces",
                columns: new[] { "Id", "CachedRating", "Group", "LastCacheUpdate", "Name" },
                values: new object[] { new Guid("706c2e99-6f6c-4472-81a5-43c56e11637c"), 0.0, 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "TestMediaPiece" });

            migrationBuilder.InsertData(
                table: "Ratings",
                columns: new[] { "Id", "AuthorId", "Grade", "MediaPieceId" },
                values: new object[] { new Guid("bbd5efd3-bd3b-4bc9-8474-a6820a313483"), null, 0, new Guid("706c2e99-6f6c-4472-81a5-43c56e11637c") });

            migrationBuilder.InsertData(
                table: "Reviews",
                columns: new[] { "Id", "CachedLikes", "Content", "CreatedDate", "LastCacheUpdate", "Name", "RatingId" },
                values: new object[] { new Guid("ddd4edd3-bd3b-4bc9-8474-a6820a313483"), 0, "test TestSearch 1234", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Test Review", new Guid("bbd5efd3-bd3b-4bc9-8474-a6820a313483") });
        }
    }
}
