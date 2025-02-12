using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmberAPICSharp.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "actor_link",
                columns: table => new
                {
                    idPerson = table.Column<int>(type: "INTEGER", nullable: true),
                    idMedia = table.Column<int>(type: "INTEGER", nullable: true),
                    media_type = table.Column<string>(type: "TEXT", nullable: true),
                    role = table.Column<string>(type: "TEXT", nullable: true),
                    cast_order = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "art",
                columns: table => new
                {
                    idArt = table.Column<int>(type: "INTEGER", nullable: false),
                    idMedia = table.Column<int>(type: "INTEGER", nullable: true),
                    media_type = table.Column<string>(type: "TEXT", nullable: true),
                    type = table.Column<string>(type: "TEXT", nullable: true),
                    url = table.Column<string>(type: "TEXT", nullable: true),
                    width = table.Column<int>(type: "INTEGER", nullable: true),
                    height = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_art", x => x.idArt);
                });

            migrationBuilder.CreateTable(
                name: "certification",
                columns: table => new
                {
                    idCertification = table.Column<int>(type: "INTEGER", nullable: false),
                    name = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_certification", x => x.idCertification);
                });

            migrationBuilder.CreateTable(
                name: "certification_link",
                columns: table => new
                {
                    idCertification = table.Column<int>(type: "INTEGER", nullable: true),
                    idMedia = table.Column<int>(type: "INTEGER", nullable: true),
                    media_type = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "certification_temp",
                columns: table => new
                {
                    idMedia = table.Column<int>(type: "INTEGER", nullable: true),
                    media_type = table.Column<string>(type: "TEXT", nullable: true),
                    name = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "country",
                columns: table => new
                {
                    idCountry = table.Column<int>(type: "INTEGER", nullable: false),
                    name = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_country", x => x.idCountry);
                });

            migrationBuilder.CreateTable(
                name: "country_link",
                columns: table => new
                {
                    idCountry = table.Column<int>(type: "INTEGER", nullable: true),
                    idMedia = table.Column<int>(type: "INTEGER", nullable: true),
                    media_type = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "creator_link",
                columns: table => new
                {
                    idPerson = table.Column<int>(type: "INTEGER", nullable: true),
                    idMedia = table.Column<int>(type: "INTEGER", nullable: true),
                    media_type = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "director_link",
                columns: table => new
                {
                    idPerson = table.Column<int>(type: "INTEGER", nullable: true),
                    idMedia = table.Column<int>(type: "INTEGER", nullable: true),
                    media_type = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "excludedpath",
                columns: table => new
                {
                    path = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_excludedpath", x => x.path);
                });

            migrationBuilder.CreateTable(
                name: "file",
                columns: table => new
                {
                    idFile = table.Column<int>(type: "INTEGER", nullable: false),
                    path = table.Column<string>(type: "TEXT", nullable: false),
                    originalFileName = table.Column<string>(type: "TEXT", nullable: true),
                    fileSize = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_file", x => x.idFile);
                });

            migrationBuilder.CreateTable(
                name: "file_temp",
                columns: table => new
                {
                    idMovie = table.Column<int>(type: "INTEGER", nullable: true),
                    path = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "genre",
                columns: table => new
                {
                    idGenre = table.Column<int>(type: "INTEGER", nullable: false),
                    name = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_genre", x => x.idGenre);
                });

            migrationBuilder.CreateTable(
                name: "genre_link",
                columns: table => new
                {
                    idGenre = table.Column<int>(type: "INTEGER", nullable: true),
                    idMedia = table.Column<int>(type: "INTEGER", nullable: true),
                    media_type = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "gueststar_link",
                columns: table => new
                {
                    idPerson = table.Column<int>(type: "INTEGER", nullable: true),
                    idMedia = table.Column<int>(type: "INTEGER", nullable: true),
                    media_type = table.Column<string>(type: "TEXT", nullable: true),
                    role = table.Column<string>(type: "TEXT", nullable: true),
                    cast_order = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "MoviesAStreams",
                columns: table => new
                {
                    MovieID = table.Column<int>(type: "INTEGER", nullable: false),
                    StreamID = table.Column<int>(type: "INTEGER", nullable: false),
                    Audio_Language = table.Column<string>(type: "TEXT", nullable: true),
                    Audio_LongLanguage = table.Column<string>(type: "TEXT", nullable: true),
                    Audio_Codec = table.Column<string>(type: "TEXT", nullable: true),
                    Audio_Channel = table.Column<string>(type: "TEXT", nullable: true),
                    Audio_Bitrate = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MoviesAStreams", x => new { x.MovieID, x.StreamID });
                });

            migrationBuilder.CreateTable(
                name: "movieset",
                columns: table => new
                {
                    idSet = table.Column<int>(type: "INTEGER", nullable: false),
                    nfoPath = table.Column<string>(type: "TEXT", nullable: true),
                    plot = table.Column<string>(type: "TEXT", nullable: true),
                    title = table.Column<string>(type: "TEXT", nullable: true),
                    @new = table.Column<bool>(name: "new", type: "bool", nullable: false),
                    marked = table.Column<bool>(type: "bool", nullable: false),
                    locked = table.Column<bool>(type: "bool", nullable: false),
                    sortMethod = table.Column<int>(type: "INTEGER", nullable: false),
                    language = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_movieset", x => x.idSet);
                });

            migrationBuilder.CreateTable(
                name: "movieset_link",
                columns: table => new
                {
                    idSet = table.Column<int>(type: "INTEGER", nullable: false),
                    idMovie = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "moviesource",
                columns: table => new
                {
                    idSource = table.Column<int>(type: "INTEGER", nullable: false),
                    scanRecursive = table.Column<bool>(type: "bool", nullable: false),
                    useFolderName = table.Column<bool>(type: "bool", nullable: false),
                    getYear = table.Column<bool>(type: "bool", nullable: false, defaultValue: true),
                    exclude = table.Column<bool>(type: "bool", nullable: false),
                    isSingle = table.Column<bool>(type: "bool", nullable: false),
                    language = table.Column<string>(type: "TEXT", nullable: false, defaultValue: "en-US"),
                    name = table.Column<string>(type: "TEXT", nullable: false),
                    path = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_moviesource", x => x.idSource);
                });

            migrationBuilder.CreateTable(
                name: "MoviesSubs",
                columns: table => new
                {
                    MovieID = table.Column<int>(type: "INTEGER", nullable: false),
                    StreamID = table.Column<int>(type: "INTEGER", nullable: false),
                    Subs_Language = table.Column<string>(type: "TEXT", nullable: true),
                    Subs_LongLanguage = table.Column<string>(type: "TEXT", nullable: true),
                    Subs_Type = table.Column<string>(type: "TEXT", nullable: true),
                    Subs_Path = table.Column<string>(type: "TEXT", nullable: true),
                    Subs_Forced = table.Column<bool>(type: "BOOL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MoviesSubs", x => new { x.MovieID, x.StreamID });
                });

            migrationBuilder.CreateTable(
                name: "MoviesVStreams",
                columns: table => new
                {
                    MovieID = table.Column<int>(type: "INTEGER", nullable: false),
                    StreamID = table.Column<int>(type: "INTEGER", nullable: false),
                    Video_Width = table.Column<string>(type: "TEXT", nullable: true),
                    Video_Height = table.Column<string>(type: "TEXT", nullable: true),
                    Video_Codec = table.Column<string>(type: "TEXT", nullable: true),
                    Video_Duration = table.Column<string>(type: "TEXT", nullable: true),
                    Video_ScanType = table.Column<string>(type: "TEXT", nullable: true),
                    Video_AspectDisplayRatio = table.Column<string>(type: "TEXT", nullable: true),
                    Video_Language = table.Column<string>(type: "TEXT", nullable: true),
                    Video_LongLanguage = table.Column<string>(type: "TEXT", nullable: true),
                    Video_Bitrate = table.Column<string>(type: "TEXT", nullable: true),
                    Video_MultiViewCount = table.Column<string>(type: "TEXT", nullable: true),
                    Video_MultiViewLayout = table.Column<string>(type: "TEXT", nullable: true),
                    Video_StereoMode = table.Column<string>(type: "TEXT", nullable: true),
                    Video_FileSize = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MoviesVStreams", x => new { x.MovieID, x.StreamID });
                });

            migrationBuilder.CreateTable(
                name: "person",
                columns: table => new
                {
                    idPerson = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    name = table.Column<string>(type: "TEXT", nullable: false),
                    thumb = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_person", x => x.idPerson);
                });

            migrationBuilder.CreateTable(
                name: "rating",
                columns: table => new
                {
                    idRating = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    idMedia = table.Column<int>(type: "INTEGER", nullable: true),
                    media_type = table.Column<string>(type: "TEXT", nullable: true),
                    rating_type = table.Column<string>(type: "TEXT", nullable: true),
                    rating_max = table.Column<int>(type: "INTEGER", nullable: true),
                    rating = table.Column<double>(type: "float(50)", nullable: true),
                    votes = table.Column<int>(type: "INTEGER", nullable: true),
                    isDefault = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rating", x => x.idRating);
                });

            migrationBuilder.CreateTable(
                name: "season",
                columns: table => new
                {
                    idSeason = table.Column<int>(type: "INTEGER", nullable: false),
                    idShow = table.Column<int>(type: "INTEGER", nullable: true),
                    season = table.Column<int>(type: "INTEGER", nullable: true),
                    title = table.Column<string>(type: "TEXT", nullable: true),
                    locked = table.Column<bool>(type: "bool", nullable: false),
                    marked = table.Column<bool>(type: "bool", nullable: false),
                    @new = table.Column<bool>(name: "new", type: "bool", nullable: false),
                    aired = table.Column<string>(type: "TEXT", nullable: true),
                    plot = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_season", x => x.idSeason);
                });

            migrationBuilder.CreateTable(
                name: "streamdetail",
                columns: table => new
                {
                    idFile = table.Column<int>(type: "INTEGER", nullable: true),
                    streamType = table.Column<int>(type: "INTEGER", nullable: true),
                    videoCodec = table.Column<string>(type: "TEXT", nullable: true),
                    videoAspect = table.Column<double>(type: "float", nullable: true),
                    videoBitrate = table.Column<int>(type: "INTEGER", nullable: true),
                    videoLanguage = table.Column<string>(type: "TEXT", nullable: true),
                    videoWidth = table.Column<int>(type: "INTEGER", nullable: true),
                    videoHeight = table.Column<int>(type: "INTEGER", nullable: true),
                    videoScantype = table.Column<string>(type: "TEXT", nullable: true),
                    videoDuration = table.Column<int>(type: "INTEGER", nullable: true),
                    videoMultiViewCount = table.Column<int>(type: "INTEGER", nullable: true),
                    videoMultiViewLayout = table.Column<string>(type: "TEXT", nullable: true),
                    videoStereoMode = table.Column<string>(type: "TEXT", nullable: true),
                    videoBitDepth = table.Column<int>(type: "INTEGER", nullable: true),
                    videoChromaSubsampling = table.Column<string>(type: "TEXT", nullable: true),
                    videoColourPrimaries = table.Column<string>(type: "TEXT", nullable: true),
                    audioCodec = table.Column<string>(type: "TEXT", nullable: true),
                    audioChannels = table.Column<int>(type: "INTEGER", nullable: true),
                    audioBitrate = table.Column<int>(type: "INTEGER", nullable: true),
                    audioLanguage = table.Column<string>(type: "TEXT", nullable: true),
                    audioBitDepth = table.Column<int>(type: "INTEGER", nullable: true),
                    audioAdditionalFeatures = table.Column<string>(type: "TEXT", nullable: true),
                    subtitleLanguage = table.Column<string>(type: "TEXT", nullable: true),
                    subtitleForced = table.Column<bool>(type: "boolean", nullable: true),
                    subtitlePath = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "studio",
                columns: table => new
                {
                    idStudio = table.Column<int>(type: "INTEGER", nullable: false),
                    name = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_studio", x => x.idStudio);
                });

            migrationBuilder.CreateTable(
                name: "studio_link",
                columns: table => new
                {
                    idStudio = table.Column<int>(type: "INTEGER", nullable: true),
                    idMedia = table.Column<int>(type: "INTEGER", nullable: true),
                    media_type = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "tag",
                columns: table => new
                {
                    idTag = table.Column<int>(type: "INTEGER", nullable: false),
                    name = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tag", x => x.idTag);
                });

            migrationBuilder.CreateTable(
                name: "tag_link",
                columns: table => new
                {
                    idTag = table.Column<int>(type: "INTEGER", nullable: true),
                    idMedia = table.Column<int>(type: "INTEGER", nullable: true),
                    media_type = table.Column<string>(type: "TEXT", nullable: true),
                    sorting = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "TVAStreams",
                columns: table => new
                {
                    TVEpID = table.Column<int>(type: "INTEGER", nullable: false),
                    StreamID = table.Column<int>(type: "INTEGER", nullable: false),
                    Audio_Language = table.Column<string>(type: "TEXT", nullable: true),
                    Audio_LongLanguage = table.Column<string>(type: "TEXT", nullable: true),
                    Audio_Codec = table.Column<string>(type: "TEXT", nullable: true),
                    Audio_Channel = table.Column<string>(type: "TEXT", nullable: true),
                    Audio_Bitrate = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TVAStreams", x => new { x.TVEpID, x.StreamID });
                });

            migrationBuilder.CreateTable(
                name: "tvshow",
                columns: table => new
                {
                    idShow = table.Column<int>(type: "INTEGER", nullable: false),
                    idSource = table.Column<int>(type: "INTEGER", nullable: false),
                    @new = table.Column<bool>(name: "new", type: "bool", nullable: true, defaultValue: false),
                    marked = table.Column<bool>(type: "bool", nullable: false),
                    path = table.Column<string>(type: "TEXT", nullable: false),
                    locked = table.Column<bool>(type: "bool", nullable: false),
                    episodeGuide = table.Column<string>(type: "TEXT", nullable: true),
                    plot = table.Column<string>(type: "TEXT", nullable: true),
                    premiered = table.Column<string>(type: "TEXT", nullable: true),
                    mpaa = table.Column<string>(type: "TEXT", nullable: true),
                    nfoPath = table.Column<string>(type: "TEXT", nullable: true),
                    language = table.Column<string>(type: "TEXT", nullable: true),
                    episodeOrdering = table.Column<int>(type: "INTEGER", nullable: false),
                    status = table.Column<string>(type: "TEXT", nullable: true),
                    themePath = table.Column<string>(type: "TEXT", nullable: true),
                    efanartsPath = table.Column<string>(type: "TEXT", nullable: true),
                    runtime = table.Column<string>(type: "TEXT", nullable: true),
                    title = table.Column<string>(type: "TEXT", nullable: true),
                    episodeSorting = table.Column<int>(type: "INTEGER", nullable: false),
                    sortTitle = table.Column<string>(type: "TEXT", nullable: true),
                    originalTitle = table.Column<string>(type: "TEXT", nullable: true),
                    userRating = table.Column<int>(type: "INTEGER", nullable: false),
                    dateModified = table.Column<int>(type: "INTEGER", nullable: true),
                    dateAdded = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tvshow", x => x.idShow);
                });

            migrationBuilder.CreateTable(
                name: "tvshow_link",
                columns: table => new
                {
                    idShow = table.Column<int>(type: "INTEGER", nullable: true),
                    idMovie = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "tvshowsource",
                columns: table => new
                {
                    idSource = table.Column<int>(type: "INTEGER", nullable: false),
                    episodeOrdering = table.Column<int>(type: "INTEGER", nullable: false),
                    episodeSorting = table.Column<int>(type: "INTEGER", nullable: false),
                    exclude = table.Column<bool>(type: "bool", nullable: false),
                    isSingle = table.Column<bool>(type: "bool", nullable: false),
                    language = table.Column<string>(type: "TEXT", nullable: false, defaultValue: "en-US"),
                    name = table.Column<string>(type: "TEXT", nullable: false),
                    path = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tvshowsource", x => x.idSource);
                });

            migrationBuilder.CreateTable(
                name: "TVSubs",
                columns: table => new
                {
                    TVEpID = table.Column<int>(type: "INTEGER", nullable: false),
                    StreamID = table.Column<int>(type: "INTEGER", nullable: false),
                    Subs_Language = table.Column<string>(type: "TEXT", nullable: true),
                    Subs_LongLanguage = table.Column<string>(type: "TEXT", nullable: true),
                    Subs_Type = table.Column<string>(type: "TEXT", nullable: true),
                    Subs_Path = table.Column<string>(type: "TEXT", nullable: true),
                    Subs_Forced = table.Column<bool>(type: "BOOL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TVSubs", x => new { x.TVEpID, x.StreamID });
                });

            migrationBuilder.CreateTable(
                name: "TVVStreams",
                columns: table => new
                {
                    TVEpID = table.Column<int>(type: "INTEGER", nullable: false),
                    StreamID = table.Column<int>(type: "INTEGER", nullable: false),
                    Video_Width = table.Column<string>(type: "TEXT", nullable: true),
                    Video_Height = table.Column<string>(type: "TEXT", nullable: true),
                    Video_Codec = table.Column<string>(type: "TEXT", nullable: true),
                    Video_Duration = table.Column<string>(type: "TEXT", nullable: true),
                    Video_ScanType = table.Column<string>(type: "TEXT", nullable: true),
                    Video_AspectDisplayRatio = table.Column<string>(type: "TEXT", nullable: true),
                    Video_Language = table.Column<string>(type: "TEXT", nullable: true),
                    Video_LongLanguage = table.Column<string>(type: "TEXT", nullable: true),
                    Video_Bitrate = table.Column<string>(type: "TEXT", nullable: true),
                    Video_MultiViewCount = table.Column<string>(type: "TEXT", nullable: true),
                    Video_MultiViewLayout = table.Column<string>(type: "TEXT", nullable: true),
                    Video_StereoMode = table.Column<string>(type: "TEXT", nullable: true),
                    Video_FileSize = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TVVStreams", x => new { x.TVEpID, x.StreamID });
                });

            migrationBuilder.CreateTable(
                name: "uniqueid",
                columns: table => new
                {
                    idUniqueID = table.Column<int>(type: "INTEGER", nullable: false),
                    idMedia = table.Column<int>(type: "INTEGER", nullable: true),
                    media_type = table.Column<string>(type: "TEXT", nullable: true),
                    value = table.Column<string>(type: "TEXT", nullable: true),
                    type = table.Column<string>(type: "TEXT", nullable: true),
                    isDefault = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uniqueid", x => x.idUniqueID);
                });

            migrationBuilder.CreateTable(
                name: "writer_link",
                columns: table => new
                {
                    idPerson = table.Column<int>(type: "INTEGER", nullable: true),
                    idMedia = table.Column<int>(type: "INTEGER", nullable: true),
                    media_type = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "episode",
                columns: table => new
                {
                    idEpisode = table.Column<int>(type: "INTEGER", nullable: false),
                    idShow = table.Column<int>(type: "INTEGER", nullable: false),
                    idFile = table.Column<int>(type: "INTEGER", nullable: false),
                    idSource = table.Column<int>(type: "INTEGER", nullable: false),
                    episode = table.Column<int>(type: "INTEGER", nullable: true),
                    title = table.Column<string>(type: "TEXT", nullable: true),
                    originalTitle = table.Column<string>(type: "TEXT", nullable: true),
                    @new = table.Column<bool>(name: "new", type: "bool", nullable: true, defaultValue: false),
                    marked = table.Column<bool>(type: "bool", nullable: false),
                    locked = table.Column<bool>(type: "bool", nullable: false),
                    season = table.Column<int>(type: "INTEGER", nullable: true),
                    plot = table.Column<string>(type: "TEXT", nullable: true),
                    aired = table.Column<string>(type: "TEXT", nullable: true),
                    nfoPath = table.Column<string>(type: "TEXT", nullable: true),
                    playcount = table.Column<int>(type: "INTEGER", nullable: true),
                    displaySeason = table.Column<int>(type: "INTEGER", nullable: true),
                    displayEpisode = table.Column<int>(type: "INTEGER", nullable: true),
                    dateAdded = table.Column<int>(type: "INTEGER", nullable: true),
                    runtime = table.Column<string>(type: "TEXT", nullable: true),
                    videoSource = table.Column<string>(type: "TEXT", nullable: true),
                    hasSub = table.Column<bool>(type: "bool", nullable: false),
                    subEpisode = table.Column<int>(type: "INTEGER", nullable: true),
                    lastPlayed = table.Column<int>(type: "INTEGER", nullable: true),
                    userRating = table.Column<int>(type: "INTEGER", nullable: false),
                    dateModified = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_episode", x => x.idEpisode);
                    table.ForeignKey(
                        name: "FK_episode_file_idFile",
                        column: x => x.idFile,
                        principalTable: "file",
                        principalColumn: "idFile",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "movie",
                columns: table => new
                {
                    idMovie = table.Column<int>(type: "INTEGER", nullable: false),
                    idSource = table.Column<int>(type: "INTEGER", nullable: false),
                    idFile = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: -1),
                    isSingle = table.Column<bool>(type: "bool", nullable: true),
                    hasSub = table.Column<bool>(type: "bool", nullable: false),
                    @new = table.Column<bool>(name: "new", type: "bool", nullable: false),
                    marked = table.Column<bool>(type: "bool", nullable: false),
                    locked = table.Column<bool>(type: "bool", nullable: true),
                    title = table.Column<string>(type: "TEXT", nullable: true),
                    originalTitle = table.Column<string>(type: "TEXT", nullable: true),
                    mpaa = table.Column<string>(type: "TEXT", nullable: true),
                    top250 = table.Column<int>(type: "INTEGER", nullable: true),
                    outline = table.Column<string>(type: "TEXT", nullable: true),
                    plot = table.Column<string>(type: "TEXT", nullable: true),
                    tagline = table.Column<string>(type: "TEXT", nullable: true),
                    runtime = table.Column<string>(type: "TEXT", nullable: true),
                    premiered = table.Column<string>(type: "TEXT", nullable: true),
                    playcount = table.Column<int>(type: "INTEGER", nullable: true),
                    trailer = table.Column<string>(type: "TEXT", nullable: true),
                    ethumbsPath = table.Column<string>(type: "TEXT", nullable: true),
                    nfoPath = table.Column<string>(type: "TEXT", nullable: true),
                    trailerPath = table.Column<string>(type: "TEXT", nullable: true),
                    subPath = table.Column<string>(type: "TEXT", nullable: true),
                    outOfTolerance = table.Column<bool>(type: "bool", nullable: true),
                    videoSource = table.Column<string>(type: "TEXT", nullable: true),
                    sortTitle = table.Column<string>(type: "TEXT", nullable: true),
                    dateAdded = table.Column<int>(type: "INTEGER", nullable: true),
                    efanartsPath = table.Column<string>(type: "TEXT", nullable: true),
                    themePath = table.Column<string>(type: "TEXT", nullable: true),
                    dateModified = table.Column<int>(type: "INTEGER", nullable: true),
                    markCustom1 = table.Column<bool>(type: "bool", nullable: false),
                    markCustom2 = table.Column<bool>(type: "bool", nullable: false),
                    markCustom3 = table.Column<bool>(type: "bool", nullable: false),
                    markCustom4 = table.Column<bool>(type: "bool", nullable: false),
                    hasSet = table.Column<bool>(type: "bool", nullable: false),
                    lastPlayed = table.Column<int>(type: "INTEGER", nullable: true),
                    language = table.Column<string>(type: "TEXT", nullable: true),
                    userRating = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_movie", x => x.idMovie);
                    table.ForeignKey(
                        name: "FK_movie_file_idFile",
                        column: x => x.idFile,
                        principalTable: "file",
                        principalColumn: "idFile",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_movie_moviesource_idSource",
                        column: x => x.idSource,
                        principalTable: "moviesource",
                        principalColumn: "idSource",
                        onDelete: ReferentialAction.Cascade);
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
                name: "ix_art",
                table: "art",
                columns: new[] { "idMedia", "media_type", "type" });

            migrationBuilder.CreateIndex(
                name: "ix_certification",
                table: "certification",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "certification_link_1",
                table: "certification_link",
                columns: new[] { "idCertification", "media_type", "idMedia" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "certification_link_2",
                table: "certification_link",
                columns: new[] { "idMedia", "media_type", "idCertification" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "certification_link_3",
                table: "certification_link",
                column: "media_type");

            migrationBuilder.CreateIndex(
                name: "country_link_1",
                table: "country_link",
                columns: new[] { "idCountry", "media_type", "idMedia" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "country_link_2",
                table: "country_link",
                columns: new[] { "idMedia", "media_type", "idCountry" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "country_link_3",
                table: "country_link",
                column: "media_type");

            migrationBuilder.CreateIndex(
                name: "ix_creator_link_1",
                table: "creator_link",
                columns: new[] { "idPerson", "media_type", "idMedia" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_creator_link_2",
                table: "creator_link",
                columns: new[] { "idMedia", "media_type", "idPerson" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_creator_link_3",
                table: "creator_link",
                column: "media_type");

            migrationBuilder.CreateIndex(
                name: "ix_director_link_1",
                table: "director_link",
                columns: new[] { "idPerson", "media_type", "idMedia" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_director_link_2",
                table: "director_link",
                columns: new[] { "idMedia", "media_type", "idPerson" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_director_link_3",
                table: "director_link",
                column: "media_type");

            migrationBuilder.CreateIndex(
                name: "IX_episode_idFile",
                table: "episode",
                column: "idFile");

            migrationBuilder.CreateIndex(
                name: "UniqueFilename",
                table: "file",
                column: "path",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "genre_link_1",
                table: "genre_link",
                columns: new[] { "idGenre", "media_type", "idMedia" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "genre_link_2",
                table: "genre_link",
                columns: new[] { "idMedia", "media_type", "idGenre" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "genre_link_3",
                table: "genre_link",
                column: "media_type");

            migrationBuilder.CreateIndex(
                name: "ix_gueststar_link_1",
                table: "gueststar_link",
                columns: new[] { "idPerson", "media_type", "idMedia" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_gueststar_link_2",
                table: "gueststar_link",
                columns: new[] { "idMedia", "media_type", "idPerson" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_gueststar_link_3",
                table: "gueststar_link",
                column: "media_type");

            migrationBuilder.CreateIndex(
                name: "ix_movie_file_1",
                table: "movie",
                columns: new[] { "idFile", "idMovie" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_movie_file_2",
                table: "movie",
                columns: new[] { "idMovie", "idFile" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_movie_idSource",
                table: "movie",
                column: "idSource");

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

            migrationBuilder.CreateIndex(
                name: "UniqueMovieSourcePath",
                table: "moviesource",
                column: "path",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_rating",
                table: "rating",
                columns: new[] { "idMedia", "media_type" });

            migrationBuilder.CreateIndex(
                name: "ix_season",
                table: "season",
                columns: new[] { "idShow", "season" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_streamdetail",
                table: "streamdetail",
                column: "idFile");

            migrationBuilder.CreateIndex(
                name: "ix_studio_link_1",
                table: "studio_link",
                columns: new[] { "idStudio", "media_type", "idMedia" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_studio_link_2",
                table: "studio_link",
                columns: new[] { "idMedia", "media_type", "idStudio" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_studio_link_3",
                table: "studio_link",
                column: "media_type");

            migrationBuilder.CreateIndex(
                name: "ix_tag_1",
                table: "tag",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_taglinks_1",
                table: "tag_link",
                columns: new[] { "idTag", "media_type", "idMedia" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_taglinks_2",
                table: "tag_link",
                columns: new[] { "idMedia", "media_type", "idTag" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_taglinks_3",
                table: "tag_link",
                column: "media_type");

            migrationBuilder.CreateIndex(
                name: "UniquePath",
                table: "tvshow",
                column: "path",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_tvshow_link_1",
                table: "tvshow_link",
                columns: new[] { "idShow", "idMovie" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_tvshow_link_2",
                table: "tvshow_link",
                columns: new[] { "idMovie", "idShow" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UniqueTVShowSourcePath",
                table: "tvshowsource",
                column: "path",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_uniqueid1",
                table: "uniqueid",
                columns: new[] { "idMedia", "media_type", "type" });

            migrationBuilder.CreateIndex(
                name: "ix_uniqueid2",
                table: "uniqueid",
                columns: new[] { "media_type", "value" });

            migrationBuilder.CreateIndex(
                name: "ix_writer_link_1",
                table: "writer_link",
                columns: new[] { "idPerson", "media_type", "idMedia" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_writer_link_2",
                table: "writer_link",
                columns: new[] { "idMedia", "media_type", "idPerson" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_writer_link_3",
                table: "writer_link",
                column: "media_type");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "actor_link");

            migrationBuilder.DropTable(
                name: "art");

            migrationBuilder.DropTable(
                name: "certification");

            migrationBuilder.DropTable(
                name: "certification_link");

            migrationBuilder.DropTable(
                name: "certification_temp");

            migrationBuilder.DropTable(
                name: "country");

            migrationBuilder.DropTable(
                name: "country_link");

            migrationBuilder.DropTable(
                name: "creator_link");

            migrationBuilder.DropTable(
                name: "director_link");

            migrationBuilder.DropTable(
                name: "episode");

            migrationBuilder.DropTable(
                name: "excludedpath");

            migrationBuilder.DropTable(
                name: "file_temp");

            migrationBuilder.DropTable(
                name: "genre");

            migrationBuilder.DropTable(
                name: "genre_link");

            migrationBuilder.DropTable(
                name: "gueststar_link");

            migrationBuilder.DropTable(
                name: "movie");

            migrationBuilder.DropTable(
                name: "MoviesAStreams");

            migrationBuilder.DropTable(
                name: "movieset");

            migrationBuilder.DropTable(
                name: "movieset_link");

            migrationBuilder.DropTable(
                name: "MoviesSubs");

            migrationBuilder.DropTable(
                name: "MoviesVStreams");

            migrationBuilder.DropTable(
                name: "person");

            migrationBuilder.DropTable(
                name: "rating");

            migrationBuilder.DropTable(
                name: "season");

            migrationBuilder.DropTable(
                name: "streamdetail");

            migrationBuilder.DropTable(
                name: "studio");

            migrationBuilder.DropTable(
                name: "studio_link");

            migrationBuilder.DropTable(
                name: "tag");

            migrationBuilder.DropTable(
                name: "tag_link");

            migrationBuilder.DropTable(
                name: "TVAStreams");

            migrationBuilder.DropTable(
                name: "tvshow");

            migrationBuilder.DropTable(
                name: "tvshow_link");

            migrationBuilder.DropTable(
                name: "tvshowsource");

            migrationBuilder.DropTable(
                name: "TVSubs");

            migrationBuilder.DropTable(
                name: "TVVStreams");

            migrationBuilder.DropTable(
                name: "uniqueid");

            migrationBuilder.DropTable(
                name: "writer_link");

            migrationBuilder.DropTable(
                name: "file");

            migrationBuilder.DropTable(
                name: "moviesource");
        }
    }
}
