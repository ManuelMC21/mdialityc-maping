using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Maping.Migrations
{
    /// <inheritdoc />
    public partial class CorrectionMigration10 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Restaurants_EntityId",
                table: "Restaurants");

            migrationBuilder.CreateIndex(
                name: "IX_Restaurants_EntityId",
                table: "Restaurants",
                column: "EntityId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Restaurants_EntityId",
                table: "Restaurants");

            migrationBuilder.CreateIndex(
                name: "IX_Restaurants_EntityId",
                table: "Restaurants",
                column: "EntityId");
        }
    }
}
