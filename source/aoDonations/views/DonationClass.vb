
Option Strict On
Option Explicit On
'
Namespace Contensive.Addons.aoDonations
    '
    '====================================================================================================
    ''' <summary>
    ''' This is the addon dropped on the page
    ''' </summary>
    Public Class DonationClass
        Inherits BaseClasses.AddonBaseClass
        '
        Public Overrides Function Execute(ByVal CP As BaseClasses.CPBaseClass) As Object
            Dim result As String = ""
            Try
                result = My.Resources.DonationForm
                result = result.Replace("{{State Select}}", GenericController.getStateSelect(CP, "DFStateID"))
                result = result.Replace("{{reCaptcha}}", CP.Addon.Execute("reCAPTCHA Display").ToString())
                result += GenericController.getHelpWrapper(CP, "")
                result = CP.Html.div(result, "", "", "DFContainer")
            Catch ex As Exception
                CP.Site.ErrorReport(ex, "error in Contensive.Addons.aoDonations.execute")
            End Try
            Return result
        End Function
    End Class
End Namespace

