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

Imports EmberAPI
Imports NLog

Public Class Addon
    Implements Interfaces.IAddon_Data_Scraper_TV


#Region "Fields"

    Shared logger As Logger = LogManager.GetCurrentClassLogger()

    Public Shared ConfigScrapeOptions As New Structures.ScrapeOptions
    Public Shared ConfigScrapeModifiers As New Structures.ScrapeModifiers
    Public Shared _AssemblyName As String

    Private _SpecialSettings As New AddonSettings
    Private _Name As String = "TVDB_Data"
    Private _ScraperEnabled As Boolean = False
    Private _setup As frmSettingsHolder
    Private strPrivateAPIKey As String = String.Empty

    Private _AddonSettings As New AddonSettings

#End Region 'Fields

#Region "Events"

    Public Event AddonSettingsChanged() Implements Interfaces.IAddon_Data_Scraper_TV.AddonSettingsChanged
    Public Event AddonStateChanged(ByVal name As String, ByVal State As Boolean, ByVal difforder As Integer) Implements Interfaces.IAddon_Data_Scraper_TV.AddonStateChanged
    Public Event AddonNeedsRestart() Implements Interfaces.IAddon_Data_Scraper_TV.AddonNeedsRestart

#End Region 'Events

#Region "Properties"

    ReadOnly Property Name() As String Implements Interfaces.IAddon_Data_Scraper_TV.Name
        Get
            Return _Name
        End Get
    End Property

    ReadOnly Property Version() As String Implements Interfaces.IAddon_Data_Scraper_TV.Version
        Get
            Return System.Diagnostics.FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly.Location).FileVersion.ToString
        End Get
    End Property

    Property ScraperEnabled() As Boolean Implements Interfaces.IAddon_Data_Scraper_TV.ScraperEnabled
        Get
            Return _ScraperEnabled
        End Get
        Set(ByVal value As Boolean)
            _ScraperEnabled = value
        End Set
    End Property

#End Region 'Properties

#Region "Methods"

    Private Sub Handle_ModuleSettingsChanged()
        RaiseEvent AddonSettingsChanged()
    End Sub

    Private Sub Handle_SetupNeedsRestart()
        RaiseEvent AddonNeedsRestart()
    End Sub

    Private Sub Handle_SetupScraperChanged(ByVal state As Boolean, ByVal difforder As Integer)
        ScraperEnabled = state
        RaiseEvent AddonStateChanged(String.Concat(_Name, "_TV"), state, difforder)
    End Sub

    Sub Init(ByVal sAssemblyName As String) Implements Interfaces.IAddon_Data_Scraper_TV.Init
        _AssemblyName = sAssemblyName
        LoadSettings()
    End Sub

    Function InjectSettingsPanel() As Containers.SettingsPanel Implements Interfaces.IAddon_Data_Scraper_TV.InjectSettingsPanel
        Dim SPanel As New Containers.SettingsPanel
        _setup = New frmSettingsHolder
        LoadSettings()
        _setup.chkEnabled.Checked = _ScraperEnabled
        _setup.txtApiKey.Text = strPrivateAPIKey
        _setup.chkFallBackEng.Checked = _SpecialSettings.FallBackEng
        _setup.chkScraperEpisodeActors.Checked = ConfigScrapeOptions.Actors
        _setup.chkScraperEpisodeAired.Checked = ConfigScrapeOptions.Aired
        _setup.chkScraperEpisodeCredits.Checked = ConfigScrapeOptions.Credits
        _setup.chkScraperEpisodeDirectors.Checked = ConfigScrapeOptions.Directors
        _setup.chkScraperEpisodeGuestStars.Checked = ConfigScrapeOptions.GuestStars
        _setup.chkScraperEpisodePlot.Checked = ConfigScrapeOptions.Plot
        _setup.chkScraperEpisodeRating.Checked = ConfigScrapeOptions.bEpisodeRating
        _setup.chkScraperEpisodeTitle.Checked = ConfigScrapeOptions.Title
        _setup.chkScraperShowActors.Checked = ConfigScrapeOptions.Actors
        _setup.chkScraperShowEpisodeGuide.Checked = ConfigScrapeOptions.bMainEpisodeGuide
        _setup.chkScraperShowGenres.Checked = ConfigScrapeOptions.Genres
        _setup.chkScraperShowMPAA.Checked = ConfigScrapeOptions.bMainMPAA
        _setup.chkScraperShowPlot.Checked = ConfigScrapeOptions.Plot
        _setup.chkScraperShowPremiered.Checked = ConfigScrapeOptions.Premiered
        _setup.chkScraperShowRating.Checked = ConfigScrapeOptions.bMainRating
        _setup.chkScraperShowRuntime.Checked = ConfigScrapeOptions.Runtime
        _setup.chkScraperShowStatus.Checked = ConfigScrapeOptions.Status
        _setup.chkScraperShowStudios.Checked = ConfigScrapeOptions.Studios
        _setup.chkScraperShowTitle.Checked = ConfigScrapeOptions.Title

        If Not String.IsNullOrEmpty(strPrivateAPIKey) Then
            _setup.btnUnlockAPI.Text = Master.eLang.GetString(443, "Use embedded API Key")
            _setup.lblEMMAPI.Visible = False
            _setup.txtApiKey.Enabled = True
        End If

        _setup.orderChanged()

        SPanel.UniqueName = String.Concat(_Name, "_TV")
        SPanel.Title = "TheTVDb.com (old API)"
        SPanel.Order = 110
        SPanel.Parent = "pnlTVData"
        SPanel.Type = Master.eLang.GetString(653, "TV Shows")
        SPanel.ImageIndex = If(_ScraperEnabled, 9, 10)
        SPanel.Panel = _setup.pnlSettings

        AddHandler _setup.SetupScraperChanged, AddressOf Handle_SetupScraperChanged
        AddHandler _setup.ModuleSettingsChanged, AddressOf Handle_ModuleSettingsChanged
        AddHandler _setup.SetupNeedsRestart, AddressOf Handle_SetupNeedsRestart
        Return SPanel
    End Function

    Sub LoadSettings()
        ConfigScrapeOptions.Actors = Master.eAdvancedSettings.GetBooleanSetting("DoActors", True, , Enums.ContentType.TVEpisode)
        ConfigScrapeOptions.Aired = Master.eAdvancedSettings.GetBooleanSetting("DoAired", True, , Enums.ContentType.TVEpisode)
        ConfigScrapeOptions.Credits = Master.eAdvancedSettings.GetBooleanSetting("DoCredits", True, , Enums.ContentType.TVEpisode)
        ConfigScrapeOptions.Directors = Master.eAdvancedSettings.GetBooleanSetting("DoDirector", True, , Enums.ContentType.TVEpisode)
        ConfigScrapeOptions.GuestStars = Master.eAdvancedSettings.GetBooleanSetting("DoGuestStars", True, , Enums.ContentType.TVEpisode)
        ConfigScrapeOptions.Plot = Master.eAdvancedSettings.GetBooleanSetting("DoPlot", True, , Enums.ContentType.TVEpisode)
        ConfigScrapeOptions.bEpisodeRating = Master.eAdvancedSettings.GetBooleanSetting("DoRating", True, , Enums.ContentType.TVEpisode)
        ConfigScrapeOptions.Title = Master.eAdvancedSettings.GetBooleanSetting("DoTitle", True, , Enums.ContentType.TVEpisode)
        ConfigScrapeOptions.Actors = Master.eAdvancedSettings.GetBooleanSetting("DoActors", True, , Enums.ContentType.TVShow)
        ConfigScrapeOptions.bMainEpisodeGuide = Master.eAdvancedSettings.GetBooleanSetting("DoEpisodeGuide", False, , Enums.ContentType.TVShow)
        ConfigScrapeOptions.Genres = Master.eAdvancedSettings.GetBooleanSetting("DoGenre", True, , Enums.ContentType.TVShow)
        ConfigScrapeOptions.bMainMPAA = Master.eAdvancedSettings.GetBooleanSetting("DoMPAA", True, , Enums.ContentType.TVShow)
        ConfigScrapeOptions.Plot = Master.eAdvancedSettings.GetBooleanSetting("DoPlot", True, , Enums.ContentType.TVShow)
        ConfigScrapeOptions.Premiered = Master.eAdvancedSettings.GetBooleanSetting("DoPremiered", True, , Enums.ContentType.TVShow)
        ConfigScrapeOptions.bMainRating = Master.eAdvancedSettings.GetBooleanSetting("DoRating", True, , Enums.ContentType.TVShow)
        ConfigScrapeOptions.Runtime = Master.eAdvancedSettings.GetBooleanSetting("DoRuntime", True, , Enums.ContentType.TVShow)
        ConfigScrapeOptions.Status = Master.eAdvancedSettings.GetBooleanSetting("DoStatus", True, , Enums.ContentType.TVShow)
        ConfigScrapeOptions.Studios = Master.eAdvancedSettings.GetBooleanSetting("DoStudio", True, , Enums.ContentType.TVShow)
        ConfigScrapeOptions.Title = Master.eAdvancedSettings.GetBooleanSetting("DoTitle", True, , Enums.ContentType.TVShow)

        strPrivateAPIKey = Master.eAdvancedSettings.GetStringSetting("APIKey", "")
        _SpecialSettings.APIKey = If(String.IsNullOrEmpty(strPrivateAPIKey), "353783CE455412FD", strPrivateAPIKey)
        _SpecialSettings.FallBackEng = Master.eAdvancedSettings.GetBooleanSetting("FallBackEn", False, , Enums.ContentType.TV)
        ConfigScrapeModifiers.DoSearch = True
        ConfigScrapeModifiers.EpisodeMeta = True
        ConfigScrapeModifiers.MainNFO = True
    End Sub

    Sub SaveSettings()
        Using settings = New AdvancedSettings()
            settings.SetBooleanSetting("DoActors", ConfigScrapeOptions.Actors, , , Enums.ContentType.TVEpisode)
            settings.SetBooleanSetting("DoAired", ConfigScrapeOptions.Aired, , , Enums.ContentType.TVEpisode)
            settings.SetBooleanSetting("DoCredits", ConfigScrapeOptions.Credits, , , Enums.ContentType.TVEpisode)
            settings.SetBooleanSetting("DoDirector", ConfigScrapeOptions.Directors, , , Enums.ContentType.TVEpisode)
            settings.SetBooleanSetting("DoGuestStars", ConfigScrapeOptions.GuestStars, , , Enums.ContentType.TVEpisode)
            settings.SetBooleanSetting("DoPlot", ConfigScrapeOptions.Plot, , , Enums.ContentType.TVEpisode)
            settings.SetBooleanSetting("DoRating", ConfigScrapeOptions.bEpisodeRating, , , Enums.ContentType.TVEpisode)
            settings.SetBooleanSetting("DoTitle", ConfigScrapeOptions.Title, , , Enums.ContentType.TVEpisode)
            settings.SetBooleanSetting("DoActors", ConfigScrapeOptions.Actors, , , Enums.ContentType.TVShow)
            settings.SetBooleanSetting("DoEpisodeGuide", ConfigScrapeOptions.bMainEpisodeGuide, , , Enums.ContentType.TVShow)
            settings.SetBooleanSetting("DoGenre", ConfigScrapeOptions.Genres, , , Enums.ContentType.TVShow)
            settings.SetBooleanSetting("DoMPAA", ConfigScrapeOptions.bMainMPAA, , , Enums.ContentType.TVShow)
            settings.SetBooleanSetting("DoPlot", ConfigScrapeOptions.Plot, , , Enums.ContentType.TVShow)
            settings.SetBooleanSetting("DoPremiered", ConfigScrapeOptions.Premiered, , , Enums.ContentType.TVShow)
            settings.SetBooleanSetting("DoRating", ConfigScrapeOptions.bMainRating, , , Enums.ContentType.TVShow)
            settings.SetBooleanSetting("DoRuntime", ConfigScrapeOptions.Runtime, , , Enums.ContentType.TVShow)
            settings.SetBooleanSetting("DoStatus", ConfigScrapeOptions.Status, , , Enums.ContentType.TVShow)
            settings.SetBooleanSetting("DoStudio", ConfigScrapeOptions.Studios, , , Enums.ContentType.TVShow)
            settings.SetBooleanSetting("DoTitle", ConfigScrapeOptions.Title, , , Enums.ContentType.TVShow)
            settings.SetBooleanSetting("FallBackEn", _SpecialSettings.FallBackEng, , , Enums.ContentType.TV)
            settings.SetStringSetting("APIKey", _setup.txtApiKey.Text)
        End Using
    End Sub

    Sub SaveSettings(ByVal DoDispose As Boolean) Implements Interfaces.IAddon_Data_Scraper_TV.SaveSettings
        ConfigScrapeOptions.Actors = _setup.chkScraperEpisodeActors.Checked
        ConfigScrapeOptions.Aired = _setup.chkScraperEpisodeAired.Checked
        ConfigScrapeOptions.Credits = _setup.chkScraperEpisodeCredits.Checked
        ConfigScrapeOptions.Directors = _setup.chkScraperEpisodeDirectors.Checked
        ConfigScrapeOptions.GuestStars = _setup.chkScraperEpisodeGuestStars.Checked
        ConfigScrapeOptions.Plot = _setup.chkScraperEpisodePlot.Checked
        ConfigScrapeOptions.bEpisodeRating = _setup.chkScraperEpisodeRating.Checked
        ConfigScrapeOptions.Title = _setup.chkScraperEpisodeTitle.Checked
        ConfigScrapeOptions.Actors = _setup.chkScraperShowActors.Checked
        ConfigScrapeOptions.bMainEpisodeGuide = _setup.chkScraperShowEpisodeGuide.Checked
        ConfigScrapeOptions.Genres = _setup.chkScraperShowGenres.Checked
        ConfigScrapeOptions.bMainMPAA = _setup.chkScraperShowMPAA.Checked
        ConfigScrapeOptions.Plot = _setup.chkScraperShowPlot.Checked
        ConfigScrapeOptions.Premiered = _setup.chkScraperShowPremiered.Checked
        ConfigScrapeOptions.bMainRating = _setup.chkScraperShowRating.Checked
        ConfigScrapeOptions.Runtime = _setup.chkScraperShowRuntime.Checked
        ConfigScrapeOptions.Status = _setup.chkScraperShowStatus.Checked
        ConfigScrapeOptions.Studios = _setup.chkScraperShowStudios.Checked
        ConfigScrapeOptions.Title = _setup.chkScraperShowTitle.Checked
        _SpecialSettings.FallBackEng = _setup.chkFallBackEng.Checked
        SaveSettings()
        If DoDispose Then
            RemoveHandler _setup.SetupScraperChanged, AddressOf Handle_SetupScraperChanged
            RemoveHandler _setup.ModuleSettingsChanged, AddressOf Handle_ModuleSettingsChanged
            _setup.Dispose()
        End If
    End Sub

    Public Function Scraper_TVEpisode(ByRef dbElement As Database.DBElement,
                                      ByVal scrapeOptions As Structures.ScrapeOptions
                                      ) As Interfaces.AddonResult_Data_Scraper_TVEpisode Implements Interfaces.IAddon_Data_Scraper_TV.Scraper_TVEpisode
        logger.Trace("[TVDB_Data] [Scraper_TVEpisode] [Start]")

        LoadSettings()

        Dim AddonSettings As New AddonSettings With {
            .APIKey = _SpecialSettings.APIKey,
            .FallBackEng = _SpecialSettings.FallBackEng,
            .Language = dbElement.Language_Main
        }

        Dim FilteredOptions As Structures.ScrapeOptions = Functions.ScrapeOptionsAndAlso(scrapeOptions, ConfigScrapeOptions)
        Dim Result As New MediaContainers.EpisodeDetails
        Dim TVDbApi As New Scraper(AddonSettings)

        If dbElement.TVShow.UniqueIDs.TVDbIdSpecified Then
            If Not dbElement.MainDetails.Episode = -1 AndAlso Not DBElement.MainDetails.Season = -1 Then
                Result = TVDbApi.GetInfo_TVEpisode(dbElement.TVShow.UniqueIDs.TVDbId, DBElement.MainDetails.Season, dbElement.MainDetails.Episode, dbElement.Ordering, FilteredOptions)
            ElseIf DBElement.MainDetails.AiredSpecified Then
                Result = TVDbApi.GetInfo_TVEpisode(dbElement.TVShow.UniqueIDs.TVDbId, DBElement.MainDetails.Aired, FilteredOptions)
            Else
                logger.Trace("[TVDB_Data] [Scraper_TVEpisode] [Abort] No TV Show TVDb ID and also no AiredDate available")
                Return New Interfaces.AddonResult_Data_Scraper_TVEpisode(Interfaces.ResultStatus.NoResult)
            End If
        End If

        If Result IsNot Nothing Then
            logger.Trace("[TVDB_Data] [Scraper_TVEpisode] [Done]")
            Return New Interfaces.AddonResult_Data_Scraper_TVEpisode(Result)
        Else
            logger.Trace("[TVDB_Data] [Scraper_TVEpisode] [Abort] No result found")
            Return New Interfaces.AddonResult_Data_Scraper_TVEpisode(Interfaces.ResultStatus.NoResult)
        End If
    End Function

    Public Function Scraper_TVSeason(ByRef dbElement As Database.DBElement,
                                     ByVal scrapeOptions As Structures.ScrapeOptions
                                     ) As Interfaces.AddonResult_Data_Scraper_TVSeason Implements Interfaces.IAddon_Data_Scraper_TV.Scraper_TVSeason
        Return New Interfaces.AddonResult_Data_Scraper_TVSeason(Interfaces.ResultStatus.NoResult)
    End Function
    ''' <summary>
    '''  Scrape TVShowDetails from TVDB
    ''' </summary>
    ''' <param name="dbElement">TV Show to be scraped. DBTV as ByRef to use existing data for identifing tv show and to fill with IMDB/TVDB ID for next scraper</param>
    ''' <param name="scrapeOptions">What kind of data is being requested from the scrape(global scraper settings)</param>
    ''' <returns>Database.DBElement Object (nMovie) which contains the scraped data</returns>
    ''' <remarks></remarks>
    Function Scraper_TVShow(ByRef dbElement As Database.DBElement,
                            ByRef scrapeModifiers As Structures.ScrapeModifiers,
                            ByRef scrapeType As Enums.ScrapeType,
                            ByRef scrapeOptions As Structures.ScrapeOptions
                            ) As Interfaces.AddonResult_Data_Scraper_TVShow Implements Interfaces.IAddon_Data_Scraper_TV.Scraper_TVShow
        logger.Trace("[TVDB_Data] [Scraper_TV] [Start]")

        LoadSettings()

        Dim AddonSettings As New AddonSettings With {
            .APIKey = _SpecialSettings.APIKey,
            .FallBackEng = _SpecialSettings.FallBackEng,
            .Language = dbElement.Language_Main
        }

        Dim FilteredOptions As Structures.ScrapeOptions = Functions.ScrapeOptionsAndAlso(scrapeOptions, ConfigScrapeOptions)
        Dim Result As MediaContainers.TVShow = Nothing
        Dim TVDbApi As New Scraper(AddonSettings)

        If Not scrapeModifiers.DoSearch AndAlso
            (scrapeModifiers.MainNFO OrElse
            (scrapeModifiers.withEpisodes AndAlso scrapeModifiers.EpisodeNFO) OrElse
            (scrapeModifiers.withSeasons AndAlso scrapeModifiers.SeasonNFO)) Then
            If dbElement.TVShow.UniqueIDs.TVDbIdSpecified Then
                'TVDB-ID already available -> scrape and save data into an empty tv show container (nShow)
                Result = TVDbApi.GetInfo_TVShow(dbElement.TVShow.UniqueIDs.TVDbId.ToString, FilteredOptions, scrapeModifiers)
            ElseIf dbElement.TVShow.UniqueIDs.IMDbIdSpecified Then
                dbElement.TVShow.UniqueIDs.TVDbId = TVDbApi.GetTVDBbyIMDB(dbElement.TVShow.UniqueIDs.IMDbId)
                If Not dbElement.TVShow.UniqueIDs.TVDbIdSpecified Then Return New Interfaces.AddonResult_Data_Scraper_TVShow(Interfaces.ResultStatus.NoResult)
                Result = TVDbApi.GetInfo_TVShow(dbElement.TVShow.UniqueIDs.TVDbId.ToString, FilteredOptions, scrapeModifiers)
            ElseIf Not scrapeType = Enums.ScrapeType.SingleScrape Then
                'no TVDB-ID for tv show --> search first and try to get ID!
                If dbElement.MainDetails.TitleSpecified Then
                    Result = TVDbApi.Process_SearchResults_TVShow(dbElement.MainDetails.Title, dbElement, scrapeType, FilteredOptions, scrapeModifiers)
                End If
                'if still no search result -> exit
                If Result Is Nothing Then
                    logger.Trace("[TVDB_Data] [Scraper_TV] [Abort] No result found")
                    Return New Interfaces.AddonResult_Data_Scraper_TVShow(Interfaces.ResultStatus.NoResult)
                End If
            End If
        End If

        If Result Is Nothing Then
            Select Case scrapeType
                Case Enums.ScrapeType.AllAuto, Enums.ScrapeType.FilterAuto, Enums.ScrapeType.MarkedAuto, Enums.ScrapeType.MissingAuto, Enums.ScrapeType.NewAuto, Enums.ScrapeType.SelectedAuto
                    logger.Trace("[TVDB_Data] [Scraper_TV] [Abort] No result found")
                    Return New Interfaces.AddonResult_Data_Scraper_TVShow(Interfaces.ResultStatus.NoResult)
            End Select
        Else
            logger.Trace("[TVDB_Data] [Scraper_TV] [Done]")
            Return New Interfaces.AddonResult_Data_Scraper_TVShow(Result)
        End If

        If scrapeType = Enums.ScrapeType.SingleScrape OrElse scrapeType = Enums.ScrapeType.SingleAuto Then
            If Not dbElement.TVShow.UniqueIDs.TVDbIdSpecified Then
                Using dlgSearch As New dlgSearchResults(TVDbApi, "tvdb", New List(Of String) From {"TVDb", "IMDb"}, Enums.ContentType.TVShow)
                    Select Case dlgSearch.ShowDialog(dbElement.MainDetails.Title, dbElement.ShowPath)
                        Case DialogResult.Cancel
                            logger.Trace(String.Format("[TVDbv4_Data] [Scraper_TV] [Cancelled] Cancelled by user"))
                            Return New Interfaces.AddonResult_Data_Scraper_TVShow(Interfaces.ResultStatus.Cancelled)
                        Case DialogResult.OK
                            Return New Interfaces.AddonResult_Data_Scraper_TVShow(TVDbApi.GetInfo_TVShow(dlgSearch.Result_TVShow.UniqueIDs.TVDbId.ToString, FilteredOptions, scrapeModifiers))
                        Case DialogResult.Retry
                            logger.Trace(String.Format("[TVDB_Data] [Scraper_TV] [Skipped] Skipped by user"))
                            Return New Interfaces.AddonResult_Data_Scraper_TVShow(Interfaces.ResultStatus.Skipped)
                    End Select
                End Using
            End If
        End If

        If Result IsNot Nothing Then
            logger.Trace("[TVDB_Data] [Scraper_TV] [Done]")
            Return New Interfaces.AddonResult_Data_Scraper_TVShow(Result)
        Else
            logger.Trace("[TVDB_Data] [Scraper_TV] [Abort] No result found")
            Return New Interfaces.AddonResult_Data_Scraper_TVShow(Interfaces.ResultStatus.NoResult)
        End If
    End Function

    Public Sub ScraperOrderChanged() Implements Interfaces.IAddon_Data_Scraper_TV.ScraperOrderChanged
        _setup.orderChanged()
    End Sub

#End Region 'Methods

#Region "Nested Types"

    Structure AddonSettings

#Region "Fields"

        Dim APIKey As String
        Dim FallBackEng As Boolean
        Dim Language As String

#End Region 'Fields

    End Structure

#End Region 'Nested Types

End Class