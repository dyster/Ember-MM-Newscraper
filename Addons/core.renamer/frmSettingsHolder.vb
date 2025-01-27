﻿Imports EmberAPI
Imports System.IO

' ################################################################################
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

Public Class frmSettingsHolder
    Implements Interfaces.ISettingsPanel

#Region "Fields"

    Private _fDummyMultiEpisode As New Renamer.FileRename
    Private _fDummyMultiSeason As New Renamer.FileRename
    Private _fDummySingleEpisode As New Renamer.FileRename
    Private _fDummySingleMovie As New Renamer.FileRename

#End Region 'Fields

#Region "Events"

    Public Event ModuleEnabledChanged(ByVal State As Boolean)
    Public Event ModuleSettingsChanged()

    'interface events
    Public Event NeedsDBClean_Movie() Implements Interfaces.ISettingsPanel.NeedsDBClean_Movie
    Public Event NeedsDBClean_TV() Implements Interfaces.ISettingsPanel.NeedsDBClean_TV
    Public Event NeedsDBUpdate_Movie(ByVal id As Long) Implements Interfaces.ISettingsPanel.NeedsDBUpdate_Movie
    Public Event NeedsDBUpdate_TV(ByVal id As Long) Implements Interfaces.ISettingsPanel.NeedsDBUpdate_TV
    Public Event NeedsReload_Movie() Implements Interfaces.ISettingsPanel.NeedsReload_Movie
    Public Event NeedsReload_MovieSet() Implements Interfaces.ISettingsPanel.NeedsReload_Movieset
    Public Event NeedsReload_TVEpisode() Implements Interfaces.ISettingsPanel.NeedsReload_TVEpisode
    Public Event NeedsReload_TVShow() Implements Interfaces.ISettingsPanel.NeedsReload_TVShow
    Public Event NeedsRestart() Implements Interfaces.ISettingsPanel.NeedsRestart
    Public Event SettingsChanged() Implements Interfaces.ISettingsPanel.SettingsChanged
    Public Event StateChanged(ByVal uniqueSettingsPanelId As String, ByVal state As Boolean, ByVal diffOrder As Integer) Implements Interfaces.ISettingsPanel.StateChanged

#End Region 'Events

#Region "Properties"

    Public ReadOnly Property ChildType As Enums.SettingsPanelType Implements Interfaces.ISettingsPanel.ChildType

    Public Property Image As Image Implements Interfaces.ISettingsPanel.Image

    Public Property ImageIndex As Integer Implements Interfaces.ISettingsPanel.ImageIndex
        Get
            Return If(IsEnabled, 9, 10) 'TODO these values are copied from globalmapping, I don't know what they do
        End Get
        Set(value As Integer)
            Return
        End Set
    End Property

    Public Property IsEnabled As Boolean Implements Interfaces.ISettingsPanel.IsEnabled

    Public ReadOnly Property MainPanel As Panel Implements Interfaces.ISettingsPanel.MainPanel

    Public Property Order As Integer Implements Interfaces.ISettingsPanel.Order

    Public ReadOnly Property Title As String Implements Interfaces.ISettingsPanel.Title

    Public ReadOnly Property ParentType As Enums.SettingsPanelType Implements Interfaces.ISettingsPanel.ParentType

    Public Property UniqueId As String Implements Interfaces.ISettingsPanel.UniqueId

#End Region 'Properties

#Region "Interface Methods"

    Public Sub DoDispose() Implements Interfaces.ISettingsPanel.DoDispose
        Dispose()
    End Sub

    Public Sub Addon_Order_Changed(ByVal totalCount As Integer) Implements Interfaces.ISettingsPanel.OrderChanged
        Return
    End Sub

    Public Sub SaveSettings() Implements Interfaces.ISettingsPanel.SaveSettings
        Return
    End Sub

#End Region 'Interface Methods

#Region "Methods"

    Private Sub CreateDummies()
        _fDummyMultiEpisode = New Renamer.FileRename With {
            .AudioChannels = "2",
            .AudioCodec = "mp3",
            .BasePath = "",
            .Collection = "",
            .Countries = New List(Of String),
            .Directors = New List(Of String),
            .DirExist = False,
            .FileExist = False,
            .OldFileName = "OldFileName",
            .Genres = New List(Of String) From {{"Comedy"}, {"Lovestory"}},
            .ID = -1,
            .IMDB = "tt8440328",
            .IsBDMV = False,
            .IsLock = False,
            .IsMultiEpisode = True,
            .DoRename = False,
            .IsSingle = True,
            .IsVideoTS = False,
            .ListTitle = "Mess You Leave Behind, The",
            .MPAA = "TV-14",
            .MultiViewCount = "3d",
            .MultiViewLayout = "Side by Side (left eye first)",
            .NewFileName = "",
            .NewPath = "",
            .OldPath = "",
            .OriginalTitle = "El desorden que dejas",
            .Parent = "OldDirectoryName",
            .Path = "",
            .Rating = "7.3",
            .Resolution = "720p",
            .ShortStereoMode = "sbs",
            .ShowTitle = "The Mess You Leave Behind",
            .SortTitle = "Mess You Leave Behind",
            .StereoMode = "left_right",
            .Title = "Into the Lion's Den",
            .TVDBID = "58056",
            .VideoCodec = "xvid",
            .VideoSource = "dvd",
            .Year = "2007"
        }
        Dim dMEpisode1 As New Renamer.Episode With {.ID = 1, .Episode = 1, .Title = "Into the Lion's Den"}
        Dim dMEpisode2 As New Renamer.Episode With {.ID = 2, .Episode = 2, .Title = "They Know"}
        Dim dMEpisodeList As New List(Of Renamer.Episode)
        dMEpisodeList.Add(dMEpisode1)
        dMEpisodeList.Add(dMEpisode2)
        _fDummyMultiEpisode.SeasonsEpisodes.Add(New Renamer.SeasonsEpisodes With {.Season = 1, .Episodes = dMEpisodeList})

        _fDummyMultiSeason = New Renamer.FileRename With {
            .AudioChannels = "2",
            .AudioCodec = "mp3",
            .BasePath = "",
            .Collection = "",
            .Countries = New List(Of String),
            .Directors = New List(Of String),
            .DirExist = False,
            .FileExist = False,
            .OldFileName = "OldFileName",
            .Genres = New List(Of String) From {{"Comedy"}, {"Lovestory"}},
            .ID = -1,
            .IMDB = "tt8440328",
            .IsBDMV = False,
            .IsLock = False,
            .IsMultiEpisode = True,
            .DoRename = False,
            .IsSingle = True,
            .IsVideoTS = False,
            .ListTitle = "Mess You Leave Behind, The",
            .MPAA = "TV-14",
            .MultiViewCount = "3d",
            .MultiViewLayout = "Side by Side (left eye first)",
            .NewFileName = "",
            .NewPath = "",
            .OldPath = "",
            .OriginalTitle = "El desorden que dejas",
            .Parent = "OldDirectoryName",
            .Path = "",
            .Rating = "7.3",
            .Resolution = "720p",
            .ShortStereoMode = "sbs",
            .ShowTitle = "The Mess You Leave Behind",
            .SortTitle = "Mess You Leave Behind",
            .StereoMode = "left_right",
            .Title = "Into the Lion's Den",
            .TVDBID = "58056",
            .VideoCodec = "xvid",
            .VideoSource = "dvd",
            .Year = "2007"
        }
        Dim dMSEpisode1 As New Renamer.Episode With {.ID = 1, .Episode = 1, .Title = "Into the Lion's Den"}
        Dim dMSEpisode2 As New Renamer.Episode With {.ID = 2, .Episode = 2, .Title = "They Know"}
        Dim dMSEpisode3 As New Renamer.Episode With {.ID = 3, .Episode = 1, .Title = "Count to Three"}
        Dim dMSEpisode4 As New Renamer.Episode With {.ID = 4, .Episode = 2, .Title = "Downward Spiral"}
        Dim dMSEpisodeList1 As New List(Of Renamer.Episode)
        Dim dMSEpisodeList2 As New List(Of Renamer.Episode)
        dMSEpisodeList1.Add(dMSEpisode1)
        dMSEpisodeList1.Add(dMSEpisode2)
        dMSEpisodeList2.Add(dMSEpisode3)
        dMSEpisodeList2.Add(dMSEpisode4)
        _fDummyMultiSeason.SeasonsEpisodes.Add(New Renamer.SeasonsEpisodes With {.Season = 1, .Episodes = dMSEpisodeList1})
        _fDummyMultiSeason.SeasonsEpisodes.Add(New Renamer.SeasonsEpisodes With {.Season = 2, .Episodes = dMSEpisodeList2})

        _fDummySingleEpisode = New Renamer.FileRename With {
            .AudioChannels = "2",
            .AudioCodec = "mp3",
            .BasePath = "",
            .Collection = "",
            .Countries = New List(Of String),
            .Directors = New List(Of String),
            .DirExist = False,
            .FileExist = False,
            .OldFileName = "OldFileName",
            .Genres = New List(Of String) From {{"Comedy"}, {"Lovestory"}},
            .ID = -1,
            .IMDB = "tt8440328",
            .IsBDMV = False,
            .IsLock = False,
            .IsMultiEpisode = False,
            .DoRename = False,
            .IsSingle = True,
            .IsVideoTS = False,
            .ListTitle = "Mess You Leave Behind, The",
            .MPAA = "TV-14",
            .MultiViewCount = "3d",
            .MultiViewLayout = "Side by Side (left eye first)",
            .NewFileName = "",
            .NewPath = "",
            .OldPath = "",
            .OriginalTitle = "El desorden que dejas",
            .Parent = "OldDirectoryName",
            .Path = "",
            .Rating = "7.3",
            .Resolution = "720p",
            .ShortStereoMode = "sbs",
            .ShowTitle = "The Mess You Leave Behind",
            .SortTitle = "Mess You Leave Behind",
            .StereoMode = "left_right",
            .Title = "Into the Lion's Den",
            .TVDBID = "58056",
            .VideoCodec = "xvid",
            .VideoSource = "dvd",
            .Year = "2007"
        }
        Dim dSEpisode As New Renamer.Episode With {.ID = 1, .Episode = 1, .Title = "Into the Lion's Den"}
        Dim dSEpisodeList As New List(Of Renamer.Episode)
        dSEpisodeList.Add(dSEpisode)
        _fDummySingleEpisode.SeasonsEpisodes.Add(New Renamer.SeasonsEpisodes With {.Season = 1, .Episodes = dSEpisodeList})

        _fDummySingleMovie = New Renamer.FileRename With {
            .AudioChannels = "6",
            .AudioCodec = "dts",
            .BasePath = "D:\Movies",
            .Collection = "The Avengers Collection",
            .CollectionListTitle = "Avengers Collection, The",
            .Countries = New List(Of String) From {{"United States of America"}, {"Japan"}},
            .Directors = New List(Of String) From {{"Joss Whedon"}},
            .Edition = "Direcor's Cut",
            .DirExist = False,
            .FileExist = False,
            .OldFileName = "OldFileName",
            .Genres = New List(Of String) From {{"Action"}, {"Sci-Fi"}},
            .ID = -1,
            .IMDB = "tt0848228",
            .IsBDMV = False,
            .IsLock = False,
            .IsMultiEpisode = False,
            .DoRename = False,
            .IsSingle = True,
            .IsVideoTS = False,
            .ListTitle = "Avengers, The",
            .MPAA = "13",
            .MultiViewCount = "3d",
            .MultiViewLayout = "Side by Side (left eye first)",
            .NewFileName = "",
            .NewPath = "",
            .OldPath = "",
            .OriginalTitle = "Marvel's The Avengers",
            .Parent = "OldDirectoryName",
            .Path = "",
            .Rating = "7.3",
            .Resolution = "1080p",
            .ShortStereoMode = "sbs",
            .ShowTitle = "",
            .SortTitle = "Avengers",
            .StereoMode = "left_right",
            .Title = "The Avengers",
            .TVDBID = "",
            .VideoCodec = "h264",
            .VideoSource = "bluray",
            .Year = "2012"
        }
    End Sub

    Private Sub CreatePreview_MultiEpisode()
        If Not String.IsNullOrEmpty(txtFilePatternEpisodes.Text) AndAlso Not String.IsNullOrEmpty(txtFolderPatternShows.Text) Then
            Dim dFilename As String = Renamer.ProccessPattern(_fDummyMultiEpisode, txtFilePatternEpisodes.Text, False, False)
            Dim dSeasonPath As String = Renamer.ProccessPattern(_fDummyMultiEpisode, txtFolderPatternSeasons.Text, True, False)
            Dim dShowPath As String = Renamer.ProccessPattern(_fDummyMultiEpisode, txtFolderPatternShows.Text, True, False)

            txtMultiEpisodeFile.Text = Path.Combine(dShowPath, dSeasonPath, dFilename)
        Else
            txtMultiEpisodeFile.Text = String.Empty
        End If
    End Sub

    Private Sub CreatePreview_MultiSeason()
        If Not String.IsNullOrEmpty(txtFilePatternEpisodes.Text) AndAlso Not String.IsNullOrEmpty(txtFolderPatternShows.Text) Then
            Dim dFilename As String = Renamer.ProccessPattern(_fDummyMultiSeason, txtFilePatternEpisodes.Text, False, False)
            Dim dSeasonPath As String = Renamer.ProccessPattern(_fDummyMultiSeason, txtFolderPatternSeasons.Text, True, False)
            Dim dShowPath As String = Renamer.ProccessPattern(_fDummyMultiSeason, txtFolderPatternShows.Text, True, False)

            txtMultiSeasonFile.Text = Path.Combine(dShowPath, dSeasonPath, dFilename)
        Else
            txtMultiSeasonFile.Text = String.Empty
        End If
    End Sub

    Private Sub CreatePreview_SingleEpisode()
        If Not String.IsNullOrEmpty(txtFilePatternEpisodes.Text) AndAlso Not String.IsNullOrEmpty(txtFolderPatternShows.Text) Then
            Dim dFilename As String = Renamer.ProccessPattern(_fDummySingleEpisode, txtFilePatternEpisodes.Text, False, False)
            Dim dSeasonPath As String = Renamer.ProccessPattern(_fDummySingleEpisode, txtFolderPatternSeasons.Text, True, False)
            Dim dShowPath As String = Renamer.ProccessPattern(_fDummySingleEpisode, txtFolderPatternShows.Text, True, False)

            txtSingleEpisodeFile.Text = Path.Combine(dShowPath, dSeasonPath, dFilename)
        Else
            txtSingleEpisodeFile.Text = String.Empty
        End If
    End Sub

    Private Sub CreatePreview_SingleMovie()
        If Not String.IsNullOrEmpty(txtFilePatternMovies.Text) AndAlso Not String.IsNullOrEmpty(txtFolderPatternMovies.Text) Then
            Dim dFilename As String = Renamer.ProccessPattern(_fDummySingleMovie, txtFilePatternMovies.Text, False, False)
            Dim dPath As String = Renamer.ProccessPattern(_fDummySingleMovie, txtFolderPatternMovies.Text, True, False)

            txtSingleMovieFile.Text = Path.Combine(dPath, dFilename)
        Else
            txtSingleMovieFile.Text = String.Empty
        End If
    End Sub

    Private Sub btnFilePatternEpisodesReset_Click(sender As Object, e As EventArgs) Handles btnFilePatternEpisodesReset.Click
        txtFilePatternEpisodes.Text = "$Z - $W2_S?2E?{ - $T}"
    End Sub

    Private Sub btnFilePatternMoviesReset_Click(sender As Object, e As EventArgs) Handles btnFilePatternMoviesReset.Click
        txtFilePatternMovies.Text = "$T{.$S}"
    End Sub

    Private Sub btnFolderPatternMoviesReset_Click(sender As Object, e As EventArgs) Handles btnFolderPatternMoviesReset.Click
        txtFolderPatternMovies.Text = "$T{ ($6)}{ ($Y)}"
    End Sub

    Private Sub btnFolderPatternSeasonsReset_Click(sender As Object, e As EventArgs) Handles btnFolderPatternSeasonsReset.Click
        txtFolderPatternSeasons.Text = "Season $K2_?"
    End Sub

    Private Sub btnFolderPatternShowsReset_Click(sender As Object, e As EventArgs) Handles btnFolderPatternShowsReset.Click
        txtFolderPatternShows.Text = "$Z"
    End Sub

    Private Sub chkEnabled_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkEnabled.CheckedChanged
        RaiseEvent ModuleEnabledChanged(chkEnabled.Checked)
    End Sub

    Private Sub chkRenameEditMovies_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkRenameEditMovies.CheckedChanged
        RaiseEvent ModuleSettingsChanged()
    End Sub

    Private Sub chkRenameEditEpisodes_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkRenameEditEpisodes.CheckedChanged
        RaiseEvent ModuleSettingsChanged()
    End Sub

    Private Sub chkRenameMulti_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkRenameMultiMovies.CheckedChanged
        RaiseEvent ModuleSettingsChanged()
    End Sub

    Private Sub chkRenameMultiShows_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkRenameMultiShows.CheckedChanged
        RaiseEvent ModuleSettingsChanged()
    End Sub

    Private Sub chkRenameSingleMovies_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkRenameSingleMovies.CheckedChanged
        RaiseEvent ModuleSettingsChanged()
    End Sub

    Private Sub chkRenameSingleShows_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkRenameSingleShows.CheckedChanged
        RaiseEvent ModuleSettingsChanged()
    End Sub

    Private Sub chkRenameUpdateEpisodes_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkRenameUpdateEpisodes.CheckedChanged
        RaiseEvent ModuleSettingsChanged()
    End Sub

    Sub SetUp()
        chkRenameEditMovies.Text = Master.eLang.GetString(466, "Automatically Rename Files After Edit")
        chkRenameEditEpisodes.Text = Master.eLang.GetString(467, "Automatically Rename Files After Edit Episodes")
        chkRenameMultiMovies.Text = Master.eLang.GetString(281, "Automatically Rename Files During Multi-Scraper")
        chkRenameSingleMovies.Text = Master.eLang.GetString(282, "Automatically Rename Files During Single-Scraper")
        chkRenameUpdateEpisodes.Text = Master.eLang.GetString(468, "Automatically Rename Files During DB Update")
        gbRenamerPatternsMovie.Text = Master.eLang.GetString(285, "Default Movie Renaming Patterns")
        gbRenamerPatternsTV.Text = Master.eLang.GetString(470, "Default TV Renaming Patterns")
        lblFilePatternEpisodes.Text = Master.eLang.GetString(469, "Episode Files Pattern")
        lblFilePatternMovies.Text = Master.eLang.GetString(286, "Files Pattern")
        lblFolderPatternMovies.Text = Master.eLang.GetString(287, "Folders Pattern")
        chkEnabled.Text = Master.eLang.GetString(774, "Enabled")
        lblTips.Text = String.Format(Master.eLang.GetString(262, "$1 = First Letter of the Title{0}$2 = Aired date (episodes only){0}$3 = ShortStereoMode{0}$4 = StereoMode{0}$5 = Collection List Title{0}$6 = Edition{0}$A = Audio Channels{0}$B = Base Path{0}$C = Director{0}$D = Directory{0}$E = Sort Title{0}$F = File Name{0}$G = Genre (Follow with a space, dot or hyphen, comma to change separator){0}$H = Video Codec{0}$I = IMDB ID{0}$J = Audio Codec{0}$K#.S? = #Padding (0-9), Season Separator (. or _ or x), Season Prefix{0}$L = List Title{0}$M = MPAA{0}$N = Collection Name{0}$O = OriginalTitle{0}$OO = OriginalTitle if different from Title{0}$P = Rating{0}$Q#.E? = #Padding (0-9), Episode Separator (. or _ or x), Episode Prefix{0}$R = Resolution{0}$S = Video Source{0}$T = Title{0}$U = Country (Follow with a space, dot, comma or hyphen to change separator){0}$V = 3D (If Multiview > 1){0}$W#.S?#.E? = #Padding (0-9), Seasons Separator (. or _), Season Prefix, #Padding (0-9), Episode Separator (. or _ or x), Episode Prefix{0}$Y = Year{0}$X. (Replace Space with .){0}$Z = Show Title{0}{{}} = Optional{0}$?aaa?bbb? = Replace aaa with bbb{0}$! = Uppercase first letter in each word{0}$; = Lowercase all letters{0}$- = Remove previous char if next pattern does not have a value{0}$+ = Remove next char if previous pattern does not have a value{0}$^ = Remove previous and next char if next pattern does not have a value"), Environment.NewLine)
    End Sub

    Private Sub txtFilePatternMovies_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtFilePatternMovies.TextChanged
        RaiseEvent ModuleSettingsChanged()
        CreatePreview_SingleMovie()
    End Sub

    Private Sub txtFolderPatternMovies_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtFolderPatternMovies.TextChanged
        RaiseEvent ModuleSettingsChanged()
        CreatePreview_SingleMovie()
    End Sub

    Private Sub txtFolderPatternSeasons_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtFolderPatternSeasons.TextChanged
        RaiseEvent ModuleSettingsChanged()
        CreatePreview_MultiEpisode()
        CreatePreview_MultiSeason()
        CreatePreview_SingleEpisode()
    End Sub

    Private Sub txtFolderPatternShows_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtFolderPatternShows.TextChanged
        RaiseEvent ModuleSettingsChanged()
        CreatePreview_MultiEpisode()
        CreatePreview_MultiSeason()
        CreatePreview_SingleEpisode()
    End Sub

    Private Sub txtFilePatternEpisodes_TextChanged(sender As Object, e As EventArgs) Handles txtFilePatternEpisodes.TextChanged
        RaiseEvent ModuleSettingsChanged()
        CreatePreview_MultiEpisode()
        CreatePreview_MultiSeason()
        CreatePreview_SingleEpisode()
    End Sub

    Public Sub New()
        InitializeComponent()
        Title = Master.eLang.GetString(295, "Renamer")
        MainPanel = pnlSettings()
        SetUp()
        CreateDummies()
    End Sub

#End Region 'Methods

End Class