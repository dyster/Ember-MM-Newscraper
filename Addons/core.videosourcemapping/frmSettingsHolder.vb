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
Imports System.Drawing
Imports System.Windows.Forms

Public Class frmSettingsHolder
    Implements Interfaces.ISettingsPanel

#Region "Fields"

    Private _temp_VideoSourceByExtensionMappings As New List(Of SimpleMapping)
    Private _temp_VideoSourceByNameMappings As New List(Of RegexMapping)

#End Region 'Fields

#Region "Events"

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

#Region "Dialog Methods"

    Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.

        _temp_VideoSourceByExtensionMappings = APIXML.VideoSourceByExtensionMappings.Mappings
        _temp_VideoSourceByNameMappings = APIXML.VideoSourceByNameMappings.Mappings
        Title = Master.eLang.GetString(784, "Video Source Mapping")
        MainPanel = pnlMain
        DataGridView_Load_ByExtension()
        DataGridView_Load_ByName()
        Setup()
    End Sub

    Sub Setup()
        btnLoadDefaultsByName.Text = Master.eLang.GetString(713, "Defaults")
        dgvByExtension.Columns(0).HeaderText = Master.eLang.GetString(775, "File Extension")
        dgvByExtension.Columns(1).HeaderText = Master.eLang.GetString(1116, "Mapped to")
        dgvByName.Columns(0).HeaderText = Master.eLang.GetString(699, "Regex")
        dgvByName.Columns(1).HeaderText = Master.eLang.GetString(1116, "Mapped to")
        lblByExtension.Text = Master.eLang.GetString(765, "Map Video Source by File Extension")
        lblByName.Text = Master.eLang.GetString(848, "Map Video Source by Filename")
    End Sub

#End Region 'Dialog Methods

#Region "Methods"

    Private Sub DataGridView_CurrentCellDirtyStateChanged(ByVal sender As Object, ByVal e As EventArgs) Handles dgvByExtension.CurrentCellDirtyStateChanged, dgvByName.CurrentCellDirtyStateChanged
        RaiseEvent ModuleSettingsChanged()
    End Sub

    Private Sub DataGridView_CellEndEdit_ByExtension(sender As Object, e As DataGridViewCellEventArgs) Handles dgvByExtension.CellEndEdit
        Dim currRow = dgvByExtension.Rows(e.RowIndex)
        If e.ColumnIndex = 0 AndAlso
            Not currRow.IsNewRow AndAlso
            currRow.Cells(e.ColumnIndex).Value IsNot Nothing AndAlso
            Not String.IsNullOrEmpty(currRow.Cells(e.ColumnIndex).Value.ToString) AndAlso
            Not dgvByExtension.Rows(e.RowIndex).Cells(e.ColumnIndex).Value.ToString.StartsWith(".") Then
            dgvByExtension.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = String.Concat(".", dgvByExtension.Rows(e.RowIndex).Cells(e.ColumnIndex).Value)
        End If
    End Sub

    Private Sub DataGridView_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs) Handles dgvByExtension.KeyDown, dgvByName.KeyDown
        e.Handled = (e.KeyCode = Keys.Enter) OrElse (e.KeyCode = Keys.Escape)
    End Sub

    Private Sub DataGridView_Load_ByExtension()
        dgvByExtension.Rows.Clear()
        For Each item In _temp_VideoSourceByExtensionMappings
            dgvByExtension.Rows.Add(New Object() {item.Input, item.MappedTo})
        Next
        dgvByExtension.ClearSelection()
    End Sub

    Private Sub DataGridView_Load_ByName()
        dgvByName.Rows.Clear()
        For Each item In _temp_VideoSourceByNameMappings
            dgvByName.Rows.Add(New Object() {item.RegExp, item.Result})
        Next
        dgvByName.ClearSelection()
    End Sub

    Private Sub LoadDefaults_ByName(ByVal sender As Object, ByVal e As EventArgs) Handles btnLoadDefaultsByName.Click
        _temp_VideoSourceByNameMappings = APIXML.VideoSourceByNameMappings.GetDefaults()
        DataGridView_Load_ByName()
        RaiseEvent ModuleSettingsChanged()
    End Sub

    Public Sub SaveChanges()
        APIXML.VideoSourceByExtensionMappings.Mappings.Clear()
        For Each r As DataGridViewRow In dgvByExtension.Rows
            If Not r.IsNewRow AndAlso
                r.Cells(0).Value IsNot Nothing AndAlso
                r.Cells(1).Value IsNot Nothing AndAlso
                Not String.IsNullOrEmpty(r.Cells(0).Value.ToString) AndAlso
                Not String.IsNullOrEmpty(r.Cells(1).Value.ToString) AndAlso
                APIXML.VideoSourceByExtensionMappings.Mappings.FindIndex(Function(f) f.Input = r.Cells(0).Value.ToString) = -1 Then
                APIXML.VideoSourceByExtensionMappings.Mappings.Add(New SimpleMapping With {
                                                                   .Input = r.Cells(0).Value.ToString,
                                                                   .MappedTo = r.Cells(1).Value.ToString
                                                                   })
            End If
        Next
        APIXML.VideoSourceByExtensionMappings.Save()

        APIXML.VideoSourceByNameMappings.Mappings.Clear()
        For Each r As DataGridViewRow In dgvByName.Rows
            If Not r.IsNewRow AndAlso
                r.Cells(0).Value IsNot Nothing AndAlso
                r.Cells(1).Value IsNot Nothing AndAlso
                Not String.IsNullOrEmpty(r.Cells(0).Value.ToString) AndAlso
                Not String.IsNullOrEmpty(r.Cells(1).Value.ToString) AndAlso
                APIXML.VideoSourceByNameMappings.Mappings.FindIndex(Function(f) f.RegExp = r.Cells(0).Value.ToString) = -1 Then
                APIXML.VideoSourceByNameMappings.Mappings.Add(New RegexMapping With {
                                                              .RegExp = r.Cells(0).Value.ToString,
                                                              .Result = r.Cells(1).Value.ToString
                                                              })
            End If
        Next
        APIXML.VideoSourceByNameMappings.Save()
    End Sub

#End Region 'Methods

End Class