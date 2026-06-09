using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAppUsers.Migrations
{
    public partial class AddHasTakenExamToUserScore : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ExamScore",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExamScore",
                table: "Users");
        }
    }
}
