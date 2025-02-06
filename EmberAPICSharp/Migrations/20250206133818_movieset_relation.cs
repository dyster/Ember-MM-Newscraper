using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmberAPICSharp.Migrations
{
    /// <inheritdoc />
    public partial class movieset_relation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "actor_link");

            migrationBuilder.DropTable(
                name: "movieset_link");

            migrationBuilder.CreateTable(
                name: "MovieMovieset",
                columns: table => new
                {
                    MoviesId = table.Column<long>(type: "INTEGER", nullable: false),
                    MoviesetsId = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieMovieset", x => new { x.MoviesId, x.MoviesetsId });
                    table.ForeignKey(
                        name: "FK_MovieMovieset_movie_MoviesId",
                        column: x => x.MoviesId,
                        principalTable: "movie",
                        principalColumn: "idMovie",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MovieMovieset_movieset_MoviesetsId",
                        column: x => x.MoviesetsId,
                        principalTable: "movieset",
                        principalColumn: "idSet",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MovieMovieset_MoviesetsId",
                table: "MovieMovieset",
                column: "MoviesetsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MovieMovieset");

            migrationBuilder.CreateTable(
                name: "actor_link",
                columns: table => new
                {
                    cast_order = table.Column<int>(type: "INTEGER", nullable: true),
                    idMedia = table.Column<int>(type: "INTEGER", nullable: true),
                    idPerson = table.Column<int>(type: "INTEGER", nullable: true),
                    media_type = table.Column<string>(type: "TEXT", nullable: true),
                    role = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "movieset_link",
                columns: table => new
                {
                    idMovie = table.Column<int>(type: "INTEGER", nullable: false),
                    idSet = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateIndex(
                name: "ix_actor_link_1",
                table: "actor_link",
                columns: new[] { "idPerson", "media_type", "idMedia" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_actor_link_2",
                table: "actor_link",
                columns: new[] { "idMedia", "media_type", "idPerson" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_actor_link_3",
                table: "actor_link",
                column: "media_type");

            migrationBuilder.CreateIndex(
                name: "movieset_link_1",
                table: "movieset_link",
                columns: new[] { "idSet", "idMovie" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "movieset_link_2",
                table: "movieset_link",
                columns: new[] { "idMovie", "idSet" },
                unique: true);
        }
    }
}
