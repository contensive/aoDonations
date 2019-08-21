
Option Strict On
Option Explicit On
'
Namespace Contensive.Addons.aoDonations
    Public Class donationFormView
        '
        '====================================================================================================
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="CP"></param>
        ''' <returns></returns>
        Public Shared Function getView(ByVal CP As BaseClasses.CPBaseClass) As String
            Dim returnHtml As String = ""
            Try
                Dim cs As BaseClasses.CPCSBaseClass = CP.CSNew()

                If cs.Open("Layouts", "ccGUID='{5F7A9A40-C01D-4B30-8720-26BF4E81C9AA}'", "", True, "layout") Then
                    returnHtml = cs.GetText("layout")
                End If
                returnHtml = returnHtml.Replace("{{State Select}}", commonClass.getStateSelect(CP, "DFStateID"))
                returnHtml = returnHtml.Replace("{{reCaptcha}}", CP.Utils.ExecuteAddon("reCAPTCHA Display"))
                returnHtml += commonClass.getHelpWrapper(CP, "")
            Catch ex As Exception
                CP.Site.ErrorReport(ex)
            End Try
            Return returnHtml
        End Function
    End Class
End Namespace

