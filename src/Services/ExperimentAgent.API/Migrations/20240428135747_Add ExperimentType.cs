using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExperimentAgent.API.Migrations
{
    /// <inheritdoc />
    public partial class AddExperimentType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ExperimentType",
                table: "ExperimentOutcomes",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExperimentType",
                table: "ExperimentOutcomes");
        }
    }
}
