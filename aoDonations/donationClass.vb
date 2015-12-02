
Option Strict On
Option Explicit On

Imports System
Imports System.Collections.Generic
Imports System.Text
Imports Contensive.BaseClasses
Imports Contensive
Imports common = Contensive.Addons.donationForm.commonClass
Imports Contensive.Addons.aoAccountBilling
'
Namespace Contensive.Addons.aoDonations
    '
    Public Class donationClass
        Inherits BaseClasses.AddonBaseClass
        '
        Public Overrides Function Execute(ByVal CP As BaseClasses.CPBaseClass) As Object
            Dim s As String = ""
            Try
                s = CP.Utils.ExecuteAddon("{63CE6314-F25D-459E-8A7B-14E6C44DB7FD}")
                '
                s = CP.Html.div(s, , , "DFContainer")
                '
            Catch ex As Exception
                Try
                    CP.Site.ErrorReport(ex, "error in Contensive.Addons.Foundation.groupSignupClass.execute")
                Catch errorObj As Exception
                    '
                End Try
            End Try
            Return s
        End Function
        '
    End Class
    '
    Public Class donationHandlerClass
        Inherits BaseClasses.AddonBaseClass
        '
        Private hiddenString As String = "" '   accumulates all hiddens from throughout process
        Private errMsg As String = ""
        Dim rnCardNumber As String
        Dim rnExpMonth As String
        Dim rnExpYear As String
        Dim rnCardAmount As String
        Dim rnCardCode As String
        Dim rnFName As String
        Dim rnLName As String
        Dim rnStat As String
        Dim contactID As String
        Dim Address As Object
        '
        Public Overrides Function Execute(ByVal CP As BaseClasses.CPBaseClass) As Object
            Try
                Dim s As String = ""
                Dim cs As BaseClasses.CPCSBaseClass = CP.CSNew()
                Dim processNow As Boolean = CP.Utils.EncodeBoolean(CP.Doc.Var("DFSubmitted"))
                Dim ecomapi As aoAccountBilling.apiClass = New aoAccountBilling.apiClass()

                Dim userError As String = ""
                '
                If processNow Then
                    If processForm(CP) Then
                        s += CP.Content.GetCopy("Donation Form Thank You", "Thank you for your donation. Your request has been received and will be processed shortly.")
                    End If
                End If
                '
                If (Not processNow) Or (errMsg <> "") Then
                    If cs.Open("Layouts", "ccGUID='{60E11B77-4DAE-4798-ABC4-1628A74F1E36}'", , , "layout") Then
                        s = cs.GetText("layout")
                    End If
                    '
                    If errMsg <> "" Then
                        s = CP.Html.p(errMsg, , "ccError") & s
                    End If
                    '
                    s = s.Replace("&&&&&theform*****", ecomapi.getOnlinePaymentFields(CP, userError))
                    s = s.Replace("{{State Select}}", common.getStateSelect(CP, "DFStateID"))
                    s = s.Replace("{{Expiration Select}}", common.getExpirationSelect(CP, "DFCardExpMonth", "DFCardExpYear", "DFSelectShort"))
                End If
                '
                s += common.getHelpWrapper(CP, "")
                '
                Return s
            Catch ex As Exception
                Try
                    CP.Site.ErrorReport(ex, "error in Contensive.Addons.Foundation.groupSignupHandlerClass.execute")
                Catch errorObj As Exception
                    '
                End Try
            End Try
        End Function
        '
        Private Function processForm(ByVal CP As BaseClasses.CPBaseClass) As Boolean
            Try
                Dim processed As Boolean = False
                Dim cs As BaseClasses.CPCSBaseClass = CP.CSNew()
                Dim csA As BaseClasses.CPCSBaseClass = CP.CSNew()
                Dim firstName As String = CP.Doc.Var("DFFirstName")
                Dim lastName As String = CP.Doc.Var("DFLastName")
                Dim Name As String = CP.Doc.Var("DFFirstName") & " " & CP.Doc.Var("DFLastName")
                Dim Address As String = CP.Doc.Var("DFAddress")
                Dim Address2 As String = CP.Doc.Var("DFAddress2")
                Dim City As String = CP.Doc.Var("DFCity")
                Dim State As String = CP.Doc.Var("State")
                Dim Zip As String = CP.Doc.Var("DFZip")
                Dim Phone As String = CP.Doc.Var("DFPhone")
                Dim Email As String = CP.Doc.Var("DFEmail")
                Dim cardNumber As String = CP.Doc.Var("DFCardNumber")
                Dim cardExpiration As String = CP.Doc.Var("DFCardExpMonth") & "/" & CP.Doc.Var("DFCardExpYear")
                Dim cardCVV As String = CP.Doc.Var("DFCardCVV")
                Dim cardAddress As String = CP.Doc.Var("DFCardAddress")
                Dim cardZip As String = CP.Doc.Var("DFCardZip")
                Dim rS As String = ""   '   response stream
                Dim resultDoc As New System.Xml.XmlDocument
                Dim ppResponse As String = ""
                Dim ppApproved As Boolean = False
                Dim ppReference As String = ""
                Dim ppCSCMatch As Boolean = False
                Dim ppAVSMatch As Boolean = False
                Dim br As String = "<br />"
                Dim copy As String = br & br
                Dim copyPlus As String = ""
                Dim recordLink As String = ""
                Dim amount As Double = CP.Utils.EncodeNumber(CP.Doc.Var("DFAmount"))
                Dim donationID As Integer = 0
                Dim eCom As New aoAccountBilling.apiClass
                Dim acctStatus As New aoAccountBilling.apiClass.accountStatusStructAPIVersion
                Dim locUserError As String = ""
                'Dim aMan As New aoMembershipManager.apiClass
                Dim locAccountID As Integer = getExistingAccountID(CP)
                Dim errFlag As Boolean
                Dim appName As String = ""
                Dim locOrderID As Integer = 0
                Dim locOrderDetailID As Integer
                Dim locItemGuid As String = "{F12533E8-F736-40A7-94E3-BCBF874D11DE}"
                Dim locItemID As Integer = 0
                Dim paymentInfo As Contensive.Addons.aoAccountBilling.apiClass.onDemandMethodStructApiVersion = Nothing
                Dim stateID As Integer = CP.Doc.GetInteger(rnStat)
                Dim rnErr As String = ""

                '
                If Not (CP.User.IsAuthenticated()) Then
                    If (CP.User.IsRecognized()) Then
                        CP.User.Logout()
                    End If
                Else
                    'CP.User.Logout()
                End If

                If cs.Open("people", "id=" & CP.User.Id) Then

                    Call cs.SetField("firstName", firstName)
                    Call cs.SetField("lastName", lastName)
                    Call cs.SetField("Name", Name)
                    Call cs.SetField("Address", Address)
                    Call cs.SetField("Address2", Address2)
                    Call cs.SetField("City", City)
                    Call cs.SetField("State", State)
                    Call cs.SetField("Zip", Zip)
                    Call cs.SetField("Phone", Phone)
                    Call cs.SetField("Email", Email)

                    'cs.SetField("name", "Donation made " & Date.Today & " by " & firstName & " " & lastName)
                    'cs.SetField("firstName", firstName)
                    'cs.SetFormInput("middleName", "DFMiddleName")
                    'cs.SetField("lastName", lastName)
                    'cs.SetFormInput("address", "DFAddress")
                    'cs.SetFormInput("address2", "DFAddress2")
                    'cs.SetFormInput("city", "DFCity")
                    'cs.SetField("state", CP.Content.GetRecordName("States", CP.Utils.EncodeInteger(CP.Doc.Var("DFStateID"))))
                    'cs.SetFormInput("zip", "DFZip")
                    'cs.SetFormInput("phone", "DFPhone")
                    'cs.SetFormInput("email", "DFEmail")

                End If
                cs.Close()



                '
                If cs.Open("items", "ccguid=" & CP.Db.EncodeSQLText(locItemGuid)) Then
                    locItemID = cs.GetInteger("id")
                End If
                Call cs.Close()

                '


                '
                '   verify payment
                '
                'CP.Doc.Var("CreditCardNumber") = cardNumber
                'CP.Doc.Var("CreditCardExpiration") = cardExpiration
                'CP.Doc.Var("SecurityCode") = cardCVV
                'CP.Doc.Var("AVSAddress") = cardAddress
                'CP.Doc.Var("AVSZip") = cardZip
                'CP.Doc.Var("PaymentAmount") = amount
                '
                '

                '
                '
                If locAccountID = 0 Then
                    locAccountID = eCom.createAccount(CP, locUserError, CP.User.Id, appName)
                    If locUserError <> "" Then
                        errFlag = True
                        errMsg += locUserError & br
                        locUserError = ""
                    End If
                End If
                '
                '
                If Not errFlag Then
                    locOrderID = eCom.createOrder(CP, locUserError)
                    If locUserError <> "" Then
                        errFlag = True
                        errMsg += locUserError & br
                        locUserError = ""
                    End If
                    '
                    '
                    eCom.setOrderAccount(CP, locUserError, locOrderID, locAccountID)
                    If locUserError <> "" Then
                        errFlag = True
                        errMsg += locUserError & br
                        locUserError = ""
                    End If
                End If
                '
                '
                If Not errFlag Then
                    locOrderDetailID = eCom.addOrderItem(CP, locUserError, locOrderID, locItemID)
                    If locUserError <> "" Then
                        errFlag = True
                        errMsg += locUserError & br
                        locUserError = ""
                    End If
                    Call CP.Db.ExecuteSQL("update orderdetails set unitPriceOverride=" & CP.Db.EncodeSQLNumber(amount) & " where id=" & locOrderDetailID)
                End If
                '
                '
                '   payment for order if no errors
                '
                If eCom.payOrdersByOnlinePaymentFields(CP, locUserError, locOrderID.ToString, "Online Donation Form", "") Then
                    '
                    ' the payment ran OK
                    '
                    processed = True
                    '
                    '   login user
                    '
                    CP.User.LoginByID(CP.User.Id.ToString)
                    '
                    eCom.setAccountPrimaryContact(CP, locUserError, locAccountID, CP.User.Id)
                    If locUserError <> "" Then
                        errFlag = True
                        errMsg += locUserError & br
                        locUserError = ""
                    End If
                    '
                    eCom.setAccountBillingContact(CP, locUserError, locAccountID, CP.User.Id)
                    If locUserError <> "" Then
                        errFlag = True
                        errMsg += locUserError & br
                        locUserError = ""
                    End If

                End If
                '
                If processed Then
                    '
                    '   insert Donation
                    '
                    If cs.Insert("Donations") Then
                        donationID = cs.GetInteger("id")
                        cs.SetField("name", "Donation made " & Date.Today & " by " & firstName & " " & lastName)
                        cs.SetField("firstName", firstName)
                        cs.SetFormInput("middleName", "DFMiddleName")
                        cs.SetField("lastName", lastName)
                        cs.SetFormInput("address", "DFAddress")
                        cs.SetFormInput("address2", "DFAddress2")
                        cs.SetFormInput("city", "DFCity")
                        cs.SetField("state", CP.Content.GetRecordName("States", CP.Utils.EncodeInteger(CP.Doc.Var("DFStateID"))))
                        cs.SetFormInput("zip", "DFZip")
                        cs.SetFormInput("phone", "DFPhone")
                        cs.SetFormInput("email", "DFEmail")
                        '
                        cs.SetFormInput("amount", "DFAmount")
                        'cs.SetFormInput("cardName", "DFCardName")
                        'cs.SetField("cardNumber", cardNumber)
                        'cs.SetField("cardExpiration", cardExpiration)
                        'cs.SetField("cardBillingAddress", cardAddress)
                        'cs.SetField("cardBillingZip", cardZip)
                        'cs.SetField("cardCode", cardCVV)
                        '
                        cs.SetField("processorReference", "Order #" & locOrderID)
                        cs.SetField("processorResponse", "Processed OK")
                        '
                        cs.SetField("memberID", CP.User.Id.ToString)
                        cs.SetField("visitID", CP.Visit.Id.ToString)
                    End If
                    cs.Close()
                    '
                    '   send notifications
                    '
                    copy += "First Name: " & firstName & br
                    copy += "Last Name: " & lastName & br
                    copy += "Total Amount: " & amount & br
                    copy += "Amount Paid: " & amount & br
                    copy += "Payment Date: " & Date.Today & br
                    copy += "Account Number: " & Right(cardNumber, 4) & br
                    '
                    '   add link to email
                    '
                    recordLink = CP.Site.DomainPrimary & "/admin/index.php?af=4&cid=" & CP.Content.GetRecordID("Content", "Donations")
                    recordLink += "&id=" & donationID
                    '
                    copyPlus += copy
                    copyPlus += CP.Html.p("<a target=""_blank"" href=""http://" & recordLink & """>Click here for details</a>")
                    '
                    CP.Email.sendSystem("Dontaion Form Auto Responder", copy, CP.User.Id)
                    CP.Email.sendSystem("Donation Form Notification", copyPlus)
                End If
                '
                Return processed
            Catch ex As Exception
                Try
                    CP.Site.ErrorReport(ex, "error in Contensive.Addons.Foundation.groupSignupHandlerClass.processFormPayment")
                Catch errorObj As Exception
                    '
                    '   if something goes wrong - set user error message
                    '
                    errMsg += "There was a problem processing your payment. Please try again.<br />"
                End Try
            End Try
        End Function
        '
        '
        '
        Private Function getExistingAccountID(ByVal CP As BaseClasses.CPBaseClass) As Integer
            Dim recordID As Integer = 0
            '
            Try
                Dim cs As BaseClasses.CPCSBaseClass = CP.CSNew()
                Dim clearFlag As Boolean = False

                If Not (CP.User.IsAuthenticated()) Then
                    If (CP.User.IsRecognized()) Then
                        CP.User.Logout()
                    End If
                Else
                    '
                    If cs.Open("People", "id=" & CP.User.Id, , , "accountID") Then
                        recordID = cs.GetInteger("accountID")
                    End If
                    cs.Close()
                    '
                    '   verify account record exists
                    '
                    If Not cs.Open("Accounts", "id=" & recordID, , , "id") Then
                        recordID = 0
                    End If
                    cs.Close()
                End If
            Catch ex As Exception
                Try
                    CP.Site.ErrorReport(ex, "error in Contensive.Addons.fma.profileClass.getExistingAccountID")
                Catch errObj As Exception
                End Try
            End Try
            '
            Return recordID
        End Function
        '

        Private Function getRegionID(CP As BaseClasses.CPBaseClass, p2 As Object) As Object
            Throw New NotImplementedException
        End Function

    End Class
    '
End Namespace

