using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExperimentAgent.API.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExperimentOutcomes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Query = table.Column<string>(type: "text", nullable: false),
                    DatabaseType = table.Column<int>(type: "integer", nullable: false),
                    IsExecutedFromCache = table.Column<bool>(type: "boolean", nullable: false),
                    QueryExecutionNumber = table.Column<int>(type: "integer", nullable: false),
                    CacheHitRate = table.Column<double>(type: "double precision", nullable: false),
                    CacheMissRate = table.Column<double>(type: "double precision", nullable: false),
                    QueryExecutionTime = table.Column<string>(type: "text", nullable: false),
                    Resources = table.Column<string>(type: "text", nullable: false),
                    CacheSize = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExperimentOutcomes", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExperimentOutcomes");
        }
    }
}
