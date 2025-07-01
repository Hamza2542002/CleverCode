using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleverCode.Migrations
{
    /// <inheritdoc />
    public partial class Test : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Questions",
                table: "FAQs",
                newName: "QuestionsEn");

            migrationBuilder.RenameColumn(
                name: "Answer",
                table: "FAQs",
                newName: "AnswerEn");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Complaints",
                newName: "Type_EN");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Complaints",
                newName: "Status_EN");

            migrationBuilder.RenameColumn(
                name: "Priority",
                table: "Complaints",
                newName: "Status_AR");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Complaints",
                newName: "Type_AR");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Complaints",
                newName: "Description_EN");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "CompanyValues",
                newName: "NameEn");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "CompanyValues",
                newName: "NameAr");

            migrationBuilder.RenameColumn(
                name: "Vision",
                table: "CompanyInformations",
                newName: "VisionEn");

            migrationBuilder.RenameColumn(
                name: "Story",
                table: "CompanyInformations",
                newName: "StoryEn");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "CompanyInformations",
                newName: "NameEn");

            migrationBuilder.RenameColumn(
                name: "Mission",
                table: "CompanyInformations",
                newName: "VisionAr");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "CompanyInformations",
                newName: "StoryAr");

            migrationBuilder.RenameColumn(
                name: "ContactInfo_Address",
                table: "CompanyInformations",
                newName: "ContactInfo_AddressEn");

            migrationBuilder.AddColumn<string>(
                name: "AnswerAr",
                table: "FAQs",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "QuestionsAr",
                table: "FAQs",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description_AR",
                table: "Complaints",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name_AR",
                table: "Complaints",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name_EN",
                table: "Complaints",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Priority_AR",
                table: "Complaints",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Priority_EN",
                table: "Complaints",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionAr",
                table: "CompanyValues",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionEn",
                table: "CompanyValues",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContactInfo_AddressAr",
                table: "CompanyInformations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionAr",
                table: "CompanyInformations",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionEn",
                table: "CompanyInformations",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MissionAr",
                table: "CompanyInformations",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MissionEn",
                table: "CompanyInformations",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameAr",
                table: "CompanyInformations",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnswerAr",
                table: "FAQs");

            migrationBuilder.DropColumn(
                name: "QuestionsAr",
                table: "FAQs");

            migrationBuilder.DropColumn(
                name: "Description_AR",
                table: "Complaints");

            migrationBuilder.DropColumn(
                name: "Name_AR",
                table: "Complaints");

            migrationBuilder.DropColumn(
                name: "Name_EN",
                table: "Complaints");

            migrationBuilder.DropColumn(
                name: "Priority_AR",
                table: "Complaints");

            migrationBuilder.DropColumn(
                name: "Priority_EN",
                table: "Complaints");

            migrationBuilder.DropColumn(
                name: "DescriptionAr",
                table: "CompanyValues");

            migrationBuilder.DropColumn(
                name: "DescriptionEn",
                table: "CompanyValues");

            migrationBuilder.DropColumn(
                name: "ContactInfo_AddressAr",
                table: "CompanyInformations");

            migrationBuilder.DropColumn(
                name: "DescriptionAr",
                table: "CompanyInformations");

            migrationBuilder.DropColumn(
                name: "DescriptionEn",
                table: "CompanyInformations");

            migrationBuilder.DropColumn(
                name: "MissionAr",
                table: "CompanyInformations");

            migrationBuilder.DropColumn(
                name: "MissionEn",
                table: "CompanyInformations");

            migrationBuilder.DropColumn(
                name: "NameAr",
                table: "CompanyInformations");

            migrationBuilder.RenameColumn(
                name: "QuestionsEn",
                table: "FAQs",
                newName: "Questions");

            migrationBuilder.RenameColumn(
                name: "AnswerEn",
                table: "FAQs",
                newName: "Answer");

            migrationBuilder.RenameColumn(
                name: "Type_EN",
                table: "Complaints",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "Type_AR",
                table: "Complaints",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "Status_EN",
                table: "Complaints",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "Status_AR",
                table: "Complaints",
                newName: "Priority");

            migrationBuilder.RenameColumn(
                name: "Description_EN",
                table: "Complaints",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "NameEn",
                table: "CompanyValues",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "NameAr",
                table: "CompanyValues",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "VisionEn",
                table: "CompanyInformations",
                newName: "Vision");

            migrationBuilder.RenameColumn(
                name: "VisionAr",
                table: "CompanyInformations",
                newName: "Mission");

            migrationBuilder.RenameColumn(
                name: "StoryEn",
                table: "CompanyInformations",
                newName: "Story");

            migrationBuilder.RenameColumn(
                name: "StoryAr",
                table: "CompanyInformations",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "NameEn",
                table: "CompanyInformations",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "ContactInfo_AddressEn",
                table: "CompanyInformations",
                newName: "ContactInfo_Address");
        }
    }
}
