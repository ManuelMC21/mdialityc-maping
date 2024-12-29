using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Maping.Migrations
{
    /// <inheritdoc />
    public partial class ImagesFunctionsAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImagesUrl",
                table: "Entities",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagesUrl",
                table: "Entities");
        }
    }
}
