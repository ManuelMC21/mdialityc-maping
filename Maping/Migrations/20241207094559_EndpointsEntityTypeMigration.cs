using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Maping.Migrations
{
    /// <inheritdoc />
    public partial class EndpointsEntityTypeMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Entities_EntityType_EntityTypeId",
                table: "Entities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EntityType",
                table: "EntityType");

            migrationBuilder.RenameTable(
                name: "EntityType",
                newName: "Types");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Types",
                table: "Types",
                column: "EntityTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Entities_Types_EntityTypeId",
                table: "Entities",
                column: "EntityTypeId",
                principalTable: "Types",
                principalColumn: "EntityTypeId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Entities_Types_EntityTypeId",
                table: "Entities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Types",
                table: "Types");

            migrationBuilder.RenameTable(
                name: "Types",
                newName: "EntityType");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EntityType",
                table: "EntityType",
                column: "EntityTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Entities_EntityType_EntityTypeId",
                table: "Entities",
                column: "EntityTypeId",
                principalTable: "EntityType",
                principalColumn: "EntityTypeId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
