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


Imports EmberAPI
Imports NLog

Public Class Addon
    Implements Interfaces.IAddon_Data_Scraper_Movie
    Implements Interfaces.IAddon_Data_Scraper_TV


#Region "Fields"

    Shared logger As Logger = LogManager.GetCurrentClassLogger()

    Public Shared _AssemblyName As String
    Public Shared ConfigScrapeOptions_Movie As New Structures.ScrapeOptions
    Public Shared ConfigScrapeOptions_TV As New Structures.ScrapeOptions
    Public Shared ConfigScrapeModifier_Movie As New Structures.ScrapeModifiers
    Public Shared ConfigScrapeModifier_TV As New Structures.ScrapeModifiers

    Private _SpecialSettings_Movie As New AddonSettings
    Private _SpecialSettings_TV As New AddonSettings
    Private _Name As String = "IMDB_Data"
    Private _ScraperEnabled_Movie As Boolean = False
    Private _ScraperEnabled_TV As Boolean = False
    Private _setup_Movie As frmSettingsHolder_Movie
    Private _setup_TV As frmSettingsHolder_TV

#End Region 'Fields

#Region "Events"

    'Movie part
    Public Event ModuleSettingsChanged_Movie() Implements Interfaces.IAddon_Data_Scraper_Movie.AddonSettingsChanged
    Public Event ScraperSetupChanged_Movie(ByVal name As String, ByVal State As Boolean, ByVal difforder As Integer) Implements Interfaces.IAddon_Data_Scraper_Movie.AddonStateChanged
    Public Event SetupNeedsRestart_Movie() Implements Interfaces.IAddon_Data_Scraper_Movie.AddonNeedsRestart

    'TV part
    Public Event ModuleSettingsChanged_TV() Implements Interfaces.IAddon_Data_Scraper_TV.AddonSettingsChanged
    Public Event ScraperSetupChanged_TV(ByVal name As String, ByVal State As Boolean, ByVal difforder As Integer) Implements Interfaces.IAddon_Data_Scraper_TV.AddonStateChanged
    Public Event SetupNeedsRestart_TV() Implements Interfaces.IAddon_Data_Scraper_TV.AddonNeedsRestart

#End Region 'Events

#Region "Properties"

    Public ReadOnly Property ModuleName() As String Implements Interfaces.IAddon_Data_Scraper_Movie.ModuleName, Interfaces.IAddon_Data_Scraper_TV.ModuleName
        Get
            Return _Name
        End Get
    End Property

    Public ReadOnly Property ModuleVersion() As String Implements Interfaces.IAddon_Data_Scraper_Movie.ModuleVersion, Interfaces.IAddon_Data_Scraper_TV.ModuleVersion
        Get
            Return FileVersionInfo.GetVersionInfo(Reflection.Assembly.GetExecutingAssembly.Location).FileVersion.ToString
        End Get
    End Property

    Public Property ScraperEnabled_Movie() As Boolean Implements Interfaces.IAddon_Data_Scraper_Movie.ScraperEnabled
        Get
            Return _ScraperEnabled_Movie
        End Get
        Set(ByVal value As Boolean)
            _ScraperEnabled_Movie = value
        End Set
    End Property

    Public Property ScraperEnabled_TV() As Boolean Implements Interfaces.IAddon_Data_Scraper_TV.ScraperEnabled
        Get
            Return _ScraperEnabled_TV
        End Get
        Set(ByVal value As Boolean)
            _ScraperEnabled_TV = value
        End Set
    End Property

#End Region 'Properties

#Region "Methods"

    Private Sub Handle_ModuleSettingsChanged_Movie()
        RaiseEvent ModuleSettingsChanged_Movie()
    End Sub

    Private Sub Handle_ModuleSettingsChanged_TV()
        RaiseEvent ModuleSettingsChanged_TV()
    End Sub

    Private Sub Handle_SetupNeedsRestart_Movie()
        RaiseEvent SetupNeedsRestart_Movie()
    End Sub

    Private Sub Handle_SetupNeedsRestart_TV()
        RaiseEvent SetupNeedsRestart_TV()
    End Sub

    Private Sub Handle_SetupScraperChanged_Movie(ByVal state As Boolean, ByVal difforder As Integer)
        ScraperEnabled_Movie = state
        RaiseEvent ScraperSetupChanged_Movie(String.Concat(_Name, "_Movie"), state, difforder)
    End Sub

    Private Sub Handle_SetupScraperChanged_TV(ByVal state As Boolean, ByVal difforder As Integer)
        ScraperEnabled_TV = state
        RaiseEvent ScraperSetupChanged_TV(String.Concat(_Name, "_TV"), state, difforder)
    End Sub

    Sub Init_Movie(ByVal sAssemblyName As String) Implements Interfaces.IAddon_Data_Scraper_Movie.Init
        _AssemblyName = sAssemblyName
        LoadSettings_Movie()
    End Sub

    Sub Init_TV(ByVal sAssemblyName As String) Implements Interfaces.IAddon_Data_Scraper_TV.Init
        _AssemblyName = sAssemblyName
        LoadSettings_TV()
    End Sub

    Function InjectSetupScraper_Movie() As Containers.SettingsPanel Implements Interfaces.IAddon_Data_Scraper_Movie.InjectSetupScraper
        Dim SPanel As New Containers.SettingsPanel
        _setup_Movie = New frmSettingsHolder_Movie
        LoadSettings_Movie()
        _setup_Movie.chkEnabled.Checked = _ScraperEnabled_Movie

        _setup_Movie.chkActors.Checked = ConfigScrapeOptions_Movie.Actors
        _setup_Movie.chkCertifications.Checked = ConfigScrapeOptions_Movie.Certifications
        _setup_Movie.chkCountries.Checked = ConfigScrapeOptions_Movie.Countries
        _setup_Movie.chkDirectors.Checked = ConfigScrapeOptions_Movie.Directors
        _setup_Movie.chkGenres.Checked = ConfigScrapeOptions_Movie.Genres
        _setup_Movie.chkMPAA.Checked = ConfigScrapeOptions_Movie.bMainMPAA
        _setup_Movie.chkOriginalTitle.Checked = ConfigScrapeOptions_Movie.OriginalTitle
        _setup_Movie.chkOutline.Checked = ConfigScrapeOptions_Movie.bMainOutline
        _setup_Movie.chkPlot.Checked = ConfigScrapeOptions_Movie.Plot
        _setup_Movie.chkPremiered.Checked = ConfigScrapeOptions_Movie.Premiered
        _setup_Movie.chkRating.Checked = ConfigScrapeOptions_Movie.bMainRating
        _setup_Movie.chkRuntime.Checked = ConfigScrapeOptions_Movie.Runtime
        _setup_Movie.chkStudios.Checked = ConfigScrapeOptions_Movie.Studios
        _setup_Movie.chkTagline.Checked = ConfigScrapeOptions_Movie.Tagline
        _setup_Movie.chkTitle.Checked = ConfigScrapeOptions_Movie.Title
        _setup_Movie.chkTop250.Checked = ConfigScrapeOptions_Movie.bMainTop250
        _setup_Movie.chkWriters.Checked = ConfigScrapeOptions_Movie.bMainWriters

        _setup_Movie.cbForceTitleLanguage.Text = _SpecialSettings_Movie.ForceTitleLanguage
        _setup_Movie.chkFallBackworldwide.Checked = _SpecialSettings_Movie.FallBackWorldwide
        _setup_Movie.chkMPAADescription.Checked = _SpecialSettings_Movie.MPAADescription
        _setup_Movie.chkPartialTitles.Checked = _SpecialSettings_Movie.SearchPartialTitles
        _setup_Movie.chkPopularTitles.Checked = _SpecialSettings_Movie.SearchPopularTitles
        _setup_Movie.chkTvTitles.Checked = _SpecialSettings_Movie.SearchTvTitles
        _setup_Movie.chkVideoTitles.Checked = _SpecialSettings_Movie.SearchVideoTitles
        _setup_Movie.chkShortTitles.Checked = _SpecialSettings_Movie.SearchShortTitles
        _setup_Movie.chkStudiowithDistributors.Checked = _SpecialSettings_Movie.StudiowithDistributors

        _setup_Movie.orderChanged()

        SPanel.UniqueId = String.Concat(_Name, "_Movie")
        SPanel.Title = "IMDb.com"
        SPanel.Order = 110
        SPanel.Parent = "pnlMovieData"
        SPanel.Type = Master.eLang.GetString(36, "Movies")
        SPanel.ImageIndex = If(_ScraperEnabled_Movie, 9, 10)
        SPanel.Panel = _setup_Movie.pnlSettings

        AddHandler _setup_Movie.SetupScraperChanged, AddressOf Handle_SetupScraperChanged_Movie
        AddHandler _setup_Movie.ModuleSettingsChanged, AddressOf Handle_ModuleSettingsChanged_Movie
        Return SPanel
    End Function

    Function InjectSetupScraper_TV() As Containers.SettingsPanel Implements Interfaces.IAddon_Data_Scraper_TV.InjectSetupScraper
        Dim SPanel As New Containers.SettingsPanel
        _setup_TV = New frmSettingsHolder_TV
        LoadSettings_TV()
        _setup_TV.chkEnabled.Checked = _ScraperEnabled_TV

        _setup_TV.chkScraperEpActors.Checked = ConfigScrapeOptions_TV.Actors
        _setup_TV.chkScraperEpAired.Checked = ConfigScrapeOptions_TV.Aired
        _setup_TV.chkScraperEpCredits.Checked = ConfigScrapeOptions_TV.Credits
        _setup_TV.chkScraperEpDirectors.Checked = ConfigScrapeOptions_TV.Directors
        _setup_TV.chkScraperEpPlot.Checked = ConfigScrapeOptions_TV.Plot
        _setup_TV.chkScraperEpRating.Checked = ConfigScrapeOptions_TV.bEpisodeRating
        _setup_TV.chkScraperEpTitle.Checked = ConfigScrapeOptions_TV.Title
        _setup_TV.chkScraperShowActors.Checked = ConfigScrapeOptions_TV.Actors
        _setup_TV.chkScraperShowCertifications.Checked = ConfigScrapeOptions_TV.Certifications
        _setup_TV.chkScraperShowCountries.Checked = ConfigScrapeOptions_TV.Countries
        _setup_TV.chkScraperShowCreators.Checked = ConfigScrapeOptions_TV.Creators
        _setup_TV.chkScraperShowGenres.Checked = ConfigScrapeOptions_TV.Genres
        _setup_TV.chkScraperShowOriginalTitle.Checked = ConfigScrapeOptions_TV.OriginalTitle
        _setup_TV.chkScraperShowPlot.Checked = ConfigScrapeOptions_TV.Plot
        _setup_TV.chkScraperShowPremiered.Checked = ConfigScrapeOptions_TV.Premiered
        _setup_TV.chkScraperShowRating.Checked = ConfigScrapeOptions_TV.bMainRating
        _setup_TV.chkScraperShowRuntime.Checked = ConfigScrapeOptions_TV.Runtime
        _setup_TV.chkScraperShowStudios.Checked = ConfigScrapeOptions_TV.Studios
        _setup_TV.chkScraperShowTitle.Checked = ConfigScrapeOptions_TV.Title

        _setup_TV.cbForceTitleLanguage.Text = _SpecialSettings_TV.ForceTitleLanguage
        _setup_TV.chkFallBackworldwide.Checked = _SpecialSettings_TV.FallBackWorldwide

        _setup_TV.orderChanged()

        SPanel.UniqueId = String.Concat(_Name, "_TV")
        SPanel.Title = "IMDb.com"
        SPanel.Order = 110
        SPanel.Parent = "pnlTVData"
        SPanel.Type = Master.eLang.GetString(653, "TV Shows")
        SPanel.ImageIndex = If(_ScraperEnabled_TV, 9, 10)
        SPanel.Panel = _setup_TV.pnlSettings

        AddHandler _setup_TV.SetupScraperChanged, AddressOf Handle_SetupScraperChanged_TV
        AddHandler _setup_TV.ModuleSettingsChanged, AddressOf Handle_ModuleSettingsChanged_TV
        Return SPanel
    End Function

    Sub LoadSettings_Movie()
        ConfigScrapeOptions_Movie.Actors = Master.eAdvancedSettings.GetBooleanSetting("DoCast", True, , Enums.ContentType.Movie)
        ConfigScrapeOptions_Movie.Certifications = Master.eAdvancedSettings.GetBooleanSetting("DoCert", True, , Enums.ContentType.Movie)
        ConfigScrapeOptions_Movie.Countries = Master.eAdvancedSettings.GetBooleanSetting("DoCountry", True, , Enums.ContentType.Movie)
        ConfigScrapeOptions_Movie.Directors = Master.eAdvancedSettings.GetBooleanSetting("DoDirector", True, , Enums.ContentType.Movie)
        ConfigScrapeOptions_Movie.Genres = Master.eAdvancedSettings.GetBooleanSetting("DoGenres", True, , Enums.ContentType.Movie)
        ConfigScrapeOptions_Movie.bMainMPAA = Master.eAdvancedSettings.GetBooleanSetting("DoMPAA", True, , Enums.ContentType.Movie)
        ConfigScrapeOptions_Movie.OriginalTitle = Master.eAdvancedSettings.GetBooleanSetting("DoOriginalTitle", True, , Enums.ContentType.Movie)
        ConfigScrapeOptions_Movie.bMainOutline = Master.eAdvancedSettings.GetBooleanSetting("DoOutline", True, , Enums.ContentType.Movie)
        ConfigScrapeOptions_Movie.Plot = Master.eAdvancedSettings.GetBooleanSetting("DoPlot", True, , Enums.ContentType.Movie)
        ConfigScrapeOptions_Movie.Premiered = Master.eAdvancedSettings.GetBooleanSetting("DoPremiered", True, , Enums.ContentType.Movie)
        ConfigScrapeOptions_Movie.bMainRating = Master.eAdvancedSettings.GetBooleanSetting("DoRating", True, , Enums.ContentType.Movie)
        ConfigScrapeOptions_Movie.Runtime = Master.eAdvancedSettings.GetBooleanSetting("DoRuntime", True, , Enums.ContentType.Movie)
        ConfigScrapeOptions_Movie.Studios = Master.eAdvancedSettings.GetBooleanSetting("DoStudio", True, , Enums.ContentType.Movie)
        ConfigScrapeOptions_Movie.Tagline = Master.eAdvancedSettings.GetBooleanSetting("DoTagline", True, , Enums.ContentType.Movie)
        ConfigScrapeOptions_Movie.Title = Master.eAdvancedSettings.GetBooleanSetting("DoTitle", True, , Enums.ContentType.Movie)
        ConfigScrapeOptions_Movie.bMainTop250 = Master.eAdvancedSettings.GetBooleanSetting("DoTop250", True, , Enums.ContentType.Movie)
        ConfigScrapeOptions_Movie.bMainWriters = Master.eAdvancedSettings.GetBooleanSetting("DoWriters", True, , Enums.ContentType.Movie)

        _SpecialSettings_Movie.FallBackWorldwide = Master.eAdvancedSettings.GetBooleanSetting("FallBackWorldwide", False, , Enums.ContentType.Movie)
        _SpecialSettings_Movie.ForceTitleLanguage = Master.eAdvancedSettings.GetSetting("ForceTitleLanguage", String.Empty, , Enums.ContentType.Movie)
        _SpecialSettings_Movie.MPAADescription = Master.eAdvancedSettings.GetBooleanSetting("MPAADescription", False, , Enums.ContentType.Movie)
        _SpecialSettings_Movie.SearchPartialTitles = Master.eAdvancedSettings.GetBooleanSetting("SearchPartialTitles", True, , Enums.ContentType.Movie)
        _SpecialSettings_Movie.SearchPopularTitles = Master.eAdvancedSettings.GetBooleanSetting("SearchPopularTitles", True, , Enums.ContentType.Movie)
        _SpecialSettings_Movie.SearchTvTitles = Master.eAdvancedSettings.GetBooleanSetting("SearchTvTitles", False, , Enums.ContentType.Movie)
        _SpecialSettings_Movie.SearchVideoTitles = Master.eAdvancedSettings.GetBooleanSetting("SearchVideoTitles", False, , Enums.ContentType.Movie)
        _SpecialSettings_Movie.SearchShortTitles = Master.eAdvancedSettings.GetBooleanSetting("SearchShortTitles", False, , Enums.ContentType.Movie)
        _SpecialSettings_Movie.StudiowithDistributors = Master.eAdvancedSettings.GetBooleanSetting("StudiowithDistributors", False, , Enums.ContentType.Movie)
    End Sub

    Sub LoadSettings_TV()
        ConfigScrapeOptions_TV.Actors = Master.eAdvancedSettings.GetBooleanSetting("DoActors", True, , Enums.ContentType.TVEpisode)
        ConfigScrapeOptions_TV.Aired = Master.eAdvancedSettings.GetBooleanSetting("DoAired", True, , Enums.ContentType.TVEpisode)
        ConfigScrapeOptions_TV.Credits = Master.eAdvancedSettings.GetBooleanSetting("DoCredits", True, , Enums.ContentType.TVEpisode)
        ConfigScrapeOptions_TV.Directors = Master.eAdvancedSettings.GetBooleanSetting("DoDirector", True, , Enums.ContentType.TVEpisode)
        ConfigScrapeOptions_TV.Plot = Master.eAdvancedSettings.GetBooleanSetting("DoPlot", True, , Enums.ContentType.TVEpisode)
        ConfigScrapeOptions_TV.bEpisodeRating = Master.eAdvancedSettings.GetBooleanSetting("DoRating", True, , Enums.ContentType.TVEpisode)
        ConfigScrapeOptions_TV.Title = Master.eAdvancedSettings.GetBooleanSetting("DoTitle", True, , Enums.ContentType.TVEpisode)
        ConfigScrapeOptions_TV.Actors = Master.eAdvancedSettings.GetBooleanSetting("DoActors", True, , Enums.ContentType.TVShow)
        ConfigScrapeOptions_TV.Certifications = Master.eAdvancedSettings.GetBooleanSetting("DoCert", True, , Enums.ContentType.TVShow)
        ConfigScrapeOptions_TV.Countries = Master.eAdvancedSettings.GetBooleanSetting("DoCountry", True, , Enums.ContentType.TVShow)
        ConfigScrapeOptions_TV.Creators = Master.eAdvancedSettings.GetBooleanSetting("DoCreator", True, , Enums.ContentType.TVShow)
        ConfigScrapeOptions_TV.Genres = Master.eAdvancedSettings.GetBooleanSetting("DoGenre", True, , Enums.ContentType.TVShow)
        ConfigScrapeOptions_TV.OriginalTitle = Master.eAdvancedSettings.GetBooleanSetting("DoOriginalTitle", True, , Enums.ContentType.TVShow)
        ConfigScrapeOptions_TV.Plot = Master.eAdvancedSettings.GetBooleanSetting("DoPlot", True, , Enums.ContentType.TVShow)
        ConfigScrapeOptions_TV.Premiered = Master.eAdvancedSettings.GetBooleanSetting("DoPremiered", True, , Enums.ContentType.TVShow)
        ConfigScrapeOptions_TV.bMainRating = Master.eAdvancedSettings.GetBooleanSetting("DoRating", True, , Enums.ContentType.TVShow)
        ConfigScrapeOptions_TV.Runtime = Master.eAdvancedSettings.GetBooleanSetting("DoRuntime", True, , Enums.ContentType.TVShow)
        ConfigScrapeOptions_TV.Studios = Master.eAdvancedSettings.GetBooleanSetting("DoStudio", True, , Enums.ContentType.TVShow)
        ConfigScrapeOptions_TV.Title = Master.eAdvancedSettings.GetBooleanSetting("DoTitle", True, , Enums.ContentType.TVShow)

        _SpecialSettings_TV.FallBackWorldwide = Master.eAdvancedSettings.GetBooleanSetting("FallBackWorldwide", False, , Enums.ContentType.TVShow)
        _SpecialSettings_TV.ForceTitleLanguage = Master.eAdvancedSettings.GetSetting("ForceTitleLanguage", String.Empty, , Enums.ContentType.TVShow)
    End Sub

    Sub SaveSettings_Movie()
        Using settings = New AdvancedSettings()
            settings.SetBooleanSetting("DoCast", ConfigScrapeOptions_Movie.Actors, , , Enums.ContentType.Movie)
            settings.SetBooleanSetting("DoCert", ConfigScrapeOptions_Movie.Certifications, , , Enums.ContentType.Movie)
            settings.SetBooleanSetting("DoCountry", ConfigScrapeOptions_Movie.Countries, , , Enums.ContentType.Movie)
            settings.SetBooleanSetting("DoDirector", ConfigScrapeOptions_Movie.Directors, , , Enums.ContentType.Movie)
            settings.SetBooleanSetting("DoGenres", ConfigScrapeOptions_Movie.Genres, , , Enums.ContentType.Movie)
            settings.SetBooleanSetting("DoMPAA", ConfigScrapeOptions_Movie.bMainMPAA, , , Enums.ContentType.Movie)
            settings.SetBooleanSetting("DoOriginalTitle", ConfigScrapeOptions_Movie.OriginalTitle, , , Enums.ContentType.Movie)
            settings.SetBooleanSetting("DoOutline", ConfigScrapeOptions_Movie.bMainOutline, , , Enums.ContentType.Movie)
            settings.SetBooleanSetting("DoPlot", ConfigScrapeOptions_Movie.Plot, , , Enums.ContentType.Movie)
            settings.SetBooleanSetting("DoPremiered", ConfigScrapeOptions_Movie.Premiered, , , Enums.ContentType.Movie)
            settings.SetBooleanSetting("DoRating", ConfigScrapeOptions_Movie.bMainRating, , , Enums.ContentType.Movie)
            settings.SetBooleanSetting("DoRuntime", ConfigScrapeOptions_Movie.Runtime, , , Enums.ContentType.Movie)
            settings.SetBooleanSetting("DoStudio", ConfigScrapeOptions_Movie.Studios, , , Enums.ContentType.Movie)
            settings.SetBooleanSetting("DoTagline", ConfigScrapeOptions_Movie.Tagline, , , Enums.ContentType.Movie)
            settings.SetBooleanSetting("DoTitle", ConfigScrapeOptions_Movie.Title, , , Enums.ContentType.Movie)
            settings.SetBooleanSetting("DoTop250", ConfigScrapeOptions_Movie.bMainTop250, , , Enums.ContentType.Movie)
            settings.SetBooleanSetting("DoWriters", ConfigScrapeOptions_Movie.bMainWriters, , , Enums.ContentType.Movie)
            settings.SetBooleanSetting("FallBackWorldwide", _SpecialSettings_Movie.FallBackWorldwide, , , Enums.ContentType.Movie)
            settings.SetBooleanSetting("MPAADescription", _SpecialSettings_Movie.MPAADescription, , , Enums.ContentType.Movie)
            settings.SetBooleanSetting("SearchPartialTitles", _SpecialSettings_Movie.SearchPartialTitles, , , Enums.ContentType.Movie)
            settings.SetBooleanSetting("SearchPopularTitles", _SpecialSettings_Movie.SearchPopularTitles, , , Enums.ContentType.Movie)
            settings.SetBooleanSetting("SearchTvTitles", _SpecialSettings_Movie.SearchTvTitles, , , Enums.ContentType.Movie)
            settings.SetBooleanSetting("SearchVideoTitles", _SpecialSettings_Movie.SearchVideoTitles, , , Enums.ContentType.Movie)
            settings.SetBooleanSetting("SearchShortTitles", _SpecialSettings_Movie.SearchShortTitles, , , Enums.ContentType.Movie)
            settings.SetSetting("ForceTitleLanguage", _SpecialSettings_Movie.ForceTitleLanguage, , , Enums.ContentType.Movie)
            settings.SetBooleanSetting("StudiowithDistributors", _SpecialSettings_Movie.StudiowithDistributors, , , Enums.ContentType.Movie)
        End Using
    End Sub

    Sub SaveSettings_TV()
        Using settings = New AdvancedSettings()
            settings.SetBooleanSetting("DoActors", ConfigScrapeOptions_TV.Actors, , , Enums.ContentType.TVEpisode)
            settings.SetBooleanSetting("DoAired", ConfigScrapeOptions_TV.Aired, , , Enums.ContentType.TVEpisode)
            settings.SetBooleanSetting("DoCredits", ConfigScrapeOptions_TV.Credits, , , Enums.ContentType.TVEpisode)
            settings.SetBooleanSetting("DoDirector", ConfigScrapeOptions_TV.Directors, , , Enums.ContentType.TVEpisode)
            settings.SetBooleanSetting("DoPlot", ConfigScrapeOptions_TV.Plot, , , Enums.ContentType.TVEpisode)
            settings.SetBooleanSetting("DoRating", ConfigScrapeOptions_TV.bEpisodeRating, , , Enums.ContentType.TVEpisode)
            settings.SetBooleanSetting("DoTitle", ConfigScrapeOptions_TV.Title, , , Enums.ContentType.TVEpisode)
            settings.SetBooleanSetting("DoActors", ConfigScrapeOptions_TV.Actors, , , Enums.ContentType.TVShow)
            settings.SetBooleanSetting("DoCert", ConfigScrapeOptions_TV.Certifications, , , Enums.ContentType.TVShow)
            settings.SetBooleanSetting("DoCountry", ConfigScrapeOptions_TV.Countries, , , Enums.ContentType.TVShow)
            settings.SetBooleanSetting("DoCreator", ConfigScrapeOptions_TV.Creators, , , Enums.ContentType.TVShow)
            settings.SetBooleanSetting("DoGenre", ConfigScrapeOptions_TV.Genres, , , Enums.ContentType.TVShow)
            settings.SetBooleanSetting("DoOriginalTitle", ConfigScrapeOptions_TV.OriginalTitle, , , Enums.ContentType.TVShow)
            settings.SetBooleanSetting("DoPlot", ConfigScrapeOptions_TV.Plot, , , Enums.ContentType.TVShow)
            settings.SetBooleanSetting("DoPremiered", ConfigScrapeOptions_TV.Premiered, , , Enums.ContentType.TVShow)
            settings.SetBooleanSetting("DoRating", ConfigScrapeOptions_TV.bMainRating, , , Enums.ContentType.TVShow)
            settings.SetBooleanSetting("DoRuntime", ConfigScrapeOptions_TV.Runtime, , , Enums.ContentType.TVShow)
            settings.SetBooleanSetting("DoStudio", ConfigScrapeOptions_TV.Studios, , , Enums.ContentType.TVShow)
            settings.SetBooleanSetting("DoTitle", ConfigScrapeOptions_TV.Title, , , Enums.ContentType.TVShow)
            settings.SetBooleanSetting("FallBackWorldwide", _SpecialSettings_TV.FallBackWorldwide, , , Enums.ContentType.TVShow)
            settings.SetSetting("ForceTitleLanguage", _SpecialSettings_TV.ForceTitleLanguage, , , Enums.ContentType.TVShow)
        End Using
    End Sub

    Sub SaveSetupScraper_Movie(ByVal DoDispose As Boolean) Implements Interfaces.IAddon_Data_Scraper_Movie.SaveSetupScraper
        ConfigScrapeOptions_Movie.Actors = _setup_Movie.chkActors.Checked
        ConfigScrapeOptions_Movie.Certifications = _setup_Movie.chkCertifications.Checked
        ConfigScrapeOptions_Movie.Countries = _setup_Movie.chkCountries.Checked
        ConfigScrapeOptions_Movie.Directors = _setup_Movie.chkDirectors.Checked
        ConfigScrapeOptions_Movie.Genres = _setup_Movie.chkGenres.Checked
        ConfigScrapeOptions_Movie.bMainMPAA = _setup_Movie.chkMPAA.Checked
        ConfigScrapeOptions_Movie.OriginalTitle = _setup_Movie.chkOriginalTitle.Checked
        ConfigScrapeOptions_Movie.bMainOutline = _setup_Movie.chkOutline.Checked
        ConfigScrapeOptions_Movie.Plot = _setup_Movie.chkPlot.Checked
        ConfigScrapeOptions_Movie.Premiered = _setup_Movie.chkPremiered.Checked
        ConfigScrapeOptions_Movie.bMainRating = _setup_Movie.chkRating.Checked
        ConfigScrapeOptions_Movie.Runtime = _setup_Movie.chkRuntime.Checked
        ConfigScrapeOptions_Movie.Studios = _setup_Movie.chkStudios.Checked
        ConfigScrapeOptions_Movie.Tagline = _setup_Movie.chkTagline.Checked
        ConfigScrapeOptions_Movie.Title = _setup_Movie.chkTitle.Checked
        ConfigScrapeOptions_Movie.bMainTop250 = _setup_Movie.chkTop250.Checked
        ConfigScrapeOptions_Movie.bMainWriters = _setup_Movie.chkWriters.Checked

        _SpecialSettings_Movie.FallBackWorldwide = _setup_Movie.chkFallBackworldwide.Checked
        _SpecialSettings_Movie.ForceTitleLanguage = _setup_Movie.cbForceTitleLanguage.Text
        _SpecialSettings_Movie.MPAADescription = _setup_Movie.chkMPAADescription.Checked
        _SpecialSettings_Movie.SearchPartialTitles = _setup_Movie.chkPartialTitles.Checked
        _SpecialSettings_Movie.SearchPopularTitles = _setup_Movie.chkPopularTitles.Checked
        _SpecialSettings_Movie.SearchTvTitles = _setup_Movie.chkTvTitles.Checked
        _SpecialSettings_Movie.SearchVideoTitles = _setup_Movie.chkVideoTitles.Checked
        _SpecialSettings_Movie.SearchShortTitles = _setup_Movie.chkShortTitles.Checked
        _SpecialSettings_Movie.StudiowithDistributors = _setup_Movie.chkStudiowithDistributors.Checked

        SaveSettings_Movie()
        If DoDispose Then
            RemoveHandler _setup_Movie.SetupScraperChanged, AddressOf Handle_SetupScraperChanged_Movie
            RemoveHandler _setup_Movie.ModuleSettingsChanged, AddressOf Handle_ModuleSettingsChanged_Movie
            _setup_Movie.Dispose()
        End If
    End Sub

    Sub SaveSetupScraper_TV(ByVal DoDispose As Boolean) Implements Interfaces.IAddon_Data_Scraper_TV.SaveSetupScraper
        ConfigScrapeOptions_TV.Actors = _setup_TV.chkScraperEpActors.Checked
        ConfigScrapeOptions_TV.Aired = _setup_TV.chkScraperEpAired.Checked
        ConfigScrapeOptions_TV.Credits = _setup_TV.chkScraperEpCredits.Checked
        ConfigScrapeOptions_TV.Directors = _setup_TV.chkScraperEpDirectors.Checked
        ConfigScrapeOptions_TV.Plot = _setup_TV.chkScraperEpPlot.Checked
        ConfigScrapeOptions_TV.bEpisodeRating = _setup_TV.chkScraperEpRating.Checked
        ConfigScrapeOptions_TV.Title = _setup_TV.chkScraperEpTitle.Checked
        ConfigScrapeOptions_TV.Actors = _setup_TV.chkScraperShowActors.Checked
        ConfigScrapeOptions_TV.Certifications = _setup_TV.chkScraperShowCertifications.Checked
        ConfigScrapeOptions_TV.Countries = _setup_TV.chkScraperShowCountries.Checked
        ConfigScrapeOptions_TV.Creators = _setup_TV.chkScraperShowCreators.Checked
        ConfigScrapeOptions_TV.Genres = _setup_TV.chkScraperShowGenres.Checked
        ConfigScrapeOptions_TV.OriginalTitle = _setup_TV.chkScraperShowOriginalTitle.Checked
        ConfigScrapeOptions_TV.Plot = _setup_TV.chkScraperShowPlot.Checked
        ConfigScrapeOptions_TV.Premiered = _setup_TV.chkScraperShowPremiered.Checked
        ConfigScrapeOptions_TV.bMainRating = _setup_TV.chkScraperShowRating.Checked
        ConfigScrapeOptions_TV.Runtime = _setup_TV.chkScraperShowRuntime.Checked
        ConfigScrapeOptions_TV.Studios = _setup_TV.chkScraperShowStudios.Checked
        ConfigScrapeOptions_TV.Title = _setup_TV.chkScraperShowTitle.Checked

        _SpecialSettings_TV.FallBackWorldwide = _setup_TV.chkFallBackworldwide.Checked
        _SpecialSettings_TV.ForceTitleLanguage = _setup_TV.cbForceTitleLanguage.Text

        SaveSettings_TV()
        If DoDispose Then
            RemoveHandler _setup_TV.SetupScraperChanged, AddressOf Handle_SetupScraperChanged_TV
            RemoveHandler _setup_TV.ModuleSettingsChanged, AddressOf Handle_ModuleSettingsChanged_TV
            _setup_TV.Dispose()
        End If
    End Sub

    Function GetTMDbIdByIMDbId(ByVal imdbId As String, ByRef tmdbId As Integer) As Interfaces.AddonResult Implements Interfaces.IAddon_Data_Scraper_Movie.GetTMDbIdByIMDbId
        Return New Interfaces.AddonResult
    End Function
    ''' <summary>
    '''  Scrape MovieDetails from IMDB
    ''' </summary>
    ''' <param name="oDBElement">Movie to be scraped. oDBMovie as ByRef to use existing data for identifing movie and to fill with IMDB/TMDB ID for next scraper</param>
    ''' <param name="Options">What kind of data is being requested from the scrape(global scraper settings)</param>
    ''' <returns>Database.DBElement Object (nMovie) which contains the scraped data</returns>
    ''' <remarks></remarks>
    Function Scraper_Movie(ByRef oDBElement As Database.DBElement, ByRef ScrapeModifiers As Structures.ScrapeModifiers, ByRef ScrapeType As Enums.ScrapeType, ByRef ScrapeOptions As Structures.ScrapeOptions) As Interfaces.AddonResult_Data_Scraper_Movie Implements Interfaces.IAddon_Data_Scraper_Movie.Scraper_Movie
        logger.Trace("[IMDb_Data] [Scraper_Movie] [Start]")

        LoadSettings_Movie()

        Dim FilteredOptions As Structures.ScrapeOptions = Functions.ScrapeOptionsAndAlso(ScrapeOptions, ConfigScrapeOptions_Movie)
        Dim Result As MediaContainers.Movie = Nothing
        _SpecialSettings_Movie.PrefLanguage = oDBElement.Language
        Dim _scraper As New Scraper(_SpecialSettings_Movie)

        If ScrapeModifiers.MainNFO AndAlso Not ScrapeModifiers.DoSearch Then
            If Not String.IsNullOrEmpty(oDBElement.MainDetails.UniqueIDs.IMDbId) Then
                'IMDB-ID already available -> scrape and save data into an empty movie container (nMovie)
                Result = _scraper.GetInfo_Movie(oDBElement.MainDetails.UniqueIDs.IMDbId, FilteredOptions)
            ElseIf Not ScrapeType = Enums.ScrapeType.SingleScrape Then
                'no IMDB-ID for movie --> search first!
                Result = _scraper.Process_SearchResults_Movie(oDBElement.MainDetails.Title, oDBElement, ScrapeType, FilteredOptions)
                'if still no search result -> exit
                logger.Trace(String.Format("[IMDB_Data] [Scraper_Movie] [Abort] No search result found"))
                If Result Is Nothing Then Return New Interfaces.AddonResult_Data_Scraper_Movie(Interfaces.ResultStatus.NoResult)
            End If
        End If

        If Result Is Nothing Then
            Select Case ScrapeType
                Case Enums.ScrapeType.AllAuto, Enums.ScrapeType.FilterAuto, Enums.ScrapeType.MarkedAuto, Enums.ScrapeType.MissingAuto, Enums.ScrapeType.NewAuto, Enums.ScrapeType.SelectedAuto
                    logger.Trace(String.Format("[IMDb_Data] [Scraper_Movie] [Abort] No search result found"))
                    Return New Interfaces.AddonResult_Data_Scraper_Movie(Interfaces.ResultStatus.NoResult)
            End Select
        Else
            logger.Trace("[IMDb_Data] [Scraper_Movie] [Done]")
            Return New Interfaces.AddonResult_Data_Scraper_Movie(Interfaces.ResultStatus.Successful)
        End If

        If ScrapeType = Enums.ScrapeType.SingleScrape OrElse ScrapeType = Enums.ScrapeType.SingleAuto Then
            If Not oDBElement.MainDetails.UniqueIDs.IMDbIdSpecified Then
                Using dlgSearch As New dlgSearchResults(_scraper, "imdb", New List(Of String) From {"IMDb"}, Enums.ContentType.Movie)
                    Select Case dlgSearch.ShowDialog(oDBElement.MainDetails.Title, oDBElement.Filename, oDBElement.Movie.Year)
                        Case DialogResult.Cancel
                            logger.Trace(String.Format("[IMDb_Data] [Scraper_Movie] [Cancelled] Cancelled by user"))
                            Return New Interfaces.AddonResult_Data_Scraper_Movie(Interfaces.ResultStatus.Cancelled)
                        Case DialogResult.OK
                            Result = _scraper.GetInfo_Movie(dlgSearch.Result_Movie.UniqueIDs.IMDbId.ToString, FilteredOptions)
                            'if a movie is found, set DoSearch back to "false" for following scrapers
                            ScrapeModifiers.DoSearch = False
                        Case DialogResult.Retry
                            logger.Trace(String.Format("[IMDb_Data] [Scraper_Movie] [Retry] Skipped by user"))
                            Return New Interfaces.AddonResult_Data_Scraper_Movie(Interfaces.ResultStatus.Skipped)
                    End Select
                End Using
            End If
        End If

        logger.Trace("[IMDb_Data] [Scraper_Movie] [Done]")
        Return New Interfaces.AddonResult_Data_Scraper_Movie(Interfaces.ResultStatus.Successful)
    End Function
    ''' <summary>
    '''  Scrape MovieDetails from IMDB
    ''' </summary>
    ''' <param name="oDBElement">TV Show to be scraped. DBTV as ByRef to use existing data for identifing tv show and to fill with IMDB/TMDB/TVDB ID for next scraper</param>
    ''' <param name="Options">What kind of data is being requested from the scrape(global scraper settings)</param>
    ''' <returns>Database.DBElement Object (nMovie) which contains the scraped data</returns>
    ''' <remarks></remarks>
    Function Scraper_TV(ByRef oDBElement As Database.DBElement, ByRef ScrapeModifiers As Structures.ScrapeModifiers, ByRef ScrapeType As Enums.ScrapeType, ByRef ScrapeOptions As Structures.ScrapeOptions) As Interfaces.AddonResult_Data_Scraper_TVShow Implements Interfaces.IAddon_Data_Scraper_TV.Scraper_TVShow
        logger.Trace("[IMDb_Data] [Scraper_TV] [Start]")

        LoadSettings_TV()

        Dim FilteredOptions As Structures.ScrapeOptions = Functions.ScrapeOptionsAndAlso(ScrapeOptions, ConfigScrapeOptions_TV)
        Dim Result As MediaContainers.TVShow = Nothing
        _SpecialSettings_TV.PrefLanguage = oDBElement.Language
        Dim _scraper As New Scraper(_SpecialSettings_TV)

        If Not ScrapeModifiers.DoSearch AndAlso
            (ScrapeModifiers.MainNFO OrElse
            (ScrapeModifiers.withEpisodes AndAlso ScrapeModifiers.EpisodeNFO) OrElse
            (ScrapeModifiers.withSeasons AndAlso ScrapeModifiers.SeasonNFO)) Then
            If oDBElement.TVShow.UniqueIDs.IMDbIdSpecified Then
                'IMDB-ID already available -> scrape and save data into an empty tvshow container (nTVShow)
                Result = _scraper.GetInfo_TVShow(oDBElement.TVShow.UniqueIDs.IMDbId, FilteredOptions, ScrapeModifiers)
            ElseIf Not ScrapeType = Enums.ScrapeType.SingleScrape Then
                'no IMDB-ID for tvshow --> search first!
                Result = _scraper.Process_SearchResults_TVShow(oDBElement.MainDetails.Title, oDBElement, ScrapeType, FilteredOptions, ScrapeModifiers)
                'if still no search result -> exit
                logger.Trace(String.Format("[IMDb_Data] [Scraper_TV] [Abort] No search result found"))
                If Result Is Nothing Then Return New Interfaces.AddonResult_Data_Scraper_TVShow(Interfaces.ResultStatus.NoResult)
            End If
        End If

        If Result Is Nothing Then
            Select Case ScrapeType
                Case Enums.ScrapeType.AllAuto, Enums.ScrapeType.FilterAuto, Enums.ScrapeType.MarkedAuto, Enums.ScrapeType.MissingAuto, Enums.ScrapeType.NewAuto, Enums.ScrapeType.SelectedAuto
                    logger.Trace(String.Format("[IMDb_Data] [Scraper_TV] [Abort] No search result found"))
                    Return New Interfaces.AddonResult_Data_Scraper_TVShow(Interfaces.ResultStatus.NoResult)
            End Select
        Else
            logger.Trace("[IMDB_Data] [Scraper_TV] [Done]")
            Return New Interfaces.AddonResult_Data_Scraper_TVShow(Interfaces.ResultStatus.Successful)
        End If

        If ScrapeType = Enums.ScrapeType.SingleScrape OrElse ScrapeType = Enums.ScrapeType.SingleAuto Then
            If Not oDBElement.TVShow.UniqueIDs.IMDbIdSpecified Then
                Using dlgSearch As New dlgSearchResults(_scraper, "imdb", New List(Of String) From {"IMDb"}, Enums.ContentType.TVShow)
                    Select Case dlgSearch.ShowDialog(oDBElement.MainDetails.Title, oDBElement.ShowPath)
                        Case DialogResult.Cancel
                            logger.Trace(String.Format("[IMDb_Data] [Scraper_TV] [Cancelled] Cancelled by user"))
                            Return New Interfaces.AddonResult_Data_Scraper_TVShow(Interfaces.ResultStatus.Cancelled)
                        Case DialogResult.OK
                            Result = _scraper.GetInfo_TVShow(dlgSearch.Result_TVShow.UniqueIDs.IMDbId, FilteredOptions, ScrapeModifiers)
                            'if a tvshow is found, set DoSearch back to "false" for following scrapers
                            ScrapeModifiers.DoSearch = False
                        Case DialogResult.Retry
                            logger.Trace(String.Format("[IMDb_Data] [Scraper_TV] [Retry] Skiped by user"))
                            Return New Interfaces.AddonResult_Data_Scraper_TVShow(Interfaces.ResultStatus.Skipped)
                    End Select
                End Using
            End If
        End If

        logger.Trace("[IMDb_Data] [Scraper_TV] [Done]")
        Return New Interfaces.AddonResult_Data_Scraper_TVShow(Interfaces.ResultStatus.Successful)
    End Function

    Public Function Scraper_TVEpisode(ByRef oDBTVEpisode As Database.DBElement, ByVal ScrapeOptions As Structures.ScrapeOptions) As Interfaces.AddonResult_Data_Scraper_TVEpisode Implements Interfaces.IAddon_Data_Scraper_TV.Scraper_TVEpisode
        logger.Trace("[IMDb_Data] [Scraper_TVEpisode] [Start]")

        LoadSettings_TV()

        Dim FilteredOptions As Structures.ScrapeOptions = Functions.ScrapeOptionsAndAlso(ScrapeOptions, ConfigScrapeOptions_TV)
        Dim Result As New MediaContainers.EpisodeDetails
        _SpecialSettings_TV.PrefLanguage = oDBTVEpisode.Language
        Dim _scraper As New Scraper(_SpecialSettings_TV)

        If oDBTVEpisode.TVEpisode.UniqueIDs.IMDbIdSpecified Then
            Result = _scraper.GetInfo_TVEpisode(oDBTVEpisode.TVEpisode.UniqueIDs.IMDbId, FilteredOptions)
        ElseIf oDBTVEpisode.TVShow.UniqueIDs.IMDbIdSpecified AndAlso oDBTVEpisode.TVEpisode.SeasonSpecified AndAlso oDBTVEpisode.MainDetails.EpisodeSpecified Then
            Result = _scraper.GetInfo_TVEpisode(oDBTVEpisode.TVShow.UniqueIDs.IMDbId, oDBTVEpisode.TVEpisode.Season, oDBTVEpisode.MainDetails.Episode, FilteredOptions)
        Else
            logger.Trace("[IMDb_Data] [Scraper_TVEpisode] [Abort] No Episode and TV Show IMDB ID available")
            Return New Interfaces.AddonResult_Data_Scraper_TVEpisode(Interfaces.ResultStatus.NoResult)
        End If

        logger.Trace("[IMDb_Data] [Scraper_TVEpisode] [Done]")
        Return New Interfaces.AddonResult_Data_Scraper_TVEpisode(Interfaces.ResultStatus.Successful)
    End Function

    Public Function Scraper_TVSeason(ByRef oDBTVSeason As Database.DBElement, ByVal ScrapeOptions As Structures.ScrapeOptions) As Interfaces.AddonResult_Data_Scraper_TVSeason Implements Interfaces.IAddon_Data_Scraper_TV.Scraper_TVSeason
        Return New Interfaces.AddonResult_Data_Scraper_TVSeason(Interfaces.ResultStatus.NoResult)
    End Function

    Public Sub ScraperOrderChanged_Movie() Implements Interfaces.IAddon_Data_Scraper_Movie.ScraperOrderChanged
        _setup_Movie.orderChanged()
    End Sub

    Public Sub ScraperOrderChanged_tv() Implements Interfaces.IAddon_Data_Scraper_TV.ScraperOrderChanged
        _setup_TV.orderChanged()
    End Sub

#End Region 'Methods

#Region "Nested Types"

    Structure AddonSettings

#Region "Fields"

        Dim FallBackWorldwide As Boolean
        Dim ForceTitleLanguage As String
        Dim MPAADescription As Boolean
        Dim PrefLanguage As String
        Dim SearchPartialTitles As Boolean
        Dim SearchPopularTitles As Boolean
        Dim SearchTvTitles As Boolean
        Dim SearchVideoTitles As Boolean
        Dim SearchShortTitles As Boolean
        Dim StudiowithDistributors As Boolean

#End Region 'Fields

    End Structure

#End Region 'Nested Types

End Class