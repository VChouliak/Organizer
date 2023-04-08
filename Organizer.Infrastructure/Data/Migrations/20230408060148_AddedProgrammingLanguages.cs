using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace Organizer.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedProgrammingLanguages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProgrammingLanguageId",
                table: "Friends",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ProgrammingLanguages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProgrammingLanguages", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Friends_ProgrammingLanguageId",
                table: "Friends",
                column: "ProgrammingLanguageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Friends_ProgrammingLanguages_ProgrammingLanguageId",
                table: "Friends",
                column: "ProgrammingLanguageId",
                principalTable: "ProgrammingLanguages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Friends_ProgrammingLanguages_ProgrammingLanguageId",
                table: "Friends");

            migrationBuilder.DropTable(
                name: "ProgrammingLanguages");

            migrationBuilder.DropIndex(
                name: "IX_Friends_ProgrammingLanguageId",
                table: "Friends");

            migrationBuilder.DropColumn(
                name: "ProgrammingLanguageId",
                table: "Friends");
        }
    }
}
