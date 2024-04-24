using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExperimentAgent.API.Migrations
{
    /// <inheritdoc />
    public partial class ChangeProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "QueryExecutionTime",
                table: "ExperimentOutcomes",
                newName: "ExperimentExecutionTime");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ExperimentExecutionTime",
                table: "ExperimentOutcomes",
                newName: "QueryExecutionTime");
        }
    }
}
