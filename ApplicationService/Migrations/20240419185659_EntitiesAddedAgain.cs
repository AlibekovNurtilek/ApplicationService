using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApplicationService.Migrations
{
    /// <inheritdoc />
    public partial class EntitiesAddedAgain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AvgGrades",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationUserId = table.Column<int>(type: "int", nullable: false),
                    ApplicationUserId1 = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AvgGrades", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AvgGrades_AspNetUsers_ApplicationUserId1",
                        column: x => x.ApplicationUserId1,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Exams",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApplicationUserStudId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ApplicationUserEmpId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Exams_AspNetUsers_ApplicationUserEmpId",
                        column: x => x.ApplicationUserEmpId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Exams_AspNetUsers_ApplicationUserStudId",
                        column: x => x.ApplicationUserStudId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ExamImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExamId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExamImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExamImages_Exams_ExamId",
                        column: x => x.ExamId,
                        principalTable: "Exams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Grades",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExamId = table.Column<int>(type: "int", nullable: false),
                    ApplicationUserStudId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ApplicationUserEmpId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    StudGrade = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grades", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Grades_AspNetUsers_ApplicationUserEmpId",
                        column: x => x.ApplicationUserEmpId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Grades_AspNetUsers_ApplicationUserStudId",
                        column: x => x.ApplicationUserStudId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Grades_Exams_ExamId",
                        column: x => x.ExamId,
                        principalTable: "Exams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AvgGrades_ApplicationUserId1",
                table: "AvgGrades",
                column: "ApplicationUserId1");

            migrationBuilder.CreateIndex(
                name: "IX_ExamImages_ExamId",
                table: "ExamImages",
                column: "ExamId");

            migrationBuilder.CreateIndex(
                name: "IX_Exams_ApplicationUserEmpId",
                table: "Exams",
                column: "ApplicationUserEmpId");

            migrationBuilder.CreateIndex(
                name: "IX_Exams_ApplicationUserStudId",
                table: "Exams",
                column: "ApplicationUserStudId");

            migrationBuilder.CreateIndex(
                name: "IX_Grades_ApplicationUserEmpId",
                table: "Grades",
                column: "ApplicationUserEmpId");

            migrationBuilder.CreateIndex(
                name: "IX_Grades_ApplicationUserStudId",
                table: "Grades",
                column: "ApplicationUserStudId");

            migrationBuilder.CreateIndex(
                name: "IX_Grades_ExamId",
                table: "Grades",
                column: "ExamId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AvgGrades");

            migrationBuilder.DropTable(
                name: "ExamImages");

            migrationBuilder.DropTable(
                name: "Grades");

            migrationBuilder.DropTable(
                name: "Exams");
        }
    }
}
