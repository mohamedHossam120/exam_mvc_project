using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAppUsers.Migrations
{
    /// <inheritdoc />
    public partial class AddSubjectScoresToUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SubjectScores",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubjectScores",
                table: "Users");
        }
    }
}
