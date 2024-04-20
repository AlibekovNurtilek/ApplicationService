using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApplicationService.Migrations
{
    /// <inheritdoc />
    public partial class GlobalexamUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ExamId",
                table: "GlobalExams",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExamId",
                table: "GlobalExams");
        }
    }
}
