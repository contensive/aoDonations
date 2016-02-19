
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
                returnHtml = CP.Utils.EncodeText(donationHandler.Execute(CP))
                'returnHtml = CP.Utils.ExecuteAddon(donationHandlerAddon)
                '
                returnHtml = CP.Html.div(returnHtml, , , "DFContainer")
                '
            Catch ex As Exception
                Try
                    CP.Site.ErrorReport(ex, "error in Contensive.Addons.Foundation.groupSignupClass.execute")
                Catch errorObj As Exception
                    '
                End Try
            End Try
            Return returnHtml
        End Function
        '
    End Class
    '
    Public Class donationHandlerClass
        Inherits BaseClasses.AddonBaseClass
        '

        Private Const itemGuidOnce1 As String = "{F12533E8-F736-40A7-94E3-BCBF874D11DE}"
        Private Const itemGuidWeekly2 As String = "{D475BE89-1B7A-4AB1-B9E1-C8ED4768AE90}"
        Private Const itemGuidMonthly3 As String = "{B5A437F1-A6EB-4B82-9ED0-089EA230D06F}"
        Private Const itemGuidAnnual4 As String = "{184FC137-64EB-4BD0-865F-97ECDA1B970E}"
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
        Private _csDonationUser As Boolean

        Private Property csDonationUser(p1 As String) As Boolean
            Get
                Return _csDonationUser
            End Get
            Set(value As Boolean)
                _csDonationUser = value
            End Set
        End Property

        '
        '====================================================================================================
        ''' <summary>
        ''' donation form remote method
        ''' </summary>
        ''' <param name="CP"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overrides Function Execute(ByVal CP As BaseClasses.CPBaseClass) As Object
            Dim returnHtml As String = ""
            Try
                Dim cs As BaseClasses.CPCSBaseClass = CP.CSNew()
                Dim processNow As Boolean = CP.Utils.EncodeBoolean(CP.Doc.GetText("DFSubmitted"))
                'Dim processNow As String = CP.Utils.EncodeText(CP.Doc.GetText("DFSubmitted"))
                Dim ecomapi As aoAccountBilling.apiClass = New aoAccountBilling.apiClass()
                'Dim layout As CPBlockBaseClass = CP.BlockNew
                Dim blockLayoutHtml As String = ""
                Dim message As String = ""
                '

                Dim userError As String = ""
                '
                If (Not processNow) Or (errMsg <> "") Then
                    If cs.Open("Layouts", "ccGUID='{5F7A9A40-C01D-4B30-8720-26BF4E81C9AA}'", , , "layout") Then
                        returnHtml = cs.GetText("layout")
                    End If
                    '
                    If errMsg <> "" Then
                        returnHtml = CP.Html.p(errMsg, , "ccError") & returnHtml
                    End If
                    '
                    'returnHtml = returnHtml.Replace("&&&&&theform*****", ecomapi.getOnlinePaymentFields(CP, userError))
                    returnHtml = returnHtml.Replace("{{State Select}}", common.getStateSelect(CP, "DFStateID"))
                    'returnHtml = returnHtml.Replace("{{Expiration Select}}", common.getExpirationSelect(CP, "DFCardExpMonth", "DFCardExpYear", "DFSelectShort"))
                Else
                    returnHtml = processFormReturnResult(CP)

                End If
                '
                returnHtml += common.getHelpWrapper(CP, "")
                CP.Utils.AppendLog("donationHandlerClass.log", "returnHtml:" & returnHtml)
                '
            Catch ex As Exception
                Try
                    CP.Site.ErrorReport(ex, "error in Contensive.Addons.Foundation.groupSignupHandlerClass.execute")
                Catch errorObj As Exception
                    '
                End Try
            End Try
            Return returnHtml
        End Function
        '
        Public Class donationFormThankYouClass
            Public ProcessedOk As Boolean = False
            Public name As String
            Public completedDate As String
            Public address As String
            Public city As String
            Public state As String
            Public zip As String
            Public donationType As Integer
            Public donationAmount As String
            Public donationMethod As String
            Public paymentHolderName As String
            Public CreditCardLastFour As String
            Public creditCardExpDate As String
            Public creditCardType As String
            Public myDFPaymentType As String

        End Class
        '
        Private Function processFormReturnResult(ByVal CP As BaseClasses.CPBaseClass) As String ' was a boolean
            Dim returnResult As String
            Try
                Dim processed As Boolean = False
                Dim cs As BaseClasses.CPCSBaseClass = CP.CSNew()
                Dim csA As BaseClasses.CPCSBaseClass = CP.CSNew()



                Dim firstName As String = CP.Doc.GetText("DFFirstName")
                Dim lastName As String = CP.Doc.GetText("DFLastName")
                Dim Name As String = CP.Doc.GetText("DFFirstName") & " " & CP.Doc.GetText("DFLastName")
                Dim Address As String = CP.Doc.GetText("DFAddress")
                Dim Address2 As String = CP.Doc.GetText("DFAddress2")
                Dim City As String = CP.Doc.GetText("DFCity")
                Dim State As String ' = CP.Doc.GetText("State")
                Dim Zip As String = CP.Doc.GetText("DFZip")
                Dim Phone As String = CP.Doc.GetText("DFPhone")
                Dim Email As String = CP.Doc.GetText("DFEmail")
                Dim DonationType As Integer = CP.Doc.GetInteger("DFType")
                Dim cardName As String = CP.Doc.GetText("DFcardName")
                Dim cardNumber As String = CP.Doc.GetText("DFCardNumber")
                Dim cardType As String = CP.Doc.GetText("DFcardType")
                'Dim cardExpiration As String = CP.Doc.GetText("DFCardExpMonth") & "/" & CP.Doc.GetText("DFCardExpYear")
                Dim cardCVV As String = CP.Doc.GetText("DFCardCVV")
                Dim cardAddress As String = CP.Doc.GetText("DFCardAddress")
                Dim cardZip As String = CP.Doc.GetText("DFCardZip")
                Dim DFPaymentType As Integer = CP.Doc.GetInteger("DFPaymentType")
                Dim checkacctName As String = CP.Doc.GetText("DFChkAccount")
                Dim checkacctNumber As String = CP.Doc.GetText("DFChkAccountNo")
                Dim checkacctroutingNumber As String = CP.Doc.GetText("DFChkRoutNo")
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
                Dim amount As Double = CP.Doc.GetNumber("DFAmount")
                Dim tlpaDonationAmountString As String

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

                Dim locItemID As Integer = 0
                Dim paymentInfo As Contensive.Addons.aoAccountBilling.apiClass.onDemandMethodStructApiVersion = Nothing
                'Dim stateID As Integer  ' = CP.Doc.GetInteger(rnStat)
                Dim rnErr As String = ""
                Dim message As String = ""
                Dim jsonSerializer As New System.Web.Script.Serialization.JavaScriptSerializer
                Dim response As New donationFormThankYouClass
                '
                tlpaDonationAmountString = FormatCurrency(amount)
                response.ProcessedOk = False
                '
                CP.Utils.AppendLog("createAccount.log", "Start Process Form")
                '
                '
                If Not (CP.User.IsAuthenticated()) Then
                    If (CP.User.IsRecognized()) Then
                        CP.User.Logout()
                    End If
                Else
                    'CP.User.Logout()
                End If
                '
                Dim itemGuid As String = ""
                Select Case DonationType
                    Case 1
                        itemGuid = itemGuidOnce1
                    Case 2
                        itemGuid = itemGuidWeekly2
                    Case 3
                        itemGuid = itemGuidMonthly3
                    Case 4
                        itemGuid = itemGuidAnnual4
                    Case Else
                        '
                        ' something is wrong
                        '
                        itemGuid = itemGuidOnce1
                End Select
                If cs.Open("items", "ccguid=" & CP.Db.EncodeSQLText(itemGuid)) Then
                    locItemID = cs.GetInteger("id")
                End If
                Call cs.Close()
                '
                '
                CP.Utils.AppendLog("createAccount.log", "locAccountID: " & locAccountID)
                If locAccountID = 0 Then
                    CP.Utils.AppendLog("createAccount.log", "appName: " & appName)
                    locAccountID = eCom.createAccount(CP, locUserError, CP.User.Id, appName)
                    If locUserError <> "" Then
                        CP.Utils.AppendLog("createAccount.log", "locUserError: " & locUserError)
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
                Dim cardExpiration As String = CP.Doc.GetText("DFcardExp") & "/" & "1" & "/" & CP.Doc.GetText("DFcardYr")
                Dim regDate As Date = Date.Now()
                Dim completedDate As String = regDate.ToString("MMMM" & " " & "dd" & ", " & "yyyy")
                'Dim complatedDate As String = CP.Doc.GetText("DFcardExp") & "/" & "1" & "/" & CP.Doc.GetText("DFcardYr")
                '
                '
                Dim onDemandMethod As New Contensive.Addons.aoAccountBilling.apiClass.onDemandMethodStructApiVersion
                onDemandMethod.useAch = CP.Doc.GetBoolean("DFPaymentType")
                If onDemandMethod.useAch = True Then
                    '
                    ' check
                    '

                    onDemandMethod.achAccountName = checkacctName
                    onDemandMethod.achAccountNumber = checkacctNumber
                    onDemandMethod.achRoutingNumber = checkacctroutingNumber
                Else
                    '
                    ' credit card
                    '
                    onDemandMethod.CreditCardNumber = cardNumber
                    onDemandMethod.CreditCardExpiration = cardExpiration
                    onDemandMethod.CreditCardExpiration = (New Date(CP.Doc.GetInteger("DFcardYr"), CP.Doc.GetInteger("DFcardExp"), 1)).ToShortDateString
                    onDemandMethod.SecurityCode = cardCVV
                End If
                onDemandMethod.FirstName = firstName
                onDemandMethod.LastName = lastName


                If eCom.payOrder(CP, locUserError, locOrderID, onDemandMethod, 0, "Taxicab, Limousine and Paratransit Association Donation") Then
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
                    State = ""
                    If cs.Insert("Donations") Then
                        donationID = cs.GetInteger("id")
                        cs.SetField("name", "Donation made " & Date.Today & " by " & firstName & " " & lastName)
                        cs.SetField("firstName", firstName)
                        cs.SetFormInput("middleName", "DFMiddleName")
                        cs.SetField("lastName", lastName)
                        cs.SetFormInput("address", "DFAddress")
                        cs.SetFormInput("address2", "DFAddress2")
                        cs.SetFormInput("city", "DFCity")
                        State = CP.Content.GetRecordName("States", CP.Doc.GetInteger("DFStateID"))
                        cs.SetField("state", State)
                        cs.SetFormInput("zip", "DFZip")
                        cs.SetFormInput("phone", "DFPhone")
                        cs.SetFormInput("email", Email)
                        cs.SetFormInput("amount", amount)
                        '
                        cs.SetField("processorReference", "Order #" & locOrderID)
                        cs.SetField("processorResponse", "Processed OK")
                        '
                        cs.SetField("memberID", CP.User.Id.ToString)
                        cs.SetField("visitID", CP.Visit.Id.ToString)

                    End If
                    cs.Close()
                    '
                    ' populate thank you page
                    '
                    '
                    response.name = "Full Name:" & " " & firstName & " " & lastName & "</br>" & "Address:" & " " & Address & "</br>" & "City:" & " " & City & "</br>" & "State/Province:" & " " & State & "</br> " & "Zip/Postal Code:" & " " & Zip & "</br> " & "Email:" & " " & Email
                    response.completedDate = completedDate
                    response.donationType = DonationType
                    response.donationAmount = tlpaDonationAmountString
                    response.myDFPaymentType = CStr(DFPaymentType)
                    If onDemandMethod.useAch = True Then
                        '
                        '
                        '                      
                        response.paymentHolderName = "Name on Account:" & " " & checkacctName & "</br>" & "Account Number: " & " " & checkacctNumber & "</br>" & "Bank Routing Number: " & " " & checkacctroutingNumber
                    Else
                        '
                        '
                        Dim newcardNumber = cardNumber.Substring(cardNumber.Length - 4, 4)
                        '
                        response.paymentHolderName = "Cardholder Name:" & " " & cardName & "</br>" & " " & newcardNumber & "</br>" & " " & cardType
                    End If
                    '
                    '
                    '   send notifications
                    '
                    copy += "First Name: " & firstName & br
                    copy += "Last Name: " & lastName & br
                    'copy += "Total Amount: " & amount & br
                    copy += "Amount Paid: " & tlpaDonationAmountString & br
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
                    Dim csDon As BaseClasses.CPCSBaseClass = CP.CSNew()
                    Dim donUserId As Integer = 0
                    'Dim existUser As Boolean = False
                    '
                    If csDon.Open("people", "email = " & CP.Db.EncodeSQLText(Email)) Then
                        '
                        ' he exists
                        '
                        donUserId = csDon.GetInteger("id")
                    Else
                        '
                        ' does not exist
                        '
                        Call csDon.Close()
                        Call csDon.Insert("people")
                        Call csDon.SetField("Name", firstName & " " & lastName)
                        Call csDon.SetField("email", Email)
                        donUserId = csDon.GetInteger("id")
                    End If
                    Call csDon.Close()
                    '
                    CP.Utils.AppendLog("donationConfirmation.log", "returnCopy " & copy)
                    CP.Email.sendSystem("Dontaion Form Auto Responder", copy, donUserId)
                    CP.Email.sendSystem("Donation Form Notification", copy)
                End If
                '
                response.ProcessedOk = processed
                '
                returnResult = jsonSerializer.Serialize(response)
                Return returnResult
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
            Return returnResult
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

        Private Sub text()
            Throw New NotImplementedException
        End Sub

      

       

    End Class
    '
End Namespace

