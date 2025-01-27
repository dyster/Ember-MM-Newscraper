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

Public Class Addon
    Implements Interfaces.IAddon

#Region "Fields"

    Private _setup As frmSettingsHolder
    Private _AssemblyName As String = String.Empty

#End Region 'Fields

#Region "Events"

    'Public Event GenericEvent(ByVal eventType As Enums.AddonEventType, ByRef parameters As List(Of Object)) Implements Interfaces.IAddon_Generic.GenericEvent
    'Public Event AddonSettingsChanged() Implements Interfaces.IAddon_Generic.AddonSettingsChanged
    'Public Event AddonStateChanged(ByVal name As String, ByVal state As Boolean, ByVal diffOrder As Integer) Implements Interfaces.IAddon_Generic.AddonStateChanged
    'Public Event AddonNeedsRestart() Implements Interfaces.IAddon_Generic.AddonNeedsRestart

#End Region 'Events

#Region "Properties"

    Public Property Enabled() As Boolean Implements Interfaces.IAddon.IsEnabled_Generic
        Get
            Return True
        End Get
        Set(ByVal value As Boolean)
            Return
        End Set
    End Property

    ReadOnly Property IsBusy() As Boolean Implements Interfaces.IAddon.IsBusy
        Get
            Return False
        End Get
    End Property

    'Public ReadOnly Property Name() As String Implements Interfaces.IAddon_Generic.Name
    '    Get
    '        Return "Video Source Mapping"
    '    End Get
    'End Property

    Public ReadOnly Property Capabilities_AddonEventTypes() As List(Of Enums.AddonEventType) Implements Interfaces.IAddon.Capabilities_AddonEventTypes
        Get
            Return New List(Of Enums.AddonEventType)(New Enums.AddonEventType() {Enums.AddonEventType.Generic})
        End Get
    End Property

    Public ReadOnly Property Capabilities_ScraperCapatibilities As List(Of Enums.ScraperCapatibility) Implements Interfaces.IAddon.Capabilities_ScraperCapatibilities
        Get
            Return New List(Of Enums.ScraperCapatibility)
        End Get
    End Property

    'Public ReadOnly Property Version() As String Implements Interfaces.IAddon_Generic.Version
    '    Get
    '        Return FileVersionInfo.GetVersionInfo(Reflection.Assembly.GetExecutingAssembly.Location).FileVersion.ToString
    '    End Get
    'End Property

    Public Property IsEnabled_Data_Movie As Boolean Implements Interfaces.IAddon.IsEnabled_Data_Movie

    Public Property IsEnabled_Data_Movieset As Boolean Implements Interfaces.IAddon.IsEnabled_Data_Movieset

    Public Property IsEnabled_Data_TV As Boolean Implements Interfaces.IAddon.IsEnabled_Data_TV

    Public Property IsEnabled_Image_Movie As Boolean Implements Interfaces.IAddon.IsEnabled_Image_Movie

    Public Property IsEnabled_Image_Movieset As Boolean Implements Interfaces.IAddon.IsEnabled_Image_Movieset

    Public Property IsEnabled_Image_TV As Boolean Implements Interfaces.IAddon.IsEnabled_Image_TV

    Public Property IsEnabled_Theme_Movie As Boolean Implements Interfaces.IAddon.IsEnabled_Theme_Movie

    Public Property IsEnabled_Theme_TV As Boolean Implements Interfaces.IAddon.IsEnabled_Theme_TV

    Public Property IsEnabled_Trailer_Movie As Boolean Implements Interfaces.IAddon.IsEnabled_Trailer_Movie

    Public Property SettingsPanels As New List(Of Interfaces.ISettingsPanel) Implements Interfaces.IAddon.SettingsPanels

#End Region 'Properties

#Region "Methods"

    Public Sub Init() Implements Interfaces.IAddon.Init

    End Sub

    Public Sub InjectSettingsPanels() Implements Interfaces.IAddon.InjectSettingsPanels

        _setup = New frmSettingsHolder
        _setup.UniqueId = _AssemblyName

        '_setup.Type = Master.eLang.GetString(429, "Miscellaneous")
        _setup.ImageIndex = -1
        _setup.Image = My.Resources.MediaSourcesEditor
        _setup.Order = 100

        SettingsPanels.Add(_setup)
    End Sub

    Public Function Run(ByRef dbElement As Database.DBElement, ByVal eventType As Enums.AddonEventType, parameters As List(Of Object)) As Interfaces.AddonResult Implements Interfaces.IAddon.Run
        Return New Interfaces.AddonResult
    End Function

    Public Sub SaveSettings(ByVal doDispose As Boolean) Implements Interfaces.IAddon.SaveSettings
        If Not _setup Is Nothing Then _setup.SaveChanges()
        If doDispose Then
            'RemoveHandler _setup.ModuleSettingsChanged, AddressOf Handle_ModuleSettingsChanged
            _setup.Dispose()
        End If
    End Sub

#End Region 'Methods

End Class