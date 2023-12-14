using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace University_System.Migrations
{
    /// <inheritdoc />
    public partial class UniSystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    studentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    studentName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    gender = table.Column<bool>(type: "bit", nullable: false),
                    email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    examSelected = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.studentId);
                });

            migrationBuilder.CreateTable(
                name: "Teachers",
                columns: table => new
                {
                    teacherId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    teacherName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    gender = table.Column<bool>(type: "bit", nullable: false),
                    email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teachers", x => x.teacherId);
                });

            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    courseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    courseName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    teacherId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.courseId);
                    table.ForeignKey(
                        name: "FK_Courses_Teachers_teacherId",
                        column: x => x.teacherId,
                        principalTable: "Teachers",
                        principalColumn: "teacherId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ScoreResults",
                columns: table => new
                {
                    scoreResultId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    mark = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    grade = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    courseId = table.Column<int>(type: "int", nullable: false),
                    studentId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScoreResults", x => x.scoreResultId);
                    table.ForeignKey(
                        name: "FK_ScoreResults_Courses_courseId",
                        column: x => x.courseId,
                        principalTable: "Courses",
                        principalColumn: "courseId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScoreResults_Students_studentId",
                        column: x => x.studentId,
                        principalTable: "Students",
                        principalColumn: "studentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Courses_teacherId",
                table: "Courses",
                column: "teacherId");

            migrationBuilder.CreateIndex(
                name: "IX_ScoreResults_courseId",
                table: "ScoreResults",
                column: "courseId");

            migrationBuilder.CreateIndex(
                name: "IX_ScoreResults_studentId",
                table: "ScoreResults",
                column: "studentId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_email",
                table: "Students",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_email",
                table: "Teachers",
                column: "email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ScoreResults");

            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "Teachers");
        }
    }
}
