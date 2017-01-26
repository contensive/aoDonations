
Option Strict On
Option Explicit On

Imports common = Contensive.Addons.donationForm.commonClass
'
Namespace Contensive.Addons.aoDonations

    Public Class donationHandlerControllerAndView

        Private Const itemGuidOnce1 As String = "{F12533E8-F736-40A7-94E3-BCBF874D11DE}"
        Private Const itemGuidWeekly2 As String = "{D475BE89-1B7A-4AB1-B9E1-C8ED4768AE90}"
        Private Const itemGuidMonthly3 As String = "{B5A437F1-A6EB-4B82-9ED0-089EA230D06F}"
        Private Const itemGuidAnnual4 As String = "{184FC137-64EB-4BD0-865F-97ECDA1B970E}"
        '
        ''' <summary>
        ''' processes the donation form from the donationsDetails model. It returns an object with the return results
        ''' </summary>
        ''' <param name="CP"></param>
        ''' <param name="errMsg"></param>
        ''' <param name="donationDetails"></param>
        ''' <returns></returns>
        Public Shared Function processAndReturn(ByVal CP As BaseClasses.CPBaseClass, ByRef errMsg As String, donationDetails As donationDetailsViewModel) As donationFormRequestModel
            Dim response As New donationFormRequestModel
            Try
                ' Dim processed As Boolean = False
                Dim cs As BaseClasses.CPCSBaseClass = CP.CSNew()
                Dim csA As BaseClasses.CPCSBaseClass = CP.CSNew()
                'donationDetails = New donationDetailsViewModel(CP)
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
                Dim donationAccountID As Integer = 0
                Dim donationPersonId As Integer = 0
                'Dim errFlag As Boolean
                Dim newAccountName As String = ""
                Dim locOrderID As Integer = 0
                Dim locOrderDetailID As Integer
                Dim locItemID As Integer = 0
                Dim paymentInfo As Contensive.Addons.aoAccountBilling.apiClass.onDemandMethodStructApiVersion = Nothing
                Dim rnErr As String = ""
                Dim message As String = ""
                Dim accountUserId As Integer = 0
                Dim existingEmail As String = ""
                Dim errMessage As String = ""
                '
                If True Then
                    tlpaDonationAmountString = FormatCurrency(amount)
                    response.ProcessedOk = True
                    '
                    CP.Utils.AppendLog("createAccount.log", "Start Process Form")
                    '
                    With donationDetails
                        '
                        If (.firstName = "") Then
                            response.errorMessage = donationErrorFirstName
                            response.ProcessedOk = False
                        ElseIf (.lastName = "") Then
                            response.errorMessage = donationErrorLastname
                            response.ProcessedOk = False
                        ElseIf (.Phone = "") Then

                            response.errorMessage = donationErrorPhone
                            response.ProcessedOk = False
                        ElseIf (.Address = "") Then
                            response.errorMessage = donationErrorAddress
                            response.ProcessedOk = False
                        ElseIf (.Email = "") Then
                            response.errorMessage = donationErrorEmail
                            response.ProcessedOk = False

                        ElseIf (.Zip = "") Then
                            response.errorMessage = donationErrorZip
                            response.ProcessedOk = False
                            'ElseIf Not verifyUserAndAccount(CP, donationDetails, donationAccountID, donationPersonId, errMessage) Then
                        ElseIf Not verifyUserAndAccount(CP, donationDetails, donationAccountID, donationPersonId, errMessage) Then
                            '
                            ' problem verifying the user or account
                            '
                            response.ProcessedOk = False
                            response.errorMessage = errMessage
                        Else
                            '
                            ' if authenticated, accountUserid = cp.user.id
                            '
                            Dim itemGuid As String = ""
                            Select Case .DonationType
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
                            If locItemID = 0 Then
                                If cs.Insert("items") Then
                                    cs.SetField("name", "Donation")
                                    cs.SetField("ccguid", "{F12533E8-F736-40A7-94E3-BCBF874D11DE}")
                                End If
                                locItemID = cs.GetInteger("id")
                                Call cs.Close()
                            End If
                            '
                            '
                            CP.Utils.AppendLog("createAccount.log", "locAccountID: " & donationAccountID)
                            If donationAccountID = 0 Then
                                newAccountName = .firstName & " " & .lastName
                                CP.Utils.AppendLog("createAccount.log", "newAccountName: " & newAccountName)
                                donationAccountID = eCom.createAccount(CP, locUserError, CP.User.Id, newAccountName)
                                If locUserError <> "" Then
                                    CP.Utils.AppendLog("createAccount.log", "locUserError: " & locUserError)
                                    response.ProcessedOk = False
                                    response.errorMessage = locUserError & br
                                    locUserError = ""
                                End If
                            End If
                            '
                            '
                            If response.ProcessedOk Then
                                locOrderID = eCom.createOrder(CP, locUserError)
                                If locUserError <> "" Then
                                    response.ProcessedOk = False
                                    response.errorMessage = locUserError & br
                                    locUserError = ""
                                End If
                                '
                                '
                                eCom.setOrderAccount(CP, locUserError, locOrderID, donationAccountID)
                                If locUserError <> "" Then
                                    response.ProcessedOk = False
                                    response.errorMessage = locUserError & br
                                    locUserError = ""
                                End If
                            End If
                            '
                            '
                            If response.ProcessedOk Then
                                locOrderDetailID = eCom.addOrderItem(CP, locUserError, locOrderID, locItemID)
                                If locUserError <> "" Then
                                    response.ProcessedOk = False
                                    response.errorMessage = locUserError & br
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

                                onDemandMethod.achAccountName = .checkacctName
                                onDemandMethod.achAccountNumber = .checkacctNumber
                                onDemandMethod.achRoutingNumber = .checkacctroutingNumber
                            Else
                                '
                                ' credit card
                                '
                                onDemandMethod.CreditCardNumber = .cardNumber
                                onDemandMethod.CreditCardExpiration = cardExpiration
                                onDemandMethod.CreditCardExpiration = (New Date(CP.Doc.GetInteger("DFcardYr"), CP.Doc.GetInteger("DFcardExp"), 1)).ToShortDateString
                                onDemandMethod.SecurityCode = .cardCVV
                            End If
                            onDemandMethod.FirstName = .firstName
                            onDemandMethod.LastName = .lastName

                            response.ProcessedOk = eCom.payOrder(CP, locUserError, locOrderID, onDemandMethod, 0, "Taxicab, Limousine and Paratransit Association Donation")
                            If response.ProcessedOk Then
                                '
                                '   login user
                                '
                                CP.User.LoginByID(CP.User.Id.ToString)
                                '
                                eCom.setAccountPrimaryContact(CP, locUserError, donationAccountID, CP.User.Id)
                                If locUserError <> "" Then
                                    response.ProcessedOk = False
                                    response.errorMessage = locUserError & br
                                    locUserError = ""
                                End If
                                '
                                eCom.setAccountBillingContact(CP, locUserError, donationAccountID, CP.User.Id)
                                If locUserError <> "" Then
                                    response.ProcessedOk = False
                                    response.errorMessage = locUserError & br
                                End If
                            End If
                            '
                            If response.ProcessedOk Then
                                '
                                '   insert Donation
                                '
                                .State = ""
                                If cs.Insert("Donations") Then
                                    donationID = cs.GetInteger("id")
                                    cs.SetField("name", "Donation made " & Date.Today & " by " & .firstName & " " & .lastName)
                                    cs.SetField("firstName", .firstName)
                                    cs.SetFormInput("middleName", "DFMiddleName")
                                    cs.SetField("lastName", .lastName)
                                    cs.SetFormInput("address", "DFAddress")
                                    cs.SetFormInput("address2", "DFAddress2")
                                    cs.SetFormInput("city", "DFCity")
                                    .State = CP.Content.GetRecordName("States", CP.Doc.GetInteger("DFStateID"))
                                    cs.SetField("state", .State)
                                    cs.SetFormInput("zip", "DFZip")
                                    cs.SetFormInput("phone", "DFPhone")
                                    cs.SetFormInput("email", .Email)
                                    cs.SetField("amount", amount.ToString)
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
                                response.name = "Full Name:" & " " & .firstName & " " & .lastName & "</br>" & "Address:" & " " & .Address & "</br>" & "City:" & " " & .City & "</br>" & "State/Province:" & " " & .State & "</br> " & "Zip/Postal Code:" & " " & .Zip & "</br> " & "Email:" & " " & .Email
                                response.completedDate = completedDate
                                response.donationType = .DonationType
                                response.donationAmount = tlpaDonationAmountString
                                response.myDFPaymentType = CStr(.DFPaymentType)
                                If onDemandMethod.useAch = True Then
                                    '
                                    '
                                    '                      
                                    response.paymentHolderName = "Name on Account:" & " " & .checkacctName & "</br>" & "Account Number: " & " " & .checkacctNumber & "</br>" & "Bank Routing Number: " & " " & .checkacctroutingNumber
                                Else
                                    '
                                    '
                                    Dim newcardNumber = .cardNumber.Substring(.cardNumber.Length - 4, 4)
                                    '
                                    response.paymentHolderName = "Cardholder Name:" & " " & .cardName & "</br>" & " " & newcardNumber & "</br>" & " " & .cardType
                                End If
                                '
                                '
                                '   send notifications
                                '
                                copy += "First Name: " & .firstName & br
                                copy += "Last Name: " & .lastName & br
                                'copy += "Total Amount: " & amount & br
                                copy += "Amount Paid: " & tlpaDonationAmountString & br
                                copy += "Payment Date: " & Date.Today & br
                                copy += "Account Number: " & Right(.cardNumber, 4) & br
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
                                If csDon.Open("people", "email = " & CP.Db.EncodeSQLText(.Email)) Then
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
                                    Call csDon.SetField("Name", .firstName & " " & .lastName)
                                    Call csDon.SetField("email", .Email)
                                    donUserId = csDon.GetInteger("id")
                                End If
                                Call csDon.Close()
                                '
                                CP.Utils.AppendLog("donationConfirmation.log", "returnCopy " & copy)
                                CP.Email.sendSystem("Dontaion Form Auto Responder", copy, donUserId)
                                CP.Email.sendSystem("Donation Form Notification", copy)
                            End If
                            '
                        End If
                    End With
                End If
            Catch ex As Exception
                CP.Site.ErrorReport(ex, "processFormPayment")
            End Try
            Return response
        End Function
        '
        '====================================================================================================
        ''' <summary>
        ''' verify the user is authenticated or update his account from the donation model. Return the user's accountId. If cannot create the account, return false and the errMessage is a user error.
        ''' </summary>
        ''' <param name="CP"></param>
        ''' <returns></returns>
        Private Shared Function verifyUserAndAccount(ByVal CP As BaseClasses.CPBaseClass, donationDetails As donationDetailsViewModel, ByRef returnDonationAccountID As Integer, ByRef returnDonationPersonId As Integer, ByRef returnErrMessage As String) As Boolean
            Dim result As Boolean = True
            '
            Try
                Dim cs As BaseClasses.CPCSBaseClass = CP.CSNew()
                Dim clearFlag As Boolean = False
                Dim donationUserName As String = ""
                'Dim donationAccountID As Integer = 0
                Dim mustSetAccountName As Boolean
                Dim eCommerce As New aoAccountBilling.apiClass
                Dim newAccountMessage As String = "Donations created account. "
                'im DonationUserId As Integer
                Dim ecommerceErrorMessage As String = ""
                Dim Name As String = CP.Doc.GetText("DFFirstName") & " " & CP.Doc.GetText("DFLastName")

                If Not (CP.User.IsAuthenticated()) Then
                    If (CP.User.IsRecognized()) Then
                        CP.User.Logout()
                    End If
                    '
                    ' create new account for this user
                    '
                    returnDonationAccountID = 0
                    If cs.Open("people", "email=" & CP.Db.EncodeSQLText(donationDetails.Email)) Then
                        '
                        ' this email is in user
                        '
                        returnErrMessage = "Please login or select a different email address"
                        cs.Close()
                        Return False
                    Else
                        '
                        ' ok to create a new donation user
                        '
                        cs.Close()
                        If cs.Insert("people") Then
                            returnDonationPersonId = cs.GetInteger("id")
                            cs.SetField("lastName", donationDetails.lastName)
                            cs.SetField("firstName", donationDetails.firstName)
                            cs.SetField("email", donationDetails.Email)
                        End If
                        cs.Close()
                    End If
                Else
                    '
                    ' authenticated - return this user's accountid
                    '
                    returnDonationPersonId = CP.User.Id
                    If cs.Open("People", "id=" & returnDonationPersonId, , , "accountID") Then
                        returnDonationAccountID = cs.GetInteger("accountID")
                    End If
                    cs.Close()
                    '
                    '   verify account record exists
                    '
                    If Not cs.Open("Accounts", "id=" & returnDonationAccountID, , , "id") Then
                        returnDonationAccountID = 0
                    End If
                    cs.Close()
                End If
                '
                ' verify the donationUser has a donationAccount
                '
                If (returnDonationAccountID = 0) Then
                    returnDonationAccountID = eCommerce.createAccount(CP, ecommerceErrorMessage, returnDonationPersonId)
                    If Not String.IsNullOrEmpty(ecommerceErrorMessage) Then
                        '
                        ' there was an ecommerce error
                        '
                        returnErrMessage = ecommerceErrorMessage
                        Return False
                    Else
                        eCommerce.addAccountNote(CP, ecommerceErrorMessage, returnDonationAccountID, newAccountMessage, newAccountMessage, False)
                        If Not String.IsNullOrEmpty(ecommerceErrorMessage) Then
                            '
                            ' there was an ecommerce error
                            '
                            returnErrMessage = ecommerceErrorMessage
                            Return False
                        Else
                            If cs.Open("people", "id=" & returnDonationPersonId) Then
                                Call cs.SetField("accountId", returnDonationAccountID.ToString)
                            End If
                            Call cs.Close()
                        End If
                    End If
                End If
            Catch ex As Exception
                Try
                    CP.Site.ErrorReport(ex, "error in Contensive.Addons.fma.profileClass.getExistingAccountID")
                Catch errObj As Exception
                End Try
            End Try
            Return result
        End Function
        '

        Private Function getRegionID(CP As BaseClasses.CPBaseClass, p2 As Object) As Object
            Throw New NotImplementedException
        End Function

        Private Sub text()
            Throw New NotImplementedException
        End Sub








    End Class

End Namespace
