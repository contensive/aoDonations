
Option Strict On
Option Explicit On

Namespace Contensive.Addons.aoDonations
    Public Class donationHandlerClass
        Inherits BaseClasses.AddonBaseClass
        '
        '====================================================================================================
        ''' <summary>
        ''' donation form remote method
        ''' </summary>
        ''' <param name="CP"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overrides Function Execute(ByVal CP As BaseClasses.CPBaseClass) As Object
            Dim resultJSON As String = ""
            Try
                Dim errMsg As String = ""
                Dim donationDetails = New donationDetailsViewModel(CP)
                Dim response As donationFormRequestModel
                Dim jsonSerializer As New System.Web.Script.Serialization.JavaScriptSerializer
                '
                ' verify if the user is not logged in, we log them out
                '
                If Not (CP.User.IsAuthenticated()) Then
                    If (CP.User.IsRecognized()) Then
                        CP.User.Logout()
                    End If
                End If
                '
                ' process the form
                ' 
                response = donationHandlerControllerAndView.processAndReturn(CP, errMsg, donationDetails)
                resultJSON = jsonSerializer.Serialize(response)

            Catch ex As Exception
                CP.Site.ErrorReport(ex)
            End Try
            Return resultJSON
        End Function
    End Class
    '
End Namespace
