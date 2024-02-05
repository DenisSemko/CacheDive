using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SQLServerAgent.API.Migrations
{
    /// <inheritdoc />
    public partial class AddNullableFKs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProductOrders_OrderId",
                table: "ProductOrders");

            migrationBuilder.DropIndex(
                name: "IX_ProductBaskets_BasketId",
                table: "ProductBaskets");

            migrationBuilder.CreateIndex(
                name: "IX_ProductOrders_OrderId",
                table: "ProductOrders",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductBaskets_BasketId",
                table: "ProductBaskets",
                column: "BasketId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProductOrders_OrderId",
                table: "ProductOrders");

            migrationBuilder.DropIndex(
                name: "IX_ProductBaskets_BasketId",
                table: "ProductBaskets");

            migrationBuilder.CreateIndex(
                name: "IX_ProductOrders_OrderId",
                table: "ProductOrders",
                column: "OrderId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductBaskets_BasketId",
                table: "ProductBaskets",
                column: "BasketId",
                unique: true);
        }
    }
}
