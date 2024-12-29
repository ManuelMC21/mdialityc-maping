using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Maping.Migrations
{
    /// <inheritdoc />
    public partial class UserAndEntityRelationMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Entities",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Entities_UserId",
                table: "Entities",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Entities_Users_UserId",
                table: "Entities",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Entities_Users_UserId",
                table: "Entities");

            migrationBuilder.DropIndex(
                name: "IX_Entities_UserId",
                table: "Entities");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Entities");
        }
    }
}
