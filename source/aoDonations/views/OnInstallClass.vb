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
                End If
                result = My.Resources.DonationForm
            Catch ex As Exception
                CP.Site.ErrorReport(ex)
            End Try
            Return result
        End Function
    End Class
End Namespace

