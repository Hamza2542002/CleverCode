using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleverCode.Migrations
{
    /// <inheritdoc />
    public partial class updateContactInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContactInfo");

            migrationBuilder.AddColumn<string>(
                name: "ContactInfo_Address",
                table: "CompanyInformations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContactInfo_Email",
                table: "CompanyInformations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContactInfo_Phone",
                table: "CompanyInformations",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContactInfo_Address",
                table: "CompanyInformations");

            migrationBuilder.DropColumn(
                name: "ContactInfo_Email",
                table: "CompanyInformations");

            migrationBuilder.DropColumn(
                name: "ContactInfo_Phone",
                table: "CompanyInformations");

            migrationBuilder.CreateTable(
                name: "ContactInfo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Company_ID = table.Column<int>(type: "int", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContactInfo_CompanyInformations_Company_ID",
                        column: x => x.Company_ID,
                        principalTable: "CompanyInformations",
                        principalColumn: "Company_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContactInfo_Company_ID",
                table: "ContactInfo",
                column: "Company_ID",
                unique: true);
        }
    }
}
