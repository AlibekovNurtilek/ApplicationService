using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApplicationService.Migrations
{
    /// <inheritdoc />
    public partial class AddStatement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Statements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DoB = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PassportFrontImage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PassportBackImage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DiplomImage = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Statements", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Statements");
        }
    }
}
