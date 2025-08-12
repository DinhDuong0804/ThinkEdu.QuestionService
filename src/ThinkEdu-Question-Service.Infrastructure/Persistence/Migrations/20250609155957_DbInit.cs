using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThinkEdu_Question_Service.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class DbInit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    lesson_id = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    title = table.Column<string>(type: "varchar(2000)", maxLength: 2000, nullable: false),
                    type = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    level = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    group = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    file_url = table.Column<string>(type: "varchar(2000)", maxLength: 2000, nullable: true),
                    explanation = table.Column<string>(type: "varchar(2000)", maxLength: 2000, nullable: true),
                    parent_id = table.Column<string>(type: "varchar(2555)", maxLength: 255, nullable: true),
                    stt = table.Column<int>(type: "int", maxLength: 10, nullable: true),
                    tenant_id = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    status = table.Column<string>(type: "varchar(50)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Answers",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    content = table.Column<string>(type: "varchar(2000)", maxLength: 255, nullable: true),
                    file_url = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    is_correct = table.Column<bool>(type: "boolean", nullable: false),
                    tenant_id = table.Column<int>(type: "integer", nullable: true),
                    question_id = table.Column<string>(type: "varchar(255)", nullable: true),
                    status = table.Column<string>(type: "varchar(50)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Answers", x => x.id);
                    table.ForeignKey(
                        name: "FK_Answers_Questions_question_id",
                        column: x => x.question_id,
                        principalTable: "Questions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Answers_question_id",
                table: "Answers",
                column: "question_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Answers");

            migrationBuilder.DropTable(
                name: "Questions");
        }
    }
}
