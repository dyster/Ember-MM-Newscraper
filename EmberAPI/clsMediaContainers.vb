﻿' ################################################################################
' #                             EMBER MEDIA MANAGER                              #
' ################################################################################
' ################################################################################
' # This file is part of Ember Media Manager.                                    #
' #                                                                              #
' # Ember Media Manager is free software: you can redistribute it and/or modify  #
' # it under the terms of the GNU General Public License as published by         #
' # the Free Software Foundation, either version 3 of the License, or            #
' # (at your option) any later version.                                          #
' #                                                                              #
' # Ember Media Manager is distributed in the hope that it will be useful,       #
' # but WITHOUT ANY WARRANTY; without even the implied warranty of               #
' # MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the                #
' # GNU General Public License for more details.                                 #
' #                                                                              #
' # You should have received a copy of the GNU General Public License            #
' # along with Ember Media Manager.  If not, see <http://www.gnu.org/licenses/>. #
' ################################################################################

Imports NLog
Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Xml
Imports System.Xml.Serialization
Imports EmberAPI.EFModels
Imports System.Numerics

Namespace MediaContainers

    <Serializable()>
    Public Class Audio

#Region "Fields"

        Private _language As String = String.Empty
        Private _longLanguage As String = String.Empty

#End Region 'Fields

#Region "Properties"
        ''' <summary>
        ''' Additional audio features like (Dolby) "Atmos" or (DTS:)"X"
        ''' </summary>
        ''' <returns></returns>
        <XmlElement("additionalfeatures")>
        Public Property AdditionalFeatures As String = String.Empty

        <XmlIgnore>
        Public ReadOnly Property AdditionalFeaturesSpecified As Boolean
            Get
                Return Not String.IsNullOrEmpty(AdditionalFeatures)
            End Get
        End Property
        ''' <summary>
        ''' Resolution in bits (8, 16, 20, 24)
        ''' </summary>
        ''' <returns></returns>
        <XmlElement("bitdepth")>
        Public Property BitDepth As Integer = 0

        <XmlIgnore>
        Public ReadOnly Property BitDepthSpecified As Boolean
            Get
                Return Not BitDepth = 0
            End Get
        End Property
        ''' <summary>
        ''' Bitrate in kb/s (16, 24, 32)
        ''' </summary>
        ''' <returns></returns>
        <XmlElement("bitrate")>
        Public Property Bitrate As Integer = 0

        <XmlIgnore>
        Public ReadOnly Property BitrateSpecified As Boolean
            Get
                Return Not Bitrate = 0
            End Get
        End Property

        <XmlElement("channels")>
        Public Property Channels As Integer = 0

        <XmlIgnore>
        Public ReadOnly Property ChannelsSpecified As Boolean
            Get
                Return Not Channels = 0
            End Get
        End Property

        <XmlElement("codec")>
        Public Property Codec As String = String.Empty

        <XmlIgnore>
        Public ReadOnly Property CodecSpecified As Boolean
            Get
                Return Not String.IsNullOrEmpty(Codec)
            End Get
        End Property

        <XmlIgnore>
        Public Property HasPreferred As Boolean = False

        <XmlElement("language")>
        Public Property Language As String
            Get
                Return _language
            End Get
            Set(value As String)
                _language = Localization.Languages.Check_Alpha2_IsValid(value)
                _longLanguage = Localization.Languages.Get_Name_By_Alpha3(_language)
            End Set
        End Property

        <XmlIgnore>
        Public ReadOnly Property LanguageSpecified As Boolean
            Get
                Return Not String.IsNullOrEmpty(Language)
            End Get
        End Property

        <XmlElement("longlanguage")>
        Public Property LongLanguage As String
            Get
                Return _longLanguage
            End Get
            Set(value As String)
                _longLanguage = Localization.Languages.Check_Alpha2_IsValid(value)
                _language = Localization.Languages.Get_Alpha3_B_By_Name(value)
            End Set
        End Property

        <XmlIgnore>
        Public ReadOnly Property LongLanguageSpecified As Boolean
            Get
                Return Not String.IsNullOrEmpty(LongLanguage)
            End Get
        End Property

#End Region 'Properties

    End Class

    <Serializable>
    Public Class DefaultId

#Region "Properties"

        <XmlText>
        Public Property Value As String = String.Empty

        <XmlIgnore>
        Public ReadOnly Property ValueSpecified As Boolean
            Get
                Return Not String.IsNullOrEmpty(Value)
            End Get
        End Property

        <XmlAttribute("type")>
        Public Property Type As String = String.Empty

        <XmlIgnore>
        Public ReadOnly Property TypeSpecified As Boolean
            Get
                Return Not String.IsNullOrEmpty(Type)
            End Get
        End Property

#End Region 'Properties

    End Class


    <Serializable()>
    <XmlRoot("episodedetails")>
    Public Class EpisodeDetails_old
        Implements ICloneable

#Region "Fields"

        Private _contentType As Enums.ContentType = Enums.ContentType.TVEpisode
        Private _rating As Double = 0
        Private _votes As Integer = 0

#End Region 'Fields

#Region "Properties"

        <XmlElement("id")>
        Public Property DefaultId() As DefaultId
            Get
                Return UniqueIDs.GetDefaultId()
            End Get
            Set(value As DefaultId)
                UniqueIDs.Add(value)
            End Set
        End Property

        <XmlIgnore()>
        Public ReadOnly Property DefaultIdSpecified() As Boolean
            Get
                Return DefaultId.ValueSpecified AndAlso Master.eSettings.TVScraperIdWriteNodeDefaultId
            End Get
        End Property

        <XmlElement("imdb")>
        Public Property IMDbId() As String
            Get
                Return UniqueIDs.IMDbId.ToString
            End Get
            Set(value As String)
                UniqueIDs.Add("imdb", value)
            End Set
        End Property

        <Obsolete()>
        <XmlIgnore()>
        Public ReadOnly Property IMDbIdSpecified() As Boolean
            Get
                Return UniqueIDs.IMDbIdSpecified AndAlso Master.eSettings.TVScraperIdWriteNodeIMDbId
            End Get
        End Property

        <XmlElement("tmdb")>
        Public Property TMDbId() As String
            Get
                Return UniqueIDs.TMDbId.ToString
            End Get
            Set(value As String)
                UniqueIDs.Add("tmdb", value)
            End Set
        End Property

        <Obsolete()>
        <XmlIgnore()>
        Public ReadOnly Property TMDbIdSpecified() As Boolean
            Get
                Return UniqueIDs.TMDbIdSpecified AndAlso Master.eSettings.TVScraperIdWriteNodeTMDbId
            End Get
        End Property

        <XmlIgnore()>
        Public Property UniqueIDs() As UniqueidContainer = New UniqueidContainer(_contentType)

        <XmlIgnore()>
        Public ReadOnly Property UniqueIDsSpecified() As Boolean
            Get
                Return UniqueIDs.Items.Count > 0
            End Get
        End Property

        <XmlElement("uniqueid")>
        Public Property UniqueIDs_Kodi() As Uniqueid()
            Get
                Return UniqueIDs.Items.ToArray
            End Get
            Set(value As Uniqueid())
                If value IsNot Nothing Then UniqueIDs.AddRange(value.ToList)
            End Set
        End Property

        <XmlElement("title")>
        Public Property Title() As String = String.Empty

        <XmlIgnore()>
        Public ReadOnly Property TitleSpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(Title) AndAlso Not Regex.IsMatch(Title, "s\d{2}e\d{2}$", RegexOptions.IgnoreCase)
            End Get
        End Property

        <XmlElement("originaltitle")>
        Public Property OriginalTitle() As String = String.Empty

        <XmlIgnore()>
        Public ReadOnly Property OriginalTitleSpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(OriginalTitle)
            End Get
        End Property

        <XmlElement("runtime")>
        Public Property Runtime() As String = String.Empty

        <XmlIgnore()>
        Public ReadOnly Property RuntimeSpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(Runtime) AndAlso Not Runtime = "0"
            End Get
        End Property

        <XmlElement("aired")>
        Public Property Aired() As String = String.Empty

        <XmlIgnore()>
        Public ReadOnly Property AiredSpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(Aired)
            End Get
        End Property

        <XmlElement("rating")>
        Public Property Rating() As String
            Get
                Dim nRating = Ratings.GetDefaultRating()
                If nRating IsNot Nothing Then
                    Return nRating.ValueNormalized.ToString
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                Dim dblRatings As Double
                If Double.TryParse(value.Replace(",", "."),
                                   Globalization.NumberStyles.AllowDecimalPoint,
                                   Globalization.CultureInfo.InvariantCulture,
                                   dblRatings
                                   ) Then
                    _rating = dblRatings
                    If Not _votes = 0 Then Ratings.Add(New RatingDetails With {
                                                       .IsDefault = True,
                                                       .Max = If(_rating <= 10, 10, 100),
                                                       .Type = "default",
                                                       .Value = _rating,
                                                       .Votes = _votes
                                                       })
                Else
                    _rating = 0
                End If
            End Set
        End Property

        <Obsolete()>
        <XmlIgnore()>
        Public ReadOnly Property RatingSpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(Rating) AndAlso Not String.IsNullOrEmpty(Votes) AndAlso Master.eSettings.TVScraperRatingVotesWriteNode
            End Get
        End Property

        <XmlElement("votes")>
        Public Property Votes() As String
            Get
                Dim nRating = Ratings.GetDefaultRating()
                If nRating IsNot Nothing AndAlso nRating.VotesSpecified Then
                    Return nRating.Votes.ToString
                Else
                    Return String.Empty
                End If
            End Get
            Set(value As String)
                Dim iVotes As Integer
                If Integer.TryParse(Regex.Replace(value, "\D", String.Empty), iVotes) Then
                    _votes = iVotes
                    If Not _rating = 0 Then Ratings.Add(New RatingDetails With {
                                                        .IsDefault = True,
                                                        .Max = If(_rating <= 10, 10, 100),
                                                        .Type = "default",
                                                        .Value = _rating,
                                                        .Votes = _votes
                                                        })
                Else
                    _votes = 0
                End If
            End Set
        End Property

        <Obsolete()>
        <XmlIgnore()>
        Public ReadOnly Property VotesSpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(Votes) AndAlso Not String.IsNullOrEmpty(Rating) AndAlso Master.eSettings.TVScraperRatingVotesWriteNode
            End Get
        End Property

        <XmlIgnore()>
        Public Property Ratings() As New RatingContainer(_contentType)

        <XmlIgnore()>
        Public ReadOnly Property RatingsSpecified() As Boolean
            Get
                Return Ratings.AnyRatingSpecified
            End Get
        End Property

        <XmlArray("ratings")>
        <XmlArrayItem("rating")>
        Public Property Ratings_Kodi() As RatingDetails()
            Get
                Return Ratings.Items.ToArray
            End Get
            Set(value As RatingDetails())
                Ratings.AddRange(value.ToList)
            End Set
        End Property

        <XmlElement("userrating")>
        Public Property UserRating() As Integer = 0

        <XmlIgnore()>
        Public ReadOnly Property UserRatingSpecified() As Boolean
            Get
                Return Not UserRating = 0
            End Get
        End Property

        <XmlElement("videosource")>
        Public Property VideoSource() As String = String.Empty

        <XmlIgnore()>
        Public ReadOnly Property VideoSourceSpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(VideoSource)
            End Get
        End Property

        <XmlElement("season")>
        Public Property Season() As Integer = -1

        <XmlIgnore()>
        Public ReadOnly Property SeasonSpecified() As Boolean
            Get
                Return Not Season = -1
            End Get
        End Property

        <XmlElement("episode")>
        Public Property Episode() As Integer = -1

        <XmlIgnore()>
        Public ReadOnly Property EpisodeSpecified() As Boolean
            Get
                Return Not Episode = -1
            End Get
        End Property

        <XmlElement("subepisode")>
        Public Property SubEpisode() As Integer = -1

        <XmlIgnore()>
        Public ReadOnly Property SubEpisodeSpecified() As Boolean
            Get
                Return Not SubEpisode = -1
            End Get
        End Property

        <XmlElement("displayseason")>
        Public Property DisplaySeason() As Integer = -1

        <XmlIgnore()>
        Public ReadOnly Property DisplaySeasonSpecified() As Boolean
            Get
                Return Not DisplaySeason = -1
            End Get
        End Property

        <XmlElement("displayepisode")>
        Public Property DisplayEpisode() As Integer = -1

        <XmlIgnore()>
        Public ReadOnly Property DisplayEpisodeSpecified() As Boolean
            Get
                Return Not DisplayEpisode = -1
            End Get
        End Property

        <XmlElement("plot")>
        Public Property Plot() As String = String.Empty

        <XmlIgnore()>
        Public ReadOnly Property PlotSpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(Plot)
            End Get
        End Property

        <XmlElement("credits")>
        Public Property Credits() As New List(Of String)

        <XmlIgnore()>
        Public ReadOnly Property CreditsSpecified() As Boolean
            Get
                Return Credits.Count > 0
            End Get
        End Property

        <XmlElement("playcount")>
        Public Property Playcount() As Integer = 0

        <XmlIgnore()>
        Public ReadOnly Property PlaycountSpecified() As Boolean
            Get
                Return Not Playcount = 0
            End Get
        End Property

        <XmlElement("lastplayed")>
        Public Property LastPlayed() As String = String.Empty

        <XmlIgnore()>
        Public ReadOnly Property LastPlayedSpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(LastPlayed)
            End Get
        End Property

        <XmlElement("director")>
        Public Property Directors() As New List(Of String)

        <XmlIgnore()>
        Public ReadOnly Property DirectorsSpecified() As Boolean
            Get
                Return Directors.Count > 0
            End Get
        End Property

        <XmlElement("actor")>
        Public Property Actors() As New List(Of RoleLink)

        <XmlIgnore()>
        Public ReadOnly Property ActorsSpecified() As Boolean
            Get
                Return Actors.Count > 0
            End Get
        End Property

        <XmlElement("gueststar")>
        Public Property GuestStars() As New List(Of RoleLink)

        <XmlIgnore()>
        Public ReadOnly Property GuestStarsSpecified() As Boolean
            Get
                Return GuestStars.Count > 0
            End Get
        End Property

        <XmlElement("fileinfo")>
        Public Property FileInfo() As New Fileinfo

        <XmlIgnore()>
        Public ReadOnly Property FileInfoSpecified() As Boolean
            Get
                Return FileInfo.StreamDetails.Video.Count > 0 OrElse
                    FileInfo.StreamDetails.Audio.Count > 0 OrElse
                    FileInfo.StreamDetails.Subtitle.Count > 0
            End Get
        End Property
        ''' <summary>
        ''' Poster Thumb for preview in search results
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <XmlIgnore()>
        Public Property ThumbPoster() As New Image

        <XmlElement("dateadded")>
        Public Property DateAdded() As String = String.Empty

        <XmlIgnore()>
        Public ReadOnly Property DateAddedSpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(DateAdded)
            End Get
        End Property

        <XmlElement("locked")>
        Public Property Locked() As Boolean

        <XmlElement("user_note")>
        Public Property UserNote() As String = String.Empty

        <XmlIgnore()>
        Public Property Scrapersource() As String = String.Empty

        <XmlIgnore()>
        Public Property EpisodeAbsolute() As Integer = -1

        <XmlIgnore()>
        Public Property EpisodeCombined() As Double = -1

        <XmlIgnore()>
        Public Property EpisodeDVD() As Double = -1

        <XmlIgnore()>
        Public Property SeasonCombined() As Integer = -1

        <XmlIgnore()>
        Public Property SeasonDVD() As Integer = -1

#End Region 'Properties

#Region "Methods"

        Public Function CloneDeep() As Object Implements ICloneable.Clone
            Throw New NotImplementedException("Binaryformatter is obsolete, the Deepclone method needs fixing")
            Dim Stream As New MemoryStream(50000)
            'Dim Formatter As New Runtime.Serialization.Formatters.Binary.BinaryFormatter()
            ' Serialisierung über alle Objekte hinweg in einen Stream 
            'Formatter.Serialize(Stream, Me)
            ' Zurück zum Anfang des Streams und... 
            'Stream.Seek(0, SeekOrigin.Begin)
            ' ...aus dem Stream in ein Objekt deserialisieren 
            'CloneDeep = Formatter.Deserialize(Stream)
            Stream.Close()
        End Function

        Public Sub CreateCachePaths_ActorsThumbs()
            Dim sPath As String = Path.Combine(Master.TempPath, "Global")

            For Each tActor As RoleLink In Actors
                Master.Thumbs(tActor.PersonId).CacheOriginalPath = Path.Combine(sPath, String.Concat("actorthumbs", Path.DirectorySeparatorChar, Path.GetFileName(tActor.Person.URLOriginal)))
                If Not String.IsNullOrEmpty(Master.Thumbs(tActor.PersonId).URLThumb) Then
                    Master.Thumbs(tActor.PersonId).CacheThumbPath = Path.Combine(sPath, String.Concat("actorthumbs\_thumbs", Path.DirectorySeparatorChar, Path.GetFileName(tActor.Person.URLOriginal)))
                End If
            Next
        End Sub

        Public Sub SaveAllActorThumbs(ByRef DBElement As Database.DBElement)
            If Not DBElement.FilenameSpecified Then Return

            If ActorsSpecified AndAlso Master.eSettings.TVEpisodeActorThumbsAnyEnabled Then
                Images.Save_Actorthumbs(DBElement)
            Else
                'Images.DeleteTVEpisodeActorThumbs(DBElement) 'TODO: find a way to only remove actor thumbs that not needed in other episodes with same actor thumbs path
                DBElement.ActorThumbs.Clear()
            End If
        End Sub

#End Region 'Methods

    End Class

    <Serializable()>
    Public Class EpisodeGuide

#Region "Properties"

        <XmlElement("url")>
        Public Property URL() As String = String.Empty

#End Region 'Properties 

    End Class

    <Serializable()>
    Public Class EpisodeOrSeasonImagesContainer

#Region "Properties"

        Public Property AlreadySaved() As Boolean

        Public Property Banner() As New Image

        Public Property Episode() As Integer = -1

        Public Property Fanart() As New Image

        Public Property Landscape() As New Image

        Public Property Poster() As New Image

        Public Property Season() As Integer = -1

#End Region 'Properties

    End Class

    <Serializable()>
    Public Class Fanart

#Region "Properties"

        <XmlElement("thumb")>
        Public Property Thumb() As New List(Of Thumb)

        <XmlAttribute("url")>
        Public Property URL() As String = String.Empty

#End Region 'Properties 

    End Class

    <Serializable()>
    <XmlRoot("fileinfo")>
    Public Class Fileinfo
        Implements ICloneable

#Region "Properties"

        <XmlElement("streamdetails")>
        Property StreamDetails() As New StreamData

        <XmlIgnore>
        Public ReadOnly Property StreamDetailsSpecified() As Boolean
            Get
                Return (StreamDetails.Video IsNot Nothing AndAlso StreamDetails.Video.Count > 0) OrElse
                (StreamDetails.Audio IsNot Nothing AndAlso StreamDetails.Audio.Count > 0) OrElse
                (StreamDetails.Subtitle IsNot Nothing AndAlso StreamDetails.Subtitle.Count > 0)
            End Get
        End Property

#End Region 'Properties

#Region "Methods"

        Public Function CloneDeep() As Object Implements ICloneable.Clone
            Throw New NotImplementedException("Binaryformatter is obsolete, the Deepclone method needs fixing")
            Dim Stream As New MemoryStream(50000)
            'Dim Formatter As New Runtime.Serialization.Formatters.Binary.BinaryFormatter()
            ' Serialisierung über alle Objekte hinweg in einen Stream 
            'Formatter.Serialize(Stream, Me)
            ' Zurück zum Anfang des Streams und... 
            'Stream.Seek(0, SeekOrigin.Begin)
            ' ...aus dem Stream in ein Objekt deserialisieren 
            'CloneDeep = Formatter.Deserialize(Stream)
            Stream.Close()
        End Function

#End Region 'Methods

    End Class

    <Serializable()>
    Public Class [Image]
        Implements IComparable(Of [Image])

#Region "Fields"

        Private _Language As String = String.Empty
        Private _LongLanguage As String = String.Empty

        Private _DiscType As String = String.Empty
        Private _Height As Integer = 0
        Private _ImageSize As Enums.ImageSize = Enums.ImageSize.Any
        Private _TVBannerType As Enums.TVBannerType = Enums.TVBannerType.Any

#End Region 'Fields 

#Region "Properties"

        Public Property CacheOriginalPath As String = String.Empty

        Public Property CacheThumbPath As String = String.Empty

        Public Property Disc As Integer = 0

        Public Property DiscType As String
            Get
                Return _DiscType
            End Get
            Set(ByVal value As String)
                _DiscType = If(value.ToLower = "3d", "3D", If(value.ToLower = "cd", "CD", If(value.ToLower = "dvd", "DVD", If(value.ToLower = "bluray", "BluRay", value))))
            End Set
        End Property

        Public Property Episode As Integer = -1

        Public Property Height As Integer
            Get
                Return _Height
            End Get
            Set(ByVal value As Integer)
                _Height = value
                DetectImageSize(value)
            End Set
        End Property

        Public ReadOnly Property HeightSpecified() As Boolean
            Get
                Return Not _Height = 0
            End Get
        End Property

        Public Property ImageOriginal As New Images

        Public ReadOnly Property ImageSize() As Enums.ImageSize
            Get
                Return _ImageSize
            End Get
        End Property

        Public Property ImageThumb As New Images

        Public Property Index As Integer = 0

        Public Property IsDuplicate As Boolean
        ''' <summary>
        ''' ISO 639 Alpha 2 Code, e.g. "en", "de", "es"
        ''' </summary>
        ''' <returns></returns>
        Public Property Language() As String
            Get
                Return _Language
            End Get
            Set(value As String)
                _Language = value 'Localization.IsoCheckLangByCode2(value)
                _LongLanguage = Localization.Languages.Get_Name_By_Alpha2(_Language)
            End Set
        End Property
        ''' <summary>
        ''' ISO 639 Short Name, e.g. "English", "German", "Spanish"
        ''' </summary>
        ''' <returns></returns>
        Public Property LongLanguage() As String
            Get
                Return _LongLanguage
            End Get
            Set(value As String)
                _LongLanguage = value 'Localization.IsoCheckLangByCode2(value)
                _Language = Localization.Languages.Get_Alpha2_By_Name(value)
            End Set
        End Property

        Public Property Likes As Integer = 0

        Public Property LocalFilePath As String = String.Empty

        <XmlIgnore()>
        Public ReadOnly Property LocalFilePathSpecified As Boolean
            Get
                Return Not String.IsNullOrEmpty(LocalFilePath)
            End Get
        End Property

        Public Property Scraper As String = String.Empty

        Public Property Season As Integer = -1

        Public Property TVBannerType As Enums.TVBannerType
            Get
                Return _TVBannerType
            End Get
            Set(value As Enums.TVBannerType)

            End Set
        End Property

        Public Property URLOriginal As String = String.Empty

        Public ReadOnly Property URLOriginalSpecified As Boolean
            Get
                Return Not String.IsNullOrEmpty(URLOriginal)
            End Get
        End Property

        Public Property URLThumb As String = String.Empty

        Public ReadOnly Property URLThumbSpecified As Boolean
            Get
                Return Not String.IsNullOrEmpty(URLThumb)
            End Get
        End Property

        Public Property VoteAverage As Double = 0

        Public Property VoteCount As Integer = 0

        Public Property Width() As Integer = 0

        Public ReadOnly Property WidthSpecified() As Boolean
            Get
                Return Not Width = 0
            End Get
        End Property

#End Region 'Properties

#Region "Methods"

        Private Sub DetectImageSize(ByVal height As Integer)
            Select Case height
                Case 3000
                    _ImageSize = Enums.ImageSize.HD3000
                Case 2160
                    _ImageSize = Enums.ImageSize.UHD2160
                Case 2100
                    _ImageSize = Enums.ImageSize.HD2100
                Case 1500
                    _ImageSize = Enums.ImageSize.HD1500
                Case 1440
                    _ImageSize = Enums.ImageSize.QHD1440
                Case 1426
                    _ImageSize = Enums.ImageSize.HD1426
                Case 1080
                    _ImageSize = Enums.ImageSize.HD1080
                Case 1000
                    _ImageSize = Enums.ImageSize.HD1000
                Case 720
                    _ImageSize = Enums.ImageSize.HD720
                Case 578
                    _ImageSize = Enums.ImageSize.HD578
                Case 562
                    _ImageSize = Enums.ImageSize.HD562
                Case 512
                    _ImageSize = Enums.ImageSize.HD512
                Case 310
                    _ImageSize = Enums.ImageSize.HD310
                Case 281
                    _ImageSize = Enums.ImageSize.SD281
                Case 225, 300 '225 = 16:9 / 300 = 4:3
                    _ImageSize = Enums.ImageSize.SD225
                Case 185
                    _ImageSize = Enums.ImageSize.HD185
                Case 155
                    _ImageSize = Enums.ImageSize.SD155
                Case 140
                    _ImageSize = Enums.ImageSize.HD140
                Case Else
                    _ImageSize = Enums.ImageSize.Any
            End Select
        End Sub

        Public Function LoadAndCache(ByVal tContentType As Enums.ContentType, ByVal needFullsize As Boolean, Optional ByVal loadBitmap As Boolean = False) As Boolean
            Dim doCache As Boolean = False

            Select Case tContentType
                Case Enums.ContentType.Movie
                    doCache = Master.eSettings.Movie.ImageSettings.CacheEnabled
                Case Enums.ContentType.Movieset
                    doCache = Master.eSettings.Movieset.ImageSettings.CacheEnabled
                Case Enums.ContentType.TV, Enums.ContentType.TVEpisode, Enums.ContentType.TVSeason, Enums.ContentType.TVShow
                    doCache = Master.eSettings.TVShow.ImageSettings.CacheEnabled
            End Select

            If Not ((ImageOriginal.HasMemoryStream OrElse (ImageThumb.HasMemoryStream AndAlso Not needFullsize)) AndAlso Not loadBitmap) Then
                If (ImageOriginal.Image Is Nothing AndAlso needFullsize) OrElse (ImageThumb.Image Is Nothing AndAlso Not needFullsize) Then
                    If File.Exists(LocalFilePath) AndAlso Not ImageOriginal.HasMemoryStream Then
                        ImageOriginal.LoadFromFile(LocalFilePath, loadBitmap)
                    ElseIf ImageThumb.HasMemoryStream AndAlso Not needFullsize AndAlso loadBitmap Then
                        ImageThumb.LoadFromMemoryStream()
                    ElseIf ImageOriginal.HasMemoryStream AndAlso loadBitmap Then
                        ImageOriginal.LoadFromMemoryStream()
                    ElseIf File.Exists(CacheThumbPath) AndAlso Not needFullsize Then
                        ImageThumb.LoadFromFile(CacheThumbPath, loadBitmap)
                    ElseIf File.Exists(CacheOriginalPath) Then
                        ImageOriginal.LoadFromFile(CacheOriginalPath, loadBitmap)
                    Else
                        If Not String.IsNullOrEmpty(URLThumb) AndAlso Not needFullsize Then
                            ImageThumb.LoadFromWeb(URLThumb, loadBitmap)
                            If doCache AndAlso Not String.IsNullOrEmpty(CacheThumbPath) AndAlso ImageThumb.HasMemoryStream Then
                                Directory.CreateDirectory(Directory.GetParent(CacheThumbPath).FullName)
                                ImageThumb.SaveToFile(CacheThumbPath)
                            End If
                        ElseIf Not String.IsNullOrEmpty(URLOriginal) Then
                            ImageOriginal.LoadFromWeb(URLOriginal, loadBitmap)
                            If doCache AndAlso Not String.IsNullOrEmpty(CacheOriginalPath) AndAlso ImageOriginal.HasMemoryStream Then
                                Directory.CreateDirectory(Directory.GetParent(CacheOriginalPath).FullName)
                                ImageOriginal.SaveToFile(CacheOriginalPath)
                            End If
                        End If
                    End If
                End If
            End If

            If (ImageOriginal.Image IsNot Nothing OrElse (ImageThumb.Image IsNot Nothing AndAlso Not needFullsize)) OrElse
                (Not loadBitmap AndAlso (ImageOriginal.HasMemoryStream OrElse (ImageThumb.HasMemoryStream AndAlso Not needFullsize))) Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Function CompareTo(ByVal other As [Image]) As Integer Implements IComparable(Of [Image]).CompareTo
            Try
                Dim retVal As Integer = (Language).CompareTo(other.Language)
                Return retVal
            Catch ex As Exception
                Return 0
            End Try
        End Function

#End Region 'Methods

    End Class

    <Serializable()>
    Public Class ImagesContainer

#Region "Properties"

        Public Property Banner As New Image

        Public ReadOnly Property BannerSpecified() As Boolean
            Get
                Return Banner IsNot Nothing
            End Get
        End Property

        Public Property CharacterArt As New Image

        Public ReadOnly Property CharacterArtSpecified() As Boolean
            Get
                Return CharacterArt IsNot Nothing
            End Get
        End Property

        Public Property ClearArt As New Image

        Public ReadOnly Property ClearArtSpecified() As Boolean
            Get
                Return ClearArt IsNot Nothing
            End Get
        End Property

        Public Property ClearLogo As New Image

        Public ReadOnly Property ClearLogoSpecified() As Boolean
            Get
                Return ClearLogo IsNot Nothing
            End Get
        End Property

        Public Property DiscArt As New Image

        Public ReadOnly Property DiscArtSpecified() As Boolean
            Get
                Return DiscArt IsNot Nothing
            End Get
        End Property

        Public Property Extrafanarts As New List(Of Image)

        Public ReadOnly Property ExtrafanartsSpecified() As Boolean
            Get
                Return Extrafanarts IsNot Nothing AndAlso Extrafanarts.Count > 0
            End Get
        End Property

        Public Property Extrathumbs As New List(Of Image)

        Public ReadOnly Property ExtrathumbsSpecified() As Boolean
            Get
                Return Extrathumbs IsNot Nothing AndAlso Extrathumbs.Count > 0
            End Get
        End Property

        Public Property Fanart As New Image

        Public ReadOnly Property FanartSpecified() As Boolean
            Get
                Return Fanart IsNot Nothing
            End Get
        End Property

        Public Property Keyart As New Image

        Public ReadOnly Property KeyartSpecified() As Boolean
            Get
                Return Keyart IsNot Nothing
            End Get
        End Property

        Public Property Landscape As New Image

        Public ReadOnly Property LandscapeSpecified() As Boolean
            Get
                Return Landscape IsNot Nothing
            End Get
        End Property

        Public Property Poster As New Image

        Public ReadOnly Property PosterSpecified() As Boolean
            Get
                Return Poster IsNot Nothing
            End Get
        End Property

#End Region 'Properties

#Region "Methods"

        Public Function GetImageByType(ByVal ImageType As Enums.ModifierType) As Image
            Select Case ImageType
                Case Enums.ModifierType.MainBanner, Enums.ModifierType.AllSeasonsBanner, Enums.ModifierType.SeasonBanner
                    Return Banner
                Case Enums.ModifierType.MainCharacterArt
                    Return CharacterArt
                Case Enums.ModifierType.MainClearArt
                    Return ClearArt
                Case Enums.ModifierType.MainClearLogo
                    Return ClearLogo
                Case Enums.ModifierType.MainDiscArt
                    Return DiscArt
                Case Enums.ModifierType.MainFanart, Enums.ModifierType.AllSeasonsFanart, Enums.ModifierType.EpisodeFanart, Enums.ModifierType.SeasonFanart
                    Return Fanart
                Case Enums.ModifierType.MainKeyart
                    Return Keyart
                Case Enums.ModifierType.MainLandscape, Enums.ModifierType.AllSeasonsLandscape, Enums.ModifierType.SeasonLandscape
                    Return Landscape
                Case Enums.ModifierType.MainPoster, Enums.ModifierType.AllSeasonsPoster, Enums.ModifierType.EpisodePoster, Enums.ModifierType.SeasonPoster
                    Return Poster
            End Select
            Return Nothing
        End Function

        Public Function GetImagesByType(ByVal ImageType As Enums.ModifierType) As List(Of Image)
            Select Case ImageType
                Case Enums.ModifierType.MainExtrafanarts
                    Return Extrafanarts
                Case Enums.ModifierType.MainExtrathumbs
                    Return Extrathumbs
            End Select
            Return Nothing
        End Function

        Public Sub LoadAllImages(ByVal Type As Enums.ContentType, ByVal LoadBitmap As Boolean, ByVal withExtraImages As Boolean)
            Banner.LoadAndCache(Type, True, LoadBitmap)
            CharacterArt.LoadAndCache(Type, True, LoadBitmap)
            ClearArt.LoadAndCache(Type, True, LoadBitmap)
            ClearLogo.LoadAndCache(Type, True, LoadBitmap)
            DiscArt.LoadAndCache(Type, True, LoadBitmap)
            Fanart.LoadAndCache(Type, True, LoadBitmap)
            Keyart.LoadAndCache(Type, True, LoadBitmap)
            Landscape.LoadAndCache(Type, True, LoadBitmap)
            Poster.LoadAndCache(Type, True, LoadBitmap)

            If withExtraImages Then
                For Each tImg As Image In Extrafanarts
                    tImg.LoadAndCache(Type, True, LoadBitmap)
                Next
                For Each tImg As Image In Extrathumbs
                    tImg.LoadAndCache(Type, True, LoadBitmap)
                Next
            End If
        End Sub

        Public Sub SaveAllImages(ByRef dbElement As Database.DBElement, ByVal forceFileCleanup As Boolean)
            If Not dbElement.FilenameSpecified AndAlso (dbElement.ContentType = Enums.ContentType.Movie OrElse dbElement.ContentType = Enums.ContentType.TVEpisode) Then Return

            Dim tContentType As Enums.ContentType = dbElement.ContentType

            Select Case tContentType
                Case Enums.ContentType.Movie

                    'Movie Banner
                    If Banner.LoadAndCache(tContentType, True) Then
                        If forceFileCleanup Then Images.Delete(dbElement, Enums.ModifierType.MainBanner, forceFileCleanup)
                        Banner.LocalFilePath = Banner.ImageOriginal.Save(dbElement, Enums.ModifierType.MainBanner)
                    Else
                        Images.Delete(dbElement, Enums.ModifierType.MainBanner, forceFileCleanup)
                        Banner = New Image
                    End If

                    'Movie ClearArt
                    If ClearArt.LoadAndCache(tContentType, True) Then
                        If forceFileCleanup Then Images.Delete(dbElement, Enums.ModifierType.MainClearArt, forceFileCleanup)
                        ClearArt.LocalFilePath = ClearArt.ImageOriginal.Save(dbElement, Enums.ModifierType.MainClearArt)
                    Else
                        Images.Delete(dbElement, Enums.ModifierType.MainClearArt, forceFileCleanup)
                        ClearArt = New Image
                    End If

                    'Movie ClearLogo
                    If ClearLogo.LoadAndCache(tContentType, True) Then
                        If forceFileCleanup Then Images.Delete(dbElement, Enums.ModifierType.MainClearLogo, forceFileCleanup)
                        ClearLogo.LocalFilePath = ClearLogo.ImageOriginal.Save(dbElement, Enums.ModifierType.MainClearLogo)
                    Else
                        Images.Delete(dbElement, Enums.ModifierType.MainClearLogo, forceFileCleanup)
                        ClearLogo = New Image
                    End If

                    'Movie DiscArt
                    If DiscArt.LoadAndCache(tContentType, True) Then
                        If forceFileCleanup Then Images.Delete(dbElement, Enums.ModifierType.MainDiscArt, forceFileCleanup)
                        DiscArt.LocalFilePath = DiscArt.ImageOriginal.Save(dbElement, Enums.ModifierType.MainDiscArt)
                    Else
                        Images.Delete(dbElement, Enums.ModifierType.MainDiscArt, forceFileCleanup)
                        DiscArt = New Image
                    End If

                    'Movie Extrafanarts
                    If Extrafanarts.Count > 0 Then
                        dbElement.ExtrafanartsPath = Images.Save_Extrafanarts(dbElement)
                    Else
                        Images.Delete(dbElement, Enums.ModifierType.MainExtrafanarts, forceFileCleanup)
                        Extrafanarts = New List(Of Image)
                        dbElement.ExtrafanartsPath = String.Empty
                    End If

                    'Movie Extrathumbs
                    If Extrathumbs.Count > 0 Then
                        dbElement.ExtrathumbsPath = Images.Save_Extrathumbs(dbElement)
                    Else
                        Images.Delete(dbElement, Enums.ModifierType.MainExtrathumbs, forceFileCleanup)
                        Extrathumbs = New List(Of Image)
                        dbElement.ExtrathumbsPath = String.Empty
                    End If

                    'Movie Fanart
                    If Fanart.LoadAndCache(tContentType, True) Then
                        If forceFileCleanup Then Images.Delete(dbElement, Enums.ModifierType.MainFanart, forceFileCleanup)
                        Fanart.LocalFilePath = Fanart.ImageOriginal.Save(dbElement, Enums.ModifierType.MainFanart)
                    Else
                        Images.Delete(dbElement, Enums.ModifierType.MainFanart, forceFileCleanup)
                        Fanart = New Image
                    End If

                    'Movie Keyart
                    If Keyart.LoadAndCache(tContentType, True) Then
                        If forceFileCleanup Then Images.Delete(dbElement, Enums.ModifierType.MainKeyart, forceFileCleanup)
                        Keyart.LocalFilePath = Keyart.ImageOriginal.Save(dbElement, Enums.ModifierType.MainKeyart)
                    Else
                        Images.Delete(dbElement, Enums.ModifierType.MainKeyart, forceFileCleanup)
                        Keyart = New Image
                    End If

                    'Movie Landscape
                    If Landscape.LoadAndCache(tContentType, True) Then
                        If forceFileCleanup Then Images.Delete(dbElement, Enums.ModifierType.MainLandscape, forceFileCleanup)
                        Landscape.LocalFilePath = Landscape.ImageOriginal.Save(dbElement, Enums.ModifierType.MainLandscape)
                    Else
                        Images.Delete(dbElement, Enums.ModifierType.MainLandscape, forceFileCleanup)
                        Landscape = New Image
                    End If

                    'Movie Poster
                    If Poster.LoadAndCache(tContentType, True) Then
                        If forceFileCleanup Then Images.Delete(dbElement, Enums.ModifierType.MainPoster, forceFileCleanup)
                        Poster.LocalFilePath = Poster.ImageOriginal.Save(dbElement, Enums.ModifierType.MainPoster)
                    Else
                        Images.Delete(dbElement, Enums.ModifierType.MainPoster, forceFileCleanup)
                        Poster = New Image
                    End If

                Case Enums.ContentType.Movieset

                    'Movieset Banner
                    If Banner.LoadAndCache(tContentType, True) Then
                        If dbElement.MainDetails.Title_HasChanged Then Images.Delete(dbElement, Enums.ModifierType.MainBanner, True)
                        Banner.LocalFilePath = Banner.ImageOriginal.Save(dbElement, Enums.ModifierType.MainBanner)
                    Else
                        Images.Delete(dbElement, Enums.ModifierType.MainBanner, dbElement.MainDetails.Title_HasChanged)
                        Banner = New Image
                    End If

                    'Movieset ClearArt
                    If ClearArt.LoadAndCache(tContentType, True) Then
                        If dbElement.MainDetails.Title_HasChanged Then Images.Delete(dbElement, Enums.ModifierType.MainClearArt, True)
                        ClearArt.LocalFilePath = ClearArt.ImageOriginal.Save(dbElement, Enums.ModifierType.MainClearArt)
                    Else
                        Images.Delete(dbElement, Enums.ModifierType.MainClearArt, dbElement.MainDetails.Title_HasChanged)
                        ClearArt = New Image
                    End If

                    'Movieset ClearLogo
                    If ClearLogo.LoadAndCache(tContentType, True) Then
                        If dbElement.MainDetails.Title_HasChanged Then Images.Delete(dbElement, Enums.ModifierType.MainClearLogo, True)
                        ClearLogo.LocalFilePath = ClearLogo.ImageOriginal.Save(dbElement, Enums.ModifierType.MainClearLogo)
                    Else
                        Images.Delete(dbElement, Enums.ModifierType.MainClearLogo, dbElement.MainDetails.Title_HasChanged)
                        ClearLogo = New Image
                    End If

                    'Movieset DiscArt
                    If DiscArt.LoadAndCache(tContentType, True) Then
                        If dbElement.MainDetails.Title_HasChanged Then Images.Delete(dbElement, Enums.ModifierType.MainDiscArt, True)
                        DiscArt.LocalFilePath = DiscArt.ImageOriginal.Save(dbElement, Enums.ModifierType.MainDiscArt)
                    Else
                        Images.Delete(dbElement, Enums.ModifierType.MainDiscArt, dbElement.MainDetails.Title_HasChanged)
                        DiscArt = New Image
                    End If

                    'Movieset Fanart
                    If Fanart.LoadAndCache(tContentType, True) Then
                        If dbElement.MainDetails.Title_HasChanged Then Images.Delete(dbElement, Enums.ModifierType.MainFanart, True)
                        Fanart.LocalFilePath = Fanart.ImageOriginal.Save(dbElement, Enums.ModifierType.MainFanart)
                    Else
                        Images.Delete(dbElement, Enums.ModifierType.MainFanart, dbElement.MainDetails.Title_HasChanged)
                        Fanart = New Image
                    End If

                    'Movieset Keyart
                    If Keyart.LoadAndCache(tContentType, True) Then
                        If dbElement.MainDetails.Title_HasChanged Then Images.Delete(dbElement, Enums.ModifierType.MainKeyart, True)
                        Keyart.LocalFilePath = Keyart.ImageOriginal.Save(dbElement, Enums.ModifierType.MainKeyart)
                    Else
                        Images.Delete(dbElement, Enums.ModifierType.MainKeyart, dbElement.MainDetails.Title_HasChanged)
                        Keyart = New Image
                    End If

                    'Movieset Landscape
                    If Landscape.LoadAndCache(tContentType, True) Then
                        If dbElement.MainDetails.Title_HasChanged Then Images.Delete(dbElement, Enums.ModifierType.MainLandscape, True)
                        Landscape.LocalFilePath = Landscape.ImageOriginal.Save(dbElement, Enums.ModifierType.MainLandscape)
                    Else
                        Images.Delete(dbElement, Enums.ModifierType.MainLandscape, dbElement.MainDetails.Title_HasChanged)
                        Landscape = New Image
                    End If

                    'Movieset Poster
                    If Poster.LoadAndCache(tContentType, True) Then
                        If dbElement.MainDetails.Title_HasChanged Then Images.Delete(dbElement, Enums.ModifierType.MainPoster, True)
                        Poster.LocalFilePath = Poster.ImageOriginal.Save(dbElement, Enums.ModifierType.MainPoster)
                    Else
                        Images.Delete(dbElement, Enums.ModifierType.MainPoster, dbElement.MainDetails.Title_HasChanged)
                        Poster = New Image
                    End If

                Case Enums.ContentType.TVEpisode

                    'Episode Fanart
                    If Fanart.LoadAndCache(tContentType, True) Then
                        Fanart.LocalFilePath = Fanart.ImageOriginal.Save(dbElement, Enums.ModifierType.EpisodeFanart)
                    Else
                        Images.Delete(dbElement, Enums.ModifierType.EpisodeFanart)
                        Fanart = New Image
                    End If

                    'Episode Poster
                    If Poster.LoadAndCache(tContentType, True) Then
                        Poster.LocalFilePath = Poster.ImageOriginal.Save(dbElement, Enums.ModifierType.EpisodePoster)
                    Else
                        Images.Delete(dbElement, Enums.ModifierType.EpisodePoster)
                        Poster = New Image
                    End If

                Case Enums.ContentType.TVSeason

                    'Season Banner
                    If Banner.LoadAndCache(tContentType, True) Then
                        If dbElement.MainDetails.Season_IsAllSeasons Then
                            Banner.LocalFilePath = Banner.ImageOriginal.Save(dbElement, Enums.ModifierType.AllSeasonsBanner)
                        Else
                            Banner.LocalFilePath = Banner.ImageOriginal.Save(dbElement, Enums.ModifierType.SeasonBanner)
                        End If
                    Else
                        If dbElement.MainDetails.Season_IsAllSeasons Then
                            Images.Delete(dbElement, Enums.ModifierType.AllSeasonsBanner)
                            Banner = New Image
                        Else
                            Images.Delete(dbElement, Enums.ModifierType.SeasonBanner)
                            Banner = New Image
                        End If
                    End If

                    'Season Fanart
                    If Fanart.LoadAndCache(tContentType, True) Then
                        If dbElement.MainDetails.Season_IsAllSeasons Then
                            Fanart.LocalFilePath = Fanart.ImageOriginal.Save(dbElement, Enums.ModifierType.AllSeasonsFanart)
                        Else
                            Fanart.LocalFilePath = Fanart.ImageOriginal.Save(dbElement, Enums.ModifierType.SeasonFanart)
                        End If
                    Else
                        If dbElement.MainDetails.Season_IsAllSeasons Then
                            Images.Delete(dbElement, Enums.ModifierType.AllSeasonsFanart)
                            Fanart = New Image
                        Else
                            Images.Delete(dbElement, Enums.ModifierType.SeasonFanart)
                            Fanart = New Image
                        End If
                    End If

                    'Season Landscape
                    If Landscape.LoadAndCache(tContentType, True) Then
                        If dbElement.MainDetails.Season_IsAllSeasons Then
                            Landscape.LocalFilePath = Landscape.ImageOriginal.Save(dbElement, Enums.ModifierType.AllSeasonsLandscape)
                        Else
                            Landscape.LocalFilePath = Landscape.ImageOriginal.Save(dbElement, Enums.ModifierType.SeasonLandscape)
                        End If
                    Else
                        If dbElement.MainDetails.Season_IsAllSeasons Then
                            Images.Delete(dbElement, Enums.ModifierType.AllSeasonsLandscape)
                            Landscape = New Image
                        Else
                            Images.Delete(dbElement, Enums.ModifierType.SeasonLandscape)
                            Landscape = New Image
                        End If
                    End If

                    'Season Poster
                    If Poster.LoadAndCache(tContentType, True) Then
                        If dbElement.MainDetails.Season_IsAllSeasons Then
                            Poster.LocalFilePath = Poster.ImageOriginal.Save(dbElement, Enums.ModifierType.AllSeasonsPoster)
                        Else
                            Poster.LocalFilePath = Poster.ImageOriginal.Save(dbElement, Enums.ModifierType.SeasonPoster)
                        End If
                    Else
                        If dbElement.MainDetails.Season_IsAllSeasons Then
                            Images.Delete(dbElement, Enums.ModifierType.AllSeasonsPoster)
                            Poster = New Image
                        Else
                            Images.Delete(dbElement, Enums.ModifierType.SeasonPoster)
                            Poster = New Image
                        End If
                    End If

                Case Enums.ContentType.TVShow

                    'Show Banner
                    If Banner.LoadAndCache(tContentType, True) Then
                        Banner.LocalFilePath = Banner.ImageOriginal.Save(dbElement, Enums.ModifierType.MainBanner)
                    Else
                        Images.Delete(dbElement, Enums.ModifierType.MainBanner)
                        Banner = New Image
                    End If

                    'Show CharacterArt
                    If CharacterArt.LoadAndCache(tContentType, True) Then
                        CharacterArt.LocalFilePath = CharacterArt.ImageOriginal.Save(dbElement, Enums.ModifierType.MainCharacterArt)
                    Else
                        Images.Delete(dbElement, Enums.ModifierType.MainCharacterArt)
                        CharacterArt = New Image
                    End If

                    'Show ClearArt
                    If ClearArt.LoadAndCache(tContentType, True) Then
                        ClearArt.LocalFilePath = ClearArt.ImageOriginal.Save(dbElement, Enums.ModifierType.MainClearArt)
                    Else
                        Images.Delete(dbElement, Enums.ModifierType.MainClearArt)
                        ClearArt = New Image
                    End If

                    'Show ClearLogo
                    If ClearLogo.LoadAndCache(tContentType, True) Then
                        ClearLogo.LocalFilePath = ClearLogo.ImageOriginal.Save(dbElement, Enums.ModifierType.MainClearLogo)
                    Else
                        Images.Delete(dbElement, Enums.ModifierType.MainClearLogo)
                        ClearLogo = New Image
                    End If

                    'Show Extrafanarts
                    If Extrafanarts.Count > 0 Then
                        dbElement.ExtrafanartsPath = Images.Save_Extrafanarts(dbElement)
                    Else
                        Images.Delete(dbElement, Enums.ModifierType.MainExtrafanarts)
                        Extrafanarts = New List(Of Image)
                        dbElement.ExtrafanartsPath = String.Empty
                    End If

                    'Show Fanart
                    If Fanart.LoadAndCache(tContentType, True) Then
                        Fanart.LocalFilePath = Fanart.ImageOriginal.Save(dbElement, Enums.ModifierType.MainFanart)
                    Else
                        Images.Delete(dbElement, Enums.ModifierType.MainFanart)
                        Fanart = New Image
                    End If

                    'Show Keyart
                    If Keyart.LoadAndCache(tContentType, True) Then
                        Keyart.LocalFilePath = Keyart.ImageOriginal.Save(dbElement, Enums.ModifierType.MainKeyart)
                    Else
                        Images.Delete(dbElement, Enums.ModifierType.MainKeyart)
                        Keyart = New Image
                    End If

                    'Show Landscape
                    If Landscape.LoadAndCache(tContentType, True) Then
                        Landscape.LocalFilePath = Landscape.ImageOriginal.Save(dbElement, Enums.ModifierType.MainLandscape)
                    Else
                        Images.Delete(dbElement, Enums.ModifierType.MainLandscape)
                        Landscape = New Image
                    End If

                    'Show Poster
                    If Poster.LoadAndCache(tContentType, True) Then
                        Poster.LocalFilePath = Poster.ImageOriginal.Save(dbElement, Enums.ModifierType.MainPoster)
                    Else
                        Images.Delete(dbElement, Enums.ModifierType.MainPoster)
                        Poster = New Image
                    End If
            End Select
        End Sub

        Public Sub SetImageByType(ByRef Image As Image, ByVal ImageType As Enums.ModifierType)
            Select Case ImageType
                Case Enums.ModifierType.MainBanner, Enums.ModifierType.AllSeasonsBanner, Enums.ModifierType.SeasonBanner
                    Banner = Image
                Case Enums.ModifierType.MainCharacterArt
                    CharacterArt = Image
                Case Enums.ModifierType.MainClearArt
                    ClearArt = Image
                Case Enums.ModifierType.MainClearLogo
                    ClearLogo = Image
                Case Enums.ModifierType.MainDiscArt
                    DiscArt = Image
                Case Enums.ModifierType.MainFanart, Enums.ModifierType.AllSeasonsFanart, Enums.ModifierType.EpisodeFanart, Enums.ModifierType.SeasonFanart
                    Fanart = Image
                Case Enums.ModifierType.MainKeyart
                    Keyart = Image
                Case Enums.ModifierType.MainLandscape, Enums.ModifierType.AllSeasonsLandscape, Enums.ModifierType.SeasonLandscape
                    Landscape = Image
                Case Enums.ModifierType.MainPoster, Enums.ModifierType.AllSeasonsPoster, Enums.ModifierType.EpisodePoster, Enums.ModifierType.SeasonPoster
                    Poster = Image
            End Select
        End Sub

        Public Sub SetImagesByType(ByRef Images As List(Of Image), ByVal ImageType As Enums.ModifierType)
            Select Case ImageType
                Case Enums.ModifierType.MainExtrafanarts
                    Extrafanarts = Images
                Case Enums.ModifierType.MainExtrathumbs
                    Extrathumbs = Images
            End Select
        End Sub

        Public Sub SortExtrathumbs()
            Dim newList As New List(Of Image)
            Dim newIndex As Integer = 0
            For Each tImg As Image In Extrathumbs.OrderBy(Function(f) f.Index)
                tImg.Index = newIndex
                newList.Add(tImg)
                newIndex += 1
            Next
            Extrathumbs = newList
        End Sub

#End Region 'Methods

    End Class

    <Obsolete("Use MainDetails")>
    Public Class SeasonDetails
        Inherits MainDetails
    End Class
    <Obsolete("Use MainDetails")>
    Public Class EpisodeDetails
        Inherits MainDetails
    End Class
    <Obsolete("Use MainDetails")>
    Public Class Movie
        Inherits MainDetails
    End Class
    <Obsolete("Use MainDetails")>
    Public Class TVShow
        Inherits MainDetails
    End Class

    <Serializable()>
    Public Class MainDetails
        Implements ICloneable
        Implements IComparable(Of MainDetails)

#Region "Fields"

        Shared _Logger As Logger = LogManager.GetCurrentClassLogger()

        Private _ContentType As Enums.ContentType = Enums.ContentType.None
        Private _Certifications As New List(Of String)
        Private _LastPlayed As String = String.Empty
        Private _Rating As Double = 0
        Private _Tags As New List(Of String)
        Private _Votes As Integer = 0
        Private _Year As String = String.Empty

#End Region 'Fields

#Region "Properties"

        <XmlElement("actor")>
        Public Property Actors() As New List(Of RoleLink)

        <XmlIgnore()>
        Public ReadOnly Property ActorsSpecified() As Boolean
            Get
                Return Actors.Count > 0
            End Get
        End Property

        <XmlElement("aired")>
        Public Property Aired() As String = String.Empty

        <XmlIgnore()>
        Public ReadOnly Property AiredSpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(Aired)
            End Get
        End Property

        <XmlElement("boxeeTvDb")>
        Public Property BoxeeTVDb() As Integer = -1

        <XmlIgnore()>
        Public ReadOnly Property BoxeeTvDbSpecified() As Boolean
            Get
                Return Not BoxeeTVDb = -1
            End Get
        End Property

        <XmlElement("certification")>
        Public Property Certifications() As List(Of String)
            Get
                Return _Certifications
            End Get
            Set(ByVal value As List(Of String))
                If value Is Nothing Then
                    _Certifications.Clear()
                Else
                    _Certifications = value
                End If
            End Set
        End Property

        <XmlIgnore()>
        Public ReadOnly Property CertificationsSpecified() As Boolean
            Get
                Return Certifications.Count > 0
            End Get
        End Property

        <XmlElement("country")>
        Public Property Countries() As List(Of String) = New List(Of String)

        <XmlIgnore()>
        Public ReadOnly Property CountriesSpecified() As Boolean
            Get
                Return Countries.Count > 0
            End Get
        End Property

        <XmlElement("creator")>
        Public Property Creators() As List(Of String) = New List(Of String)

        <XmlIgnore()>
        Public ReadOnly Property CreatorsSpecified() As Boolean
            Get
                Return Creators.Count > 0
            End Get
        End Property

        <XmlElement("credits")>
        Public Property Credits() As List(Of String) = New List(Of String)

        <XmlIgnore()>
        Public ReadOnly Property CreditsSpecified() As Boolean
            Get
                Return Credits.Count > 0
            End Get
        End Property

        <XmlElement("dateadded")>
        Public Property DateAdded() As String = String.Empty

        <XmlIgnore()>
        Public ReadOnly Property DateAddedSpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(DateAdded)
            End Get
        End Property

        <XmlElement("datemodified")>
        Public Property DateModified() As String = String.Empty

        <XmlIgnore()>
        Public ReadOnly Property DateModifiedSpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(DateModified)
            End Get
        End Property

        <XmlElement("director")>
        Public Property Directors() As List(Of String) = New List(Of String)

        <XmlIgnore()>
        Public ReadOnly Property DirectorsSpecified() As Boolean
            Get
                Return Directors.Count > 0
            End Get
        End Property

        <XmlElement("displayepisode")>
        Public Property DisplayEpisode() As Integer = -1

        <XmlIgnore()>
        Public ReadOnly Property DisplayEpisodeSpecified() As Boolean
            Get
                Return DisplayEpisode > -1
            End Get
        End Property

        <XmlElement("displayseason")>
        Public Property DisplaySeason() As Integer = -1

        <XmlIgnore()>
        Public ReadOnly Property DisplaySeasonSpecified() As Boolean
            Get
                Return DisplaySeason > -1
            End Get
        End Property

        <XmlElement("edition")>
        Public Property Edition() As String = String.Empty

        <XmlIgnore()>
        Public ReadOnly Property EditionSpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(Edition)
            End Get
        End Property

        <XmlElement("episode")>
        Public Property Episode() As Integer = -1

        <XmlIgnore()>
        Public ReadOnly Property EpisodeSpecified() As Boolean
            Get
                Return Episode > -1
            End Get
        End Property

        <XmlIgnore()>
        Public Property EpisodeAbsolute() As Integer = -1

        <XmlIgnore()>
        Public Property EpisodeCombined() As Double = -1

        <XmlIgnore()>
        Public Property EpisodeDVD() As Double = -1

        <XmlElement("episodeguide")>
        Public Property EpisodeGuideURL() As New EpisodeGuide

        <XmlIgnore()>
        Public ReadOnly Property EpisodeGuideURLSpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(EpisodeGuideURL.URL)
            End Get
        End Property

        <XmlElement("fanart")>
        Public Property Fanart() As Fanart = New Fanart

        <XmlIgnore()>
        Public ReadOnly Property FanartSpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(Fanart.URL)
            End Get
        End Property

        <XmlElement("fileinfo")>
        Public Property FileInfo() As Fileinfo = New Fileinfo

        <XmlIgnore()>
        Public ReadOnly Property FileInfoSpecified() As Boolean
            Get
                Return FileInfo.StreamDetails.AudioSpecified OrElse
                    FileInfo.StreamDetails.SubtitleSpecified OrElse
                    FileInfo.StreamDetails.VideoSpecified
            End Get
        End Property

        <XmlElement("genre")>
        Public Property Genres() As List(Of String) = New List(Of String)

        <XmlIgnore()>
        Public ReadOnly Property GenresSpecified() As Boolean
            Get
                Return Genres.Count > 0
            End Get
        End Property

        <XmlElement("gueststar")>
        Public Property GuestStars() As List(Of RoleLink) = New List(Of RoleLink)

        <XmlIgnore()>
        Public ReadOnly Property GuestStarsSpecified() As Boolean
            Get
                Return GuestStars.Count > 0
            End Get
        End Property

        <XmlElement("id")>
        Public Property DefaultId() As DefaultId
            Get
                Return UniqueIDs.GetDefaultId()
            End Get
            Set(value As DefaultId)
                UniqueIDs.Add(value)
            End Set
        End Property

        <XmlIgnore()>
        Public ReadOnly Property DefaultIdSpecified() As Boolean
            Get
                Select Case _ContentType
                    Case Enums.ContentType.Movie
                        Return DefaultId.ValueSpecified AndAlso Master.eSettings.Movie.InformationSettings.UniqueId.CreateNodeId
                    Case Enums.ContentType.TVEpisode, Enums.ContentType.TVSeason, Enums.ContentType.TVShow
                        Return DefaultId.ValueSpecified AndAlso Master.eSettings.TVShow.InformationSettings.UniqueId.CreateNodeId
                End Select
                Return False
            End Get
        End Property

        <XmlElement("locked")>
        Public Property IsLocked() As Boolean = False

        <XmlIgnore()>
        Public Property KnownEpisodes() As New List(Of MainDetails)

        <XmlIgnore()>
        Public ReadOnly Property KnownEpisodesSpecified() As Boolean
            Get
                Return KnownEpisodes.Count > 0
            End Get
        End Property

        <XmlIgnore()>
        Public Property KnownSeasons() As New List(Of MainDetails)

        <XmlIgnore()>
        Public ReadOnly Property KnownSeasonsSpecified() As Boolean
            Get
                Return KnownSeasons.Count > 0
            End Get
        End Property

        <XmlElement("language")>
        Public Property Language() As String = String.Empty

        <XmlIgnore()>
        Public ReadOnly Property LanguageSpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(Language)
            End Get
        End Property

        <XmlElement("lastplayed")>
        Public Property LastPlayed() As String
            Get
                Return _LastPlayed
            End Get
            Set(ByVal value As String)
                _LastPlayed = Functions.ConvertToProperDateTime(value)
            End Set
        End Property

        <XmlIgnore()>
        Public ReadOnly Property LastPlayedSpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(LastPlayed)
            End Get
        End Property

        <XmlIgnore()>
        Public Property Lev() As Integer = 0

        <XmlElement("mpaa")>
        Public Property MPAA() As String = String.Empty

        <XmlIgnore()>
        Public ReadOnly Property MPAASpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(MPAA)
            End Get
        End Property

        <XmlElement("originaltitle")>
        Public Property OriginalTitle() As String = String.Empty

        <XmlIgnore()>
        Public ReadOnly Property OriginalTitleSpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(OriginalTitle)
            End Get
        End Property

        <XmlElement("outline")>
        Public Property Outline() As String = String.Empty

        <XmlIgnore()>
        Public ReadOnly Property OutlineSpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(Outline)
            End Get
        End Property

        <XmlElement("playcount")>
        Public Property PlayCount() As Integer = 0

        <XmlIgnore()>
        Public ReadOnly Property PlayCountSpecified() As Boolean
            Get
                Return PlayCount > 0
            End Get
        End Property

        <XmlElement("plot")>
        Public Property Plot() As String = String.Empty

        <XmlIgnore()>
        Public ReadOnly Property PlotSpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(Plot)
            End Get
        End Property

        <XmlElement("premiered")>
        Public Property Premiered() As String = String.Empty

        <XmlIgnore()>
        Public ReadOnly Property PremieredSpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(Premiered)
            End Get
        End Property

        <XmlElement("rating")>
        Public Property Rating() As String
            Get
                Dim nRating = Ratings.GetDefaultRating()
                If nRating IsNot Nothing Then
                    Return nRating.ValueNormalized.ToString
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                Dim dblRatings As Double
                If Double.TryParse(value.Replace(",", "."),
                                   Globalization.NumberStyles.AllowDecimalPoint,
                                   Globalization.CultureInfo.InvariantCulture,
                                   dblRatings
                                   ) Then
                    _Rating = dblRatings
                    If Not _Votes = 0 Then Ratings.Add(New RatingDetails With {
                                                       .IsDefault = True,
                                                       .Max = If(_Rating <= 10, 10, 100),
                                                       .Type = "default",
                                                       .Value = _Rating,
                                                       .Votes = _Votes
                                                       })
                Else
                    _Rating = 0
                End If
            End Set
        End Property

        <Obsolete()>
        <XmlIgnore()>
        Public ReadOnly Property RatingSpecified As Boolean
            Get
                Select Case _ContentType
                    Case Enums.ContentType.Movie
                        Return Not String.IsNullOrEmpty(Rating) AndAlso Not String.IsNullOrEmpty(Votes) AndAlso Master.eSettings.Movie.InformationSettings.Ratings.CreateNodes
                    Case Enums.ContentType.TVEpisode, Enums.ContentType.TVSeason, Enums.ContentType.TVShow
                        Return Not String.IsNullOrEmpty(Rating) AndAlso Not String.IsNullOrEmpty(Votes) AndAlso Master.eSettings.TVShow.InformationSettings.Ratings.CreateNodes
                End Select
                Return False
            End Get
        End Property

        <XmlIgnore()>
        Public Property Ratings() As New RatingContainer(_ContentType)

        <XmlIgnore()>
        Public ReadOnly Property RatingsSpecified() As Boolean
            Get
                Return Ratings.AnyRatingSpecified
            End Get
        End Property

        <XmlArray("ratings")>
        <XmlArrayItem("rating")>
        Public Property Ratings_Kodi() As RatingDetails()
            Get
                Return Ratings.Items.ToArray
            End Get
            Set(value As RatingDetails())
                Ratings.AddRange(value.ToList)
            End Set
        End Property

        <Obsolete>
        <XmlElement("releasedate")>
        Public Property ReleaseDate() As String = String.Empty

        <Obsolete>
        <XmlIgnore()>
        Public ReadOnly Property ReleaseDateSpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(ReleaseDate)
            End Get
        End Property

        <XmlElement("runtime")>
        Public Property Runtime() As String = String.Empty

        <XmlIgnore()>
        Public ReadOnly Property RuntimeSpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(Runtime) AndAlso Not Runtime = "0"
            End Get
        End Property

        <XmlIgnore()>
        Public Property Scrapersource() As String = String.Empty

        <XmlElement("season")>
        Public Property Season() As Integer = -2 '-1 is reserved for * All Seasons entry  

        <XmlIgnore()>
        Public ReadOnly Property SeasonSpecified() As Boolean
            Get
                Return Season > -2
            End Get
        End Property

        <XmlIgnore()>
        Public ReadOnly Property Season_IsAllSeasons() As Boolean
            Get
                Return Season = -1
            End Get
        End Property

        <XmlIgnore()>
        Public Property SeasonCombined() As Integer = -1

        <XmlIgnore()>
        Public Property SeasonDVD() As Integer = -1

        <XmlElement("seasons")>
        Public Property Seasons() As New Seasons

        <XmlIgnore()>
        Public ReadOnly Property SeasonsSpecified() As Boolean
            Get
                Return Seasons.Seasons.Count > 0
            End Get
        End Property

        <XmlIgnore()>
        Public Property Sets() As New MoviesetContainer

        <XmlIgnore()>
        Public ReadOnly Property SetsSpecified() As Boolean
            Get
                Return Sets.Items.Count > 0
            End Get
        End Property

        <XmlAnyElement("set")>
        Public Property Set_Kodi() As Object
            Get
                Return Sets.Create_SetNode_Kodi()
            End Get
            Set(ByVal value As Object)
                Sets.Read_SetNode_Kodi(value)
            End Set
        End Property

        <XmlArray("sets")>
        <XmlArrayItem("set")>
        Public Property Sets_YAMJ() As MoviesetContainer.MoviesetDetails_YAMJ()
            Get
                Return Sets.Create_SetNode_YAMJ.ToArray
            End Get
            Set(ByVal value As MoviesetContainer.MoviesetDetails_YAMJ())
                Sets.AddRange(value.ToList)
            End Set
        End Property

        <XmlIgnore()>
        Public ReadOnly Property Sets_YAMJSpecified() As Boolean
            Get
                Return Master.eSettings.Movie.InformationSettings.Collection.SaveYAMJCompatible AndAlso Sets_YAMJ.Count > 0
            End Get
        End Property

        <XmlElement("showlink")>
        Public Property ShowLinks() As List(Of String) = New List(Of String)

        <XmlIgnore()>
        Public ReadOnly Property ShowLinksSpecified() As Boolean
            Get
                Return ShowLinks.Count > 0
            End Get
        End Property

        <XmlElement("sorttitle")>
        Public Property SortTitle() As String = String.Empty

        <XmlIgnore()>
        Public ReadOnly Property SortTitleSpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(SortTitle)
            End Get
        End Property

        <XmlElement("status")>
        Public Property Status() As String = String.Empty

        <XmlIgnore()>
        Public ReadOnly Property StatusSpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(Status)
            End Get
        End Property

        <XmlElement("studio")>
        Public Property Studios() As List(Of String) = New List(Of String)

        <XmlIgnore()>
        Public ReadOnly Property StudiosSpecified() As Boolean
            Get
                Return Studios.Count > 0
            End Get
        End Property

        <XmlElement("subepisode")>
        Public Property SubEpisode() As Integer = -1

        <XmlIgnore()>
        Public ReadOnly Property SubEpisodeSpecified() As Boolean
            Get
                Return SubEpisode > 0
            End Get
        End Property

        <XmlElement("tagline")>
        Public Property Tagline() As String = String.Empty

        <XmlIgnore()>
        Public ReadOnly Property TaglineSpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(Tagline)
            End Get
        End Property

        <XmlElement("tag")>
        Public Property Tags() As List(Of String)
            Get
                Return _Tags
            End Get
            Set(ByVal value As List(Of String))
                If value Is Nothing Then
                    _Tags.Clear()
                Else
                    _Tags = value
                End If
            End Set
        End Property

        <XmlIgnore()>
        Public ReadOnly Property TagsSpecified() As Boolean
            Get
                Return Tags.Count > 0
            End Get
        End Property
        ''' <summary>
        ''' Poster Thumb for preview in search results
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <XmlIgnore()>
        Public Property ThumbPoster() As New Image

        <XmlElement("thumb")>
        Public Property Thumbs() As New List(Of String)

        <XmlIgnore()>
        Public ReadOnly Property ThumbsSpecified() As Boolean
            Get
                Return Thumbs.Count > 0
            End Get
        End Property

        <XmlElement("title")>
        Public Property Title() As String = String.Empty

        <XmlIgnore()>
        Public ReadOnly Property TitleSpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(Title)
            End Get
        End Property

        <XmlIgnore()>
        Public ReadOnly Property Title_HasChanged() As Boolean
            Get
                Return Not Title_Old = Title
            End Get
        End Property
        ''' <summary>
        ''' Old Title before edit or scraping. Needed to remove no longer valid images and NFO.
        ''' </summary>
        ''' <returns></returns>
        <XmlIgnore()>
        Public Property Title_Old() As String = String.Empty

        <XmlElement("tmdb")>
        Public Property TMDbId() As String
            Get
                Return UniqueIDs.TMDbId.ToString
            End Get
            Set(value As String)
                UniqueIDs.Add("tmdb", value)
            End Set
        End Property

        <Obsolete()>
        <XmlIgnore()>
        Public ReadOnly Property TMDbIdSpecified() As Boolean
            Get
                Return UniqueIDs.TMDbIdSpecified AndAlso Master.eSettings.Movie.InformationSettings.UniqueId.CreateNodeTmdb
            End Get
        End Property

        <XmlElement("tmdbcolid")>
        Public Property TMDbCollectionId() As String
            Get
                Return UniqueIDs.TMDbCollectionId.ToString
            End Get
            Set(value As String)
                UniqueIDs.Add("tmdbcol", value)
            End Set
        End Property

        <Obsolete()>
        <XmlIgnore()>
        Public ReadOnly Property TMDbCollectionIdSpecified() As Boolean
            Get
                Return UniqueIDs.TMDbCollectionIdSpecified AndAlso Master.eSettings.Movie.InformationSettings.UniqueId.CreateNodeTmdbColId
            End Get
        End Property

        <XmlElement("top250")>
        Public Property Top250() As Integer = 0

        <XmlIgnore()>
        Public ReadOnly Property Top250Specified() As Boolean
            Get
                Return Top250 > 0
            End Get
        End Property

        <XmlElement("trailer")>
        Public Property Trailer() As String = String.Empty

        <XmlIgnore()>
        Public ReadOnly Property TrailerSpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(Trailer)
            End Get
        End Property

        <XmlIgnore()>
        Public Property UniqueIDs() As New UniqueidContainer(_ContentType)

        <XmlIgnore()>
        Public ReadOnly Property UniqueIDsSpecified() As Boolean
            Get
                Return UniqueIDs.Items.Count > 0
            End Get
        End Property

        <XmlElement("uniqueid")>
        Public Property UniqueIDs_Kodi() As Uniqueid()
            Get
                Return UniqueIDs.Items.ToArray
            End Get
            Set(value As Uniqueid())
                If value IsNot Nothing Then UniqueIDs.AddRange(value.ToList)
            End Set
        End Property

        <XmlElement("user_note")>
        Public Property UserNote() As String = String.Empty

        <XmlIgnore()>
        Public ReadOnly Property UserNoteSpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(UserNote)
            End Get
        End Property

        <XmlElement("userrating")>
        Public Property UserRating() As Integer = 0

        <XmlIgnore()>
        Public ReadOnly Property UserRatingSpecified() As Boolean
            Get
                Return Not UserRating = 0
            End Get
        End Property

        <XmlElement("videosource")>
        Public Property VideoSource() As String = String.Empty

        <XmlIgnore()>
        Public ReadOnly Property VideoSourceSpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(VideoSource)
            End Get
        End Property

        <XmlElement("votes")>
        Public Property Votes() As String
            Get
                Dim nRating = Ratings.GetDefaultRating()
                If nRating IsNot Nothing AndAlso nRating.VotesSpecified Then
                    Return nRating.Votes.ToString
                Else
                    Return String.Empty
                End If
            End Get
            Set(value As String)
                Dim iVotes As Integer
                If Integer.TryParse(Regex.Replace(value, "\D", String.Empty), iVotes) Then
                    _Votes = iVotes
                    If Not _Rating = 0 Then Ratings.Add(New RatingDetails With {
                                                        .IsDefault = True,
                                                        .Max = If(_Rating <= 10, 10, 100),
                                                        .Type = "default",
                                                        .Value = _Rating,
                                                        .Votes = _Votes
                                                        })
                Else
                    _Votes = 0
                End If
            End Set
        End Property

        <Obsolete()>
        <XmlIgnore()>
        Public ReadOnly Property VotesSpecified() As Boolean
            Get
                Select Case _ContentType
                    Case Enums.ContentType.Movie
                        Return Not String.IsNullOrEmpty(Votes) AndAlso Not String.IsNullOrEmpty(Rating) AndAlso Master.eSettings.Movie.InformationSettings.Ratings.CreateNodes
                    Case Enums.ContentType.TVEpisode, Enums.ContentType.TVSeason, Enums.ContentType.TVShow
                        Return Not String.IsNullOrEmpty(Votes) AndAlso Not String.IsNullOrEmpty(Rating) AndAlso Master.eSettings.TVShow.InformationSettings.Ratings.CreateNodes
                End Select
                Return False
            End Get
        End Property

        <XmlElement("year")>
        Public Property Year() As String
            Get
                Dim nDate As New Date
                If PremieredSpecified AndAlso Date.TryParse(Premiered, nDate) Then
                    Return nDate.Year.ToString
                End If
                Return _Year
            End Get
            Set(value As String)
                If Not PremieredSpecified Then
                    _Year = value
                End If
            End Set
        End Property

        <XmlIgnore()>
        Public ReadOnly Property YearSpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(Year)
            End Get
        End Property

#End Region 'Properties

#Region "Methods"

        Public Sub AddCertificationsFromString(ByVal value As String)
            _Certifications.Clear()
            If String.IsNullOrEmpty(value) Then Return

            If value.Contains(" / ") Then
                Dim values As String() = Regex.Split(value, " / ")
                For Each certification As String In values
                    certification = certification.Trim
                    If Not _Certifications.Contains(certification) Then
                        _Certifications.Add(certification)
                    End If
                Next
            Else
                If Not _Certifications.Contains(value) Then
                    _Certifications.Add(value.Trim)
                End If
            End If
        End Sub

        Public Sub AddTag(ByVal value As String)
            If String.IsNullOrEmpty(value) Then Return
            If Not _Tags.Contains(value) Then
                _Tags.Add(value.Trim)
            End If
        End Sub

        Public Function CloneDeep() As Object Implements ICloneable.Clone
            Throw New NotImplementedException("Binaryformatter is obsolete, the Deepclone method needs fixing")
            Dim Stream As New MemoryStream(50000)
            'Dim Formatter As New Runtime.Serialization.Formatters.Binary.BinaryFormatter()
            ' Serialisierung über alle Objekte hinweg in einen Stream 
            'Formatter.Serialize(Stream, Me)
            ' Zurück zum Anfang des Streams und... 
            'Stream.Seek(0, SeekOrigin.Begin)
            ' ...aus dem Stream in ein Objekt deserialisieren 
            'CloneDeep = Formatter.Deserialize(Stream)
            Stream.Close()
        End Function

        Public Sub CreateCachePaths_ActorsThumbs()
            Dim sPath As String = Path.Combine(Master.TempPath, "Global")

            For Each tActor As RoleLink In Actors
                Master.Thumbs(tActor.PersonId).CacheOriginalPath = Path.Combine(sPath, String.Concat("actorthumbs", Path.DirectorySeparatorChar, Path.GetFileName(tActor.Person.URLOriginal)))
                If Not String.IsNullOrEmpty(Master.Thumbs(tActor.PersonId).URLThumb) Then
                    Master.Thumbs(tActor.PersonId).CacheThumbPath = Path.Combine(sPath, String.Concat("actorthumbs\_thumbs", Path.DirectorySeparatorChar, Path.GetFileName(tActor.Person.URLOriginal)))
                End If
            Next
        End Sub

        Public Function CompareTo(ByVal other As MainDetails) As Integer Implements IComparable(Of MainDetails).CompareTo
            Dim retVal As Integer = (Lev).CompareTo(other.Lev)
            If retVal = 0 Then
                retVal = (Year).CompareTo(other.Year) * -1
            End If
            Return retVal
        End Function

        Public Sub SaveAllActorThumbs(ByRef DBElement As Database.DBElement)
            If ActorsSpecified AndAlso Master.eSettings.MovieActorthumbsAnyEnabled Then
                Images.Save_Actorthumbs(DBElement)
            Else
                Images.Delete(DBElement, Enums.ModifierType.MainActorThumbs, False)
                DBElement.ActorThumbs.Clear()
            End If
        End Sub

#End Region 'Methods

    End Class

    <Serializable()>
    Public Class MediaFile

#Region "Properties"

        Public Property Duration As String = String.Empty

        Public Property FileOriginal As MediaFiles = New MediaFiles

        Public ReadOnly Property HasVariantWithVideoResolution(ByVal resolution As Enums.VideoResolution) As Boolean
            Get
                Return Streams.Variants.Where(Function(f) f.VideoResolution = resolution).Count > 0
            End Get
        End Property
        ''' <summary>
        ''' If is a Dash video, we need also an audio URL to merge video and audio
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property IsAdaptive() As Boolean
            Get
                Return UrlVideoStreamSpecified AndAlso UrlAudioStreamSpecified
            End Get
        End Property

        Public Property Language As String = String.Empty

        Public Property LocalFilePath As String = String.Empty

        Public ReadOnly Property LocalFilePathSpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(LocalFilePath)
            End Get
        End Property

        Public Property LongLanguage As String = String.Empty

        Public Property Scraper As String = String.Empty

        Public Property Source As String = String.Empty

        Public Property Streams As StreamCollection = New StreamCollection

        Public ReadOnly Property StreamsSpecified() As Boolean
            Get
                Return Streams.HasStreams
            End Get
        End Property

        Public Property Title As String = String.Empty
        ''' <summary>
        ''' URL that has to be used for NFO trailer links
        ''' </summary>
        ''' <returns></returns>
        Public Property UrlForNfo As String = String.Empty

        Public ReadOnly Property UrlForNfoSpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(UrlForNfo)
            End Get
        End Property
        ''' <summary>
        ''' Download URL of the selected audio stream
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property UrlAudioStream As String = String.Empty

        Public ReadOnly Property UrlAudioStreamSpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(UrlAudioStream)
            End Get
        End Property
        ''' <summary>
        ''' Download URL of the selected video stream
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property UrlVideoStream As String = String.Empty

        Public ReadOnly Property UrlVideoStreamSpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(UrlVideoStream)
            End Get
        End Property
        ''' <summary>
        ''' Website URL of the media file for preview in browser
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property UrlWebsite As String = String.Empty

        Public ReadOnly Property UrlWebsiteSpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(UrlWebsite)
            End Get
        End Property

        Public Property VideoType As Enums.VideoType = Enums.VideoType.Any

#End Region 'Properties

#Region "Methods"

        Public Function LoadAndCache() As Boolean
            If Not FileOriginal.HasMemoryStream Then
                If File.Exists(LocalFilePath) Then
                    FileOriginal.LoadFromFile(LocalFilePath)
                Else
                    FileOriginal.LoadFromWeb(Me)
                End If
            End If

            If FileOriginal.HasMemoryStream Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Sub Save(ByRef dbElement As Database.DBElement, ByVal type As Enums.ModifierType, ByVal forceFileCleanup As Boolean)
            Dim tContentType As Enums.ContentType = dbElement.ContentType

            With dbElement
                Select Case tContentType
                    Case Enums.ContentType.Movie
                        Select Case type
                            Case Enums.ModifierType.MainTheme
                                If .Theme.LoadAndCache() Then
                                    MediaFiles.Delete_Movie(dbElement, Enums.ModifierType.MainTheme, forceFileCleanup)
                                    .Theme.LocalFilePath = .Theme.FileOriginal.Save_Movie(dbElement, Enums.ModifierType.MainTheme)
                                Else
                                    MediaFiles.Delete_Movie(dbElement, Enums.ModifierType.MainTheme, forceFileCleanup)
                                    .Theme = New MediaFile
                                End If

                            Case Enums.ModifierType.MainTrailer
                                If .Trailer.LoadAndCache() Then
                                    MediaFiles.Delete_Movie(dbElement, Enums.ModifierType.MainTrailer, forceFileCleanup)
                                    .Trailer.LocalFilePath = .Trailer.FileOriginal.Save_Movie(dbElement, Enums.ModifierType.MainTrailer)
                                Else
                                    MediaFiles.Delete_Movie(dbElement, Enums.ModifierType.MainTrailer, forceFileCleanup)
                                    .Trailer = New MediaFile
                                End If
                        End Select
                    Case Enums.ContentType.TVShow
                        Select Case type
                            Case Enums.ModifierType.MainTheme
                                If .Theme.LoadAndCache() Then
                                    MediaFiles.Delete_TVShow(dbElement, Enums.ModifierType.MainTheme) ', ForceFileCleanup)
                                    .Theme.LocalFilePath = .Theme.FileOriginal.Save_TVShow(dbElement, Enums.ModifierType.MainTheme)
                                Else
                                    MediaFiles.Delete_TVShow(dbElement, Enums.ModifierType.MainTheme) ', ForceFileCleanup)
                                    .Theme = New MediaFile
                                End If
                        End Select
                End Select
            End With
        End Sub

        Public Function SetFirstVariantWithVideoStreamResolution(ByVal resolution As Enums.VideoResolution) As Boolean
            Dim nVariant = Streams.Variants.FirstOrDefault(Function(f) f.VideoResolution = resolution)
            If nVariant IsNot Nothing Then
                SetVariant(nVariant)
                Return True
            End If
            Return False
        End Function

        Public Sub SetVariant(ByRef streamVariant As StreamCollection.StreamVariant)
            If streamVariant IsNot Nothing Then
                With streamVariant
                    UrlAudioStream = If(.AudioStream IsNot Nothing, .AudioStream.StreamUrl, String.Empty)
                    UrlVideoStream = If(.VideoStream IsNot Nothing, .VideoStream.StreamUrl, String.Empty)
                    If Not String.IsNullOrEmpty(.UrlForNfo) Then UrlForNfo = .UrlForNfo
                End With
            End If
        End Sub

#End Region 'Methods

#Region "Nested Types"

        <Serializable()>
        Public Class AudioStream
            Implements IComparable(Of AudioStream)

#Region "Fields"

            Private _url As String = String.Empty

#End Region 'Fields

#Region "Properties"

            Public Property Bitrate As Enums.AudioBitrate = Enums.AudioBitrate.UNKNOWN

            Public Property Codec As Enums.AudioCodec = Enums.AudioCodec.UNKNOWN

            Public ReadOnly Property Description() As String
                Get
                    Return String.Format("{0} ({1})", BitrateToString(Bitrate), CodecToString(Codec))
                End Get
            End Property

            Public Property FileExtension As String = String.Empty

            Public Property StreamUrl() As String
                Get
                    If YouTubeContainer IsNot Nothing AndAlso Not String.IsNullOrEmpty(YouTubeContainer.Uri) Then
                        Return YouTubeContainer.Uri
                    Else
                        Return _url
                    End If
                End Get
                Set(value As String)
                    _url = value
                End Set
            End Property

            Public Property YouTubeContainer As VideoLibrary.YouTubeVideo = Nothing

#End Region 'Properties

#Region "Methods"

            Public Shared Function BitrateToString(ByVal audioBitrate As Enums.AudioBitrate) As String
                Select Case audioBitrate
                    Case Enums.AudioBitrate.UNKNOWN
                        Return "?"
                    Case Else
                        Return [Enum].GetName(GetType(Enums.AudioBitrate), audioBitrate).Remove(0, 1).Replace("kbps", " kbit/s")
                End Select
            End Function

            Public Shared Function CodecToString(ByVal audioCodec As Enums.AudioCodec) As String
                Select Case audioCodec
                    Case Enums.AudioCodec.UNKNOWN
                        Return "?"
                    Case Enums.AudioCodec.AAC_SPATIAL
                        Return "AAC 6Ch"
                    Case Enums.AudioCodec.AC3_SPATIAL
                        Return "AC-3 6Ch"
                    Case Enums.AudioCodec.DTSE_SPATIAL
                        Return "DTSE 6Ch"
                    Case Enums.AudioCodec.EC3_SPATIAL
                        Return "EC-3 6Ch"
                    Case Enums.AudioCodec.Opus_SPATIAL
                        Return "Opus 6Ch"
                    Case Enums.AudioCodec.Vorbis_SPATIAL
                        Return "Vorbis 4Ch"
                    Case Else
                        Return [Enum].GetName(GetType(Enums.AudioCodec), audioCodec)
                End Select
            End Function

            Public Function CompareTo(ByVal other As AudioStream) As Integer Implements IComparable(Of AudioStream).CompareTo
                Return (Bitrate).CompareTo(other.Bitrate)
            End Function

#End Region 'Methods

        End Class

        <Serializable()>
        Public Class StreamCollection

#Region "Properties"

            Public Property AudioStreams As New List(Of AudioStream)

            Public ReadOnly Property HasStreams() As Boolean
                Get
                    Return AudioStreams IsNot Nothing AndAlso AudioStreams.Count > 0 OrElse VideoStreams IsNot Nothing AndAlso VideoStreams.Count > 0
                End Get
            End Property

            Public Property Variants As New List(Of StreamVariant)

            Public Property VideoStreams As New List(Of VideoStream)

#End Region 'Properties

#Region "Methods"

            Public Sub BuildStreamVariants(Optional ByVal audioStreamsOnly As Boolean = False)
                For Each vStream In VideoStreams
                    If vStream.IsAdaptive Then
                        For Each aStream In AudioStreams
                            Variants.Add(BuildVariant(vStream, aStream))
                        Next
                    Else
                        Variants.Add(BuildVariant(vStream, Nothing))
                    End If
                Next
                For Each aStream In AudioStreams
                    Variants.Add(BuildVariant(Nothing, aStream))
                Next
            End Sub

            Private Shared Function BuildVariant(ByRef videoStream As VideoStream, ByRef audioStream As AudioStream) As StreamVariant
                Dim nVariant As New StreamVariant With {.StreamType = If(videoStream IsNot Nothing, StreamType.Video, StreamType.Audio)
                }
                'Build description and set addition information
                Dim nDescription As String = String.Empty
                If videoStream IsNot Nothing Then
                    nDescription = String.Format("{0}", videoStream.Description)
                    nVariant.VideoCodec = videoStream.Codec
                    nVariant.VideoResolution = videoStream.Resolution
                    nVariant.VideoStream = videoStream
                End If
                If audioStream IsNot Nothing Then
                    'use audio description from audioStream
                    nDescription = String.Format("{0} {1} {2}",
                                                 nDescription,
                                                 If(videoStream IsNot Nothing, "|", String.Empty),
                                                 audioStream.Description
                                                 )
                    nVariant.AudioBitrate = audioStream.Bitrate
                    nVariant.AudioCodec = audioStream.Codec
                    nVariant.AudioStream = audioStream
                Else
                    'use audio description from videoStream
                    nDescription = String.Format("{0} {1} {2}",
                                                 nDescription,
                                                 "|",
                                                 videoStream.AudioDescription
                                                 )
                    nVariant.AudioBitrate = videoStream.AudioBitrate
                    nVariant.AudioCodec = videoStream.AudioCodec
                End If

                If videoStream IsNot Nothing AndAlso audioStream IsNot Nothing Then
                    nDescription = String.Format("{0} | .mkv", nDescription)
                ElseIf videoStream IsNot Nothing AndAlso Not String.IsNullOrEmpty(videoStream.FileExtension) Then
                    nDescription = String.Format("{0} | {1}", nDescription, videoStream.FileExtension)
                ElseIf audioStream IsNot Nothing AndAlso Not String.IsNullOrEmpty(audioStream.FileExtension) Then
                    nDescription = String.Format("{0} | {1}", nDescription, audioStream.FileExtension)
                End If
                nVariant.Description = nDescription.Trim

                'set UrlForNfo
                If videoStream IsNot Nothing AndAlso audioStream IsNot Nothing Then
                    'video is adpative and can't be stored as single URL because the video and audio streams are separated
                ElseIf videoStream IsNot Nothing Then
                    'set video stream URL as UrlForNfo
                    nVariant.UrlForNfo = videoStream.StreamUrl
                ElseIf audioStream IsNot Nothing Then
                    'set audio stream URL as UrlForNfo
                    nVariant.UrlForNfo = audioStream.StreamUrl
                End If

                Return nVariant
            End Function

#End Region 'Methods

#Region "Nested Types"

            <Serializable()>
            Public Class StreamVariant

#Region "Properties"

                Public Property AudioBitrate As Enums.AudioBitrate = Enums.AudioBitrate.UNKNOWN

                Public Property AudioCodec As Enums.AudioCodec = Enums.AudioCodec.UNKNOWN

                Public Property AudioStream As AudioStream = Nothing

                Public Property Description As String = String.Empty

                Public ReadOnly Property IsAdaptive As Boolean
                    Get
                        Return AudioStream IsNot Nothing AndAlso
                            VideoStream IsNot Nothing
                    End Get
                End Property

                Public Property StreamType As StreamType = StreamType.Unknown

                Public Property UrlForNfo As String = String.Empty

                Public Property VideoCodec As Enums.VideoCodec = Enums.VideoCodec.UNKNOWN

                Public Property VideoResolution As Enums.VideoResolution = Enums.VideoResolution.UNKNOWN

                Public Property VideoStream As VideoStream = Nothing

#End Region 'Properties

            End Class

            Public Enum StreamType As Integer
                Audio
                Unknown
                Video
            End Enum

#End Region 'Nested Types

        End Class

        <Serializable()>
        Public Class VideoStream
            Implements IComparable(Of VideoStream)

#Region "Fields"

            Private _url As String = String.Empty

#End Region 'Fields

#Region "Properties"

            Public Property AudioBitrate As Enums.AudioBitrate = Enums.AudioBitrate.UNKNOWN

            Public Property AudioCodec As Enums.AudioCodec = Enums.AudioCodec.UNKNOWN

            Public ReadOnly Property AudioDescription() As String
                Get
                    Return String.Format("{0} ({1})", AudioStream.BitrateToString(AudioBitrate), AudioStream.CodecToString(AudioCodec))
                End Get
            End Property

            Public Property Codec As Enums.VideoCodec = Enums.VideoCodec.UNKNOWN

            Public ReadOnly Property Description() As String
                Get
                    Return String.Format("{0} ({1})", ResolutionToString(Resolution), CodecToString(Codec))
                End Get
            End Property

            Public Property FileExtension As String = String.Empty

            Public Property IsAdaptive As Boolean = False

            Public Property Resolution As Enums.VideoResolution = Enums.VideoResolution.UNKNOWN

            Public Property StreamUrl() As String
                Get
                    If YouTubeContainer IsNot Nothing AndAlso Not String.IsNullOrEmpty(YouTubeContainer.Uri) Then
                        Return YouTubeContainer.Uri
                    Else
                        Return _url
                    End If
                End Get
                Set(value As String)
                    _url = value
                End Set
            End Property

            Public Property YouTubeContainer As VideoLibrary.YouTubeVideo = Nothing

#End Region 'Properties

#Region "Methods"

            Private Shared Function CodecToString(ByVal videoCodec As Enums.VideoCodec) As String
                Select Case videoCodec
                    Case Enums.VideoCodec.UNKNOWN
                        Return "?"
                    Case Enums.VideoCodec.VP9_HDR
                        Return "VP9, HDR"
                    Case Else
                        Return [Enum].GetName(GetType(Enums.VideoCodec), videoCodec)
                End Select
            End Function

            Public Function CompareTo(ByVal other As VideoStream) As Integer Implements IComparable(Of VideoStream).CompareTo
                Return (Resolution).CompareTo(other.Resolution)
            End Function

            Private Shared Function ResolutionToString(ByVal videoQuality As Enums.VideoResolution) As String
                Select Case videoQuality
                    Case Enums.VideoResolution.Any, Enums.VideoResolution.UNKNOWN
                        Return "?"
                    Case Enums.VideoResolution.HD1080p60fps
                        Return "1080p, 60fps"
                    Case Enums.VideoResolution.HD2160p60fps
                        Return "2160, 60fps"
                    Case Enums.VideoResolution.HD720p60fps
                        Return "720p, 60fps"
                    Case Enums.VideoResolution.SQ144p15fps
                        Return "144p, 15fps"
                    Case Else
                        Return [Enum].GetName(GetType(Enums.VideoResolution), videoQuality).Remove(0, 2)
                End Select
            End Function

#End Region 'Methods

        End Class

#End Region 'Nested Types

    End Class

    <Serializable()>
    <XmlRoot("movie")>
    Public Class Movie_old
        'Implements ICloneable
        'Implements IComparable(Of Movie_old)

        '#Region "Fields"

        '        Shared _Logger As Logger = LogManager.GetCurrentClassLogger()

        '        Private _ContentType As Enums.ContentType = Enums.ContentType.Movie
        '        Private _Certifications As New List(Of String)
        '        Private _LastPlayed As String = String.Empty
        '        Private _Rating As Double = 0
        '        Private _Tags As New List(Of String)
        '        Private _Votes As Integer = 0
        '        Private _Year As String = String.Empty

        '#End Region 'Fields

        '#Region "Properties"

        '        <XmlElement("id")>
        '        Public Property DefaultId() As DefaultId
        '            Get
        '                Return UniqueIDs.GetDefaultId()
        '            End Get
        '            Set(value As DefaultId)
        '                UniqueIDs.Add(value)
        '            End Set
        '        End Property

        '        <XmlIgnore()>
        '        Public ReadOnly Property DefaultIdSpecified() As Boolean
        '            Get
        '                Return DefaultId.ValueSpecified AndAlso Master.eSettings.MovieScraperIdWriteNodeDefaultId
        '            End Get
        '        End Property

        '        <XmlElement("tmdb")>
        '        Public Property TMDbId() As String
        '            Get
        '                Return UniqueIDs.TMDbId.ToString
        '            End Get
        '            Set(value As String)
        '                UniqueIDs.Add("tmdb", value)
        '            End Set
        '        End Property

        '        <Obsolete()>
        '        <XmlIgnore()>
        '        Public ReadOnly Property TMDbIdSpecified() As Boolean
        '            Get
        '                Return UniqueIDs.TMDbIdSpecified AndAlso Master.eSettings.MovieScraperIdWriteNodeTMDbId
        '            End Get
        '        End Property

        '        <XmlElement("tmdbcolid")>
        '        Public Property TMDbCollectionId() As String
        '            Get
        '                Return UniqueIDs.TMDbCollectionId.ToString
        '            End Get
        '            Set(value As String)
        '                UniqueIDs.Add("tmdbcol", value)
        '            End Set
        '        End Property

        '        <Obsolete()>
        '        <XmlIgnore()>
        '        Public ReadOnly Property TMDbCollectionIdSpecified() As Boolean
        '            Get
        '                Return UniqueIDs.TMDbCollectionIdSpecified AndAlso Master.eSettings.MovieScraperIdWriteNodeTMDbCollectionId
        '            End Get
        '        End Property

        '        <XmlIgnore()>
        '        Public Property UniqueIDs() As New UniqueidContainer(_ContentType)

        '        <XmlIgnore()>
        '        Public ReadOnly Property UniqueIDsSpecified() As Boolean
        '            Get
        '                Return UniqueIDs.Items.Count > 0
        '            End Get
        '        End Property

        '        <XmlElement("uniqueid")>
        '        Public Property UniqueIDs_Kodi() As Uniqueid()
        '            Get
        '                Return UniqueIDs.Items.ToArray
        '            End Get
        '            Set(value As Uniqueid())
        '                If value IsNot Nothing Then UniqueIDs.AddRange(value.ToList)
        '            End Set
        '        End Property

        '        <XmlElement("title")>
        '        Public Property Title() As String = String.Empty

        '        <XmlIgnore()>
        '        Public ReadOnly Property TitleSpecified() As Boolean
        '            Get
        '                Return Not String.IsNullOrEmpty(Title)
        '            End Get
        '        End Property

        '        <XmlElement("originaltitle")>
        '        Public Property OriginalTitle() As String = String.Empty

        '        <XmlIgnore()>
        '        Public ReadOnly Property OriginalTitleSpecified() As Boolean
        '            Get
        '                Return Not String.IsNullOrEmpty(OriginalTitle)
        '            End Get
        '        End Property

        '        <XmlElement("sorttitle")>
        '        Public Property SortTitle() As String = String.Empty

        '        <XmlIgnore()>
        '        Public ReadOnly Property SortTitleSpecified() As Boolean
        '            Get
        '                Return Not String.IsNullOrEmpty(SortTitle)
        '            End Get
        '        End Property

        '        <XmlElement("edition")>
        '        Public Property Edition() As String = String.Empty

        '        <XmlIgnore()>
        '        Public ReadOnly Property EditionSpecified() As Boolean
        '            Get
        '                Return Not String.IsNullOrEmpty(Edition)
        '            End Get
        '        End Property

        '        <XmlElement("tagline")>
        '        Public Property Tagline() As String = String.Empty

        '        <XmlIgnore()>
        '        Public ReadOnly Property TaglineSpecified() As Boolean
        '            Get
        '                Return Not String.IsNullOrEmpty(Tagline)
        '            End Get
        '        End Property

        '        <XmlElement("language")>
        '        Public Property Language() As String = String.Empty

        '        <XmlIgnore()>
        '        Public ReadOnly Property LanguageSpecified() As Boolean
        '            Get
        '                Return Not String.IsNullOrEmpty(Language)
        '            End Get
        '        End Property

        '        <XmlElement("year")>
        '        Public Property Year() As String
        '            Get
        '                Dim nDate As New Date
        '                If PremieredSpecified AndAlso Date.TryParse(Premiered, nDate) Then
        '                    Return nDate.Year.ToString
        '                End If
        '                Return _Year
        '            End Get
        '            Set(value As String)
        '                If Not PremieredSpecified Then
        '                    _Year = value
        '                End If
        '            End Set
        '        End Property

        '        <XmlIgnore()>
        '        Public ReadOnly Property YearSpecified() As Boolean
        '            Get
        '                Return Not String.IsNullOrEmpty(Year)
        '            End Get
        '        End Property
        '        ''' <summary>
        '        ''' use Premiered instead !
        '        ''' </summary>
        '        ''' <returns></returns>
        '        <XmlElement("releasedate")>
        '        Public Property ReleaseDate() As String
        '            Get
        '                Return Premiered
        '            End Get
        '            Set(value As String)
        '                Premiered = value
        '            End Set
        '        End Property

        '        <Obsolete()>
        '        <XmlIgnore()>
        '        Public ReadOnly Property ReleaseDateSpecified() As Boolean
        '            Get
        '                Return Not String.IsNullOrEmpty(ReleaseDate) AndAlso Master.eSettings.MovieScraperReleaseDateWriteNode
        '            End Get
        '        End Property

        '        <XmlElement("premiered")>
        '        Public Property Premiered() As String = String.Empty

        '        <XmlIgnore()>
        '        Public ReadOnly Property PremieredSpecified() As Boolean
        '            Get
        '                Return Not String.IsNullOrEmpty(Premiered)
        '            End Get
        '        End Property

        '        <XmlElement("top250")>
        '        Public Property Top250() As Integer = 0

        '        <XmlIgnore()>
        '        Public ReadOnly Property Top250Specified() As Boolean
        '            Get
        '                Return Not Top250 = 0
        '            End Get
        '        End Property

        '        <XmlElement("country")>
        '        Public Property Countries() As New List(Of String)

        '        <XmlIgnore()>
        '        Public ReadOnly Property CountriesSpecified() As Boolean
        '            Get
        '                Return Countries.Count > 0
        '            End Get
        '        End Property

        '        <XmlElement("rating")>
        '        Public Property Rating() As String
        '            Get
        '                Dim nRating = Ratings.GetDefaultRating()
        '                If nRating IsNot Nothing Then
        '                    Return nRating.ValueNormalized.ToString
        '                Else
        '                    Return String.Empty
        '                End If
        '            End Get
        '            Set(ByVal value As String)
        '                Dim dblRatings As Double
        '                If Double.TryParse(value.Replace(",", "."),
        '                                   Globalization.NumberStyles.AllowDecimalPoint,
        '                                   Globalization.CultureInfo.InvariantCulture,
        '                                   dblRatings
        '                                   ) Then
        '                    _Rating = dblRatings
        '                    If Not _Votes = 0 Then Ratings.Add(New RatingDetails With {
        '                                                       .IsDefault = True,
        '                                                       .Max = If(_Rating <= 10, 10, 100),
        '                                                       .Type = "default",
        '                                                       .Value = _Rating,
        '                                                       .Votes = _Votes
        '                                                       })
        '                Else
        '                    _Rating = 0
        '                End If
        '            End Set
        '        End Property

        '        <Obsolete()>
        '        <XmlIgnore()>
        '        Public ReadOnly Property RatingSpecified() As Boolean
        '            Get
        '                Return Not String.IsNullOrEmpty(Rating) AndAlso Not String.IsNullOrEmpty(Votes) AndAlso Master.eSettings.MovieScraperRatingVotesWriteNode
        '            End Get
        '        End Property

        '        <XmlElement("votes")>
        '        Public Property Votes() As String
        '            Get
        '                Dim nRating = Ratings.GetDefaultRating()
        '                If nRating IsNot Nothing AndAlso nRating.VotesSpecified Then
        '                    Return nRating.Votes.ToString
        '                Else
        '                    Return String.Empty
        '                End If
        '            End Get
        '            Set(value As String)
        '                Dim iVotes As Integer
        '                If Integer.TryParse(Regex.Replace(value, "\D", String.Empty), iVotes) Then
        '                    _Votes = iVotes
        '                    If Not _Rating = 0 Then Ratings.Add(New RatingDetails With {
        '                                                        .IsDefault = True,
        '                                                        .Max = If(_Rating <= 10, 10, 100),
        '                                                        .Type = "default",
        '                                                        .Value = _Rating,
        '                                                        .Votes = _Votes
        '                                                        })
        '                Else
        '                    _Votes = 0
        '                End If
        '            End Set
        '        End Property

        '        <Obsolete()>
        '        <XmlIgnore()>
        '        Public ReadOnly Property VotesSpecified() As Boolean
        '            Get
        '                Return Not String.IsNullOrEmpty(Votes) AndAlso Not String.IsNullOrEmpty(Rating) AndAlso Master.eSettings.MovieScraperRatingVotesWriteNode
        '            End Get
        '        End Property

        '        <XmlIgnore()>
        '        Public Property Ratings() As New RatingContainer(_ContentType)

        '        <XmlIgnore()>
        '        Public ReadOnly Property RatingsSpecified() As Boolean
        '            Get
        '                Return Ratings.AnyRatingSpecified
        '            End Get
        '        End Property

        '        <XmlArray("ratings")>
        '        <XmlArrayItem("rating")>
        '        Public Property Ratings_Kodi() As RatingDetails()
        '            Get
        '                Return Ratings.Items.ToArray
        '            End Get
        '            Set(value As RatingDetails())
        '                Ratings.AddRange(value.ToList)
        '            End Set
        '        End Property

        '        <XmlElement("userrating")>
        '        Public Property UserRating() As Integer = 0

        '        <XmlIgnore()>
        '        Public ReadOnly Property UserRatingSpecified() As Boolean
        '            Get
        '                Return Not UserRating = 0
        '            End Get
        '        End Property

        '        <XmlElement("mpaa")>
        '        Public Property MPAA() As String = String.Empty

        '        <XmlIgnore()>
        '        Public ReadOnly Property MPAASpecified() As Boolean
        '            Get
        '                Return Not String.IsNullOrEmpty(MPAA)
        '            End Get
        '        End Property

        '        <XmlElement("certification")>
        '        Public Property Certifications() As List(Of String)
        '            Get
        '                Return _Certifications
        '            End Get
        '            Set(ByVal value As List(Of String))
        '                If value Is Nothing Then
        '                    _Certifications.Clear()
        '                Else
        '                    _Certifications = value
        '                End If
        '            End Set
        '        End Property

        '        <XmlIgnore()>
        '        Public ReadOnly Property CertificationsSpecified() As Boolean
        '            Get
        '                Return Certifications.Count > 0
        '            End Get
        '        End Property

        '        <XmlElement("tag")>
        '        Public Property Tags() As List(Of String)
        '            Get
        '                Return _Tags
        '            End Get
        '            Set(ByVal value As List(Of String))
        '                If value Is Nothing Then
        '                    _Tags.Clear()
        '                Else
        '                    _Tags = value
        '                End If
        '            End Set
        '        End Property

        '        <XmlIgnore()>
        '        Public ReadOnly Property TagsSpecified() As Boolean
        '            Get
        '                Return Tags.Count > 0
        '            End Get
        '        End Property

        '        <XmlElement("genre")>
        '        Public Property Genres() As New List(Of String)

        '        <XmlIgnore()>
        '        Public ReadOnly Property GenresSpecified() As Boolean
        '            Get
        '                Return Genres.Count > 0
        '            End Get
        '        End Property

        '        <XmlElement("studio")>
        '        Public Property Studios() As New List(Of String)

        '        <XmlIgnore()>
        '        Public ReadOnly Property StudiosSpecified() As Boolean
        '            Get
        '                Return Studios.Count > 0
        '            End Get
        '        End Property

        '        <XmlElement("director")>
        '        Public Property Directors() As New List(Of String)

        '        <XmlIgnore()>
        '        Public ReadOnly Property DirectorsSpecified() As Boolean
        '            Get
        '                Return Directors.Count > 0
        '            End Get
        '        End Property

        '        <XmlElement("credits")>
        '        Public Property Credits() As New List(Of String)

        '        <XmlIgnore()>
        '        Public ReadOnly Property CreditsSpecified() As Boolean
        '            Get
        '                Return Credits.Count > 0
        '            End Get
        '        End Property

        '        <XmlIgnore()>
        '        Public Property Scrapersource() As String = String.Empty

        '        <XmlIgnore()>
        '        Public ReadOnly Property ScraperSourceSpecified As Boolean
        '            Get
        '                Return Not String.IsNullOrEmpty(Scrapersource)
        '            End Get
        '        End Property

        '        <XmlElement("outline")>
        '        Public Property Outline() As String = String.Empty

        '        <XmlIgnore()>
        '        Public ReadOnly Property OutlineSpecified() As Boolean
        '            Get
        '                Return Not String.IsNullOrEmpty(Outline)
        '            End Get
        '        End Property

        '        <XmlElement("plot")>
        '        Public Property Plot() As String = String.Empty

        '        <XmlIgnore()>
        '        Public ReadOnly Property PlotSpecified() As Boolean
        '            Get
        '                Return Not String.IsNullOrEmpty(Plot)
        '            End Get
        '        End Property

        '        <XmlElement("runtime")>
        '        Public Property Runtime() As String = String.Empty

        '        <XmlIgnore()>
        '        Public ReadOnly Property RuntimeSpecified() As Boolean
        '            Get
        '                Return Not String.IsNullOrEmpty(Runtime) AndAlso Not Runtime = "0"
        '            End Get
        '        End Property

        '        <XmlElement("trailer")>
        '        Public Property Trailer() As String = String.Empty

        '        <XmlIgnore()>
        '        Public ReadOnly Property TrailerSpecified() As Boolean
        '            Get
        '                Return Not String.IsNullOrEmpty(Trailer)
        '            End Get
        '        End Property

        '        <XmlElement("playcount")>
        '        Public Property PlayCount() As Integer = 0

        '        <XmlIgnore()>
        '        Public ReadOnly Property PlayCountSpecified() As Boolean
        '            Get
        '                Return PlayCount > 0
        '            End Get
        '        End Property

        '        <XmlElement("lastplayed")>
        '        Public Property LastPlayed() As String
        '            Get
        '                Return _LastPlayed
        '            End Get
        '            Set(ByVal value As String)
        '                _LastPlayed = Functions.ConvertToProperDateTime(value)
        '            End Set
        '        End Property

        '        <XmlIgnore()>
        '        Public ReadOnly Property LastPlayedSpecified() As Boolean
        '            Get
        '                Return Not String.IsNullOrEmpty(LastPlayed)
        '            End Get
        '        End Property

        '        <XmlElement("dateadded")>
        '        Public Property DateAdded() As String = String.Empty

        '        <XmlIgnore()>
        '        Public ReadOnly Property DateAddedSpecified() As Boolean
        '            Get
        '                Return Not String.IsNullOrEmpty(DateAdded)
        '            End Get
        '        End Property

        '        <XmlElement("datemodified")>
        '        Public Property DateModified() As String = String.Empty

        '        <XmlIgnore()>
        '        Public ReadOnly Property DateModifiedSpecified() As Boolean
        '            Get
        '                Return Not String.IsNullOrEmpty(DateModified)
        '            End Get
        '        End Property

        '        <XmlElement("actor")>
        '        Public Property Actors() As New List(Of Person)

        '        <XmlIgnore()>
        '        Public ReadOnly Property ActorsSpecified() As Boolean
        '            Get
        '                Return Actors.Count > 0
        '            End Get
        '        End Property

        '        <XmlElement("thumb")>
        '        Public Property Thumb() As New List(Of String)

        '        <XmlIgnore()>
        '        Public ReadOnly Property ThumbSpecified() As Boolean
        '            Get
        '                Return Thumb.Count > 0
        '            End Get
        '        End Property
        '        ''' <summary>
        '        ''' Poster Thumb for preview in search results
        '        ''' </summary>
        '        ''' <value></value>
        '        ''' <returns></returns>
        '        ''' <remarks></remarks>
        '        <XmlIgnore()>
        '        Public Property ThumbPoster() As New Image

        '        <XmlElement("fanart")>
        '        Public Property Fanart() As New Fanart

        '        <XmlIgnore()>
        '        Public ReadOnly Property FanartSpecified() As Boolean
        '            Get
        '                Return Not String.IsNullOrEmpty(Fanart.URL)
        '            End Get
        '        End Property

        '        <XmlIgnore()>
        '        Public Property Sets() As New MoviesetContainer

        '        <XmlIgnore()>
        '        Public ReadOnly Property SetsSpecified() As Boolean
        '            Get
        '                Return Sets.Items.Count > 0
        '            End Get
        '        End Property

        '        <XmlAnyElement("set")>
        '        Public Property Set_Kodi() As Object
        '            Get
        '                Return Sets.Create_SetNode_Kodi()
        '            End Get
        '            Set(ByVal value As Object)
        '                Sets.Read_SetNode_Kodi(value)
        '            End Set
        '        End Property

        '        <XmlArray("sets")>
        '        <XmlArrayItem("set")>
        '        Public Property Sets_YAMJ() As MoviesetContainer.MoviesetDetails_YAMJ()
        '            Get
        '                Return Sets.Create_SetNode_YAMJ.ToArray
        '            End Get
        '            Set(ByVal value As MoviesetContainer.MoviesetDetails_YAMJ())
        '                Sets.AddRange(value.ToList)
        '            End Set
        '        End Property

        '        <XmlIgnore()>
        '        Public ReadOnly Property Sets_YAMJSpecified() As Boolean
        '            Get
        '                Return Master.eSettings.Movie.InformationSettings.Collection.SaveYAMJCompatible AndAlso Sets_YAMJ.Count > 0
        '            End Get
        '        End Property

        '        <XmlElement("showlink")>
        '        Public Property ShowLinks() As List(Of String) = New List(Of String)

        '        <XmlIgnore()>
        '        Public ReadOnly Property ShowLinksSpecified() As Boolean
        '            Get
        '                Return ShowLinks.Count > 0
        '            End Get
        '        End Property

        '        <XmlElement("fileinfo")>
        '        Public Property FileInfo() As New Fileinfo

        '        <XmlIgnore()>
        '        Public ReadOnly Property FileInfoSpecified() As Boolean
        '            Get
        '                Return FileInfo.StreamDetails.Video IsNot Nothing AndAlso
        '                    (FileInfo.StreamDetails.Video.Count > 0 OrElse
        '                    FileInfo.StreamDetails.Audio.Count > 0 OrElse
        '                    FileInfo.StreamDetails.Subtitle.Count > 0)
        '            End Get
        '        End Property

        '        <XmlIgnore()>
        '        Public Property Lev() As Integer = 0

        '        <XmlElement("videosource")>
        '        Public Property VideoSource() As String = String.Empty

        '        <XmlIgnore()>
        '        Public ReadOnly Property VideoSourceSpecified() As Boolean
        '            Get
        '                Return Not String.IsNullOrEmpty(VideoSource)
        '            End Get
        '        End Property

        '        <XmlElement("locked")>
        '        Public Property Locked() As Boolean

        '        <XmlElement("user_note")>
        '        Public Property UserNote() As String = String.Empty

        '        <XmlIgnore()>
        '        Public ReadOnly Property UserNoteSpecified() As Boolean
        '            Get
        '                Return Not String.IsNullOrEmpty(UserNote)
        '            End Get
        '        End Property

        '#End Region 'Properties

        '#Region "Methods"

        '        Public Sub AddCertificationsFromString(ByVal value As String)
        '            _Certifications.Clear()
        '            If String.IsNullOrEmpty(value) Then Return

        '            If value.Contains(" / ") Then
        '                Dim values As String() = Regex.Split(value, " / ")
        '                For Each certification As String In values
        '                    certification = certification.Trim
        '                    If Not _Certifications.Contains(certification) Then
        '                        _Certifications.Add(certification)
        '                    End If
        '                Next
        '            Else
        '                If Not _Certifications.Contains(value) Then
        '                    _Certifications.Add(value.Trim)
        '                End If
        '            End If
        '        End Sub

        '        Public Sub AddTag(ByVal value As String)
        '            If String.IsNullOrEmpty(value) Then Return
        '            If Not _Tags.Contains(value) Then
        '                _Tags.Add(value.Trim)
        '            End If
        '        End Sub

        '        Public Function CloneDeep() As Object Implements ICloneable.Clone
        '            Dim Stream As New MemoryStream(50000)
        '            Dim Formatter As New Runtime.Serialization.Formatters.Binary.BinaryFormatter()
        '            ' Serialisierung über alle Objekte hinweg in einen Stream 
        '            Formatter.Serialize(Stream, Me)
        '            ' Zurück zum Anfang des Streams und... 
        '            Stream.Seek(0, SeekOrigin.Begin)
        '            ' ...aus dem Stream in ein Objekt deserialisieren 
        '            CloneDeep = Formatter.Deserialize(Stream)
        '            Stream.Close()
        '        End Function

        '        Public Sub CreateCachePaths_ActorsThumbs()
        '            Dim sPath As String = Path.Combine(Master.TempPath, "Global")

        '            For Each tActor As Person In Actors
        '                tActor.Thumb.CacheOriginalPath = Path.Combine(sPath, String.Concat("actorthumbs", Path.DirectorySeparatorChar, Path.GetFileName(tActor.Thumb.URLOriginal)))
        '                If Not String.IsNullOrEmpty(tActor.Thumb.URLThumb) Then
        '                    tActor.Thumb.CacheThumbPath = Path.Combine(sPath, String.Concat("actorthumbs\_thumbs", Path.DirectorySeparatorChar, Path.GetFileName(tActor.Thumb.URLOriginal)))
        '                End If
        '            Next
        '        End Sub

        '        Public Function CompareTo(ByVal other As Movie_old) As Integer Implements IComparable(Of Movie_old).CompareTo
        '            Dim retVal As Integer = (Lev).CompareTo(other.Lev)
        '            If retVal = 0 Then
        '                retVal = (Year).CompareTo(other.Year) * -1
        '            End If
        '            Return retVal
        '        End Function

        '        Public Sub SaveAllActorThumbs(ByRef DBElement As Database.DBElement)
        '            If ActorsSpecified AndAlso Master.eSettings.MovieActorthumbsAnyEnabled Then
        '                Images.Save_Actorthumbs(DBElement)
        '            Else
        '                Images.Delete(DBElement, Enums.ModifierType.MainActorThumbs, False)
        '                DBElement.ActorThumbs.Clear()
        '            End If
        '        End Sub

        '#End Region 'Methods

    End Class

    <Serializable()>
    Public Class MovieInSet
        Implements IComparable(Of MovieInSet)

#Region "Properties"

        Public Property DBMovie() As Database.DBElement = New Database.DBElement(Enums.ContentType.Movie)

        Public ReadOnly Property ListTitle() As String
            Get
                Return DBMovie.MainDetails.Title
            End Get
        End Property

        Public Property Order() As Integer = 0

#End Region 'Properties

#Region "Methods"

        Public Function CompareTo(ByVal other As MovieInSet) As Integer Implements IComparable(Of MovieInSet).CompareTo
            Return (Order).CompareTo(other.Order)
        End Function

#End Region 'Methods

    End Class

    '    <Serializable()>
    '    <XmlRoot("movieset")>
    '    Public Class Movieset

    '#Region "Fields"

    '        Private _contentType As Enums.ContentType = Enums.ContentType.MovieSet

    '#End Region 'Fields

    '#Region "Properties"

    '        <XmlElement("id")>
    '        Public Property DefaultId() As DefaultId
    '            Get
    '                Return UniqueIDs.GetDefaultId()
    '            End Get
    '            Set(value As DefaultId)
    '                UniqueIDs.Add(value)
    '            End Set
    '        End Property

    '        <XmlIgnore()>
    '        Public ReadOnly Property DefaultIdSpecified() As Boolean
    '            Get
    '                Return DefaultId.ValueSpecified
    '            End Get
    '        End Property

    '        <XmlIgnore()>
    '        Public Property UniqueIDs() As UniqueidContainer = New UniqueidContainer(_contentType)

    '        <XmlIgnore()>
    '        Public ReadOnly Property UniqueIDsSpecified() As Boolean
    '            Get
    '                Return UniqueIDs.Items.Count > 0
    '            End Get
    '        End Property

    '        <XmlElement("uniqueid")>
    '        Public Property UniqueIDs_Kodi() As Uniqueid()
    '            Get
    '                Return UniqueIDs.Items.ToArray
    '            End Get
    '            Set(value As Uniqueid())
    '                If value IsNot Nothing Then UniqueIDs.AddRange(value.ToList)
    '            End Set
    '        End Property

    '        <XmlElement("title")>
    '        Public Property Title() As String = String.Empty

    '        <XmlIgnore()>
    '        Public ReadOnly Property TitleSpecified() As Boolean
    '            Get
    '                Return Not String.IsNullOrEmpty(Title)
    '            End Get
    '        End Property

    '        <XmlElement("plot")>
    '        Public Property Plot() As String = String.Empty

    '        <XmlIgnore()>
    '        Public ReadOnly Property PlotSpecified() As Boolean
    '            Get
    '                Return Not String.IsNullOrEmpty(Plot)
    '            End Get
    '        End Property

    '        <XmlElement("language")>
    '        Public Property Language() As String = String.Empty

    '        <XmlIgnore()>
    '        Public ReadOnly Property LanguageSpecified() As Boolean
    '            Get
    '                Return Not String.IsNullOrEmpty(Language)
    '            End Get
    '        End Property

    '        <XmlElement("locked")>
    '        Public Property Locked() As Boolean
    '        ''' <summary>
    '        ''' Poster Thumb for preview in search results
    '        ''' </summary>
    '        ''' <value></value>
    '        ''' <returns></returns>
    '        ''' <remarks></remarks>
    '        <XmlIgnore()>
    '        Public Property ThumbPoster() As New Image
    '        ''' <summary>
    '        ''' Old Title before edit or scraping. Needed to remove no longer valid images and NFO.
    '        ''' </summary>
    '        ''' <returns></returns>
    '        <XmlIgnore()>
    '        Public Property OldTitle() As String = String.Empty

    '        <XmlIgnore()>
    '        Public ReadOnly Property TitleHasChanged() As Boolean
    '            Get
    '                Return Not OldTitle = Title
    '            End Get
    '        End Property

    '#End Region 'Properties

    '    End Class


    <Serializable()>
    Public Class MoviesetContainer

#Region "Fields"

        Shared _Logger As Logger = LogManager.GetCurrentClassLogger()

#End Region 'Fields

#Region "Properties"

        <XmlIgnore>
        Public ReadOnly Property AnyMoviesetSpecified As Boolean
            Get
                Return Items.Count > 0
            End Get
        End Property

        Public Property Items() As New List(Of MoviesetDetails)

#End Region 'Properties

#Region "Methods"

        Public Sub Add(ByVal movieset As MoviesetDetails, Optional rejectIfExists As Boolean = False)
            If movieset IsNot Nothing AndAlso movieset.TitleSpecified Then
                'search for already containing movisets with the same database ID
                Dim lstDbIds As IEnumerable(Of MoviesetDetails) = Nothing
                If movieset.IDSpecified Then
                    lstDbIds = From bSet As MoviesetDetails In Items Where bSet.ID = movieset.ID
                End If
                'search for already containing moviesets with the same TMDb ID
                Dim lstTMDbId As IEnumerable(Of MoviesetDetails) = Nothing
                If movieset.UniqueIDs.TMDbIdSpecified Then
                    lstTMDbId = From bset As MoviesetDetails In Items Where bset.UniqueIDs.TMDbId = movieset.UniqueIDs.TMDbId
                End If
                'search for already containing moviesets with the same title
                Dim lstTitle As IEnumerable(Of MoviesetDetails) = Nothing
                lstTitle = From bset As MoviesetDetails In Items Where bset.Title = movieset.Title

                If lstDbIds IsNot Nothing AndAlso lstDbIds.Count > 0 AndAlso Not rejectIfExists Then
                    Items.Remove(lstDbIds(0))
                End If

                If lstTMDbId IsNot Nothing AndAlso lstTMDbId.Count > 0 AndAlso Not rejectIfExists Then
                    Items.Remove(lstTMDbId(0))
                End If

                If lstTitle.Count > 0 AndAlso Not rejectIfExists Then
                    Items.Remove(lstTitle(0))
                End If

                If lstDbIds IsNot Nothing AndAlso lstDbIds.Count > 0 OrElse lstTMDbId IsNot Nothing AndAlso lstTMDbId.Count > 0 OrElse lstTitle.Count > 0 AndAlso rejectIfExists Then
                    Return
                Else
                    movieset.Order = Items.Count
                    Items.Add(movieset)
                End If
            End If
        End Sub

        Public Sub Add(ByVal movieset As MoviesetDetails_YAMJ)
            Add(New MoviesetDetails With {
                .Order = movieset.Order,
                .Title = movieset.Title
                }, True)
        End Sub

        Public Sub AddRange(ByVal moviesetList As List(Of MoviesetDetails))
            For Each entry In moviesetList
                Add(entry)
            Next
        End Sub

        Public Sub AddRange(ByVal moviesetList As MoviesetContainer)
            For Each entry In moviesetList.Items
                Add(entry)
            Next
        End Sub

        Public Sub AddRange(ByVal moviesetList As List(Of MoviesetDetails_YAMJ))
            For Each entry In moviesetList
                Add(entry)
            Next
        End Sub

        Public Function Create_SetNode_Kodi() As XmlDocument
            If Items.Count > 0 AndAlso Items.Item(0).TitleSpecified Then
                Dim firstSet As MoviesetDetails = Items.Item(0)

                If Master.eSettings.Movie.InformationSettings.Collection.SaveExtendedInformation Then
                    'creates a set node like:
                    '<set> 
                    '  <name>Die Hard Collection</name>
                    '  <overview>Hardest cop ever!</overview>
                    '  <tmdb>1570</tmdb>
                    '</set>

                    Dim XmlDoc As New XmlDocument

                    'Write down the XML declaration
                    Dim XmlDeclaration As XmlDeclaration = XmlDoc.CreateXmlDeclaration("1.0", "UTF-8", Nothing)

                    'Create the root element
                    Dim RootNode As XmlElement = XmlDoc.CreateElement("set")
                    XmlDoc.InsertBefore(XmlDeclaration, XmlDoc.DocumentElement)
                    XmlDoc.AppendChild(RootNode)

                    'Create a new <name> element and add it to the root node
                    Dim NodeName As XmlElement = XmlDoc.CreateElement("name")
                    RootNode.AppendChild(NodeName)
                    Dim NodeName_Text As XmlText = XmlDoc.CreateTextNode(firstSet.Title)
                    NodeName.AppendChild(NodeName_Text)

                    If firstSet.PlotSpecified Then
                        'Create a new <overview> element and add it to the root node
                        Dim NodeOverview As XmlElement = XmlDoc.CreateElement("overview")
                        RootNode.AppendChild(NodeOverview)
                        Dim NodeOverview_Text As XmlText = XmlDoc.CreateTextNode(firstSet.Plot)
                        NodeOverview.AppendChild(NodeOverview_Text)
                    End If

                    If firstSet.UniqueIDs.TMDbIdSpecified Then
                        'Create a new <tmdb> element and add it to the root node
                        Dim NodeTMDB As XmlElement = XmlDoc.CreateElement("tmdb")
                        RootNode.AppendChild(NodeTMDB)
                        Dim NodeTMDB_Text As XmlText = XmlDoc.CreateTextNode(firstSet.UniqueIDs.TMDbId.ToString)
                        NodeTMDB.AppendChild(NodeTMDB_Text)
                    End If

                    Return XmlDoc
                Else
                    'creates a set node like:
                    '<set>Die Hard Collection</set>

                    Dim XmlDoc As New XmlDocument

                    'Write down the XML declaration
                    Dim XmlDeclaration As XmlDeclaration = XmlDoc.CreateXmlDeclaration("1.0", "UTF-8", Nothing)

                    'Create the root element
                    Dim RootNode As XmlElement = XmlDoc.CreateElement("set")
                    XmlDoc.InsertBefore(XmlDeclaration, XmlDoc.DocumentElement)
                    XmlDoc.AppendChild(RootNode)
                    Dim RootNode_Text As XmlText = XmlDoc.CreateTextNode(firstSet.Title)
                    RootNode.AppendChild(RootNode_Text)

                    Return XmlDoc
                End If
            Else
                Return Nothing
            End If
        End Function

        Public Function Create_SetNode_YAMJ() As List(Of MoviesetDetails_YAMJ)
            Dim nSetDetails As New List(Of MoviesetDetails_YAMJ)
            For Each s In Items
                nSetDetails.Add(New MoviesetDetails_YAMJ With {
                                .Order = s.Order,
                                .Title = s.Title
                                })
            Next
            Return nSetDetails
        End Function
        ''' <summary>
        ''' converts both versions of moviesets declaration in movie.nfo to a proper "MoviesetDetails" object
        ''' </summary>
        ''' <remarks>      
        ''' <set>title</set>        
        ''' 
        ''' <set>
        '''     <name>title</name>
        '''     <overview>plot</overview>
        ''' </set>       
        ''' </remarks>
        ''' <param name="xmlObject"></param>
        Public Sub Read_SetNode_Kodi(ByVal xmlObject As Object)
            Try
                If xmlObject IsNot Nothing AndAlso TryCast(xmlObject, XmlElement) IsNot Nothing Then
                    Dim nSetInfo As New MoviesetDetails
                    Dim xElement As XmlElement = CType(xmlObject, XmlElement)
                    For Each xChild In xElement.ChildNodes
                        Dim xNode = CType(xChild, XmlNode)
                        Select Case xNode.NodeType
                            Case XmlNodeType.Element
                                Select Case xNode.Name
                                    Case "name"
                                        nSetInfo.Title = xNode.InnerText
                                    Case "overview"
                                        nSetInfo.Plot = xNode.InnerText
                                    Case "tmdb"
                                        Dim intTmdbId As Integer = -1
                                        If Integer.TryParse(xNode.InnerText, intTmdbId) Then
                                            nSetInfo.UniqueIDs.TMDbId = intTmdbId
                                        End If
                                End Select
                            Case XmlNodeType.Text
                                nSetInfo.Title = xNode.InnerText
                        End Select
                    Next
                    If nSetInfo.TitleSpecified Then Add(nSetInfo)
                End If
            Catch ex As Exception
                _Logger.Error(ex, New StackFrame().GetMethod().Name)
            End Try
        End Sub

        Public Sub RemoveSet(ByVal title As String)
            Dim tSet = From bSet As MoviesetDetails In Items Where bSet.Title = title
            If tSet.Count > 0 Then
                Items.Remove(tSet(0))
            End If
        End Sub

        Public Sub RemoveSet(ByVal id As Long)
            Dim tSet = From bSet As MoviesetDetails In Items Where bSet.ID = id
            If tSet.Count > 0 Then
                Items.Remove(tSet(0))
            End If
        End Sub

#End Region 'Methods

#Region "Nested Types"

        <Serializable()>
        Public Class MoviesetDetails_YAMJ

#Region "Properties"

            <XmlAttribute("order")>
            Public Property Order() As Integer = -1

            <XmlIgnore()>
            Public ReadOnly Property OrderSpecified() As Boolean
                Get
                    Return Not Order = -1
                End Get
            End Property

            <XmlText()>
            Public Property Title As String = String.Empty

            <XmlIgnore()>
            Public ReadOnly Property TitleSpecified As Boolean
                Get
                    Return Not String.IsNullOrEmpty(Title)
                End Get
            End Property

#End Region 'Properties 

        End Class

#End Region 'Nested Types

    End Class

    <Serializable()>
    Public Class MoviesetDetails
        Inherits MainDetails
        Implements IComparable(Of MoviesetDetails)

#Region "Properties"
        ''' <summary>
        ''' Database Id
        ''' </summary>
        ''' <returns></returns>
        <XmlIgnore()>
        Public Property ID() As Long = -1

        <XmlIgnore()>
        Public ReadOnly Property IDSpecified() As Boolean
            Get
                Return Not ID = -1
            End Get
        End Property

        <XmlAttribute("order")>
        Public Property Order() As Integer = -1

        <XmlIgnore()>
        Public ReadOnly Property OrderSpecified() As Boolean
            Get
                Return Not Order = -1
            End Get
        End Property

#End Region 'Properties

#Region "Methods"

        Public Shadows Function CompareTo(ByVal other As MoviesetDetails) As Integer Implements IComparable(Of MoviesetDetails).CompareTo
            Return Order.CompareTo(other.Order)
        End Function

#End Region 'Methods

    End Class

    'Public Class RoleModel
    '
    '#Region "Properties"
    '
    '    Public ReadOnly Property ID As Long
    '        Get
    '            Return Person.ID
    '        End Get
    '    End Property
    '
    '    Public ReadOnly Property Person() As Person
    '
    '
    '    Public Property Role() As String
    '    Get
    '            Return Person.
    '
    '    <XmlIgnore()>
    '    Public ReadOnly Property RoleSpecified() As Boolean
    '        Get
    '            Return Not String.IsNullOrEmpty(Role)
    '        End Get
    '    End Property
    '
    '    <XmlElement("order")>
    '    Public Property Order() As Integer = -1
    '
    '    <XmlIgnore()>
    '    Public ReadOnly Property OrderSpecified() As Boolean
    '        Get
    '            Return Not Order = -1
    '        End Get
    '    End Property
    '
    '    <XmlIgnore()>
    '    Public Property Thumb() As New Image
    '
    '    <XmlElement("thumb")>
    '    Public Property URLOriginal() As String
    '        Get
    '            Return Thumb.URLOriginal
    '        End Get
    '        Set(ByVal Value As String)
    '            Thumb.URLOriginal = Value
    '        End Set
    '    End Property
    '
    '    <XmlIgnore()>
    '    Public ReadOnly Property URLOriginalSpecified() As Boolean
    '        Get
    '            Return Not String.IsNullOrEmpty(Thumb.URLOriginal)
    '        End Get
    '    End Property
    '
    '    <XmlIgnore()>
    '    Public Property LocalFilePath() As String
    '        Get
    '            Return Thumb.LocalFilePath
    '        End Get
    '        Set(ByVal Value As String)
    '            Thumb.LocalFilePath = Value
    '        End Set
    '    End Property
    '
    '    <XmlIgnore()>
    '    Public ReadOnly Property LocalFilePathSpecified() As Boolean
    '        Get
    '            Return Not String.IsNullOrEmpty(Thumb.LocalFilePath)
    '        End Get
    '    End Property
    '
    '    <XmlIgnore()>
    '    Public Property Name As String
    '        Get
    '            Return Person.Name
    '        End Get
    '        Set
    '            Person.Name = Value
    '        End Set
    '    End Property
    '
    '    <XmlIgnore()>
    '    Public Property IMDbId As String
    '        Get
    '            Return Person.IMDbId
    '        End Get
    '        Set
    '            Person.IMDbId = Value
    '        End Set
    '    End Property
    '
    '    <XmlIgnore()>
    '    Public Property TMDbId As String
    '        Get
    '            Return Person.TMDbId
    '        End Get
    '        Set
    '            Person.TMDbId = Value
    '        End Set
    '    End Property
    '
    '#EndRegion 'Properties
    '
    'End Class

    <Serializable()>
    Public Class PreferredImagesContainer

#Region "Properties"

        Public Property Episodes() As New List(Of EpisodeOrSeasonImagesContainer)

        Public Property ImagesContainer() As New ImagesContainer

        Public Property Seasons() As New List(Of EpisodeOrSeasonImagesContainer)

#End Region 'Properties 

    End Class


    <Serializable()>
    Public Class RatingContainer

#Region "Fields"

        Private _contentType As Enums.ContentType

#End Region 'Fields

#Region "Constructors"

        Public Sub New(ByVal type As Enums.ContentType)
            _contentType = type
        End Sub

#End Region 'Constructors

#Region "Properties"

        <XmlIgnore>
        Public ReadOnly Property AnyRatingSpecified As Boolean
            Get
                Return Items.Count > 0
            End Get
        End Property

        <XmlElement("rating")>
        Public Property Items() As New List(Of RatingDetails)

#End Region 'Properties

#Region "Methods"

        Public Sub Add(ByVal rating As RatingDetails)
            If rating.TypeSpecified AndAlso rating.ValueSpecified Then
                'remove existing entry with same "type", only one entry per "type" is allowed
                RemoveAll(rating.Type)
                'set as default by settings
                rating.IsDefault = If(rating.IsDefault, True, rating.Type = GetDefaultType())
                Items.Add(rating)
            End If
        End Sub

        Public Sub AddRange(ByVal ratingList As List(Of RatingDetails))
            For Each entry In ratingList
                Add(entry)
            Next
        End Sub

        Public Sub AddRange(ByVal ratingContainer As RatingContainer)
            For Each entry In ratingContainer.Items
                Add(entry)
            Next
        End Sub

        Public Function GetDefaultRating() As RatingDetails
            Dim nRating = Items.FirstOrDefault(Function(f) f.IsDefault)
            If nRating IsNot Nothing Then
                Return nRating
            Else
                Return GetDefaultBySettings()
            End If
        End Function

        Public Function GetDefaultBySettings() As RatingDetails
            Dim strDefaultType As String = GetDefaultType()
            If Not String.IsNullOrEmpty(strDefaultType) Then
                Return Items.FirstOrDefault(Function(f) f.Type = strDefaultType)
            End If
            Return Nothing
        End Function

        Private Function GetDefaultType() As String
            Select Case _contentType
                Case Enums.ContentType.Movie
                    Return Master.eSettings.Movie.InformationSettings.Ratings.DefaultType
                Case Enums.ContentType.TVEpisode, Enums.ContentType.TVShow
                    Return Master.eSettings.TVShow.InformationSettings.Ratings.DefaultType
                Case Else
                    Return String.Empty
            End Select
        End Function

        Private Sub RemoveAll(ByVal type As String)
            Items.RemoveAll(Function(f) f.Type = type)
        End Sub

#End Region 'Methods

    End Class

    <Serializable()>
    Public Class RatingDetails
        Implements IComparable(Of RatingDetails)

#Region "Properties"

        <XmlIgnore()>
        Public Property ID() As Long = -1

        <XmlAttribute("name")>
        Public Property Type() As String = String.Empty

        <XmlIgnore()>
        Public ReadOnly Property TypeSpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(Type)
            End Get
        End Property

        <XmlAttribute("max")>
        Public Property Max() As Integer = -1

        <XmlIgnore()>
        Public ReadOnly Property MaxSpecified() As Boolean
            Get
                Return Not Max = -1
            End Get
        End Property

        <XmlAttribute("default")>
        Public Property IsDefault() As Boolean = False

        <XmlElement("value")>
        Public Property Value() As Double = -1

        <XmlIgnore()>
        Public ReadOnly Property ValueSpecified() As Boolean
            Get
                Return Not Value = -1
            End Get
        End Property

        <XmlIgnore>
        Public ReadOnly Property ValueNormalized() As Double
            Get
                If ValueSpecified AndAlso MaxSpecified Then
                    Return Value / Max * 10
                Else
                    Return 0
                End If
            End Get
        End Property

        <XmlIgnore>
        Public ReadOnly Property ValueNormalizedSpezified() As Boolean
            Get
                Return Not ValueNormalized = 0
            End Get
        End Property

        <XmlElement("votes")>
        Public Property Votes() As Integer = 0

        <XmlIgnore()>
        Public ReadOnly Property VotesSpecified() As Boolean
            Get
                Return Not Votes = 0
            End Get
        End Property

#End Region 'Properties

#Region "Methods"

        Public Function CompareTo(ByVal other As RatingDetails) As Integer Implements IComparable(Of RatingDetails).CompareTo
            Try
                Dim retVal As Integer = If(IsDefault, -1, Type.CompareTo(other.Type))
                Return retVal
            Catch ex As Exception
                Return 0
            End Try
        End Function

#End Region 'Methods

    End Class

    <Serializable()>
    Public Class SearchResultsContainer

#Region "Properties"

        Public Property EpisodeFanarts() As New List(Of Image)

        Public Property EpisodePosters() As New List(Of Image)

        Public Property SeasonBanners() As New List(Of Image)

        Public Property SeasonFanarts() As New List(Of Image)

        Public Property SeasonLandscapes() As New List(Of Image)

        Public Property SeasonPosters() As New List(Of Image)

        Public Property MainBanners() As New List(Of Image)

        Public Property MainCharacterArts() As New List(Of Image)

        Public Property MainClearArts() As New List(Of Image)

        Public Property MainClearLogos() As New List(Of Image)

        Public Property MainDiscArts() As New List(Of Image)

        Public Property MainFanarts() As New List(Of Image)

        Public Property MainKeyarts() As New List(Of Image)

        Public Property MainLandscapes() As New List(Of Image)

        Public Property MainPosters() As New List(Of Image)

#End Region 'Properties

#Region "Methods"

        Public Sub CreateCachePaths(ByRef tDBElement As Database.DBElement)
            Dim sID As String = String.Empty
            Dim sPath As String = String.Empty

            Select Case tDBElement.ContentType
                Case Enums.ContentType.Movie
                    sID = tDBElement.MainDetails.UniqueIDs.IMDbId
                    If String.IsNullOrEmpty(sID) Then
                        sID = tDBElement.MainDetails.UniqueIDs.TMDbId.ToString
                    End If
                    If String.IsNullOrEmpty(sID) Then
                        sID = "Unknown"
                    End If
                    sPath = Path.Combine(Master.TempPath, String.Concat("Movies", Path.DirectorySeparatorChar, sID))
                Case Enums.ContentType.Movieset
                    sID = tDBElement.MainDetails.UniqueIDs.TMDbId.ToString
                    If String.IsNullOrEmpty(sID) Then
                        sID = "Unknown"
                    End If
                    sPath = Path.Combine(Master.TempPath, String.Concat("Moviesets", Path.DirectorySeparatorChar, sID))
                Case Enums.ContentType.TVEpisode, Enums.ContentType.TVSeason
                    sID = tDBElement.MainDetails.UniqueIDs.TVDbId.ToString
                    If String.IsNullOrEmpty(sID) Then
                        sID = "Unknown"
                    End If
                    sPath = Path.Combine(Master.TempPath, String.Concat("Shows", Path.DirectorySeparatorChar, sID))
                Case Enums.ContentType.TV, Enums.ContentType.TVShow
                    sID = tDBElement.MainDetails.UniqueIDs.TVDbId.ToString
                    If String.IsNullOrEmpty(sID) Then
                        sID = "Unknown"
                    End If
                    sPath = Path.Combine(Master.TempPath, String.Concat("Shows", Path.DirectorySeparatorChar, sID))
                Case Else
                    Throw New ArgumentOutOfRangeException("wrong tContentType", "value must be Movie, MovieSet or TV")
                    Return
            End Select

            For Each tImg As Image In EpisodeFanarts
                tImg.CacheOriginalPath = Path.Combine(sPath, String.Concat("episodefanarts", Path.DirectorySeparatorChar, Path.GetFileName(tImg.URLOriginal)))
                If Not String.IsNullOrEmpty(tImg.URLThumb) Then
                    tImg.CacheThumbPath = Path.Combine(sPath, String.Concat("episodefanarts\_thumbs", Path.DirectorySeparatorChar, Path.GetFileName(tImg.URLOriginal)))
                End If
            Next

            For Each tImg As Image In EpisodePosters
                tImg.CacheOriginalPath = Path.Combine(sPath, String.Concat("episodeposters", Path.DirectorySeparatorChar, Path.GetFileName(tImg.URLOriginal)))
                If Not String.IsNullOrEmpty(tImg.URLThumb) Then
                    tImg.CacheThumbPath = Path.Combine(sPath, String.Concat("episodeposters\_thumbs", Path.DirectorySeparatorChar, Path.GetFileName(tImg.URLOriginal)))
                End If
            Next

            For Each tImg As Image In MainBanners
                tImg.CacheOriginalPath = Path.Combine(sPath, String.Concat("mainbanners", Path.DirectorySeparatorChar, Path.GetFileName(tImg.URLOriginal)))
                If Not String.IsNullOrEmpty(tImg.URLThumb) Then
                    tImg.CacheThumbPath = Path.Combine(sPath, String.Concat("mainbanners\_thumbs", Path.DirectorySeparatorChar, Path.GetFileName(tImg.URLOriginal)))
                End If
            Next

            For Each tImg As Image In MainCharacterArts
                tImg.CacheOriginalPath = Path.Combine(sPath, String.Concat("maincharacterarts", Path.DirectorySeparatorChar, Path.GetFileName(tImg.URLOriginal)))
                If Not String.IsNullOrEmpty(tImg.URLThumb) Then
                    tImg.CacheThumbPath = Path.Combine(sPath, String.Concat("maincharacterarts\_thumbs", Path.DirectorySeparatorChar, Path.GetFileName(tImg.URLOriginal)))
                End If
            Next

            For Each tImg As Image In MainClearArts
                tImg.CacheOriginalPath = Path.Combine(sPath, String.Concat("maincleararts", Path.DirectorySeparatorChar, Path.GetFileName(tImg.URLOriginal)))
                If Not String.IsNullOrEmpty(tImg.URLThumb) Then
                    tImg.CacheThumbPath = Path.Combine(sPath, String.Concat("maincleararts\_thumbs", Path.DirectorySeparatorChar, Path.GetFileName(tImg.URLOriginal)))
                End If
            Next

            For Each tImg As Image In MainClearLogos
                tImg.CacheOriginalPath = Path.Combine(sPath, String.Concat("mainclearlogos", Path.DirectorySeparatorChar, Path.GetFileName(tImg.URLOriginal)))
                If Not String.IsNullOrEmpty(tImg.URLThumb) Then
                    tImg.CacheThumbPath = Path.Combine(sPath, String.Concat("mainclearlogos\_thumbs", Path.DirectorySeparatorChar, Path.GetFileName(tImg.URLOriginal)))
                End If
            Next

            For Each tImg As Image In MainDiscArts
                tImg.CacheOriginalPath = Path.Combine(sPath, String.Concat("maindiscarts", Path.DirectorySeparatorChar, Path.GetFileName(tImg.URLOriginal)))
                If Not String.IsNullOrEmpty(tImg.URLThumb) Then
                    tImg.CacheThumbPath = Path.Combine(sPath, String.Concat("maindiscarts\_thumbs", Path.DirectorySeparatorChar, Path.GetFileName(tImg.URLOriginal)))
                End If
            Next

            For Each tImg As Image In MainFanarts
                tImg.CacheOriginalPath = Path.Combine(sPath, String.Concat("mainfanarts", Path.DirectorySeparatorChar, Path.GetFileName(tImg.URLOriginal)))
                If Not String.IsNullOrEmpty(tImg.URLThumb) Then
                    tImg.CacheThumbPath = Path.Combine(sPath, String.Concat("mainfanarts\_thumbs", Path.DirectorySeparatorChar, Path.GetFileName(tImg.URLOriginal)))
                End If
            Next

            For Each tImg As Image In MainLandscapes
                tImg.CacheOriginalPath = Path.Combine(sPath, String.Concat("mainlandscapes", Path.DirectorySeparatorChar, Path.GetFileName(tImg.URLOriginal)))
                If Not String.IsNullOrEmpty(tImg.URLThumb) Then
                    tImg.CacheThumbPath = Path.Combine(sPath, String.Concat("mainlandscapes\_thumbs", Path.DirectorySeparatorChar, Path.GetFileName(tImg.URLOriginal)))
                End If
            Next

            For Each tImg As Image In MainPosters
                tImg.CacheOriginalPath = Path.Combine(sPath, String.Concat("mainposters", Path.DirectorySeparatorChar, Path.GetFileName(tImg.URLOriginal)))
                If Not String.IsNullOrEmpty(tImg.URLThumb) Then
                    tImg.CacheThumbPath = Path.Combine(sPath, String.Concat("mainposters\_thumbs", Path.DirectorySeparatorChar, Path.GetFileName(tImg.URLOriginal)))
                End If
            Next

            For Each tImg As Image In SeasonBanners
                tImg.CacheOriginalPath = Path.Combine(sPath, String.Concat("seasonbanners", Path.DirectorySeparatorChar, Path.GetFileName(tImg.URLOriginal)))
                If Not String.IsNullOrEmpty(tImg.URLThumb) Then
                    tImg.CacheThumbPath = Path.Combine(sPath, String.Concat("seasonbanners\_thumbs", Path.DirectorySeparatorChar, Path.GetFileName(tImg.URLOriginal)))
                End If
            Next

            For Each tImg As Image In SeasonFanarts
                tImg.CacheOriginalPath = Path.Combine(sPath, String.Concat("seasonfanarts", Path.DirectorySeparatorChar, Path.GetFileName(tImg.URLOriginal)))
                If Not String.IsNullOrEmpty(tImg.URLThumb) Then
                    tImg.CacheThumbPath = Path.Combine(sPath, String.Concat("seasonfanarts\_thumbs", Path.DirectorySeparatorChar, Path.GetFileName(tImg.URLOriginal)))
                End If
            Next

            For Each tImg As Image In SeasonLandscapes
                tImg.CacheOriginalPath = Path.Combine(sPath, String.Concat("seasonlandscapes", Path.DirectorySeparatorChar, Path.GetFileName(tImg.URLOriginal)))
                If Not String.IsNullOrEmpty(tImg.URLThumb) Then
                    tImg.CacheThumbPath = Path.Combine(sPath, String.Concat("seasonlandscapes\_thumbs", Path.DirectorySeparatorChar, Path.GetFileName(tImg.URLOriginal)))
                End If
            Next

            For Each tImg As Image In SeasonPosters
                tImg.CacheOriginalPath = Path.Combine(sPath, String.Concat("seasonposters", Path.DirectorySeparatorChar, Path.GetFileName(tImg.URLOriginal)))
                If Not String.IsNullOrEmpty(tImg.URLThumb) Then
                    tImg.CacheThumbPath = Path.Combine(sPath, String.Concat("seasonposters\_thumbs", Path.DirectorySeparatorChar, Path.GetFileName(tImg.URLOriginal)))
                End If
            Next
        End Sub

        Public Function GetImagesByType(ByVal imageType As Enums.ModifierType) As List(Of Image)
            Select Case imageType
                Case Enums.ModifierType.AllSeasonsBanner, Enums.ModifierType.SeasonBanner
                    Return SeasonBanners
                Case Enums.ModifierType.AllSeasonsFanart, Enums.ModifierType.SeasonFanart
                    Return SeasonFanarts
                Case Enums.ModifierType.AllSeasonsLandscape, Enums.ModifierType.SeasonLandscape
                    Return SeasonLandscapes
                Case Enums.ModifierType.AllSeasonsPoster, Enums.ModifierType.SeasonPoster
                    Return SeasonPosters
                Case Enums.ModifierType.EpisodeFanart
                    Return EpisodeFanarts
                Case Enums.ModifierType.EpisodePoster
                    Return EpisodePosters
                Case Enums.ModifierType.MainBanner
                    Return MainBanners
                Case Enums.ModifierType.MainCharacterArt
                    Return MainCharacterArts
                Case Enums.ModifierType.MainClearArt
                    Return MainClearArts
                Case Enums.ModifierType.MainClearLogo
                    Return MainClearLogos
                Case Enums.ModifierType.MainDiscArt
                    Return MainDiscArts
                Case Enums.ModifierType.MainExtrafanarts, Enums.ModifierType.MainExtrathumbs, Enums.ModifierType.MainFanart
                    Return MainFanarts
                Case Enums.ModifierType.MainKeyart
                    Return MainKeyarts
                Case Enums.ModifierType.MainLandscape
                    Return MainLandscapes
                Case Enums.ModifierType.MainPoster
                    Return MainPosters
            End Select
            Return New List(Of Image)
        End Function

        Public Sub SortAndFilter(ByVal dbBElement As Database.DBElement)
            Dim Filter As New FilterSettings With {
                .ContentType = dbBElement.ContentType,
                .MediaLanguage = dbBElement.Language_Main,
                .ImageSettings = Settings.Helpers.GetImageSettings(dbBElement.ContentType)
            }

            'filter all List(Of Image) by preferred language/en/Blank/String.Empty/others
            'Language preference settings aren't needed for sorting episode posters since here we only care about size of image (unlike poster/banner)
            '_episodeposters = FilterImages(_episodeposters, cSettings)
            SeasonBanners = FilterImages(SeasonBanners, Filter)
            SeasonLandscapes = FilterImages(SeasonLandscapes, Filter)
            SeasonPosters = FilterImages(SeasonPosters, Filter)
            MainBanners = FilterImages(MainBanners, Filter)
            MainCharacterArts = FilterImages(MainCharacterArts, Filter)
            MainClearArts = FilterImages(MainClearArts, Filter)
            MainClearLogos = FilterImages(MainClearLogos, Filter)
            MainDiscArts = FilterImages(MainDiscArts, Filter)
            MainFanarts = FilterImages(MainFanarts, Filter)
            MainKeyarts = FilterImages(MainPosters, New FilterSettings With {
                                       .ImageSettings = New ImageBase With {
                                       .ForcedLanguage = String.Empty,
                                       .ForceLanguage = True,
                                       .FilterGetBlankImages = True,
                                       .FilterGetEnglishImages = False,
                                       .FilterMediaLanguage = True
                                       },
                                       .MediaLanguage = String.Empty
                                       })
            MainLandscapes = FilterImages(MainLandscapes, Filter)
            MainPosters = FilterImages(MainPosters, Filter)

            'sort all List(Of Image) by Image.ShortLang
            EpisodeFanarts.Sort()
            EpisodePosters.Sort()
            SeasonBanners.Sort()
            SeasonFanarts.Sort()
            SeasonLandscapes.Sort()
            SeasonPosters.Sort()
            MainBanners.Sort()
            MainCharacterArts.Sort()
            MainClearArts.Sort()
            MainClearLogos.Sort()
            MainDiscArts.Sort()
            MainFanarts.Sort()
            MainLandscapes.Sort()
            MainPosters.Sort()

            'sort all List(Of Image) by Votes/Size/Type
            SortImages(Filter)
        End Sub

        Private Function SortImages(ByRef images As List(Of Image), ByVal preferredSize As Enums.ImageSize) As List(Of Image)
            If Not preferredSize = Enums.ImageSize.Any Then
                Return images.OrderByDescending(Function(z) z.VoteAverage).OrderBy(Function(x) x.ImageSize).OrderByDescending(Function(y) y.ImageSize = preferredSize).ToList()
            Else
                Return images.OrderByDescending(Function(z) z.VoteAverage).OrderBy(Function(x) x.ImageSize).ToList()
            End If
        End Function

        Private Sub SortImages(ByVal filter As FilterSettings)
            MainBanners = SortImages(MainBanners, filter.ImageSettings.Banner.PreferredSize)
            MainCharacterArts = SortImages(MainCharacterArts, filter.ImageSettings.Characterart.PreferredSize)
            MainClearArts = SortImages(MainClearArts, filter.ImageSettings.Clearart.PreferredSize)
            MainClearLogos = SortImages(MainClearLogos, filter.ImageSettings.Clearlogo.PreferredSize)
            MainDiscArts = SortImages(MainDiscArts, filter.ImageSettings.Discart.PreferredSize)
            MainFanarts = SortImages(MainFanarts, filter.ImageSettings.Fanart.PreferredSize)
            MainLandscapes = SortImages(MainLandscapes, filter.ImageSettings.Landscape.PreferredSize)
            MainPosters = SortImages(MainPosters, filter.ImageSettings.Poster.PreferredSize)

            'Unique image containers

            'TVEpisode Fanart
            EpisodeFanarts = SortImages(EpisodeFanarts, Master.eSettings.TVEpisode.ImageSettings.Fanart.PreferredSize)

            'TVEpisode Poster
            EpisodePosters = SortImages(EpisodePosters, Master.eSettings.TVEpisode.ImageSettings.Poster.PreferredSize)

            'TVSeason Banner
            SeasonBanners = SortImages(SeasonBanners, Master.eSettings.TVSeason.ImageSettings.Banner.PreferredSize)

            'TVSeason Fanart
            SeasonFanarts = SortImages(SeasonFanarts, Master.eSettings.TVSeason.ImageSettings.Fanart.PreferredSize)

            'TVSeason Landscape
            SeasonLandscapes = SortImages(SeasonLandscapes, Master.eSettings.TVSeason.ImageSettings.Landscape.PreferredSize)

            'TVSeason Poster
            SeasonPosters = SortImages(SeasonPosters, Master.eSettings.TVSeason.ImageSettings.Poster.PreferredSize)
        End Sub

        Private Function FilterImages(ByRef images As List(Of Image), ByVal filter As FilterSettings) As List(Of Image)
            Dim Result As New List(Of Image)

            If filter.ImageSettings.ForceLanguage AndAlso filter.ImageSettings.ForcedLanguageSpecified Then
                Result.AddRange(images.Where(Function(f) f.Language = filter.ImageSettings.ForcedLanguage))
            End If

            If Not (filter.ImageSettings.ForceLanguage AndAlso filter.ImageSettings.ForcedLanguage = filter.MediaLanguage) Then
                Result.AddRange(images.Where(Function(f) f.Language = filter.MediaLanguage))
            End If

            If (filter.ImageSettings.FilterGetEnglishImages OrElse Not filter.ImageSettings.FilterMediaLanguage) AndAlso
                Not (filter.ImageSettings.ForceLanguage AndAlso filter.ImageSettings.ForcedLanguage = "en") AndAlso
                Not filter.MediaLanguage = "en" Then
                Result.AddRange(images.Where(Function(f) f.Language = "en"))
            End If

            If filter.ImageSettings.FilterGetBlankImages OrElse Not filter.ImageSettings.FilterMediaLanguage Then
                Result.AddRange(images.Where(Function(f) f.LongLanguage = Master.eLang.GetString(1168, "Blank")))
                Result.AddRange(images.Where(Function(f) f.Language = String.Empty))
            End If

            If Not filter.ImageSettings.FilterMediaLanguage Then
                Result.AddRange(images.Where(Function(f) Not f.Language = If(filter.ImageSettings.ForceLanguage, filter.ImageSettings.ForcedLanguage, String.Empty) AndAlso
                                                 Not f.Language = filter.MediaLanguage AndAlso
                                                 Not f.Language = "en" AndAlso
                                                 Not f.LongLanguage = Master.eLang.GetString(1168, "Blank") AndAlso
                                                 Not f.Language = String.Empty)
                                                 )
            End If

            Return Result
        End Function

#End Region 'Methods

#Region "Nested Types"

        Private Structure FilterSettings

#Region "Fields"

            Dim ContentType As Enums.ContentType
            Dim MediaLanguage As String
            Dim ImageSettings As ImageBase

#End Region 'Fields

        End Structure

#End Region 'Nested Types

    End Class

    <Serializable()>
    <XmlRoot("seasons")>
    Public Class Seasons

#Region "Properties"

        <XmlElement("seasondetails")>
        Public Property Seasons() As New List(Of MainDetails)

        <XmlIgnore()>
        Public ReadOnly Property SeasonsSpecified() As Boolean
            Get
                Return Seasons.Count > 0
            End Get
        End Property

#End Region 'Properties 

    End Class

    <Serializable()>
    <XmlRoot("streamdata")>
    Public Class StreamData

#Region "Properties"

        <XmlElement("audio")>
        Public Property Audio() As New List(Of Audio)

        <XmlIgnore>
        Public ReadOnly Property AudioSpecified() As Boolean
            Get
                Return Audio.Count > 0
            End Get
        End Property

        <XmlElement("subtitle")>
        Public Property Subtitle() As New List(Of Subtitle)

        <XmlIgnore>
        Public ReadOnly Property SubtitleSpecified() As Boolean
            Get
                Return Subtitle.Count > 0
            End Get
        End Property

        <XmlElement("video")>
        Public Property Video() As New List(Of Video)

        <XmlIgnore>
        Public ReadOnly Property VideoSpecified() As Boolean
            Get
                Return Video.Count > 0
            End Get
        End Property

#End Region 'Properties

    End Class

    <Serializable()>
    Public Class Subtitle

#Region "Properties"

        <XmlElement("language")>
        Public Property Language() As String = String.Empty

        <XmlIgnore>
        Public ReadOnly Property LanguageSpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(Language)
            End Get
        End Property

        <XmlElement("longlanguage")>
        Public Property LongLanguage() As String = String.Empty

        <XmlIgnore>
        Public ReadOnly Property LongLanguageSpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(LongLanguage)
            End Get
        End Property

        <XmlElement("forced")>
        Public Property Forced() As Boolean

        <XmlElement("path")>
        Public Property Path() As String = String.Empty

        <XmlIgnore>
        Public ReadOnly Property PathSpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(Path)
            End Get
        End Property

        <XmlElement("type")>
        Public Property Type() As String = String.Empty

        <XmlIgnore>
        Public ReadOnly Property TypeSpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(Type)
            End Get
        End Property
        ''' <summary>
        ''' Trigger to delete local/external subtitle files
        ''' </summary>
        ''' <returns></returns>
        <XmlIgnore>
        Public Property ToRemove() As Boolean

#End Region 'Properties

    End Class

    <Serializable()>
    Public Class Thumb

#Region "Properties"

        <XmlAttribute("preview")>
        Public Property Preview() As String = String.Empty

        <XmlText()>
        Public Property [Text]() As String = String.Empty

#End Region 'Properties

    End Class

    <Serializable()>
    Public Class Uniqueid
        Implements IComparable(Of Uniqueid)

#Region "Properties"

        <XmlIgnore()>
        Public Property ID() As Long = -1

        <XmlIgnore()>
        Public ReadOnly Property IDSpecified() As Boolean
            Get
                Return Not ID = -1
            End Get
        End Property

        <XmlAttribute("type")>
        Public Property Type() As String = "unknown"

        <XmlIgnore()>
        Public ReadOnly Property TypeSpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(Type)
            End Get
        End Property

        <XmlAttribute("default")>
        Public Property IsDefault() As Boolean = False

        <XmlText()>
        Public Property Value() As String = String.Empty

        <XmlIgnore()>
        Public ReadOnly Property ValueSpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(Value)
            End Get
        End Property

#End Region 'Properties

#Region "Methods"

        Public Function CompareTo(ByVal other As Uniqueid) As Integer Implements IComparable(Of Uniqueid).CompareTo
            Try
                Dim retVal As Integer = If(IsDefault, -1, Type.CompareTo(other.Type))
                Return retVal
            Catch ex As Exception
                Return 0
            End Try
        End Function

#End Region 'Methods

    End Class


    <Serializable()>
    Public Class UniqueidContainer

#Region "Fields"

        Private _contentType As Enums.ContentType

#End Region 'Fields

#Region "Constructors"

        Public Sub New(ByVal type As Enums.ContentType)
            _contentType = type
        End Sub

#End Region 'Constructors

#Region "Properties"

        <XmlIgnore>
        Public ReadOnly Property AnyUniqueIdSpecified As Boolean
            Get
                Return Items.Count > 0
            End Get
        End Property

        Public Property Items() As New List(Of Uniqueid)

        <XmlIgnore>
        Public Property IMDbId() As String
            Get
                Dim nID = Items.FirstOrDefault(Function(f) f.Type = "imdb")
                If nID IsNot Nothing AndAlso nID.ValueSpecified Then Return nID.Value
                Return String.Empty
            End Get
            Set(value As String)
                If Not String.IsNullOrEmpty(value) Then
                    Add("imdb", value)
                Else
                    RemoveAll("imdb")
                End If
            End Set
        End Property

        <XmlIgnore>
        Public ReadOnly Property IMDbIdSpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(IMDbId)
            End Get
        End Property

        <XmlIgnore>
        Public Property TMDbId() As Integer
            Get
                Dim nID = Items.FirstOrDefault(Function(f) f.Type = "tmdb")
                If nID IsNot Nothing AndAlso
                    nID.ValueSpecified AndAlso
                    Integer.TryParse(nID.Value, 0) Then Return CInt(nID.Value)
                Return -1
            End Get
            Set(value As Integer)
                If Not value = -1 Then
                    Add("tmdb", value.ToString)
                Else
                    RemoveAll("tmdb")
                End If
            End Set
        End Property

        <XmlIgnore>
        Public ReadOnly Property TMDbIdSpecified() As Boolean
            Get
                Return Not TMDbId = -1
            End Get
        End Property

        <XmlIgnore>
        Public Property TMDbCollectionId() As Integer
            Get
                Dim nID = Items.FirstOrDefault(Function(f) f.Type = "tmdbcol")
                If nID IsNot Nothing AndAlso
                    nID.ValueSpecified AndAlso
                    Integer.TryParse(nID.Value, 0) Then Return CInt(nID.Value)
                Return -1
            End Get
            Set(value As Integer)
                If Not value = -1 Then
                    Add("tmdbcol", value.ToString)
                Else
                    RemoveAll("tmdbcol")
                End If
            End Set
        End Property

        <XmlIgnore>
        Public ReadOnly Property TMDbCollectionIdSpecified() As Boolean
            Get
                Return Not TMDbCollectionId = -1
            End Get
        End Property

        <XmlIgnore>
        Public Property TVDbId() As Integer
            Get
                Dim nID = Items.FirstOrDefault(Function(f) f.Type = "tvdb")
                If nID IsNot Nothing AndAlso
                    nID.ValueSpecified AndAlso
                    Integer.TryParse(nID.Value, 0) Then Return CInt(nID.Value)
                Return -1
            End Get
            Set(value As Integer)
                If Not value = -1 Then
                    Add("tvdb", value.ToString)
                Else
                    RemoveAll("tvdb")
                End If
            End Set
        End Property

        <XmlIgnore>
        Public ReadOnly Property TVDbIdSpecified() As Boolean
            Get
                Return Not TVDbId = -1
            End Get
        End Property

#End Region 'Properties

#Region "Methods"

        Public Sub Add(ByVal type As String, ByVal value As String)
            If Not String.IsNullOrEmpty(type) AndAlso Not String.IsNullOrEmpty(value) Then
                'remove existing entry with same "type", only one entry per "type" is allowed
                RemoveAll(type)
                Items.Add(New Uniqueid With {
                          .IsDefault = type = GetDefaultType(),
                          .Type = type,
                          .Value = value
                          })
            End If
        End Sub

        Public Sub Add(ByVal defaultId As DefaultId)
            If defaultId.TypeSpecified AndAlso defaultId.ValueSpecified Then
                'remove existing entry with same "type", only one entry per "type" is allowed
                RemoveAll(defaultId.Type)
                Items.Add(New Uniqueid With {
                          .IsDefault = True,
                          .Type = defaultId.Type,
                          .Value = defaultId.Value
                          })
            ElseIf defaultId.ValueSpecified Then
                Dim strDefaultType As String = GetDefaultType()
                If Not String.IsNullOrEmpty(strDefaultType) Then
                    Add(strDefaultType, defaultId.Value)
                End If
            End If
        End Sub

        Public Sub AddRange(ByVal idList As List(Of Uniqueid))
            For Each entry In idList
                Add(entry.Type, entry.Value)
            Next
        End Sub

        Public Sub AddRange(ByVal uniqueidContainer As UniqueidContainer)
            For Each entry In uniqueidContainer.Items
                Add(entry.Type, entry.Value)
            Next
        End Sub

        Public Function GetDefaultId() As DefaultId
            Dim nID = Items.FirstOrDefault(Function(f) f.IsDefault)
            If nID IsNot Nothing Then
                Return New DefaultId With {
                    .Value = nID.Value,
                    .Type = nID.Type
                }
            Else
                Return GetDefaultBySettings()
            End If
        End Function

        Public Function GetDefaultBySettings() As DefaultId
            Dim strDefaultType As String = GetDefaultType()
            If Not String.IsNullOrEmpty(strDefaultType) Then
                Dim nID = Items.FirstOrDefault(Function(f) f.Type = strDefaultType)
                If nID IsNot Nothing Then
                    Return New DefaultId With {
                        .Value = nID.Value,
                        .Type = nID.Type
                    }
                End If
            End If
            Return New DefaultId
        End Function

        Private Function GetDefaultType() As String
            Select Case _contentType
                Case Enums.ContentType.Movie
                    Return Master.eSettings.Movie.InformationSettings.UniqueId.DefaultType
                Case Enums.ContentType.Movieset
                    Return Master.eSettings.Movieset.InformationSettings.UniqueId.DefaultType
                Case Enums.ContentType.TV, Enums.ContentType.TVEpisode, Enums.ContentType.TVSeason, Enums.ContentType.TVShow
                    Return Master.eSettings.TVShow.InformationSettings.UniqueId.DefaultType
                Case Else
                    Return String.Empty
            End Select
        End Function

        Public Function GetIdByType(ByVal type As String) As String
            Dim nID = Items.FirstOrDefault(Function(f) f.Type.ToLower = type.ToLower)
            If nID IsNot Nothing Then
                Return nID.Value
            Else
                Return String.Empty
            End If
        End Function

        Public Function IsAvailable(ByVal type As String) As Boolean
            If Not String.IsNullOrEmpty(type) Then
                Return Items.FirstOrDefault(Function(f) f.Type = type) IsNot Nothing
            End If
            Return False
        End Function

        Private Sub RemoveAll(ByVal type As String)
            Items.RemoveAll(Function(f) f.Type = type)
        End Sub

#End Region 'Methods

    End Class

    <Serializable()>
    Public Class Video

#Region "Fields"

        Private _language As String = String.Empty
        Private _longLanguage As String = String.Empty

#End Region 'Fields

#Region "Properties"

        <XmlElement("aspect")>
        Public Property Aspect As Double = 0

        <XmlIgnore>
        Public ReadOnly Property AspectSpecified As Boolean
            Get
                Return Not Aspect = 0
            End Get
        End Property
        ''' <summary>
        ''' Resolution in bits (16, 24, 24)
        ''' </summary>
        ''' <returns></returns>
        <XmlElement("bitdepth")>
        Public Property BitDepth As Integer = 0

        <XmlIgnore>
        Public ReadOnly Property BitDepthSpecified As Boolean
            Get
                Return Not BitDepth = 0
            End Get
        End Property

        <XmlElement("bitrate")>
        Public Property Bitrate As Integer = 0

        <XmlIgnore>
        Public ReadOnly Property BitrateSpecified As Boolean
            Get
                Return Not Bitrate = 0
            End Get
        End Property

        <XmlElement("chromasubsampling")>
        Public Property ChromaSubsampling As String = String.Empty

        <XmlIgnore>
        Public ReadOnly Property ChromaSubsamplingSpecified As Boolean
            Get
                Return Not String.IsNullOrEmpty(ChromaSubsampling)
            End Get
        End Property

        <XmlElement("codec")>
        Public Property Codec As String = String.Empty

        <XmlIgnore>
        Public ReadOnly Property CodecSpecified As Boolean
            Get
                Return Not String.IsNullOrEmpty(Codec)
            End Get
        End Property

        <XmlElement("colourprimaries")>
        Public Property ColourPrimaries As String = String.Empty

        <XmlIgnore>
        Public ReadOnly Property ColourPrimariesSpecified As Boolean
            Get
                Return Not String.IsNullOrEmpty(ColourPrimaries)
            End Get
        End Property
        ''' <summary>
        ''' Duration in seconds
        ''' </summary>
        ''' <returns></returns>
        <XmlElement("durationinseconds")>
        Public Property Duration As Integer = 0

        <XmlIgnore()>
        Public ReadOnly Property DurationSpecified As Boolean
            Get
                Return Not Duration = 0
            End Get
        End Property

        <XmlElement("height")>
        Public Property Height As Integer = 0

        <XmlIgnore>
        Public ReadOnly Property HeightSpecified As Boolean
            Get
                Return Not Height = 0
            End Get
        End Property

        <XmlElement("language")>
        Public Property Language As String
            Get
                Return _language
            End Get
            Set(value As String)
                _language = Localization.Languages.Check_Alpha2_IsValid(value)
                _longLanguage = Localization.Languages.Get_Name_By_Alpha3(_language)
            End Set
        End Property

        <XmlIgnore>
        Public ReadOnly Property LanguageSpecified As Boolean
            Get
                Return Not String.IsNullOrEmpty(Language)
            End Get
        End Property

        <XmlElement("longlanguage")>
        Public Property LongLanguage As String
            Get
                Return _longLanguage
            End Get
            Set(value As String)
                _longLanguage = Localization.Languages.Check_Alpha2_IsValid(value)
                _language = Localization.Languages.Get_Alpha3_B_By_Name(value)
            End Set
        End Property

        <XmlIgnore>
        Public ReadOnly Property LongLanguageSpecified As Boolean
            Get
                Return Not String.IsNullOrEmpty(LongLanguage)
            End Get
        End Property

        <XmlElement("multiview_count")>
        Public Property MultiViewCount As Integer = 1

        <XmlIgnore>
        Public ReadOnly Property MultiViewCountSpecified As Boolean
            Get
                Return Not MultiViewCount = 0
            End Get
        End Property

        <XmlElement("multiview_layout")>
        Public Property MultiViewLayout As String = String.Empty

        <XmlIgnore>
        Public ReadOnly Property MultiViewLayoutSpecified As Boolean
            Get
                Return Not String.IsNullOrEmpty(MultiViewLayout)
            End Get
        End Property

        <XmlElement("scantype")>
        Public Property Scantype As String = String.Empty

        <XmlIgnore>
        Public ReadOnly Property ScantypeSpecified As Boolean
            Get
                Return Not String.IsNullOrEmpty(Scantype)
            End Get
        End Property

        <XmlIgnore>
        Public ReadOnly Property ShortStereoMode As String
            Get
                Return ConvertStereoModeToShortStereoMode(StereoMode).Trim()
            End Get
        End Property
        ''' <summary>
        ''' XBMC multiview layout type (http://wiki.xbmc.org/index.php?title=3D)
        ''' </summary>
        ''' <returns></returns>
        <XmlElement("stereomode")>
        Public Property StereoMode As String = String.Empty

        <XmlIgnore>
        Public ReadOnly Property StereoModeSpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(StereoMode)
            End Get
        End Property

        <XmlElement("width")>
        Public Property Width As Integer = 0

        <XmlIgnore>
        Public ReadOnly Property WidthSpecified As Boolean
            Get
                Return Not Width = 0
            End Get
        End Property
        ''' <summary>
        ''' for now save filesize in bytes(default)
        ''' </summary>
        ''' <returns></returns>
        <XmlElement("filesize")>
        Public Property Filesize() As Double = 0

        <XmlIgnore()>
        Public ReadOnly Property FilesizeSpecified() As Boolean
            Get
                Return Not Filesize = 0
            End Get
        End Property

#End Region 'Properties

#Region "Methods"

        Public Shared Function ConvertStereoModeToShortStereoMode(ByVal stereoMode As String) As String
            If Not String.IsNullOrEmpty(stereoMode) Then
                Dim tFormat As String = String.Empty
                Select Case stereoMode.ToLower
                    Case "bottom_top"
                        tFormat = "tab"
                    Case "left_right", "right_left"
                        tFormat = "sbs"
                    Case Else
                        tFormat = "unknown"
                End Select

                Return tFormat
            Else
                Return String.Empty
            End If
        End Function

#End Region 'Methods

    End Class

End Namespace