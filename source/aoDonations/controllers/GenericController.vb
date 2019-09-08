
Option Strict On
Option Explicit On


Namespace Contensive.Addons.aoDonations
    '
    Public Class GenericController
        '
        '====================================================================================================
        '
        Public Shared Function getStateSelect(ByVal CP As Contensive.BaseClasses.CPBaseClass, ByVal requestName As String, Optional ByVal initialValue As Integer = 0) As String
            Dim returnHtml As String = ""
            Try
                Dim cs As BaseClasses.CPCSBaseClass = CP.CSNew()
                Dim recordID As Integer = 0
                '
                If cs.Open("States", "", "name", True, "id,name") Then
                    returnHtml += "<select name=""" & requestName & """ id=""" & requestName & """>"
                    returnHtml += "<option value="""">Select One</option>"
                    Do While cs.OK()
                        recordID = cs.GetInteger("id")
                        '
                        returnHtml += "<option "
                        If initialValue = recordID Then
                            returnHtml += "selected "
                        End If
                        returnHtml += "value=""" & recordID & """>" & cs.GetText("name") & "</option>"
                        cs.GoNext()
                    Loop
                    returnHtml += "</select>"
                End If
                cs.Close()
                '
            Catch ex As Exception
                CP.Site.ErrorReport(ex, "error in Contensive.Addons.Foundation.commonClass.getStateSelect")
            End Try
            Return returnHtml
        End Function
        '
        Public Shared Function getHelpWrapper(ByVal CP As Contensive.BaseClasses.CPBaseClass, ByVal content As String) As String
            '
            Dim returnHtml As String = ""
            '
            Try
                Dim cs As BaseClasses.CPCSBaseClass = CP.CSNew()
                '
                If (CP.User.IsAdmin()) And (CP.User.IsEditingAnything()) Then
                    If content = "" Then
                        If cs.Open("Add-on Collections", "ccGUID='{A44634C2-D002-428E-818E-1531BF1EBFB0}'", "", True, "helpLink") Then
                            content = cs.GetText("helpLink")
                            '
                            '   add some copy
                            '
                            If content <> "" Then
                                content = "<b>Administrator</b><br><br>For more information on this add on, visit the following link: <a target=""_blank"" href=""" & content & """>" & content & "</a>"
                            End If
                        End If
                        cs.Close()
                    End If
                    '
                    '   final check
                    '
                    If content <> "" Then
                        returnHtml += CP.Html.div(CP.Html.div(content, "", "ccHintWrapperContent"), "", "ccHintWrapper")
                    End If
                End If
            Catch ex As Exception
                Try
                    CP.Site.ErrorReport(ex, "error in Contensive.Addons.Foundation.commonClass.getGroupTourAttendeeString")
                Catch errObj As Exception
                End Try
            End Try
            '
            Return returnHtml
        End Function
        '
    End Class
    '
End Namespace
