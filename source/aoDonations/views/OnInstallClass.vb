Option Strict On
Option Explicit On

Imports Contensive.BaseClasses
'
Namespace Contensive.Addons.aoDonations
    '
    '====================================================================================================
    ''' <summary>
    ''' This is the addon dropped on the page
    ''' </summary>
    Public Class OnInstallClass
        Inherits BaseClasses.AddonBaseClass
        '
        '====================================================================================================
        '
        Public Overrides Function Execute(ByVal CP As BaseClasses.CPBaseClass) As Object
            Dim result As String = ""
            Try
                If (CP.Site.GetInteger(pnDonationVersion, 0) <> donationVersion) Then
                    CP.Site.SetProperty(pnDonationVersion, donationVersion)
                    '
                    ' -- check auto responder
                    Using cs As CPCSBaseClass = CP.CSNew()
                        If (Not cs.Open("system email", "(name=" & CP.Db.EncodeSQLText("Donation Form Auto Responder") & ")")) Then
                            '
                            ' -- if no email, create default
                            cs.Close()
                            cs.Insert("system email")
                            cs.SetField("name", "Donation Form Auto Responder")
                            cs.SetField("subject", "Thank you for your donation")
                            cs.SetField("fromaddress", CP.Site.GetText("EMAILFROMADDRESS"))
                            cs.SetField("testmemberid", 0)
                            cs.SetField("copyfilename", "<p>Thank you for your donation.</p>")
                            cs.Save()
                        End If
                    End Using
                    '
                    ' -- check notification
                    Using cs As CPCSBaseClass = CP.CSNew()
                        If (Not cs.Open("system email", "(name=" & CP.Db.EncodeSQLText("Donation Form Auto Responder") & ")")) Then
                            '
                            ' -- if no email, create default
                            cs.Close()
                            cs.Insert("system email")
                            cs.SetField("name", "Donation Form Notification")
                            cs.SetField("subject", "Donation Form Notification")
                            cs.SetField("fromaddress", CP.Site.GetText("EMAILFROMADDRESS"))
                            cs.SetField("testmemberid", 0)
                            cs.SetField("copyfilename", "<p>A new donation has been submitted.</p>")
                            cs.Save()
                        End If
                    End Using
                    '
                    ' -- items
                    verifyItem(CP, itemGuidOnce1, "Dontation, One-Time", 0)
                    verifyItem(CP, itemGuidMonthly2, "Dontation, Monthly", 1)
                    verifyItem(CP, itemGuidQuarterly3, "Dontation, Quarterly", 3)
                    verifyItem(CP, itemGuidAnnual4, "Dontation, Annually", 12)
                    ' 
                End If
                result = My.Resources.DonationForm
            Catch ex As Exception
                CP.Site.ErrorReport(ex)
            End Try
            Return result
        End Function
        '
        '====================================================================================================
        ''' <summary>
        ''' verify the item is created
        ''' </summary>
        ''' <param name="cp"></param>
        ''' <param name="itemGuid"></param>
        ''' <param name="itemName"></param>
        ''' <param name="periodMonths"></param>
        Public Shared Sub verifyItem(cp As CPBaseClass, itemGuid As String, itemName As String, periodMonths As Integer)
            Try
                Using cs As CPCSBaseClass = cp.CSNew()
                    If (Not cs.Open("items", "(ccguid=" & cp.Db.EncodeSQLText(itemGuid) & ")")) Then
                        '
                        ' -- if no record, create default
                        cs.Close()
                        cs.Insert("items")
                        cs.SetField("ccguid", itemGuid)
                        cs.SetField("name", itemName)
                        cs.SetField("isincatalog", False)
                        cs.SetField("inmyaccount", True)
                        If (periodMonths > 0) Then
                            cs.SetField("membershipdurationtypeid", 2) 'periodic
                            cs.SetField("groupexpirationperiodmonths", periodMonths)
                            cs.SetField("isrecurringpurchase", True)
                        End If
                        cs.Save()
                    End If
                End Using
            Catch ex As Exception
                cp.Site.ErrorReport(ex)
            End Try
        End Sub
    End Class
End Namespace

