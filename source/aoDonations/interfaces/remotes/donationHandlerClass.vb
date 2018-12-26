
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
                Dim donationDetails = New donationRequestModel(CP)
                Dim response As donationResponseModel
                Dim jsonSerializer As New System.Web.Script.Serialization.JavaScriptSerializer
                '
                ' -- track if the user is authenticated on entry. If not and their email matches an existing account, we will let them use the account then log then back out on exit
                Dim authenticatedOnEnter As Boolean = CP.User.IsAuthenticated()
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
                '
                If (Not authenticatedOnEnter) And (CP.User.IsAuthenticated) Then
                    CP.User.Logout()
                End If

            Catch ex As Exception
                CP.Site.ErrorReport(ex)
            End Try
            Return resultJSON
        End Function
    End Class
    '
End Namespace
