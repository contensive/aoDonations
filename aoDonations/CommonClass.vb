﻿
Option Strict On
Option Explicit On


Namespace Contensive.Addons.aoDonations
    '
    Public Class commonClass
        '
        Public Shared Function getDate(ByVal CP As Contensive.BaseClasses.CPBaseClass, ByVal dateVal As Date) As Date
            Try
                If dateVal.CompareTo(New Date(1900, 1, 1)) <= 0 Then
                    dateVal = Date.MinValue
                End If
                '
                Return dateVal
            Catch ex As Exception
                CP.Site.ErrorReport(ex, "error in Contensive.Addons.Foundation.commonClass.getDate")
            End Try
        End Function
        '
        Public Shared Function getStateSelect(ByVal CP As Contensive.BaseClasses.CPBaseClass, ByVal requestName As String, Optional ByVal initialValue As Integer = 0) As String
            Dim returnHtml As String = ""
            Try
                Dim cs As BaseClasses.CPCSBaseClass = CP.CSNew()
                Dim recordID As Integer = 0
                '
                If cs.Open("States", , "name", , "id,name") Then
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
        Public Shared Function getUploadField(ByVal CP As Contensive.BaseClasses.CPBaseClass, ByVal requestName As String) As String
            Dim returnHtml As String = ""
            Try
                '
                returnHtml += "<span class=""fileSelectContainer"">"
                returnHtml += "<input type=""file"" class=""file"" id=""" & requestName & """ name=""" & requestName & """>"
                returnHtml += "<input type=""text"" id=""" & requestName & "Holder"" name=""" & requestName & "Holder"">"
                returnHtml += "<a class=""SFUploadLink"" href=""#"">Upload File <span>n</span></a></span>"
                '
            Catch ex As Exception
                CP.Site.ErrorReport(ex, "error in Contensive.Addons.Foundation.commonClass.getUploadField")
            End Try
            Return returnHtml
        End Function
        '
        'Public Shared Function getCorporateSponsorshipID(ByVal CP As Contensive.BaseClasses.CPBaseClass) As Integer
        '    Try
        '        Dim recordID As Integer = 0
        '        Dim cs As BaseClasses.CPCSBaseClass = CP.CSNew()
        '        '
        '        If cs.Open("Corporate Sponsorships", "(visitID=" & CP.Visit.Id & ") and (completed<>1)", , , "id") Then
        '            recordID = cs.GetInteger("id")
        '        Else
        '            cs.Close()
        '            If cs.Insert("Corporate Sponsorships") Then
        '                cs.SetField("visitID", CP.Visit.Id)
        '                recordID = cs.GetInteger("id")
        '            End If
        '        End If
        '        cs.Close()
        '        '
        '        Return recordID
        '    Catch ex As Exception
        '        CP.Site.ErrorReport(ex, "error in Contensive.Addons.Foundation.commonClass.getCorporateSponsorshipID")
        '    End Try
        'End Function
        '
        Public Shared Function getMembershipID(ByVal CP As Contensive.BaseClasses.CPBaseClass) As Integer
            Try
                Dim recordID As Integer = 0
                Dim cs As BaseClasses.CPCSBaseClass = CP.CSNew()
                '
                If cs.Open("Membership Requests", "(visitID=" & CP.Visit.Id & ") and (completed<>1)", , , "id") Then
                    recordID = cs.GetInteger("id")
                Else
                    cs.Close()
                    If cs.Insert("Membership Requests") Then
                        cs.SetField("visitID", CP.Visit.Id.ToString())
                        cs.SetField("memberID", CP.User.Id.ToString())
                        recordID = cs.GetInteger("id")
                    End If
                End If
                cs.Close()
                '
                Return recordID
            Catch ex As Exception
                CP.Site.ErrorReport(ex, "error in Contensive.Addons.Foundation.commonClass.getMembershipID")
            End Try
        End Function
        '
        'Public Shared Function getStudentGroupTourID(ByVal CP As Contensive.BaseClasses.CPBaseClass) As Integer
        '    Try
        '        Dim recordID As Integer = 0
        '        Dim cs As BaseClasses.CPCSBaseClass = CP.CSNew()
        '        '
        '        If cs.Open("Student Group Tour Requests", "(visitID=" & CP.Visit.Id & ") and (completed<>1)", , , "id") Then
        '            recordID = cs.GetInteger("id")
        '        Else
        '            cs.Close()
        '            If cs.Insert("Student Group Tour Requests") Then
        '                cs.SetField("visitID", CP.Visit.Id)
        '                cs.SetField("memberID", CP.User.Id)
        '                recordID = cs.GetInteger("id")
        '            End If
        '        End If
        '        cs.Close()
        '        '
        '        Return recordID
        '    Catch ex As Exception
        '        CP.Site.ErrorReport(ex, "error in Contensive.Addons.Foundation.commonClass.getStudentGroupTourID")
        '    End Try
        'End Function
        '
        Public Shared Function getGroupTourID(ByVal CP As Contensive.BaseClasses.CPBaseClass) As Integer
            Try
                Dim recordID As Integer = 0
                Dim cs As BaseClasses.CPCSBaseClass = CP.CSNew()
                '
                If cs.Open("Group Tour Requests", "(visitID=" & CP.Visit.Id & ") and (completed<>1)", , , "id") Then
                    recordID = cs.GetInteger("id")
                Else
                    cs.Close()
                    If cs.Insert("Group Tour Requests") Then
                        cs.SetField("visitID", CP.Visit.Id.ToString())
                        cs.SetField("memberID", CP.User.Id.ToString())
                        recordID = cs.GetInteger("id")
                    End If
                End If
                cs.Close()
                '
                Return recordID
            Catch ex As Exception
                CP.Site.ErrorReport(ex, "error in Contensive.Addons.Foundation.commonClass.getGroupTourID")
            End Try
        End Function

        Public Shared Function getCorporateSponsorshipAmount(ByVal CP As Contensive.BaseClasses.CPBaseClass, ByVal corporateSponsorshipLevelID As Integer) As Double
            Try
                Dim amount As Double = 0
                Dim cs As BaseClasses.CPCSBaseClass = CP.CSNew()
                '
                If cs.Open("Corporate Sponsorship Levels", "id=" & corporateSponsorshipLevelID, , , "sponsorshipAmount") Then
                    amount = cs.GetNumber("sponsorshipAmount")
                End If
                cs.Close()
                '
                Return amount
            Catch ex As Exception
                CP.Site.ErrorReport(ex, "error in Contensive.Addons.Foundation.commonClass.getCorporateSponsorshipAmount")
            End Try
        End Function
        '
        Public Shared Function getMembershipAmount(ByVal CP As Contensive.BaseClasses.CPBaseClass, ByVal membershipLevelID As Integer) As Double
            Try
                Dim amount As Double = 0
                Dim cs As BaseClasses.CPCSBaseClass = CP.CSNew()
                '
                If cs.Open("Membership Levels", "id=" & membershipLevelID, , , "membershipAmount") Then
                    amount = cs.GetNumber("membershipAmount")
                End If
                cs.Close()
                '
                Return amount
            Catch ex As Exception
                CP.Site.ErrorReport(ex, "error in Contensive.Addons.Foundation.commonClass.getMembershipAmount")
            End Try
        End Function
        '
        Public Shared Function getExpirationSelect(ByVal CP As BaseClasses.CPBaseClass, ByVal requestNameA As String, ByVal requestNameB As String, ByVal selectClassName As String) As String
            Dim returnHtml As String = ""
            Try
                Dim dateMonth As String = ""
                Dim dateYear As String = ""
                Dim yearStart As Integer = 0
                Dim yearEnd As Integer = 0
                Dim counter As Integer = 0
                '
                yearStart = Year(Now)
                yearEnd = Year(Now) + 10
                '
                returnHtml += "<select id=""" & requestNameA & """ class=""" & selectClassName & """ size=""1"" name=""" & requestNameA & """>"
                returnHtml += "<option value="""" selected>MM</option>"
                For counter = 1 To 12
                    returnHtml += "<option value=""" & counter & """>" & counter & " - " & MonthName(counter) & "</option>"
                Next
                returnHtml += "</select> "
                '
                returnHtml += "<select id=""" & requestNameB & """ class=""" & selectClassName & """ size=""1"" name=""" & requestNameB & """>"
                returnHtml += "<option value="""" selected>YY</option>"
                For counter = yearStart To yearEnd
                    returnHtml += "<option value=""" & Right(counter.ToString(), 2) & """>" & counter & "</option>"
                Next
                returnHtml += "</select>"
                '
            Catch ex As Exception
                CP.Site.ErrorReport(ex, "error in Contensive.Addons.Foundation.commonClass.getExpirationSelect")
            End Try
            Return returnHtml
        End Function
        '
        'Public Shared Function getStudentGroupTourTypes(ByVal CP As BaseClasses.CPBaseClass) As String
        '    Try
        '        Dim s As String = ""
        '        Dim cs As BaseClasses.CPCSBaseClass = CP.CSNew()
        '        Dim recordID As Integer = 0
        '        Dim htmlID As String = ""
        '        '
        '        If cs.Open("Student Group Tour Types", , "sortOrder") Then
        '            Do While cs.OK()
        '                recordID = cs.GetInteger("id")
        '                htmlID = "SGTourTypeID_" & recordID
        '                '
        '                s += CP.Html.li(CP.Html.RadioBox("SGTourTypeID", recordID.ToString, "", , htmlID) & " <label class=""SGCLabelLong"" for=""" & htmlID & """>" & cs.GetText("name") & ", $" & cs.GetNumber("amount") & "/student, Minimum " & cs.GetInteger("minimumAttendees") & " students (" & cs.GetText("tourLength") & ")</label>")
        '                cs.GoNext()
        '            Loop
        '            '
        '            s = CP.Html.ul(s, , "SGTourList")
        '        End If
        '        cs.Close()
        '        '
        '        Return s
        '    Catch ex As Exception
        '        Try
        '            CP.Site.ErrorReport(ex, "error in Contensive.Addons.Foundation.commonClass.getStudentGroupTourTypes")
        '        Catch errObj As Exception
        '            '
        '        End Try
        '    End Try
        'End Function
        '
        'Public Shared Function getStudentTourAttendeeCount(ByVal CP As BaseClasses.CPBaseClass) As Integer
        '    Try
        '        Dim attendeeCount As Integer = 0
        '        Dim cs As BaseClasses.CPCSBaseClass = CP.CSNew()
        '        Dim studentTourTypeID As Integer = 0
        '        Dim minimumAttendees As Integer = 0
        '        '
        '        If cs.Open("Student Group Tour Requests", "id=" & getStudentGroupTourID(CP), , , "tourAttendeeCountA") Then
        '            attendeeCount = cs.GetInteger("tourAttendeeCountA")
        '        End If
        '        cs.Close()
        '        '
        '        If cs.Open("Student Group Tour Types", "id=" & studentTourTypeID, , , "minimumAttendees") Then
        '            minimumAttendees = cs.GetNumber("minimumAttendees")
        '        End If
        '        cs.Close()
        '        '
        '        If attendeeCount < minimumAttendees Then
        '            attendeeCount = minimumAttendees
        '        End If
        '        '
        '        Return attendeeCount
        '    Catch ex As Exception
        '        Try
        '            CP.Site.ErrorReport(ex, "error in Contensive.Addons.Foundation.commonClass.getStudentTourAttendeeCount")
        '        Catch errObj As Exception
        '            '
        '        End Try
        '    End Try
        'End Function
        '
        'Public Shared Function getStudentTourIndividualAmount(ByVal CP As Contensive.BaseClasses.CPBaseClass, ByVal studentTourTypeID As Integer) As Double
        '    Try
        '        Dim amount As Double = 0
        '        Dim cs As BaseClasses.CPCSBaseClass = CP.CSNew()
        '        '
        '        If cs.Open("Student Group Tour Types", "id=" & studentTourTypeID, , , "amount") Then
        '            amount = cs.GetNumber("amount")
        '        End If
        '        cs.Close()
        '        '
        '        Return amount
        '    Catch ex As Exception
        '        CP.Site.ErrorReport(ex, "error in Contensive.Addons.Foundation.commonClass.getStudentTourIndividualAmount")
        '    End Try
        'End Function
        ''
        'Public Shared Function getGroupAttendeeTypes(ByVal CP As Contensive.BaseClasses.CPBaseClass) As String
        '    Try
        '        Dim s As String = ""
        '        Dim inS As String = ""
        '        Dim cs As BaseClasses.CPCSBaseClass = CP.CSNew()
        '        Dim recordID As Integer = 0
        '        Dim threshold As Integer = 0
        '        Dim priceLow As Double = 0
        '        Dim priceHigh As Double = 0
        '        Dim caption As String = ""
        '        Dim htmlName As String = ""
        '        '
        '        If cs.Open("Group Tour Types", , "sortOrder") Then
        '            Do While cs.OK()
        '                recordID = cs.GetInteger("id")
        '                threshold = cs.GetInteger("priceThreshold")
        '                priceLow = cs.GetNumber("priceLow")
        '                priceHigh = cs.GetNumber("priceHigh")
        '                htmlName = "GTAttendees_" & recordID
        '                '
        '                If (priceLow <> priceHigh) And (threshold <> 0) Then
        '                    caption = "Less than " & threshold & ", " & FormatCurrency(priceLow) & " each. More than " & threshold & ", " & FormatCurrency(priceHigh) & " each."
        '                Else
        '                    caption = FormatCurrency(priceLow) & " each"
        '                End If
        '                '
        '                inS = CP.Html.li("<label for=""" & htmlName & """>" & cs.GetText("name") & "</label>")
        '                inS += CP.Html.li(CP.Html.InputText(htmlName, "", , , , "GTAttendeeTypes", htmlName) & " " & caption)
        '                '
        '                s += inS
        '                cs.GoNext()
        '            Loop
        '            '
        '            s = CP.Html.ul(s, , "GTTourList")
        '        End If
        '        cs.Close()
        '        '
        '        Return s
        '    Catch ex As Exception
        '        Try
        '            CP.Site.ErrorReport(ex, "error in Contensive.Addons.Foundation.commonClass.getGroupAttendeeTypes")
        '        Catch errObj As Exception
        '            '
        '        End Try
        '    End Try
        'End Function
        ''
        'Public Shared Function getGroupTourTotals(ByVal CP As Contensive.BaseClasses.CPBaseClass) As String
        '    Try
        '        Dim s As String = ""
        '        Dim cs As BaseClasses.CPCSBaseClass = CP.CSNew()
        '        Dim groupTourID As Integer = getGroupTourID(CP)
        '        Dim attendeeCount As Integer = 0
        '        Dim attendeeString As String = ""
        '        Dim attendeeCounts As String = ""
        '        Dim attendeeTotals As String = ""
        '        Dim lineAmount As Double = 0
        '        Dim totalAmount As Double = 0
        '        '
        '        If cs.Open("Layouts", "ccGUID='{462CB570-55CF-494C-9D11-FB8DD7E8DA34}'", , , "layout") Then
        '            s = cs.GetText("layout")
        '        End If
        '        cs.Close()
        '        '
        '        If cs.Open("Group Tour Attendee Rules", "groupTourID=" & groupTourID, "attendeeCount") Then
        '            Do While cs.OK()
        '                attendeeCount = cs.GetInteger("attendeeCount")
        '                lineAmount = (getGroupTourTypeAmount(CP, cs.GetInteger("groupTourTypeID"), attendeeCount) * attendeeCount)
        '                totalAmount += lineAmount
        '                '
        '                attendeeString += cs.GetText("groupTourTypeID") & "<br />"
        '                attendeeCounts += attendeeCount & "<br />"
        '                attendeeTotals += FormatCurrency(lineAmount) & "<br />"
        '                cs.GoNext()
        '            Loop
        '        End If
        '        cs.Close()
        '        '
        '        s = s.Replace("{{Guest Category}}", attendeeString)
        '        s = s.Replace("{{Number of Attendees}}", attendeeCounts)
        '        s = s.Replace("{{Amount Due}}", attendeeTotals)
        '        s = s.Replace("{{Total Amount}}", FormatCurrency(totalAmount))
        '        '
        '        Return s
        '    Catch ex As Exception
        '        Try
        '            CP.Site.ErrorReport(ex, "error in Contensive.Addons.Foundation.commonClass.getGroupTourTotals")
        '        Catch errObj As Exception
        '            '
        '        End Try
        '    End Try
        'End Function
        '
        'Public Shared Function getGroupTourTypeAmount(ByVal CP As Contensive.BaseClasses.CPBaseClass, ByVal groupTourTypeID As Integer, ByVal attendeeCount As Integer) As Double
        '    Try
        '        Dim cs As BaseClasses.CPCSBaseClass = CP.CSNew()
        '        Dim threshold As Integer = 0
        '        Dim priceLow As Double = 0
        '        Dim priceHigh As Double = 0
        '        Dim amount As Double = 0
        '        '
        '        If cs.Open("Group Tour Types", "id=" & groupTourTypeID) Then
        '            threshold = cs.GetInteger("priceThreshold")
        '            priceLow = cs.GetNumber("priceLow")
        '            priceHigh = cs.GetNumber("priceHigh")
        '            '
        '            If (priceLow <> priceHigh) And (threshold <> 0) Then
        '                If attendeeCount <= threshold Then
        '                    amount = priceLow
        '                Else
        '                    amount = priceHigh
        '                End If
        '            Else
        '                amount = priceLow
        '            End If
        '        End If
        '        cs.Close()
        '        '
        '        Return amount
        '    Catch ex As Exception
        '        Try
        '            CP.Site.ErrorReport(ex, "error in Contensive.Addons.Foundation.commonClass.getGroupTourTypeAmount")
        '        Catch errObj As Exception
        '            '
        '        End Try
        '    End Try
        'End Function
        '
        'Public Shared Function getGroupTourTotalAmount(ByVal CP As Contensive.BaseClasses.CPBaseClass) As Double
        '    Try
        '        Dim cs As BaseClasses.CPCSBaseClass = CP.CSNew()
        '        Dim groupTourID As Integer = getGroupTourID(CP)
        '        Dim attendeeCount As Integer = 0
        '        Dim lineAmount As Double = 0
        '        Dim totalAmount As Double = 0
        '        '
        '        If cs.Open("Group Tour Attendee Rules", "groupTourID=" & groupTourID, "attendeeCount") Then
        '            Do While cs.OK()
        '                attendeeCount = cs.GetInteger("attendeeCount")
        '                lineAmount = (getGroupTourTypeAmount(CP, cs.GetInteger("groupTourTypeID"), attendeeCount) * attendeeCount)
        '                totalAmount += lineAmount
        '                cs.GoNext()
        '            Loop
        '        End If
        '        cs.Close()
        '        '
        '        Return totalAmount
        '    Catch ex As Exception
        '        Try
        '            CP.Site.ErrorReport(ex, "error in Contensive.Addons.Foundation.commonClass.getGroupTourTotalAmount")
        '        Catch errObj As Exception
        '            '
        '        End Try
        '    End Try
        'End Function
        '
        'Public Shared Function getGroupTourAttendeeString(ByVal CP As Contensive.BaseClasses.CPBaseClass) As String
        '    Try
        '        Dim s As String = ""
        '        Dim cs As BaseClasses.CPCSBaseClass = CP.CSNew()
        '        Dim groupTourID As Integer = getGroupTourID(CP)
        '        '
        '        If cs.Open("Group Tour Attendee Rules", "groupTourID=" & groupTourID, "attendeeCount") Then
        '            Do While cs.OK()
        '                If s <> "" Then
        '                    s += ", "
        '                End If
        '                s += cs.GetText("groupTourTypeID") & " " & cs.GetInteger("attendeeCount")
        '                cs.GoNext()
        '            Loop
        '        End If
        '        cs.Close()
        '        '
        '        Return s
        '    Catch ex As Exception
        '        Try
        '            CP.Site.ErrorReport(ex, "error in Contensive.Addons.Foundation.commonClass.getGroupTourAttendeeString")
        '        Catch errObj As Exception
        '            '
        '        End Try
        '    End Try
        'End Function
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
                        If cs.Open("Add-on Collections", "ccGUID='{A44634C2-D002-428E-818E-1531BF1EBFB0}'", , , "helpLink") Then
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
                        returnHtml += CP.Html.div(CP.Html.div(content, , "ccHintWrapperContent"), , "ccHintWrapper")
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
