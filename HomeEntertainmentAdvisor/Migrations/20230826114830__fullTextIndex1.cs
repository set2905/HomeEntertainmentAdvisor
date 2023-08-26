﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeEntertainmentAdvisor.Migrations
{
    /// <inheritdoc />
    public partial class _fullTextIndex1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("CREATE FULLTEXT CATALOG ft AS DEFAULT", true);
            migrationBuilder.Sql("CREATE FULLTEXT INDEX ON dbo.Reviews(Content) KEY INDEX UI_Reviews_Content WITH STOPLIST = SYSTEM", true);
            migrationBuilder.Sql("CREATE FULLTEXT INDEX ON dbo.Reviews(Name) KEY INDEX UI_Reviews_Name WITH STOPLIST = SYSTEM", true);
            migrationBuilder.Sql("CREATE FULLTEXT INDEX ON dbo.Comments(Content) KEY INDEX UI_Comments_Content WITH STOPLIST = SYSTEM", true);

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
