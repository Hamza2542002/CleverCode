using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleverCode.Migrations
{
    /// <inheritdoc />
    public partial class InitCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Complaints_Services_Service_ID1",
                table: "Complaints");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Services_Service_ID1",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Services_Service_ID1",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_Service_ID1",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Messages_Service_ID1",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Complaints_Service_ID1",
                table: "Complaints");

            migrationBuilder.DropColumn(
                name: "Service_ID1",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "Service_ID1",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "Service_ID1",
                table: "Complaints");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Service_ID1",
                table: "Reviews",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Service_ID1",
                table: "Messages",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Service_ID1",
                table: "Complaints",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_Service_ID1",
                table: "Reviews",
                column: "Service_ID1");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_Service_ID1",
                table: "Messages",
                column: "Service_ID1");

            migrationBuilder.CreateIndex(
                name: "IX_Complaints_Service_ID1",
                table: "Complaints",
                column: "Service_ID1");

            migrationBuilder.AddForeignKey(
                name: "FK_Complaints_Services_Service_ID1",
                table: "Complaints",
                column: "Service_ID1",
                principalTable: "Services",
                principalColumn: "Service_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Services_Service_ID1",
                table: "Messages",
                column: "Service_ID1",
                principalTable: "Services",
                principalColumn: "Service_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Services_Service_ID1",
                table: "Reviews",
                column: "Service_ID1",
                principalTable: "Services",
                principalColumn: "Service_ID");
        }
    }
}
