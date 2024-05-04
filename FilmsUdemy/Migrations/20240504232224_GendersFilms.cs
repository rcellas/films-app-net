using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FilmsUdemy.Migrations
{
    /// <inheritdoc />
    public partial class GendersFilms : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GendersFilms",
                columns: table => new
                {
                    GenderId = table.Column<int>(type: "int", nullable: false),
                    FilmId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GendersFilms", x => new { x.GenderId, x.FilmId });
                    table.ForeignKey(
                        name: "FK_GendersFilms_Films_FilmId",
                        column: x => x.FilmId,
                        principalTable: "Films",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GendersFilms_Genders_GenderId",
                        column: x => x.GenderId,
                        principalTable: "Genders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GendersFilms_FilmId",
                table: "GendersFilms",
                column: "FilmId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GendersFilms");
        }
    }
}
