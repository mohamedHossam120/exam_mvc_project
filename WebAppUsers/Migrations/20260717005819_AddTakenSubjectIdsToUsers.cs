using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAppUsers.Migrations
{
    public partial class AddTakenSubjectIdsToUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasTakenExam",
                table: "Users");

            migrationBuilder.AddColumn<string>(
                name: "TakenSubjectIds",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TakenSubjectIds",
                table: "Users");

            migrationBuilder.AddColumn<bool>(
                name: "HasTakenExam",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
