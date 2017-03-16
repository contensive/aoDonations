
Option Strict On
Option Explicit On
'
Namespace Contensive.Addons.aoDonations
    '
    Public Class donationClass
        Inherits BaseClasses.AddonBaseClass
        '
        Private Const donationHandlerAddon As String = "{8D03670D-0258-4C2A-8BA4-8EC795842D88}"
        '
        '====================================================================================================
        ''' <summary>
        ''' donation form page
        ''' </summary>
        ''' <param name="CP"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overrides Function Execute(ByVal CP As BaseClasses.CPBaseClass) As Object
            Dim returnHtml As String = ""
            Try
                Dim donationHandler As New donationHandlerClass()
                returnHtml = donationFormView.getView(CP)
                'returnHtml = CP.Utils.EncodeText(donationHandler.Execute(CP))
                returnHtml = CP.Html.div(returnHtml, , , "DFContainer")
            Catch ex As Exception
                CP.Site.ErrorReport(ex, "error in Contensive.Addons.aoDonations.execute")
            End Try
            Return returnHtml
        End Function
    End Class
End Namespace

