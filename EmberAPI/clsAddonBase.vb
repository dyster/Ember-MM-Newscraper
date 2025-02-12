''' <summary>
''' Adding this base type in an attempt to rewrite the addons to the new interface
''' </summary>
Public Class AddonBase
    Implements Interfaces.IAddon

    Public ReadOnly Property Capabilities_AddonEventTypes As List(Of Enums.AddonEventType) Implements Interfaces.IAddon.Capabilities_AddonEventTypes
        Get
            Throw New NotImplementedException()
        End Get
    End Property

    Public ReadOnly Property Capabilities_ScraperCapatibilities As List(Of Enums.ScraperCapatibility) Implements Interfaces.IAddon.Capabilities_ScraperCapatibilities
        Get
            Throw New NotImplementedException()
        End Get
    End Property

    Public ReadOnly Property IsBusy As Boolean Implements Interfaces.IAddon.IsBusy
        Get
            Throw New NotImplementedException()
        End Get
    End Property

    Public Property IsEnabled_Generic As Boolean Implements Interfaces.IAddon.IsEnabled_Generic
        Get
            Throw New NotImplementedException()
        End Get
        Set(value As Boolean)
            Throw New NotImplementedException()
        End Set
    End Property

    Public Property IsEnabled_Data_Movie As Boolean Implements Interfaces.IAddon.IsEnabled_Data_Movie
        Get
            Throw New NotImplementedException()
        End Get
        Set(value As Boolean)
            Throw New NotImplementedException()
        End Set
    End Property

    Public Property IsEnabled_Data_Movieset As Boolean Implements Interfaces.IAddon.IsEnabled_Data_Movieset
        Get
            Throw New NotImplementedException()
        End Get
        Set(value As Boolean)
            Throw New NotImplementedException()
        End Set
    End Property

    Public Property IsEnabled_Data_TV As Boolean Implements Interfaces.IAddon.IsEnabled_Data_TV
        Get
            Throw New NotImplementedException()
        End Get
        Set(value As Boolean)
            Throw New NotImplementedException()
        End Set
    End Property

    Public Property IsEnabled_Image_Movie As Boolean Implements Interfaces.IAddon.IsEnabled_Image_Movie
        Get
            Throw New NotImplementedException()
        End Get
        Set(value As Boolean)
            Throw New NotImplementedException()
        End Set
    End Property

    Public Property IsEnabled_Image_Movieset As Boolean Implements Interfaces.IAddon.IsEnabled_Image_Movieset
        Get
            Throw New NotImplementedException()
        End Get
        Set(value As Boolean)
            Throw New NotImplementedException()
        End Set
    End Property

    Public Property IsEnabled_Image_TV As Boolean Implements Interfaces.IAddon.IsEnabled_Image_TV
        Get
            Throw New NotImplementedException()
        End Get
        Set(value As Boolean)
            Throw New NotImplementedException()
        End Set
    End Property

    Public Property IsEnabled_Theme_Movie As Boolean Implements Interfaces.IAddon.IsEnabled_Theme_Movie
        Get
            Throw New NotImplementedException()
        End Get
        Set(value As Boolean)
            Throw New NotImplementedException()
        End Set
    End Property

    Public Property IsEnabled_Theme_TV As Boolean Implements Interfaces.IAddon.IsEnabled_Theme_TV
        Get
            Throw New NotImplementedException()
        End Get
        Set(value As Boolean)
            Throw New NotImplementedException()
        End Set
    End Property

    Public Property IsEnabled_Trailer_Movie As Boolean Implements Interfaces.IAddon.IsEnabled_Trailer_Movie
        Get
            Throw New NotImplementedException()
        End Get
        Set(value As Boolean)
            Throw New NotImplementedException()
        End Set
    End Property

    Public Property SettingsPanels As List(Of Interfaces.ISettingsPanel) Implements Interfaces.IAddon.SettingsPanels
        Get
            Throw New NotImplementedException()
        End Get
        Set(value As List(Of Interfaces.ISettingsPanel))
            Throw New NotImplementedException()
        End Set
    End Property

    Public Sub Init() Implements Interfaces.IAddon.Init
        Throw New NotImplementedException()
    End Sub

    Public Sub InjectSettingsPanels() Implements Interfaces.IAddon.InjectSettingsPanels
        Throw New NotImplementedException()
    End Sub

    Public Sub SaveSettings(doDispose As Boolean) Implements Interfaces.IAddon.SaveSettings
        Throw New NotImplementedException()
    End Sub

    Public Function Run(ByRef dbElement As Database.DBElement, type As Enums.AddonEventType, lstCommandLineParams As List(Of Object)) As Interfaces.AddonResult Implements Interfaces.IAddon.Run
        Throw New NotImplementedException()
    End Function
End Class
