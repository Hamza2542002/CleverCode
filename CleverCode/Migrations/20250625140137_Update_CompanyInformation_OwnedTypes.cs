using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleverCode.Migrations
{
    /// <inheritdoc />
    public partial class Update_CompanyInformation_OwnedTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Values",
                table: "CompanyInformations");

            migrationBuilder.RenameColumn(
                name: "ContactInfo",
                table: "CompanyInformations",
                newName: "ContactInfo_Address");

            migrationBuilder.AddColumn<string>(
                name: "ContactInfo_Email",
                table: "CompanyInformations",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContactInfo_Phone",
                table: "CompanyInformations",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Values_Description",
                table: "CompanyInformations",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Values_Name",
                table: "CompanyInformations",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContactInfo_Email",
                table: "CompanyInformations");

            migrationBuilder.DropColumn(
                name: "ContactInfo_Phone",
                table: "CompanyInformations");

            migrationBuilder.DropColumn(
                name: "Values_Description",
                table: "CompanyInformations");

            migrationBuilder.DropColumn(
                name: "Values_Name",
                table: "CompanyInformations");

            migrationBuilder.RenameColumn(
                name: "ContactInfo_Address",
                table: "CompanyInformations",
                newName: "ContactInfo");

            migrationBuilder.AddColumn<string>(
                name: "Values",
                table: "CompanyInformations",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
