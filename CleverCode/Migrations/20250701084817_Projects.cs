using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleverCode.Migrations
{
    /// <inheritdoc />
    public partial class Projects : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "Projects");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Projects",
                newName: "DescriptionEn");

            migrationBuilder.AddColumn<string>(
                name: "DescriptionAr",
                table: "Projects",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TitleAr",
                table: "Projects",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TitleEn",
                table: "Projects",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DescriptionAr",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "TitleAr",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "TitleEn",
                table: "Projects");

            migrationBuilder.RenameColumn(
                name: "DescriptionEn",
                table: "Projects",
                newName: "Description");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Projects",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }
    }
}
