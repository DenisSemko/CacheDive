using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExperimentAgent.API.Migrations
{
    /// <inheritdoc />
    public partial class RemoveIsExecutedFromCachefield : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsExecutedFromCache",
                table: "ExperimentOutcomes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsExecutedFromCache",
                table: "ExperimentOutcomes",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
