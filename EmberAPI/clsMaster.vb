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

Imports System.IO

Public Class Master

#Region "Fields"

    Public Shared fLoading As New frmSplash

    Public Shared isUserInteractive As Boolean = True
    Public Shared isCL As Boolean
    Public Shared appArgs As ApplicationServices.StartupEventArgs

    Public Shared AddonsPath As String = Path.Combine(Functions.AppPath, "Addons")
    Public Shared AppPos As New Drawing.Rectangle
    Public Shared CanScanDiscImage As Boolean
    Public Shared DB As New Database
    Public Shared DefaultOptions_Movie As New Structures.ScrapeOptions
    Public Shared DefaultOptions_Movieset As New Structures.ScrapeOptions
    Public Shared DefaultOptions_TV As New Structures.ScrapeOptions
    Public Shared SettingsPath As String = Path.Combine(Functions.AppPath, "Profiles\Default")
    Public Shared TempPath As String = Path.Combine(Functions.AppPath, "Temp")
    Public Shared Thumbs As New Dictionary(Of Long, MediaContainers.Image)
    Public Shared eLang As New XmlTranslations
    Public Shared eProfiles As New Profiles
    Public Shared eSettings As New Settings
    Public Shared eAdvancedSettings As New XmlAdvancedSettings(Path.Combine(SettingsPath, "AdvancedSettings.xml"))

#End Region 'Fields

#Region "Properties"

    Public Shared ReadOnly Property Is32Bit() As Boolean
        Get
            Return IntPtr.Size = 4
        End Get
    End Property

    Public Shared ReadOnly Property Version() As String
        Get
            Return String.Format("Version {0}.{1}.{2} {3}",
                                 My.Application.Info.Version.Major,
                                 My.Application.Info.Version.Minor,
                                 My.Application.Info.Version.Build,
                                 If(Is32Bit, "x86", "x64"))
        End Get
    End Property

#End Region 'Properties

End Class