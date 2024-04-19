using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApplicationService.Migrations
{
    /// <inheritdoc />
    public partial class UpdateStatement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsMagistr",
                table: "Statements",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsMagistr",
                table: "Statements");
        }
    }
}
