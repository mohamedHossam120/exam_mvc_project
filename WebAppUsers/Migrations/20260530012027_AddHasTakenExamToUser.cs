using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAppUsers.Migrations
{
    /// <inheritdoc />
    public partial class AddHasTakenExamToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasTakenExam",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasTakenExam",
                table: "Users");
        }
    }
}
