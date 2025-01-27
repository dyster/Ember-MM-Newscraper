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
Imports System.IO

Public Class Addon
    Inherits AddonBase
    Implements Interfaces.IAddon

#Region "Delegates"

    Public Delegate Sub Delegate_SetToolsStripItem(value As ToolStripItem)
    Public Delegate Sub Delegate_RemoveToolsStripItem(value As ToolStripItem)
    Public Delegate Sub Delegate_SetToolsStripItemVisibility(control As ToolStripItem, value As Boolean)

#End Region 'Delegates

#Region "Fields"

    Shared logger As Logger = LogManager.GetCurrentClassLogger()

    Friend WithEvents bwCopyOrMove As New ComponentModel.BackgroundWorker

    Private _AssemblyName As String = String.Empty
    Private _MySettings As New MySettings
    Private eSettings As New Settings
    Private _enabled As Boolean = False
    Private _Name As String = Master.eLang.GetString(311, "Media File Manager")
    Private _setup As frmSettingsHolder
    Private cmnuMediaCustomList As New List(Of ToolStripMenuItem)
    Private cmnuMedia_Movies As New ToolStripMenuItem
    Private cmnuMedia_Shows As New ToolStripMenuItem
    Private cmnuSep_Movies As New ToolStripSeparator
    Private cmnuSep_Shows As New ToolStripSeparator
    Private WithEvents cmnuMediaCopy_Movies As New ToolStripMenuItem
    Private WithEvents cmnuMediaCopy_Shows As New ToolStripMenuItem
    Private WithEvents cmnuMediaMove_Movies As New ToolStripMenuItem
    Private WithEvents cmnuMediaMove_Shows As New ToolStripMenuItem

#End Region 'Fields

#Region "Events"

    Public Event GenericEvent(ByVal eventType As Enums.AddonEventType, ByRef parameters As List(Of Object)) Implements Interfaces.IAddon_Generic.GenericEvent
    Public Event AddonSettingsChanged() Implements Interfaces.IAddon_Generic.AddonSettingsChanged
    Public Event AddonStateChanged(ByVal name As String, ByVal state As Boolean, ByVal diffOrder As Integer) Implements Interfaces.IAddon_Generic.AddonStateChanged
    Public Event AddonNeedsRestart() Implements Interfaces.IAddon_Generic.AddonNeedsRestart

#End Region 'Events

#Region "Properties"

    Public ReadOnly Property Capabilities_AddonEventTypes() As List(Of Enums.AddonEventType) Implements Interfaces.IAddon.Capabilities_AddonEventTypes
        Get
            Return New List(Of Enums.AddonEventType)(New Enums.AddonEventType() {Enums.AddonEventType.Generic})
        End Get
    End Property

    Property Enabled() As Boolean Implements Interfaces.IAddon.IsEnabled_Generic
        Get
            Return _enabled
        End Get
        Set(ByVal value As Boolean)
            If _enabled = value Then Return
            _enabled = value
            If _enabled Then
                Enable()
            Else
                Disable()
            End If
        End Set
    End Property

    ReadOnly Property IsBusy() As Boolean Implements Interfaces.IAddon.IsBusy
        Get
            Return False
        End Get
    End Property

    ReadOnly Property Name() As String Implements Interfaces.IAddon_Generic.Name
        Get
            Return _Name
        End Get
    End Property

    ReadOnly Property Version() As String Implements Interfaces.IAddon_Generic.Version
        Get
            Return FileVersionInfo.GetVersionInfo(Reflection.Assembly.GetExecutingAssembly.Location).FileVersion.ToString
        End Get
    End Property

#End Region 'Properties

#Region "Methods"

    Public Sub Init(ByVal assemblyName As String, ByVal executable As String) Implements Interfaces.IAddon_Generic.Init
        _AssemblyName = assemblyName
        LoadSettings()
    End Sub

    Function InjectSettingsPanel() As Containers.SettingsPanel Implements Interfaces.IAddon.InjectSettingsPanel
        Dim SPanel As New Containers.SettingsPanel
        _setup = New frmSettingsHolder
        _setup.chkEnabled.Checked = _enabled
        _setup.chkTeraCopyEnable.Checked = _MySettings.TeraCopy
        _setup.txtTeraCopyPath.Text = _MySettings.TeraCopyPath
        _setup.lvPaths.Items.Clear()
        Dim lvItem As ListViewItem
        For Each e As SettingItem In eSettings.ModuleSettings
            lvItem = New ListViewItem
            lvItem.Text = e.Name
            lvItem.SubItems.Add(e.FolderPath)
            lvItem.SubItems.Add(e.Type.ToString)
            _setup.lvPaths.Items.Add(lvItem)
        Next
        SPanel.UniqueName = _Name
        SPanel.Title = Master.eLang.GetString(311, "Media File Manager")
        SPanel.Type = Master.eLang.GetString(802, "Addons")
        SPanel.ImageIndex = If(_enabled, 9, 10)
        SPanel.Order = 100
        SPanel.Panel = _setup.pnlSettings
        AddHandler _setup.ModuleEnabledChanged, AddressOf Handle_ModuleEnabledChanged
        AddHandler _setup.ModuleSettingsChanged, AddressOf Handle_ModuleSettingsChanged
        Return SPanel
    End Function

    Private Sub Handle_ModuleEnabledChanged(ByVal state As Boolean)
        RaiseEvent AddonStateChanged(_Name, state, 0)
    End Sub

    Private Sub Handle_ModuleSettingsChanged()
        RaiseEvent AddonSettingsChanged()
    End Sub

    Public Function Run(ByVal eventType As Enums.AddonEventType, ByRef parameters As List(Of Object), ByRef singleObject As Object, ByRef dbElement As Database.DBElement) As Interfaces.AddonResult Implements Interfaces.IAddon.Run
        Return New Interfaces.AddonResult
    End Function

    Public Sub LoadSettings()
        eSettings.ModuleSettings.Clear()
        Dim eMovies As List(Of TableItem) = Master.eAdvancedSettings.GetComplexSetting("MoviePaths")
        If eMovies IsNot Nothing Then
            For Each sett In eMovies
                eSettings.ModuleSettings.Add(New SettingItem With {.Name = sett.Name, .FolderPath = sett.Value, .Type = Enums.ContentType.Movie})
            Next
        End If
        Dim eShows As List(Of TableItem) = Master.eAdvancedSettings.GetComplexSetting("ShowPaths")
        If eShows IsNot Nothing Then
            For Each sett In eShows
                eSettings.ModuleSettings.Add(New SettingItem With {.Name = sett.Name, .FolderPath = sett.Value, .Type = Enums.ContentType.TVShow})
            Next
        End If
        _MySettings.TeraCopy = Master.eAdvancedSettings.GetBooleanSetting("TeraCopy", False)
        _MySettings.TeraCopyPath = Master.eAdvancedSettings.GetStringSetting("TeraCopyPath", String.Empty)
    End Sub

    Public Sub SaveSettings()
        Using settings = New AdvancedSettings()
            settings.SetBooleanSetting("TeraCopy", _MySettings.TeraCopy)
            settings.SetStringSetting("TeraCopyPath", _MySettings.TeraCopyPath)

            Dim eMovies As New List(Of TableItem)
            For Each e As SettingItem In eSettings.ModuleSettings.Where(Function(f) f.Type = Enums.ContentType.Movie)
                eMovies.Add(New TableItem With {.Name = e.Name, .Value = e.FolderPath})
            Next
            If eMovies IsNot Nothing Then
                settings.SetComplexSetting("MoviePaths", eMovies)
            End If

            Dim eShows As New List(Of TableItem)
            For Each e As SettingItem In eSettings.ModuleSettings.Where(Function(f) f.Type = Enums.ContentType.TVShow)
                eShows.Add(New TableItem With {.Name = e.Name, .Value = e.FolderPath})
            Next
            If eShows IsNot Nothing Then
                settings.SetComplexSetting("ShowPaths", eShows)
            End If
        End Using
    End Sub

    Private Sub bwCopyOrMove_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles bwCopyOrMove.DoWork
        Dim Args As Arguments = DirectCast(e.Argument, Arguments)
        If Not Args.SourcePath = Args.DestinationPath Then
            If Args.IsDirectory Then
                If Args.DoMove Then
                    FileUtils.Common.DirectoryMove(Args.SourcePath, Args.DestinationPath, True, True)
                Else
                    FileUtils.Common.DirectoryCopy(Args.SourcePath, Args.DestinationPath, True, True)
                End If
            Else
                If Args.DoMove Then
                    FileUtils.Common.FileMove(Args.SourcePath, Args.DestinationPath, True)
                Else
                    FileUtils.Common.FileCopy(Args.SourcePath, Args.DestinationPath, True)
                End If
            End If
        End If
    End Sub

    Sub CopyOrMove(ByVal sourcePath As String, ByVal destinationPath As String, ByVal isDirectory As Boolean, ByVal doMove As Boolean, ByVal title As String)
        Using dCopy As New dlgCopyFiles
            dCopy.Show()
            dCopy.prbStatus.Style = ProgressBarStyle.Marquee
            dCopy.Text = title
            dCopy.lblFilename.Text = Path.GetFileNameWithoutExtension(sourcePath)
            bwCopyOrMove.WorkerReportsProgress = True
            bwCopyOrMove.WorkerSupportsCancellation = True
            bwCopyOrMove.RunWorkerAsync(New Arguments With {.DestinationPath = destinationPath, .DoMove = doMove, .IsDirectory = isDirectory, .SourcePath = sourcePath})
            While bwCopyOrMove.IsBusy
                Application.DoEvents()
                Threading.Thread.Sleep(50)
            End While
        End Using
    End Sub

    Sub Disable()
        RemoveToolsStripItem_Movies(cmnuMedia_Movies)
        RemoveToolsStripItem_Movies(cmnuSep_Movies)
        RemoveToolsStripItem_Shows(cmnuMedia_Shows)
        RemoveToolsStripItem_Shows(cmnuSep_Shows)
    End Sub

    Public Sub RemoveToolsStripItem_Movies(value As ToolStripItem)
        If (Addons.Instance.RuntimeObjects.ContextMenuMovieList.InvokeRequired) Then
            Addons.Instance.RuntimeObjects.ContextMenuMovieList.Invoke(New Delegate_RemoveToolsStripItem(AddressOf RemoveToolsStripItem_Movies), New Object() {value})
            Exit Sub
        End If
        Addons.Instance.RuntimeObjects.ContextMenuMovieList.Items.Remove(value)
    End Sub

    Public Sub RemoveToolsStripItem_Shows(value As ToolStripItem)
        If (Addons.Instance.RuntimeObjects.ContextMenuTVShowList.InvokeRequired) Then
            Addons.Instance.RuntimeObjects.ContextMenuTVShowList.Invoke(New Delegate_RemoveToolsStripItem(AddressOf RemoveToolsStripItem_Shows), New Object() {value})
            Exit Sub
        End If
        Addons.Instance.RuntimeObjects.ContextMenuTVShowList.Items.Remove(value)
    End Sub

    Sub Enable()
        'cmnuMovies
        cmnuMedia_Movies.Text = Master.eLang.GetString(311, "Media File Manager")
        cmnuMediaMove_Movies.Text = Master.eLang.GetString(312, "Move To")
        cmnuMediaMove_Movies.Tag = "MOVE"
        cmnuMediaCopy_Movies.Text = Master.eLang.GetString(313, "Copy To")
        cmnuMediaCopy_Movies.Tag = "COPY"
        cmnuMedia_Movies.DropDownItems.Add(cmnuMediaMove_Movies)
        cmnuMedia_Movies.DropDownItems.Add(cmnuMediaCopy_Movies)

        SetToolsStripItem_Movies(cmnuSep_Movies)
        SetToolsStripItem_Movies(cmnuMedia_Movies)

        'cmnuShows
        cmnuMedia_Shows.Text = Master.eLang.GetString(311, "Media File Manager")
        cmnuMediaMove_Shows.Text = Master.eLang.GetString(312, "Move To")
        cmnuMediaMove_Shows.Tag = "MOVE"
        cmnuMediaCopy_Shows.Text = Master.eLang.GetString(313, "Copy To")
        cmnuMediaCopy_Shows.Tag = "COPY"
        cmnuMedia_Shows.DropDownItems.Add(cmnuMediaMove_Shows)
        cmnuMedia_Shows.DropDownItems.Add(cmnuMediaCopy_Shows)

        SetToolsStripItem_Shows(cmnuSep_Shows)
        SetToolsStripItem_Shows(cmnuMedia_Shows)

        PopulateFolders(cmnuMediaMove_Movies, Enums.ContentType.Movie)
        PopulateFolders(cmnuMediaMove_Shows, Enums.ContentType.TVShow)
        PopulateFolders(cmnuMediaCopy_Movies, Enums.ContentType.Movie)
        PopulateFolders(cmnuMediaCopy_Shows, Enums.ContentType.TVShow)
        SetToolsStripItemVisibility(cmnuMedia_Movies, True)
        SetToolsStripItemVisibility(cmnuMedia_Shows, True)
        SetToolsStripItemVisibility(cmnuSep_Movies, True)
        SetToolsStripItemVisibility(cmnuSep_Shows, True)
    End Sub

    Public Sub SetToolsStripItemVisibility(control As ToolStripItem, value As Boolean)
        If control.Owner IsNot Nothing Then
            If control.Owner.InvokeRequired Then
                control.Owner.Invoke(New Delegate_SetToolsStripItemVisibility(AddressOf SetToolsStripItemVisibility), New Object() {control, value})
            Else
                control.Visible = value
            End If
        End If
    End Sub

    Public Sub SetToolsStripItem_Movies(value As ToolStripItem)
        If Addons.Instance.RuntimeObjects.ContextMenuMovieList.InvokeRequired Then
            Addons.Instance.RuntimeObjects.ContextMenuMovieList.Invoke(New Delegate_SetToolsStripItem(AddressOf SetToolsStripItem_Movies), New Object() {value})
        Else
            Addons.Instance.RuntimeObjects.ContextMenuMovieList.Items.Add(value)
        End If
    End Sub

    Public Sub SetToolsStripItem_Shows(value As ToolStripItem)
        If Addons.Instance.RuntimeObjects.ContextMenuTVShowList.InvokeRequired Then
            Addons.Instance.RuntimeObjects.ContextMenuTVShowList.Invoke(New Delegate_SetToolsStripItem(AddressOf SetToolsStripItem_Shows), New Object() {value})
        Else
            Addons.Instance.RuntimeObjects.ContextMenuTVShowList.Items.Add(value)
        End If
    End Sub

    Private Sub FolderSubMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim ItemsToWork As New List(Of FileSystemInfo)
            Dim MediaToWork As New List(Of Long)
            Dim ID As Int64 = -1
            Dim tMItem As ToolStripMenuItem = DirectCast(sender, ToolStripMenuItem)
            Dim doMove As Boolean = False
            Dim dstPath As String = String.Empty
            Dim useTeraCopy As Boolean = False
            Dim ContentType As Enums.ContentType = DirectCast(tMItem.Tag, SettingItem).Type

            If DirectCast(tMItem.Tag, SettingItem).FolderPath = "CUSTOM" Then
                Dim fl As New FolderBrowserDialog
                fl.ShowDialog()
                dstPath = fl.SelectedPath
            Else
                dstPath = DirectCast(tMItem.Tag, SettingItem).FolderPath
            End If

            Select Case tMItem.OwnerItem.Tag.ToString
                Case "MOVE"
                    doMove = True
            End Select

            If _MySettings.TeraCopy AndAlso (String.IsNullOrEmpty(_MySettings.TeraCopyPath) OrElse Not File.Exists(_MySettings.TeraCopyPath)) Then
                MessageBox.Show(Master.eLang.GetString(398, "TeraCopy.exe not found"), Master.eLang.GetString(1134, "Error"), MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Try
            End If

            Dim mTeraCopy As New TeraCopy(_MySettings.TeraCopyPath, dstPath, doMove)

            If Not String.IsNullOrEmpty(dstPath) Then
                If ContentType = Enums.ContentType.Movie Then
                    For Each sRow As DataGridViewRow In Addons.Instance.RuntimeObjects.MediaListMovies.SelectedRows
                        ID = Convert.ToInt64(sRow.Cells("idMovie").Value)
                        If Not MediaToWork.Contains(ID) Then
                            MediaToWork.Add(ID)
                        End If
                    Next
                ElseIf ContentType = Enums.ContentType.TVShow Then
                    For Each sRow As DataGridViewRow In Addons.Instance.RuntimeObjects.MediaListTVShows.SelectedRows
                        ID = Convert.ToInt64(sRow.Cells("idShow").Value)
                        If Not MediaToWork.Contains(ID) Then
                            MediaToWork.Add(ID)
                        End If
                    Next
                End If
                If MediaToWork.Count > 0 Then
                    Dim strMove As String = String.Empty
                    Dim strCopy As String = String.Empty
                    If ContentType = Enums.ContentType.Movie Then
                        strMove = Master.eLang.GetString(314, "Move {0} Movie(s) To {1}")
                        strCopy = Master.eLang.GetString(315, "Copy {0} Movie(s) To {1}")
                    ElseIf ContentType = Enums.ContentType.TVShow Then
                        strMove = Master.eLang.GetString(888, "Move {0} TV Show(s) To {1}")
                        strCopy = Master.eLang.GetString(889, "Copy {0} TV Show(s) To {1}")
                    End If

                    If MessageBox.Show(String.Format(If(doMove, strMove, strCopy),
                                            MediaToWork.Count, dstPath), If(doMove, Master.eLang.GetString(910, "Move"), Master.eLang.GetString(911, "Copy")), MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                        If ContentType = Enums.ContentType.Movie Then
                            Dim FileDelete As New FileUtils.Delete
                            For Each movieID As Long In MediaToWork
                                Dim mMovie As Database.DBElement = Master.DB.Load_Movie(movieID)
                                ItemsToWork = FileUtils.Common.GetAllItemsOfDBElement(mMovie)
                                For Each item In ItemsToWork
                                    If item.Attributes = FileAttributes.Directory AndAlso item.Exists Then
                                        If _MySettings.TeraCopy Then
                                            mTeraCopy.Sources.Add(item.FullName)
                                        Else
                                            Select Case tMItem.OwnerItem.Tag.ToString
                                                Case "MOVE"
                                                    CopyOrMove(item.FullName, Path.Combine(dstPath, item.Name), True, True, Master.eLang.GetString(316, "Moving Movie"))
                                                    Master.DB.Delete_Movie(movieID, False)
                                                Case "COPY"
                                                    CopyOrMove(item.FullName, Path.Combine(dstPath, item.Name), True, False, Master.eLang.GetString(317, "Copying Movie"))
                                            End Select
                                        End If
                                    ElseIf item.Exists Then
                                        If _MySettings.TeraCopy Then
                                            mTeraCopy.Sources.Add(item.FullName)
                                        Else
                                            Select Case tMItem.OwnerItem.Tag.ToString
                                                Case "MOVE"
                                                    CopyOrMove(item.FullName, Path.Combine(dstPath, item.Name), False, True, Master.eLang.GetString(316, "Moving Movie"))
                                                    Master.DB.Delete_Movie(movieID, False)
                                                Case "COPY"
                                                    CopyOrMove(item.FullName, Path.Combine(dstPath, item.Name), False, False, Master.eLang.GetString(317, "Copying Movie"))
                                            End Select
                                        End If
                                    End If
                                Next
                            Next
                            If Not _MySettings.TeraCopy AndAlso doMove Then Addons.Instance.RuntimeObjects.InvokeLoadMedia(New Structures.ScanOrClean With {.Movies = True})
                        ElseIf ContentType = Enums.ContentType.TVShow Then
                            Dim FileDelete As New FileUtils.Delete
                            For Each tShowID As Long In MediaToWork
                                Dim mShow As Database.DBElement = Master.DB.Load_TVShow(tShowID, False, False)
                                Dim diShowPath As New DirectoryInfo(mShow.ShowPath)
                                If diShowPath.Exists Then
                                    If _MySettings.TeraCopy Then
                                        mTeraCopy.Sources.Add(mShow.ShowPath)
                                    Else
                                        Select Case tMItem.OwnerItem.Tag.ToString
                                            Case "MOVE"
                                                CopyOrMove(diShowPath.FullName, Path.Combine(dstPath, diShowPath.Name), True, True, Master.eLang.GetString(899, "Moving TV Show"))
                                                Master.DB.Delete_TVShow(tShowID, False)
                                            Case "COPY"
                                                CopyOrMove(diShowPath.FullName, Path.Combine(dstPath, diShowPath.Name), True, False, Master.eLang.GetString(899, "Moving TV Show"))
                                        End Select
                                    End If
                                End If
                            Next
                            If Not _MySettings.TeraCopy AndAlso doMove Then Addons.Instance.RuntimeObjects.InvokeLoadMedia(New Structures.ScanOrClean With {.TV = True})
                        End If
                        If _MySettings.TeraCopy Then mTeraCopy.RunTeraCopy()
                    End If
                End If
            End If

        Catch ex As Exception
            logger.Error(ex, New StackFrame().GetMethod().Name)
        End Try
    End Sub

    Sub PopulateFolders(ByVal menuItem As ToolStripMenuItem, ByVal contentType As Enums.ContentType)
        menuItem.DropDownItems.Clear()
        cmnuMediaCustomList.RemoveAll(Function(b) True)

        Dim FolderSubMenuItemCustom As New ToolStripMenuItem With {
            .Text = String.Concat(Master.eLang.GetString(338, "Select path"), "..."),
            .Tag = New SettingItem With {.Name = "CUSTOM", .FolderPath = "CUSTOM", .Type = contentType}
        }
        menuItem.DropDownItems.Add(FolderSubMenuItemCustom)
        AddHandler FolderSubMenuItemCustom.Click, AddressOf FolderSubMenuItem_Click

        If eSettings.ModuleSettings.Where(Function(f) f.Type = contentType).Count > 0 Then
            Dim SubMenuSep As New ToolStripSeparator
            menuItem.DropDownItems.Add(SubMenuSep)
        End If

        For Each e In eSettings.ModuleSettings.Where(Function(f) f.Type = contentType)
            Dim FolderSubMenuItem As New ToolStripMenuItem With {
                .Text = e.Name,
                .Tag = New SettingItem With {.Name = e.Name, .FolderPath = e.FolderPath, .Type = contentType}
            }
            cmnuMediaCustomList.Add(FolderSubMenuItem)
            AddHandler FolderSubMenuItem.Click, AddressOf FolderSubMenuItem_Click
        Next

        For Each i In cmnuMediaCustomList
            menuItem.DropDownItems.Add(i)
        Next

        SetToolsStripItemVisibility(cmnuSep_Movies, True)
        SetToolsStripItemVisibility(cmnuSep_Shows, True)
        SetToolsStripItemVisibility(cmnuMedia_Movies, True)
        SetToolsStripItemVisibility(cmnuMedia_Shows, True)
    End Sub

    Sub SaveSettings(ByVal doDispose As Boolean) Implements Interfaces.IAddon_Generic.SaveSettings
        Enabled = _setup.chkEnabled.Checked
        _MySettings.TeraCopy = _setup.chkTeraCopyEnable.Checked
        _MySettings.TeraCopyPath = _setup.txtTeraCopyPath.Text
        eSettings.ModuleSettings.Clear()
        For Each e As ListViewItem In _setup.lvPaths.Items
            If Not String.IsNullOrEmpty(e.SubItems(0).Text) AndAlso Not String.IsNullOrEmpty(e.SubItems(1).Text) AndAlso e.SubItems(2).Text = "Movie" Then
                eSettings.ModuleSettings.Add(New SettingItem With {.Name = e.SubItems(0).Text, .FolderPath = e.SubItems(1).Text, .Type = Enums.ContentType.Movie})
            End If
        Next
        For Each e As ListViewItem In _setup.lvPaths.Items
            If Not String.IsNullOrEmpty(e.SubItems(0).Text) AndAlso Not String.IsNullOrEmpty(e.SubItems(1).Text) AndAlso e.SubItems(2).Text = "TVShow" Then
                eSettings.ModuleSettings.Add(New SettingItem With {.Name = e.SubItems(0).Text, .FolderPath = e.SubItems(1).Text, .Type = Enums.ContentType.TVShow})
            End If
        Next
        SaveSettings()
        PopulateFolders(cmnuMediaMove_Movies, Enums.ContentType.Movie)
        PopulateFolders(cmnuMediaMove_Shows, Enums.ContentType.TVShow)
        PopulateFolders(cmnuMediaCopy_Movies, Enums.ContentType.Movie)
        PopulateFolders(cmnuMediaCopy_Shows, Enums.ContentType.TVShow)
        If doDispose Then
            RemoveHandler _setup.ModuleEnabledChanged, AddressOf Handle_ModuleEnabledChanged
            RemoveHandler _setup.ModuleSettingsChanged, AddressOf Handle_ModuleSettingsChanged
            _setup.Dispose()
        End If
    End Sub

#End Region 'Methods

#Region "Nested Types"

    Private Structure Arguments

#Region "Fields"

        Dim DestinationPath As String
        Dim DoMove As Boolean
        Dim IsDirectory As Boolean
        Dim SourcePath As String

#End Region 'Fields

    End Structure

    Private Structure MySettings

#Region "Fields"

        Dim TeraCopy As Boolean
        Dim TeraCopyPath As String

#End Region 'Fields

    End Structure

    Class SettingItem

#Region "Fields"

        Public FolderPath As String
        Public Name As String
        Public Type As Enums.ContentType

#End Region 'Fields

    End Class

    Class Settings

#Region "Properties"

        Public Property ModuleSettings() As List(Of SettingItem) = New List(Of SettingItem)

#End Region 'Properties

    End Class

#End Region 'Nested Types

End Class