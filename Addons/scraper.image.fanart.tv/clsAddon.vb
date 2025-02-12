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
    Implements Interfaces.IAddon_Image_Scraper_Movie
    Implements Interfaces.IAddon_Image_Scraper_Movieset
    Implements Interfaces.IAddon_Image_Scraper_TV

#Region "Fields"

    Shared logger As Logger = LogManager.GetCurrentClassLogger()
    Public Shared ConfigModifier_Movie As New Structures.ScrapeModifiers
    Public Shared ConfigModifier_MovieSet As New Structures.ScrapeModifiers
    Public Shared ConfigModifier_TV As New Structures.ScrapeModifiers
    Public Shared _AssemblyName As String

    ''' <summary>
    ''' Scraping Here
    ''' </summary>
    ''' <remarks></remarks>
    Private _SpecialSettings_Movie As New AddonSettings
    Private _SpecialSettings_MovieSet As New AddonSettings
    Private _SpecialSettings_TV As New AddonSettings
    Private _Name As String = "FanartTV_Image"
    Private _ScraperEnabled_Movie As Boolean = False
    Private _ScraperEnabled_MovieSet As Boolean = False
    Private _ScraperEnabled_TV As Boolean = False
    Private _setup_Movie As frmSettingsHolder_Movie
    Private _setup_MovieSet As frmSettingsHolder_MovieSet
    Private _setup_TV As frmSettingsHolder_TV

    Private _AddonSettings As New AddonSettings

#End Region 'Fields

#Region "Events"

    'Movie part
    Public Event ModuleSettingsChanged_Movie() Implements Interfaces.IAddon_Image_Scraper_Movie.AddonSettingsChanged
    Public Event SetupScraperChanged_Movie(ByVal name As String, ByVal State As Boolean, ByVal difforder As Integer) Implements Interfaces.IAddon_Image_Scraper_Movie.AddonStateChanged
    Public Event SetupNeedsRestart_Movie() Implements Interfaces.IAddon_Image_Scraper_Movie.AddonNeedsRestart
    Public Event ImagesDownloaded_Movie(ByVal Posters As List(Of MediaContainers.Image)) Implements Interfaces.IAddon_Image_Scraper_Movie.ImagesDownloaded
    Public Event ProgressUpdated_Movie(ByVal iPercent As Integer) Implements Interfaces.IAddon_Image_Scraper_Movie.ProgressUpdated

    'MovieSet part
    Public Event ModuleSettingsChanged_MovieSet() Implements Interfaces.IAddon_Image_Scraper_Movieset.AddonSettingsChanged
    Public Event SetupScraperChanged_MovieSet(ByVal name As String, ByVal State As Boolean, ByVal difforder As Integer) Implements Interfaces.IAddon_Image_Scraper_Movieset.AddonStateChanged
    Public Event SetupNeedsRestart_MovieSet() Implements Interfaces.IAddon_Image_Scraper_Movieset.AddonNeedsRestart
    Public Event ImagesDownloaded_MovieSet(ByVal Posters As List(Of MediaContainers.Image)) Implements Interfaces.IAddon_Image_Scraper_Movieset.ImagesDownloaded
    Public Event ProgressUpdated_MovieSet(ByVal iPercent As Integer) Implements Interfaces.IAddon_Image_Scraper_Movieset.ProgressUpdated

    'TV part
    Public Event ModuleSettingsChanged_TV() Implements Interfaces.IAddon_Image_Scraper_TV.AddonSettingsChanged
    Public Event SetupScraperChanged_TV(ByVal name As String, ByVal State As Boolean, ByVal difforder As Integer) Implements Interfaces.IAddon_Image_Scraper_TV.AddonStateChanged
    Public Event SetupNeedsRestart_TV() Implements Interfaces.IAddon_Image_Scraper_TV.AddonNeedsRestart
    Public Event ImagesDownloaded_TV(ByVal Posters As List(Of MediaContainers.Image)) Implements Interfaces.IAddon_Image_Scraper_TV.ImagesDownloaded
    Public Event ProgressUpdated_TV(ByVal iPercent As Integer) Implements Interfaces.IAddon_Image_Scraper_TV.ProgressUpdated

#End Region 'Events

#Region "Properties"

    ReadOnly Property ModuleName() As String Implements Interfaces.IAddon_Image_Scraper_Movie.ModuleName, Interfaces.IAddon_Image_Scraper_Movieset.ModuleName, Interfaces.IAddon_Image_Scraper_TV.ModuleName
        Get
            Return _Name
        End Get
    End Property

    ReadOnly Property ModuleVersion() As String Implements Interfaces.IAddon_Image_Scraper_Movie.ModuleVersion, Interfaces.IAddon_Image_Scraper_Movieset.ModuleVersion, Interfaces.IAddon_Image_Scraper_TV.ModuleVersion
        Get
            Return System.Diagnostics.FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly.Location).FileVersion.ToString
        End Get
    End Property

    Property ScraperEnabled_Movie() As Boolean Implements Interfaces.IAddon_Image_Scraper_Movie.ScraperEnabled
        Get
            Return _ScraperEnabled_Movie
        End Get
        Set(ByVal value As Boolean)
            _ScraperEnabled_Movie = value
        End Set
    End Property

    Property ScraperEnabled_MovieSet() As Boolean Implements Interfaces.IAddon_Image_Scraper_Movieset.ScraperEnabled
        Get
            Return _ScraperEnabled_MovieSet
        End Get
        Set(ByVal value As Boolean)
            _ScraperEnabled_MovieSet = value
        End Set
    End Property

    Property ScraperEnabled_TV() As Boolean Implements Interfaces.IAddon_Image_Scraper_TV.ScraperEnabled
        Get
            Return _ScraperEnabled_TV
        End Get
        Set(ByVal value As Boolean)
            _ScraperEnabled_TV = value
        End Set
    End Property

#End Region 'Properties

#Region "Methods"

    Function QueryScraperCapabilities_Movie(ByVal cap As Enums.ModifierType) As Boolean Implements Interfaces.IAddon_Image_Scraper_Movie.QueryScraperCapabilities
        Select Case cap
            Case Enums.ModifierType.MainBanner
                Return ConfigModifier_Movie.MainBanner
            Case Enums.ModifierType.MainClearArt
                Return ConfigModifier_Movie.MainClearArt
            Case Enums.ModifierType.MainClearLogo
                Return ConfigModifier_Movie.MainClearLogo
            Case Enums.ModifierType.MainDiscArt
                Return ConfigModifier_Movie.MainDiscArt
            Case Enums.ModifierType.MainFanart
                Return ConfigModifier_Movie.MainFanart
            Case Enums.ModifierType.MainLandscape
                Return ConfigModifier_Movie.MainLandscape
            Case Enums.ModifierType.MainPoster
                Return ConfigModifier_Movie.MainPoster
        End Select
        Return False
    End Function

    Function QueryScraperCapabilities_MovieSet(ByVal cap As Enums.ModifierType) As Boolean Implements Interfaces.IAddon_Image_Scraper_Movieset.QueryScraperCapabilities
        Select Case cap
            Case Enums.ModifierType.MainBanner
                Return ConfigModifier_MovieSet.MainBanner
            Case Enums.ModifierType.MainClearArt
                Return ConfigModifier_MovieSet.MainClearArt
            Case Enums.ModifierType.MainClearLogo
                Return ConfigModifier_MovieSet.MainClearLogo
            Case Enums.ModifierType.MainDiscArt
                Return ConfigModifier_MovieSet.MainDiscArt
            Case Enums.ModifierType.MainFanart
                Return ConfigModifier_MovieSet.MainFanart
            Case Enums.ModifierType.MainLandscape
                Return ConfigModifier_MovieSet.MainLandscape
            Case Enums.ModifierType.MainPoster
                Return ConfigModifier_MovieSet.MainPoster
        End Select
        Return False
    End Function

    Function QueryScraperCapabilities_TV(ByVal cap As Enums.ModifierType) As Boolean Implements Interfaces.IAddon_Image_Scraper_TV.QueryScraperCapabilities
        Select Case cap
            Case Enums.ModifierType.MainBanner
                Return ConfigModifier_TV.MainBanner
            Case Enums.ModifierType.MainCharacterArt
                Return ConfigModifier_TV.MainCharacterArt
            Case Enums.ModifierType.MainClearArt
                Return ConfigModifier_TV.MainClearArt
            Case Enums.ModifierType.MainClearLogo
                Return ConfigModifier_TV.MainClearLogo
            Case Enums.ModifierType.MainFanart
                Return ConfigModifier_TV.MainFanart
            Case Enums.ModifierType.MainLandscape
                Return ConfigModifier_TV.MainLandscape
            Case Enums.ModifierType.MainPoster
                Return ConfigModifier_TV.MainPoster
            Case Enums.ModifierType.SeasonBanner
                Return ConfigModifier_TV.SeasonBanner
            Case Enums.ModifierType.SeasonLandscape
                Return ConfigModifier_TV.SeasonLandscape
            Case Enums.ModifierType.SeasonPoster
                Return ConfigModifier_TV.SeasonPoster
        End Select
        Return False
    End Function

    Private Sub Handle_ModuleSettingsChanged_Movie()
        RaiseEvent ModuleSettingsChanged_Movie()
    End Sub

    Private Sub Handle_ModuleSettingsChanged_MovieSet()
        RaiseEvent ModuleSettingsChanged_MovieSet()
    End Sub

    Private Sub Handle_ModuleSettingsChanged_TV()
        RaiseEvent ModuleSettingsChanged_TV()
    End Sub

    Private Sub Handle_SetupNeedsRestart_Movie()
        RaiseEvent SetupNeedsRestart_Movie()
    End Sub

    Private Sub Handle_SetupNeedsRestart_MovieSet()
        RaiseEvent SetupNeedsRestart_MovieSet()
    End Sub

    Private Sub Handle_SetupNeedsRestart_TV()
        RaiseEvent SetupNeedsRestart_TV()
    End Sub

    Private Sub Handle_SetupScraperChanged_Movie(ByVal state As Boolean, ByVal difforder As Integer)
        ScraperEnabled_Movie = state
        RaiseEvent SetupScraperChanged_Movie(String.Concat(Me._Name, "_Movie"), state, difforder)
    End Sub

    Private Sub Handle_SetupScraperChanged_MovieSet(ByVal state As Boolean, ByVal difforder As Integer)
        ScraperEnabled_MovieSet = state
        RaiseEvent SetupScraperChanged_MovieSet(String.Concat(Me._Name, "_MovieSet"), state, difforder)
    End Sub

    Private Sub Handle_SetupScraperChanged_TV(ByVal state As Boolean, ByVal difforder As Integer)
        ScraperEnabled_TV = state
        RaiseEvent SetupScraperChanged_TV(String.Concat(Me._Name, "_TV"), state, difforder)
    End Sub

    Sub Init_Movie(ByVal sAssemblyName As String) Implements Interfaces.IAddon_Image_Scraper_Movie.Init
        _AssemblyName = sAssemblyName
        LoadSettings_Movie()
    End Sub

    Sub Init_MovieSet(ByVal sAssemblyName As String) Implements Interfaces.IAddon_Image_Scraper_Movieset.Init
        _AssemblyName = sAssemblyName
        LoadSettings_MovieSet()
    End Sub

    Sub Init_TV(ByVal sAssemblyName As String) Implements Interfaces.IAddon_Image_Scraper_TV.Init
        _AssemblyName = sAssemblyName
        LoadSettings_TV()
    End Sub

    Function InjectSetupScraper_Movie() As Containers.SettingsPanel Implements Interfaces.IAddon_Image_Scraper_Movie.InjectSetupScraper
        Dim Spanel As New Containers.SettingsPanel
        _setup_Movie = New frmSettingsHolder_Movie
        LoadSettings_Movie()
        _setup_Movie.chkEnabled.Checked = _ScraperEnabled_Movie
        _setup_Movie.chkScrapePoster.Checked = ConfigModifier_Movie.MainPoster
        _setup_Movie.chkScrapeFanart.Checked = ConfigModifier_Movie.MainFanart
        _setup_Movie.chkScrapeBanner.Checked = ConfigModifier_Movie.MainBanner
        _setup_Movie.chkScrapeClearArt.Checked = ConfigModifier_Movie.MainClearArt
        _setup_Movie.chkScrapeClearArtOnlyHD.Checked = _SpecialSettings_Movie.ClearArtOnlyHD
        _setup_Movie.chkScrapeClearLogo.Checked = ConfigModifier_Movie.MainClearLogo
        _setup_Movie.chkScrapeClearLogoOnlyHD.Checked = _SpecialSettings_Movie.ClearLogoOnlyHD
        _setup_Movie.chkScrapeDiscArt.Checked = ConfigModifier_Movie.MainDiscArt
        _setup_Movie.chkScrapeLandscape.Checked = ConfigModifier_Movie.MainLandscape
        _setup_Movie.txtApiKey.Text = _SpecialSettings_Movie.ApiKey

        If Not String.IsNullOrEmpty(_SpecialSettings_Movie.ApiKey) Then
            _setup_Movie.btnUnlockAPI.Text = Master.eLang.GetString(443, "Use embedded API Key")
            _setup_Movie.lblEMMAPI.Visible = False
            _setup_Movie.txtApiKey.Enabled = True
        End If

        _setup_Movie.orderChanged()

        Spanel.UniqueName = String.Concat(Me._Name, "_Movie")
        Spanel.Title = "Fanart.tv"
        Spanel.Order = 110
        Spanel.Parent = "pnlMovieMedia"
        Spanel.Type = Master.eLang.GetString(36, "Movies")
        Spanel.ImageIndex = If(Me._ScraperEnabled_Movie, 9, 10)
        Spanel.Panel = Me._setup_Movie.pnlSettings

        AddHandler _setup_Movie.SetupScraperChanged, AddressOf Handle_SetupScraperChanged_Movie
        AddHandler _setup_Movie.ModuleSettingsChanged, AddressOf Handle_ModuleSettingsChanged_Movie
        AddHandler _setup_Movie.SetupNeedsRestart, AddressOf Handle_SetupNeedsRestart_Movie
        Return Spanel
    End Function

    Function InjectSetupScraper_MovieSet() As Containers.SettingsPanel Implements Interfaces.IAddon_Image_Scraper_Movieset.InjectSettingsPanel
        Dim Spanel As New Containers.SettingsPanel
        _setup_MovieSet = New frmSettingsHolder_MovieSet
        LoadSettings_MovieSet()
        _setup_MovieSet.chkEnabled.Checked = _ScraperEnabled_MovieSet
        _setup_MovieSet.chkScrapePoster.Checked = ConfigModifier_MovieSet.MainPoster
        _setup_MovieSet.chkScrapeFanart.Checked = ConfigModifier_MovieSet.MainFanart
        _setup_MovieSet.chkScrapeBanner.Checked = ConfigModifier_MovieSet.MainBanner
        _setup_MovieSet.chkScrapeClearArt.Checked = ConfigModifier_MovieSet.MainClearArt
        _setup_MovieSet.chkScrapeClearArtOnlyHD.Checked = _SpecialSettings_MovieSet.ClearArtOnlyHD
        _setup_MovieSet.chkScrapeClearLogo.Checked = ConfigModifier_MovieSet.MainClearLogo
        _setup_MovieSet.chkScrapeClearLogoOnlyHD.Checked = _SpecialSettings_MovieSet.ClearLogoOnlyHD
        _setup_MovieSet.chkScrapeDiscArt.Checked = ConfigModifier_MovieSet.MainDiscArt
        _setup_MovieSet.chkScrapeLandscape.Checked = ConfigModifier_MovieSet.MainLandscape
        _setup_MovieSet.txtApiKey.Text = _SpecialSettings_MovieSet.ApiKey

        If Not String.IsNullOrEmpty(_SpecialSettings_MovieSet.ApiKey) Then
            _setup_MovieSet.btnUnlockAPI.Text = Master.eLang.GetString(443, "Use embedded API Key")
            _setup_MovieSet.lblEMMAPI.Visible = False
            _setup_MovieSet.txtApiKey.Enabled = True
        End If

        _setup_MovieSet.orderChanged()

        Spanel.UniqueName = String.Concat(Me._Name, "_MovieSet")
        Spanel.Title = "Fanart.tv"
        Spanel.Order = 110
        Spanel.Parent = "pnlMovieSetMedia"
        Spanel.Type = Master.eLang.GetString(1203, "MovieSets")
        Spanel.ImageIndex = If(Me._ScraperEnabled_MovieSet, 9, 10)
        Spanel.Panel = Me._setup_MovieSet.pnlSettings

        AddHandler _setup_MovieSet.SetupScraperChanged, AddressOf Handle_SetupScraperChanged_MovieSet
        AddHandler _setup_MovieSet.ModuleSettingsChanged, AddressOf Handle_ModuleSettingsChanged_MovieSet
        AddHandler _setup_MovieSet.SetupNeedsRestart, AddressOf Handle_SetupNeedsRestart_MovieSet
        Return Spanel
    End Function

    Function InjectSetupScraper_TV() As Containers.SettingsPanel Implements Interfaces.IAddon_Image_Scraper_TV.InjectSettingsPanel
        Dim Spanel As New Containers.SettingsPanel
        _setup_TV = New frmSettingsHolder_TV
        LoadSettings_TV()
        _setup_TV.chkEnabled.Checked = _ScraperEnabled_TV
        _setup_TV.chkScrapeSeasonBanner.Checked = ConfigModifier_TV.SeasonBanner
        _setup_TV.chkScrapeSeasonLandscape.Checked = ConfigModifier_TV.SeasonLandscape
        _setup_TV.chkScrapeSeasonPoster.Checked = ConfigModifier_TV.SeasonPoster
        _setup_TV.chkScrapeShowBanner.Checked = ConfigModifier_TV.MainBanner
        _setup_TV.chkScrapeShowCharacterArt.Checked = ConfigModifier_TV.MainCharacterArt
        _setup_TV.chkScrapeShowClearArt.Checked = ConfigModifier_TV.MainClearArt
        _setup_TV.chkScrapeShowClearArtOnlyHD.Checked = _SpecialSettings_TV.ClearArtOnlyHD
        _setup_TV.chkScrapeShowClearLogo.Checked = ConfigModifier_TV.MainClearLogo
        _setup_TV.chkScrapeShowClearLogoOnlyHD.Checked = _SpecialSettings_TV.ClearLogoOnlyHD
        _setup_TV.chkScrapeShowFanart.Checked = ConfigModifier_TV.MainFanart
        _setup_TV.chkScrapeShowLandscape.Checked = ConfigModifier_TV.MainLandscape
        _setup_TV.chkScrapeShowPoster.Checked = ConfigModifier_TV.MainPoster
        _setup_TV.txtApiKey.Text = _SpecialSettings_TV.ApiKey

        If Not String.IsNullOrEmpty(_SpecialSettings_TV.ApiKey) Then
            _setup_TV.btnUnlockAPI.Text = Master.eLang.GetString(443, "Use embedded API Key")
            _setup_TV.lblEMMAPI.Visible = False
            _setup_TV.txtApiKey.Enabled = True
        End If

        _setup_TV.orderChanged()

        Spanel.UniqueName = String.Concat(Me._Name, "_TV")
        Spanel.Title = "Fanart.tv"
        Spanel.Order = 110
        Spanel.Parent = "pnlTVMedia"
        Spanel.Type = Master.eLang.GetString(653, "TV Shows")
        Spanel.ImageIndex = If(Me._ScraperEnabled_TV, 9, 10)
        Spanel.Panel = Me._setup_TV.pnlSettings

        AddHandler _setup_TV.SetupScraperChanged, AddressOf Handle_SetupScraperChanged_TV
        AddHandler _setup_TV.ModuleSettingsChanged, AddressOf Handle_ModuleSettingsChanged_TV
        AddHandler _setup_TV.SetupNeedsRestart, AddressOf Handle_SetupNeedsRestart_TV
        Return Spanel
    End Function

    Sub LoadSettings_Movie()
        _SpecialSettings_Movie.ApiKey = Master.eAdvancedSettings.GetStringSetting("ApiKey", "", , Enums.ContentType.Movie)
        _SpecialSettings_Movie.ClearArtOnlyHD = Master.eAdvancedSettings.GetBooleanSetting("ClearArtOnlyHD", False, , Enums.ContentType.Movie)
        _SpecialSettings_Movie.ClearLogoOnlyHD = Master.eAdvancedSettings.GetBooleanSetting("ClearLogoOnlyHD", False, , Enums.ContentType.Movie)

        ConfigModifier_Movie.MainPoster = Master.eAdvancedSettings.GetBooleanSetting("DoPoster", True, , Enums.ContentType.Movie)
        ConfigModifier_Movie.MainFanart = Master.eAdvancedSettings.GetBooleanSetting("DoFanart", True, , Enums.ContentType.Movie)
        ConfigModifier_Movie.MainBanner = Master.eAdvancedSettings.GetBooleanSetting("DoBanner", True, , Enums.ContentType.Movie)
        ConfigModifier_Movie.MainClearArt = Master.eAdvancedSettings.GetBooleanSetting("DoClearArt", True, , Enums.ContentType.Movie)
        ConfigModifier_Movie.MainClearLogo = Master.eAdvancedSettings.GetBooleanSetting("DoClearLogo", True, , Enums.ContentType.Movie)
        ConfigModifier_Movie.MainDiscArt = Master.eAdvancedSettings.GetBooleanSetting("DoDiscArt", True, , Enums.ContentType.Movie)
        ConfigModifier_Movie.MainLandscape = Master.eAdvancedSettings.GetBooleanSetting("DoLandscape", True, , Enums.ContentType.Movie)
        ConfigModifier_Movie.MainExtrafanarts = ConfigModifier_Movie.MainFanart
        ConfigModifier_Movie.MainExtrathumbs = ConfigModifier_Movie.MainFanart
        ConfigModifier_Movie.MainKeyart = ConfigModifier_Movie.MainPoster
    End Sub

    Sub LoadSettings_MovieSet()
        _SpecialSettings_MovieSet.ApiKey = Master.eAdvancedSettings.GetStringSetting("ApiKey", "", , Enums.ContentType.MovieSet)
        _SpecialSettings_MovieSet.ClearArtOnlyHD = Master.eAdvancedSettings.GetBooleanSetting("ClearArtOnlyHD", False, , Enums.ContentType.MovieSet)
        _SpecialSettings_MovieSet.ClearLogoOnlyHD = Master.eAdvancedSettings.GetBooleanSetting("ClearLogoOnlyHD", False, , Enums.ContentType.MovieSet)

        ConfigModifier_MovieSet.MainPoster = Master.eAdvancedSettings.GetBooleanSetting("DoPoster", True, , Enums.ContentType.MovieSet)
        ConfigModifier_MovieSet.MainFanart = Master.eAdvancedSettings.GetBooleanSetting("DoFanart", True, , Enums.ContentType.MovieSet)
        ConfigModifier_MovieSet.MainBanner = Master.eAdvancedSettings.GetBooleanSetting("DoBanner", True, , Enums.ContentType.MovieSet)
        ConfigModifier_MovieSet.MainClearArt = Master.eAdvancedSettings.GetBooleanSetting("DoClearArt", True, , Enums.ContentType.MovieSet)
        ConfigModifier_MovieSet.MainClearLogo = Master.eAdvancedSettings.GetBooleanSetting("DoClearLogo", True, , Enums.ContentType.MovieSet)
        ConfigModifier_MovieSet.MainDiscArt = Master.eAdvancedSettings.GetBooleanSetting("DoDiscArt", True, , Enums.ContentType.MovieSet)
        ConfigModifier_MovieSet.MainLandscape = Master.eAdvancedSettings.GetBooleanSetting("DoLandscape", True, , Enums.ContentType.MovieSet)
        ConfigModifier_MovieSet.MainExtrafanarts = ConfigModifier_MovieSet.MainFanart
        ConfigModifier_MovieSet.MainKeyart = ConfigModifier_MovieSet.MainPoster
    End Sub

    Sub LoadSettings_TV()
        _SpecialSettings_TV.ApiKey = Master.eAdvancedSettings.GetStringSetting("ApiKey", "", , Enums.ContentType.TV)
        _SpecialSettings_TV.ClearArtOnlyHD = Master.eAdvancedSettings.GetBooleanSetting("ClearArtOnlyHD", False, , Enums.ContentType.TV)
        _SpecialSettings_TV.ClearLogoOnlyHD = Master.eAdvancedSettings.GetBooleanSetting("ClearLogoOnlyHD", False, , Enums.ContentType.TV)

        ConfigModifier_TV.SeasonBanner = Master.eAdvancedSettings.GetBooleanSetting("DoSeasonBanner", True, , Enums.ContentType.TV)
        ConfigModifier_TV.SeasonLandscape = Master.eAdvancedSettings.GetBooleanSetting("DoSeasonLandscape", True, , Enums.ContentType.TV)
        ConfigModifier_TV.SeasonPoster = Master.eAdvancedSettings.GetBooleanSetting("DoSeasonPoster", True, , Enums.ContentType.TV)
        ConfigModifier_TV.MainBanner = Master.eAdvancedSettings.GetBooleanSetting("DoShowBanner", True, , Enums.ContentType.TV)
        ConfigModifier_TV.MainCharacterArt = Master.eAdvancedSettings.GetBooleanSetting("DoShowCharacterArt", True, , Enums.ContentType.TV)
        ConfigModifier_TV.MainClearArt = Master.eAdvancedSettings.GetBooleanSetting("DoShowClearArt", True, , Enums.ContentType.TV)
        ConfigModifier_TV.MainClearLogo = Master.eAdvancedSettings.GetBooleanSetting("DoShowClearLogo", True, , Enums.ContentType.TV)
        ConfigModifier_TV.MainFanart = Master.eAdvancedSettings.GetBooleanSetting("DoShowFanart", True, , Enums.ContentType.TV)
        ConfigModifier_TV.MainLandscape = Master.eAdvancedSettings.GetBooleanSetting("DoShowLandscape", True, , Enums.ContentType.TV)
        ConfigModifier_TV.MainPoster = Master.eAdvancedSettings.GetBooleanSetting("DoShowPoster", True, , Enums.ContentType.TV)
        ConfigModifier_TV.MainExtrafanarts = ConfigModifier_TV.MainFanart
        ConfigModifier_TV.MainKeyart = ConfigModifier_TV.MainPoster
    End Sub

    Sub SaveSettings_Movie()
        Using settings = New AdvancedSettings()
            settings.SetBooleanSetting("ClearArtOnlyHD", _SpecialSettings_Movie.ClearArtOnlyHD, , , Enums.ContentType.Movie)
            settings.SetBooleanSetting("ClearLogoOnlyHD", _SpecialSettings_Movie.ClearLogoOnlyHD, , , Enums.ContentType.Movie)
            settings.SetBooleanSetting("DoPoster", ConfigModifier_Movie.MainPoster, , , Enums.ContentType.Movie)
            settings.SetBooleanSetting("DoFanart", ConfigModifier_Movie.MainFanart, , , Enums.ContentType.Movie)
            settings.SetBooleanSetting("DoBanner", ConfigModifier_Movie.MainBanner, , , Enums.ContentType.Movie)
            settings.SetBooleanSetting("DoClearArt", ConfigModifier_Movie.MainClearArt, , , Enums.ContentType.Movie)
            settings.SetBooleanSetting("DoClearLogo", ConfigModifier_Movie.MainClearLogo, , , Enums.ContentType.Movie)
            settings.SetBooleanSetting("DoDiscArt", ConfigModifier_Movie.MainDiscArt, , , Enums.ContentType.Movie)
            settings.SetBooleanSetting("DoLandscape", ConfigModifier_Movie.MainLandscape, , , Enums.ContentType.Movie)

            settings.SetStringSetting("ApiKey", _setup_Movie.txtApiKey.Text, , , Enums.ContentType.Movie)
        End Using
    End Sub

    Sub SaveSettings_MovieSet()
        Using settings = New AdvancedSettings()
            settings.SetBooleanSetting("ClearArtOnlyHD", _SpecialSettings_MovieSet.ClearArtOnlyHD, , , Enums.ContentType.MovieSet)
            settings.SetBooleanSetting("ClearLogoOnlyHD", _SpecialSettings_MovieSet.ClearLogoOnlyHD, , , Enums.ContentType.MovieSet)
            settings.SetBooleanSetting("DoPoster", ConfigModifier_MovieSet.MainPoster, , , Enums.ContentType.MovieSet)
            settings.SetBooleanSetting("DoFanart", ConfigModifier_MovieSet.MainFanart, , , Enums.ContentType.MovieSet)
            settings.SetBooleanSetting("DoBanner", ConfigModifier_MovieSet.MainBanner, , , Enums.ContentType.MovieSet)
            settings.SetBooleanSetting("DoClearArt", ConfigModifier_MovieSet.MainClearArt, , , Enums.ContentType.MovieSet)
            settings.SetBooleanSetting("DoClearLogo", ConfigModifier_MovieSet.MainClearLogo, , , Enums.ContentType.MovieSet)
            settings.SetBooleanSetting("DoDiscArt", ConfigModifier_MovieSet.MainDiscArt, , , Enums.ContentType.MovieSet)
            settings.SetBooleanSetting("DoLandscape", ConfigModifier_MovieSet.MainLandscape, , , Enums.ContentType.MovieSet)

            settings.SetStringSetting("ApiKey", _setup_MovieSet.txtApiKey.Text, , , Enums.ContentType.MovieSet)
        End Using
    End Sub

    Sub SaveSettings_TV()
        Using settings = New AdvancedSettings()
            settings.SetBooleanSetting("ClearArtOnlyHD", _SpecialSettings_TV.ClearArtOnlyHD, , , Enums.ContentType.TV)
            settings.SetBooleanSetting("ClearLogoOnlyHD", _SpecialSettings_TV.ClearLogoOnlyHD, , , Enums.ContentType.TV)
            settings.SetBooleanSetting("DoSeasonBanner", ConfigModifier_TV.SeasonBanner, , , Enums.ContentType.TV)
            settings.SetBooleanSetting("DoSeasonLandscape", ConfigModifier_TV.SeasonLandscape, , , Enums.ContentType.TV)
            settings.SetBooleanSetting("DoSeasonPoster", ConfigModifier_TV.SeasonPoster, , , Enums.ContentType.TV)
            settings.SetBooleanSetting("DoShowBanner", ConfigModifier_TV.MainBanner, , , Enums.ContentType.TV)
            settings.SetBooleanSetting("DoShowCharacterArt", ConfigModifier_TV.MainCharacterArt, , , Enums.ContentType.TV)
            settings.SetBooleanSetting("DoShowClearArt", ConfigModifier_TV.MainClearArt, , , Enums.ContentType.TV)
            settings.SetBooleanSetting("DoShowClearLogo", ConfigModifier_TV.MainClearLogo, , , Enums.ContentType.TV)
            settings.SetBooleanSetting("DoShowFanart", ConfigModifier_TV.MainFanart, , , Enums.ContentType.TV)
            settings.SetBooleanSetting("DoShowLandscape", ConfigModifier_TV.MainLandscape, , , Enums.ContentType.TV)
            settings.SetBooleanSetting("DoShowPoster", ConfigModifier_TV.MainPoster, , , Enums.ContentType.TV)

            settings.SetStringSetting("ApiKey", _setup_TV.txtApiKey.Text, , , Enums.ContentType.TV)
        End Using
    End Sub

    Sub SaveSetupScraper_Movie(ByVal DoDispose As Boolean) Implements Interfaces.IAddon_Image_Scraper_Movie.SaveSettings
        _SpecialSettings_Movie.ClearArtOnlyHD = _setup_Movie.chkScrapeClearArtOnlyHD.Checked
        _SpecialSettings_Movie.ClearLogoOnlyHD = _setup_Movie.chkScrapeClearLogoOnlyHD.Checked
        ConfigModifier_Movie.MainPoster = _setup_Movie.chkScrapePoster.Checked
        ConfigModifier_Movie.MainFanart = _setup_Movie.chkScrapeFanart.Checked
        ConfigModifier_Movie.MainBanner = _setup_Movie.chkScrapeBanner.Checked
        ConfigModifier_Movie.MainClearArt = _setup_Movie.chkScrapeClearArt.Checked
        ConfigModifier_Movie.MainClearLogo = _setup_Movie.chkScrapeClearLogo.Checked
        ConfigModifier_Movie.MainDiscArt = _setup_Movie.chkScrapeDiscArt.Checked
        ConfigModifier_Movie.MainLandscape = _setup_Movie.chkScrapeLandscape.Checked
        ConfigModifier_Movie.MainExtrafanarts = ConfigModifier_Movie.MainFanart
        ConfigModifier_Movie.MainExtrathumbs = ConfigModifier_Movie.MainFanart
        ConfigModifier_Movie.MainKeyart = ConfigModifier_Movie.MainPoster
        SaveSettings_Movie()
        If DoDispose Then
            RemoveHandler _setup_Movie.SetupScraperChanged, AddressOf Handle_SetupScraperChanged_Movie
            RemoveHandler _setup_Movie.ModuleSettingsChanged, AddressOf Handle_ModuleSettingsChanged_Movie
            RemoveHandler _setup_Movie.SetupNeedsRestart, AddressOf Handle_SetupNeedsRestart_Movie
            _setup_Movie.Dispose()
        End If
    End Sub

    Sub SaveSetupScraper_MovieSet(ByVal DoDispose As Boolean) Implements Interfaces.IAddon_Image_Scraper_Movieset.SaveSettings
        _SpecialSettings_MovieSet.ClearArtOnlyHD = _setup_MovieSet.chkScrapeClearArtOnlyHD.Checked
        _SpecialSettings_MovieSet.ClearLogoOnlyHD = _setup_MovieSet.chkScrapeClearLogoOnlyHD.Checked
        ConfigModifier_MovieSet.MainPoster = _setup_MovieSet.chkScrapePoster.Checked
        ConfigModifier_MovieSet.MainFanart = _setup_MovieSet.chkScrapeFanart.Checked
        ConfigModifier_MovieSet.MainBanner = _setup_MovieSet.chkScrapeBanner.Checked
        ConfigModifier_MovieSet.MainClearArt = _setup_MovieSet.chkScrapeClearArt.Checked
        ConfigModifier_MovieSet.MainClearLogo = _setup_MovieSet.chkScrapeClearLogo.Checked
        ConfigModifier_MovieSet.MainDiscArt = _setup_MovieSet.chkScrapeDiscArt.Checked
        ConfigModifier_MovieSet.MainLandscape = _setup_MovieSet.chkScrapeLandscape.Checked
        ConfigModifier_MovieSet.MainExtrafanarts = ConfigModifier_MovieSet.MainFanart
        ConfigModifier_MovieSet.MainKeyart = ConfigModifier_MovieSet.MainPoster
        SaveSettings_MovieSet()
        If DoDispose Then
            RemoveHandler _setup_MovieSet.SetupScraperChanged, AddressOf Handle_SetupScraperChanged_MovieSet
            RemoveHandler _setup_MovieSet.ModuleSettingsChanged, AddressOf Handle_ModuleSettingsChanged_MovieSet
            RemoveHandler _setup_MovieSet.SetupNeedsRestart, AddressOf Handle_SetupNeedsRestart_MovieSet
            _setup_MovieSet.Dispose()
        End If
    End Sub

    Sub SaveSetupScraper_TV(ByVal DoDispose As Boolean) Implements Interfaces.IAddon_Image_Scraper_TV.SaveSettings
        _SpecialSettings_TV.ClearArtOnlyHD = _setup_TV.chkScrapeShowClearArtOnlyHD.Checked
        _SpecialSettings_TV.ClearLogoOnlyHD = _setup_TV.chkScrapeShowClearLogoOnlyHD.Checked
        ConfigModifier_TV.SeasonBanner = _setup_TV.chkScrapeSeasonBanner.Checked
        ConfigModifier_TV.SeasonLandscape = _setup_TV.chkScrapeSeasonLandscape.Checked
        ConfigModifier_TV.SeasonPoster = _setup_TV.chkScrapeSeasonPoster.Checked
        ConfigModifier_TV.MainBanner = _setup_TV.chkScrapeShowBanner.Checked
        ConfigModifier_TV.MainCharacterArt = _setup_TV.chkScrapeShowCharacterArt.Checked
        ConfigModifier_TV.MainClearArt = _setup_TV.chkScrapeShowClearArt.Checked
        ConfigModifier_TV.MainClearLogo = _setup_TV.chkScrapeShowClearLogo.Checked
        ConfigModifier_TV.MainFanart = _setup_TV.chkScrapeShowFanart.Checked
        ConfigModifier_TV.MainLandscape = _setup_TV.chkScrapeShowLandscape.Checked
        ConfigModifier_TV.MainPoster = _setup_TV.chkScrapeShowPoster.Checked
        ConfigModifier_TV.MainExtrafanarts = ConfigModifier_TV.MainFanart
        ConfigModifier_TV.MainKeyart = ConfigModifier_TV.MainPoster
        SaveSettings_TV()
        If DoDispose Then
            RemoveHandler _setup_TV.SetupScraperChanged, AddressOf Handle_SetupScraperChanged_TV
            RemoveHandler _setup_TV.ModuleSettingsChanged, AddressOf Handle_ModuleSettingsChanged_TV
            RemoveHandler _setup_TV.SetupNeedsRestart, AddressOf Handle_SetupNeedsRestart_TV
            _setup_TV.Dispose()
        End If
    End Sub

    Function Scraper_Movie(ByRef DBMovie As Database.DBElement, ByRef ImagesContainer As MediaContainers.SearchResultsContainer, ByVal ScrapeModifiers As Structures.ScrapeModifiers) As Interfaces.AddonResult Implements Interfaces.IAddon_Image_Scraper_Movie.Scraper
        logger.Trace("[FanartTV_Image] [Scraper_Movie] [Start]")

        LoadSettings_Movie()
        Dim _scraper As New Scraper(_SpecialSettings_Movie)

        Dim FilteredModifiers As Structures.ScrapeModifiers = Functions.ScrapeModifiersAndAlso(ScrapeModifiers, ConfigModifier_Movie)

        If DBMovie.Movie.UniqueIDs.TMDbIdSpecified Then
            ImagesContainer = _scraper.GetImages_Movie_MovieSet(DBMovie.Movie.UniqueIDs.TMDbId.ToString, FilteredModifiers)
        ElseIf DBMovie.MainDetails.UniqueIDs.IMDbIdSpecified Then
            ImagesContainer = _scraper.GetImages_Movie_MovieSet(DBMovie.MainDetails.UniqueIDs.IMDbId, FilteredModifiers)
        Else
            logger.Trace(String.Concat("[FanartTV_Image] [Scraper_Movie] [Abort] No TMDB and IMDB ID exist to search: ", DBMovie.Movie.Title))
        End If

        logger.Trace("[FanartTV_Image] [Scraper_Movie] [Done]")
        Return New Interfaces.AddonResult
    End Function

    Function Scraper_MovieSet(ByRef DBMovieset As Database.DBElement, ByRef ImagesContainer As MediaContainers.SearchResultsContainer, ByVal ScrapeModifiers As Structures.ScrapeModifiers) As Interfaces.AddonResult Implements Interfaces.IAddon_Image_Scraper_Movieset.Scraper
        logger.Trace("[FanartTV_Image] [Scraper_MovieSet] [Start]")

        If Not DBMovieset.MovieSet.UniqueIDs.TMDbIdSpecified AndAlso DBMovieset.MoviesInSetSpecified Then
            DBMovieset.MovieSet.UniqueIDs.TMDbId = Addons.Instance.GetMovieCollectionId(DBMovieset.MoviesInSet.Item(0).DBMovie.MainDetails.UniqueIDs.IMDbId)
        End If

        If DBMovieset.MovieSet.UniqueIDs.TMDbIdSpecified Then
            LoadSettings_MovieSet()
            Dim _scraper As New Scraper(_SpecialSettings_MovieSet)

            Dim FilteredModifiers As Structures.ScrapeModifiers = Functions.ScrapeModifiersAndAlso(ScrapeModifiers, ConfigModifier_MovieSet)

            ImagesContainer = _scraper.GetImages_Movie_MovieSet(DBMovieset.MovieSet.UniqueIDs.TMDbId.ToString, FilteredModifiers)
        End If

        logger.Trace("[FanartTV_Image] [Scraper_MovieSet] [Done]")
        Return New Interfaces.AddonResult
    End Function

    Function Scraper_TV(ByRef DBTV As Database.DBElement, ByRef ImagesContainer As MediaContainers.SearchResultsContainer, ByVal ScrapeModifiers As Structures.ScrapeModifiers) As Interfaces.AddonResult Implements Interfaces.IAddon_Image_Scraper_TV.Scraper
        logger.Trace("[FanartTV_Image] [Scraper_TV] [Start]")

        LoadSettings_TV()
        Dim _scraper As New Scraper(_SpecialSettings_TV)

        Dim FilteredModifiers As Structures.ScrapeModifiers = Functions.ScrapeModifiersAndAlso(ScrapeModifiers, ConfigModifier_TV)

        Select Case DBTV.ContentType
            Case Enums.ContentType.TVEpisode
                If DBTV.TVShow.UniqueIDs.TVDbIdSpecified Then
                    If FilteredModifiers.MainFanart Then
                        ImagesContainer.MainFanarts = _scraper.GetImages_TV(DBTV.TVShow.UniqueIDs.TVDbId, FilteredModifiers).MainFanarts
                    End If
                Else
                    logger.Trace(String.Concat("[FanartTV_Image] [Scraper_TV] [Abort] No TVDB ID exist to search: ", DBTV.MainDetails.Title))
                End If
            Case Enums.ContentType.TVSeason
                If DBTV.TVShow.UniqueIDs.TVDbIdSpecified Then
                    ImagesContainer = _scraper.GetImages_TV(DBTV.TVShow.UniqueIDs.TVDbId, FilteredModifiers)
                Else
                    logger.Trace(String.Concat("[FanartTV_Image] [Scraper_TV] [Abort] No TVDB ID exist to search: ", DBTV.MainDetails.Title))
                End If
            Case Enums.ContentType.TVShow
                If DBTV.TVShow.UniqueIDs.TVDbIdSpecified Then
                    ImagesContainer = _scraper.GetImages_TV(DBTV.TVShow.UniqueIDs.TVDbId, FilteredModifiers)
                Else
                    logger.Trace(String.Concat("[FanartTV_Image] [Scraper_TV] [Abort] No TVDB ID exist to search: ", DBTV.MainDetails.Title))
                End If
            Case Else
                logger.Error(String.Concat("[FanartTV_Image] [Scraper_TV] [Abort] Unhandled ContentType"))
        End Select

        logger.Trace("[FanartTV_Image] [Scraper_TV] [Done]")
        Return New Interfaces.AddonResult
    End Function

    Public Sub ScraperOrderChanged_Movie() Implements Interfaces.IAddon_Image_Scraper_Movie.ScraperOrderChanged
        _setup_Movie.orderChanged()
    End Sub

    Public Sub ScraperOrderChanged_MovieSet() Implements Interfaces.IAddon_Image_Scraper_Movieset.ScraperOrderChanged
        _setup_MovieSet.orderChanged()
    End Sub

    Public Sub ScraperOrderChanged_TV() Implements Interfaces.IAddon_Image_Scraper_TV.ScraperOrderChanged
        _setup_TV.orderChanged()
    End Sub

#End Region 'Methods

#Region "Nested Types"

    Structure AddonSettings

#Region "Fields"

        Dim ApiKey As String
        Dim ClearArtOnlyHD As Boolean
        Dim ClearLogoOnlyHD As Boolean
        Dim GetEnglishImages As Boolean
        Dim GetBlankImages As Boolean
        Dim PrefLanguage As String
        Dim PrefLanguageOnly As Boolean

#End Region 'Fields

    End Structure

#End Region 'Nested Types

End Class