using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmberAPICSharp.Migrations
{
    /// <inheritdoc />
    public partial class person_changes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "imdbid",
                table: "person",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "tmdbid",
                table: "person",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "tvdbid",
                table: "person",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "EpisodeRoleLinks",
                columns: table => new
                {
                    PersonId = table.Column<long>(type: "INTEGER", nullable: false),
                    EpisodeId = table.Column<long>(type: "INTEGER", nullable: false),
                    Role = table.Column<string>(type: "TEXT", nullable: true),
                    CastOrder = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EpisodeRoleLinks", x => new { x.EpisodeId, x.PersonId });
                    table.ForeignKey(
                        name: "FK_EpisodeRoleLinks_episode_EpisodeId",
                        column: x => x.EpisodeId,
                        principalTable: "episode",
                        principalColumn: "idEpisode",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EpisodeRoleLinks_person_PersonId",
                        column: x => x.PersonId,
                        principalTable: "person",
                        principalColumn: "idPerson",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MovieRoleLinks",
                columns: table => new
                {
                    PersonId = table.Column<long>(type: "INTEGER", nullable: false),
                    MovieId = table.Column<long>(type: "INTEGER", nullable: false),
                    Role = table.Column<string>(type: "TEXT", nullable: true),
                    CastOrder = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieRoleLinks", x => new { x.MovieId, x.PersonId });
                    table.ForeignKey(
                        name: "FK_MovieRoleLinks_movie_MovieId",
                        column: x => x.MovieId,
                        principalTable: "movie",
                        principalColumn: "idMovie",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MovieRoleLinks_person_PersonId",
                        column: x => x.PersonId,
                        principalTable: "person",
                        principalColumn: "idPerson",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TvshowRoleLinks",
                columns: table => new
                {
                    PersonId = table.Column<long>(type: "INTEGER", nullable: false),
                    TvshowId = table.Column<long>(type: "INTEGER", nullable: false),
                    Role = table.Column<string>(type: "TEXT", nullable: true),
                    CastOrder = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TvshowRoleLinks", x => new { x.PersonId, x.TvshowId });
                    table.ForeignKey(
                        name: "FK_TvshowRoleLinks_person_PersonId",
                        column: x => x.PersonId,
                        principalTable: "person",
                        principalColumn: "idPerson",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TvshowRoleLinks_tvshow_TvshowId",
                        column: x => x.TvshowId,
                        principalTable: "tvshow",
                        principalColumn: "idShow",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EpisodeRoleLinks_PersonId",
                table: "EpisodeRoleLinks",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_MovieRoleLinks_PersonId",
                table: "MovieRoleLinks",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_TvshowRoleLinks_TvshowId",
                table: "TvshowRoleLinks",
                column: "TvshowId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EpisodeRoleLinks");

            migrationBuilder.DropTable(
                name: "MovieRoleLinks");

            migrationBuilder.DropTable(
                name: "TvshowRoleLinks");

            migrationBuilder.DropColumn(
                name: "imdbid",
                table: "person");

            migrationBuilder.DropColumn(
                name: "tmdbid",
                table: "person");

            migrationBuilder.DropColumn(
                name: "tvdbid",
                table: "person");
        }
    }
}
