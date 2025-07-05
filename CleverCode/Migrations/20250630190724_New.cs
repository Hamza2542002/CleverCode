using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleverCode.Migrations
{
    /// <inheritdoc />
    public partial class New : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MessageText",
                table: "Messages",
                newName: "MessageTextEn");

            migrationBuilder.AddColumn<string>(
                name: "MessageTextAr",
                table: "Messages",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MessageTextAr",
                table: "Messages");

            migrationBuilder.RenameColumn(
                name: "MessageTextEn",
                table: "Messages",
                newName: "MessageText");
        }
    }
}
