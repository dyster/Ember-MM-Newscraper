using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmberAPICSharp.Migrations
{
    /// <inheritdoc />
    public partial class createviews : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            /*migrationBuilder.Sql(@"CREATE VIEW episodelist AS
    SELECT episode.*,
           file.path AS path,
           source.name AS source,
           fanart.url AS fanartPath,
           poster.url AS posterPath,
           GROUP_CONCAT(DISTINCT writers.name) AS credits,
           GROUP_CONCAT(DISTINCT directors.name) AS director,
           CASE WHEN episode.idFile IS -1 THEN 1 ELSE 0 END AS missing,
           GROUP_CONCAT(DISTINCT uniqueid.type || ':' || uniqueid.value) AS uniqueid
      FROM episode
           LEFT OUTER JOIN
           file ON (file.idFile = episode.idFile)
           LEFT OUTER JOIN
           tvshowsource AS source ON (source.idSource = episode.idSource)
           LEFT OUTER JOIN
           art AS fanart ON (fanart.idMedia = episode.idEpisode) AND
                            (fanart.media_type = 'episode') AND
                            (fanart.type = 'fanart')
           LEFT OUTER JOIN
           art AS poster ON (poster.idMedia = episode.idEpisode) AND
                            (poster.media_type = 'episode') AND
                            (poster.type = 'thumb')
           LEFT OUTER JOIN
           writer_link AS writerlink ON (writerlink.idMedia = episode.idEpisode) AND
                                        (writerlink.media_type = 'episode')
           LEFT OUTER JOIN
           person AS writers ON (writers.idPerson = writerlink.idPerson)
           LEFT OUTER JOIN
           director_link AS directorlink ON (directorlink.idMedia = episode.idEpisode) AND
                                            (writerlink.media_type = 'episode')
           LEFT OUTER JOIN
           person AS directors ON (directors.idPerson = directorlink.idPerson)
           LEFT OUTER JOIN
           uniqueid AS uniqueids ON (uniqueid.idMedia = episode.idEpisode) AND
                                    (uniqueid.media_type = 'episode')
     GROUP BY episode.idEpisode;");

            migrationBuilder.Sql(@"CREATE VIEW movielist AS
    SELECT movie.*,
           CASE WHEN movie.sortTitle IS NOT '' THEN movie.sortTitle ELSE movie.title END AS sortedTitle,
           source.name AS source,
           file.path AS path,
           banner.url AS bannerPath,
           clearart.url AS clearartPath,
           clearlogo.url AS clearlogoPath,
           discart.url AS discartPath,
           fanart.url AS fanartPath,
           keyart.url AS keyartPath,
           landscape.url AS landscapePath,
           poster.url AS posterPath,
           GROUP_CONCAT(DISTINCT certifications.name) AS certification,
           GROUP_CONCAT(DISTINCT countries.name) AS country,
           GROUP_CONCAT(DISTINCT credits.name) AS credits,
           GROUP_CONCAT(DISTINCT directors.name) AS director,
           GROUP_CONCAT(DISTINCT genres.name) AS genre,
           GROUP_CONCAT(DISTINCT studios.name) AS studio,
           GROUP_CONCAT(DISTINCT tags.name) AS tag,
           GROUP_CONCAT(DISTINCT uniqueids.type || ':' || uniqueids.value) AS uniqueid
      FROM movie
           LEFT OUTER JOIN
           moviesource AS source ON (source.idSource = movie.idSource)
           LEFT OUTER JOIN
           art AS banner ON (banner.idMedia = movie.idMovie) AND
                            (banner.media_type = 'movie') AND
                            (banner.type = 'banner')
           LEFT OUTER JOIN
           art AS clearart ON (clearart.idMedia = movie.idMovie) AND
                              (clearart.media_type = 'movie') AND
                              (clearart.type = 'clearart')
           LEFT OUTER JOIN
           art AS clearlogo ON (clearlogo.idMedia = movie.idMovie) AND
                               (clearlogo.media_type = 'movie') AND
                               (clearlogo.type = 'clearlogo')
           LEFT OUTER JOIN
           art AS discart ON (discart.idMedia = movie.idMovie) AND
                             (discart.media_type = 'movie') AND
                             (discart.type = 'discart')
           LEFT OUTER JOIN
           art AS fanart ON (fanart.idMedia = movie.idMovie) AND
                            (fanart.media_type = 'movie') AND
                            (fanart.type = 'fanart')
           LEFT OUTER JOIN
           art AS keyart ON (keyart.idMedia = movie.idMovie) AND
                            (keyart.media_type = 'movie') AND
                            (keyart.type = 'keyart')
           LEFT OUTER JOIN
           art AS landscape ON (landscape.idMedia = movie.idMovie) AND
                               (landscape.media_type = 'movie') AND
                               (landscape.type = 'landscape')
           LEFT OUTER JOIN
           art AS poster ON (poster.idMedia = movie.idMovie) AND
                            (poster.media_type = 'movie') AND
                            (poster.type = 'poster')
           LEFT OUTER JOIN
           certification_link AS certificationlink ON (certificationlink.idMedia = movie.idMovie) AND
                                                      (certificationlink.media_type = 'movie')
           LEFT OUTER JOIN
           certification AS certifications ON (certifications.idCertification = certificationlink.idCertification)
           LEFT OUTER JOIN
           country_link AS countrylink ON (countrylink.idMedia = movie.idMovie) AND
                                          (countrylink.media_type = 'movie')
           LEFT OUTER JOIN
           country AS countries ON (countries.idCountry = countrylink.idCountry)
           LEFT OUTER JOIN
           writer_link AS writerlink ON (writerlink.idMedia = movie.idMovie) AND
                                        (writerlink.media_type = 'movie')
           LEFT OUTER JOIN
           person AS credits ON (credits.idPerson = writerlink.idPerson)
           LEFT OUTER JOIN
           director_link AS directorlink ON (directorlink.idMedia = movie.idMovie) AND
                                            (directorlink.media_type = 'movie')
           LEFT OUTER JOIN
           person AS directors ON (directors.idPerson = directorlink.idPerson)
           LEFT OUTER JOIN
           file ON (file.idFile = movie.idFile)
           LEFT OUTER JOIN
           genre_link AS genrelink ON (genrelink.idMedia = movie.idMovie) AND
                                      (genrelink.media_type = 'movie')
           LEFT OUTER JOIN
           genre AS genres ON (genres.idGenre = genrelink.idGenre)
           LEFT OUTER JOIN
           studio_link AS studiolink ON (studiolink.idMedia = movie.idMovie) AND
                                        (studiolink.media_type = 'movie')
           LEFT OUTER JOIN
           studio AS studios ON (studios.idStudio = studiolink.idStudio)
           LEFT OUTER JOIN
           tag_link AS taglink ON (taglink.idMedia = movie.idMovie) AND
                                  (taglink.media_type = 'movie')
           LEFT OUTER JOIN
           tag AS tags ON (tags.idTag = taglink.idTag)
           LEFT OUTER JOIN
           uniqueid AS uniqueids ON (uniqueids.idMedia = movie.idMovie) AND
                                    (uniqueids.media_type = 'movie')
     GROUP BY movie.idMovie;");
            
            migrationBuilder.Sql(@"CREATE VIEW moviesetlist AS
    SELECT movieset.*,
           banner.url AS bannerPath,
           clearart.url AS clearartPath,
           clearlogo.url AS clearlogoPath,
           discart.url AS discartPath,
           fanart.url AS fanartPath,
           keyart.url AS keyartPath,
           landscape.url AS landscapePath,
           poster.url AS posterPath,
           COUNT(MovieMovieset.MoviesId) AS movieCount,
           GROUP_CONCAT(DISTINCT movies.title) AS movieTitles,
           GROUP_CONCAT(DISTINCT uniqueids.type || ':' || uniqueids.value) AS uniqueid
      FROM movieset
           LEFT OUTER JOIN
           art AS banner ON (banner.idMedia = movieset.idSet) AND
                            (banner.media_type = 'movieset') AND
                            (banner.type = 'banner')
           LEFT OUTER JOIN
           art AS clearart ON (clearart.idMedia = movieset.idSet) AND
                              (clearart.media_type = 'movieset') AND
                              (clearart.type = 'clearart')
           LEFT OUTER JOIN
           art AS clearlogo ON (clearlogo.idMedia = movieset.idSet) AND
                               (clearlogo.media_type = 'movieset') AND
                               (clearlogo.type = 'clearlogo')
           LEFT OUTER JOIN
           art AS discart ON (discart.idMedia = movieset.idSet) AND
                             (discart.media_type = 'movieset') AND
                             (discart.type = 'discart')
           LEFT OUTER JOIN
           art AS fanart ON (fanart.idMedia = movieset.idSet) AND
                            (fanart.media_type = 'movieset') AND
                            (fanart.type = 'fanart')
           LEFT OUTER JOIN
           art AS keyart ON (keyart.idMedia = movieset.idSet) AND
                            (keyart.media_type = 'movieset') AND
                            (keyart.type = 'keyart')
           LEFT OUTER JOIN
           art AS landscape ON (landscape.idMedia = movieset.idSet) AND
                               (landscape.media_type = 'movieset') AND
                               (landscape.type = 'landscape')
           LEFT OUTER JOIN
           art AS poster ON (poster.idMedia = movieset.idSet) AND
                            (poster.media_type = 'movieset') AND
                            (poster.type = 'poster')
           LEFT OUTER JOIN
           MovieMovieset AS MovieMovieset ON (MovieMovieset.MoviesetsId = movieset.idSet)
           LEFT OUTER JOIN
           movie AS movies ON (movies.idMovie = MovieMovieset.MoviesId)
           LEFT OUTER JOIN
           uniqueid AS uniqueids ON (uniqueids.idMedia = movieset.idSet) AND
                                    (uniqueids.media_type = 'movieset')
     GROUP BY movieset.idSet;");
            
            migrationBuilder.Sql(@"CREATE VIEW seasonlist AS
    SELECT season.*,
           banner.url AS bannerPath,
           fanart.url AS fanartPath,
           landscape.url AS landscapePath,
           poster.url AS posterPath,
           CASE WHEN season.season IS NOT -1 THEN COUNT(DISTINCT episodelist.idEpisode) ELSE NULL END AS episodes,
           COUNT(DISTINCT CASE WHEN episodelist.lastPlayed IS NOT NULL THEN episodelist.idEpisode ELSE NULL END) AS playcount,
           CASE WHEN season.season IS NOT -1 THEN CASE WHEN COUNT(DISTINCT episodelist.idEpisode) IS NOT 0 AND
                                                            COUNT(DISTINCT episodelist.idEpisode) = COUNT(DISTINCT CASE WHEN episodelist.lastPlayed IS NOT NULL THEN episodelist.idEpisode ELSE NULL END) THEN 1 ELSE 0 END ELSE 0 END AS hasWatched,
           COUNT(DISTINCT CASE WHEN episodelist.new IS 1 THEN episodelist.idEpisode ELSE NULL END) AS newEpisodes,
           CASE WHEN season.season IS -1 OR
                     COUNT(DISTINCT episodelist.idEpisode) IS NOT 0 THEN 0 ELSE 1 END AS missing,
           GROUP_CONCAT(DISTINCT uniqueids.type || ':' || uniqueids.value) AS uniqueid
      FROM season
           LEFT OUTER JOIN
           art AS banner ON (banner.idMedia = season.idSeason) AND
                            (banner.media_type = 'season') AND
                            (banner.type = 'banner')
           LEFT OUTER JOIN
           art AS fanart ON (fanart.idMedia = season.idSeason) AND
                            (fanart.media_type = 'season') AND
                            (fanart.type = 'fanart')
           LEFT OUTER JOIN
           art AS landscape ON (landscape.idMedia = season.idSeason) AND
                               (landscape.media_type = 'season') AND
                               (landscape.type = 'landscape')
           LEFT OUTER JOIN
           art AS poster ON (poster.idMedia = season.idSeason) AND
                            (poster.media_type = 'season') AND
                            (poster.type = 'poster')
           LEFT OUTER JOIN
           episodelist ON (season.season = episodelist.season) AND
                          (season.idShow = episodelist.idShow) AND
                          (episodelist.missing = 0)
           LEFT OUTER JOIN
           uniqueid AS uniqueids ON (uniqueids.idMedia = season.idSeason) AND
                                    (uniqueids.media_type = 'season')
     GROUP BY season.idSeason;");

            migrationBuilder.Sql(@"CREATE VIEW tvshowlist AS
    SELECT tvshow.*,
           CASE WHEN tvshow.sortTitle IS NOT '' THEN tvshow.sortTitle ELSE tvshow.title END AS sortedTitle,
           source.name AS source,
           banner.url AS bannerPath,
           characterart.url AS characterartPath,
           clearart.url AS clearartPath,
           clearlogo.url AS clearlogoPath,
           fanart.url AS fanartPath,
           keyart.url AS keyartPath,
           landscape.url AS landscapePath,
           poster.url AS posterPath,
           GROUP_CONCAT(DISTINCT countries.name) AS country,
           GROUP_CONCAT(DISTINCT creators.name) AS creator,
           GROUP_CONCAT(DISTINCT genres.name) AS genre,
           GROUP_CONCAT(DISTINCT studios.name) AS studio,
           GROUP_CONCAT(DISTINCT tags.name) AS tag,
           COUNT(DISTINCT episodelist.idEpisode) AS episodes,
           COUNT(DISTINCT CASE WHEN episodelist.lastPlayed IS NOT NULL THEN episodelist.idEpisode ELSE NULL END) AS playcount,
           CASE WHEN COUNT(DISTINCT episodelist.idEpisode) IS NOT 0 AND
                     COUNT(DISTINCT episodelist.idEpisode) = COUNT(DISTINCT CASE WHEN episodelist.lastPlayed IS NOT NULL THEN episodelist.idEpisode ELSE NULL END) THEN 1 ELSE 0 END AS hasWatched,
           COUNT(DISTINCT CASE WHEN episodelist.new IS 1 THEN episodelist.idEpisode ELSE NULL END) AS newEpisodes,
           COUNT(DISTINCT CASE WHEN episodelist.marked IS 1 THEN episodelist.idEpisode ELSE NULL END) AS markedEpisodes,
           COUNT(DISTINCT CASE WHEN episodelist.locked IS 1 THEN episodelist.idEpisode ELSE NULL END) AS lockedEpisodes,
           GROUP_CONCAT(DISTINCT uniqueids.type || ':' || uniqueids.value) AS uniqueid
      FROM tvshow
           LEFT OUTER JOIN
           tvshowsource AS source ON (source.idSource = tvshow.idSource)
           LEFT OUTER JOIN
           art AS banner ON (banner.idMedia = tvshow.idShow) AND
                            (banner.media_type = 'tvshow') AND
                            (banner.type = 'banner')
           LEFT OUTER JOIN
           art AS characterart ON (characterart.idMedia = tvshow.idShow) AND
                                  (characterart.media_type = 'tvshow') AND
                                  (characterart.type = 'characterart')
           LEFT OUTER JOIN
           art AS clearart ON (clearart.idMedia = tvshow.idShow) AND
                              (clearart.media_type = 'tvshow') AND
                              (clearart.type = 'clearart')
           LEFT OUTER JOIN
           art AS clearlogo ON (clearlogo.idMedia = tvshow.idShow) AND
                               (clearlogo.media_type = 'tvshow') AND
                               (clearlogo.type = 'clearlogo')
           LEFT OUTER JOIN
           art AS fanart ON (fanart.idMedia = tvshow.idShow) AND
                            (fanart.media_type = 'tvshow') AND
                            (fanart.type = 'fanart')
           LEFT OUTER JOIN
           art AS keyart ON (keyart.idMedia = tvshow.idShow) AND
                            (keyart.media_type = 'tvshow') AND
                            (keyart.type = 'keyart')
           LEFT OUTER JOIN
           art AS landscape ON (landscape.idMedia = tvshow.idShow) AND
                               (landscape.media_type = 'tvshow') AND
                               (landscape.type = 'landscape')
           LEFT OUTER JOIN
           art AS poster ON (poster.idMedia = tvshow.idShow) AND
                            (poster.media_type = 'tvshow') AND
                            (poster.type = 'poster')
           LEFT OUTER JOIN
           certification_link AS certificationlink ON (certificationlink.idMedia = tvshow.idShow) AND
                                                      (certificationlink.media_type = 'tvshow')
           LEFT OUTER JOIN
           country_link AS countrylink ON (countrylink.idMedia = tvshow.idShow) AND
                                          (countrylink.media_type = 'tvshow')
           LEFT OUTER JOIN
           country AS countries ON (countries.idCountry = countrylink.idCountry) AND
                                   (countrylink.media_type = 'tvshow')
           LEFT OUTER JOIN
           creator_link AS creatorlink ON (creatorlink.idMedia = tvshow.idShow) AND
                                          (creatorlink.media_type = 'tvshow')
           LEFT OUTER JOIN
           person AS creators ON (creators.idPerson = creatorlink.idPerson)
           LEFT OUTER JOIN
           genre_link AS genrelink ON (genrelink.idMedia = tvshow.idShow) AND
                                      (genrelink.media_type = 'tvshow')
           LEFT OUTER JOIN
           genre AS genres ON (genres.idGenre = genrelink.idGenre)
           LEFT OUTER JOIN
           studio_link AS studiolink ON (studiolink.idMedia = tvshow.idShow) AND
                                        (studiolink.media_type = 'tvshow')
           LEFT OUTER JOIN
           studio AS studios ON (studios.idStudio = studiolink.idStudio)
           LEFT OUTER JOIN
           tag_link AS taglink ON (taglink.idMedia = tvshow.idShow) AND
                                  (taglink.media_type = 'tvshow')
           LEFT OUTER JOIN
           tag AS tags ON (tags.idTag = taglink.idTag)
           LEFT OUTER JOIN
           episodelist ON (tvshow.idShow = episodelist.idShow) AND
                          (episodelist.Missing = 0)
           LEFT OUTER JOIN
           uniqueid AS uniqueids ON (uniqueids.idMedia = tvshow.idShow) AND
                                    (uniqueids.media_type = 'tvshow')
     GROUP BY tvshow.idShow;");*/
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}