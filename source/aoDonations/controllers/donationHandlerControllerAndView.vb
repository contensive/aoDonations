
Option Strict On
Option Explicit On

'Imports common = Contensive.Addons.donationForm.commonClass
'
Namespace Contensive.Addons.aoDonations

    Public Class DonationHandlerControllerAndView

        '
        ''' <summary>
        ''' processes the donation form from the donationsDetails model. It returns an object with the return results
        ''' </summary>
        ''' <param name="CP"></param>
        ''' <param name="errMsg"></param>
        ''' <param name="donationRequest"></param>
        ''' <returns></returns>
        Public Shared Function processAndReturn(ByVal CP As BaseClasses.CPBaseClass, ByRef errMsg As String, donationRequest As DonationRequestViewModel) As DonationResponseViewModel
            Dim response As New DonationResponseViewModel
            Try
                Dim cs As BaseClasses.CPCSBaseClass = CP.CSNew()
                Dim csA As BaseClasses.CPCSBaseClass = CP.CSNew()
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
                Dim donationID As Integer = 0
                Dim eCom As New aoAccountBilling.apiClass
                Dim acctStatus As New aoAccountBilling.apiClass.AccountStatusStructAPIVersion
                Dim donationAccountID As Integer = 0
                Dim donationPersonId As Integer = 0
                Dim newAccountName As String = ""
                Dim locOrderID As Integer = 0
                Dim locOrderDetailID As Integer
                Dim locItemID As Integer = 0
                Dim paymentInfo As Contensive.Addons.aoAccountBilling.apiClass.OnDemandMethodStructApiVersion = Nothing
                Dim rnErr As String = ""
                Dim message As String = ""
                Dim accountUserId As Integer = 0
                Dim existingEmail As String = ""
                Dim errMessage As String = ""
                '
                'Dim recaptchaChallenge As String = CP.Doc.GetText("recaptcha_challenge_field")
                'Dim recaptchaResponse As String = CP.Doc.GetText("recaptcha_response_field")

                'Dim recaptchaOk As Boolean = CP.Visit.GetBoolean("recaptcha:" & recaptchaChallenge)
                'If (Not recaptchaOk) Then

                'End If
                Dim reCaptchaResponse As String = ""
                If (CP.Site.GetBoolean("Donations Add Recaptcha", False)) Then
                    CP.Doc.SetProperty("Challenge", CP.Doc.GetText("recaptcha_challenge_field"))
                    CP.Doc.SetProperty("Response", CP.Doc.GetText("recaptcha_response_field"))
                    reCaptchaResponse = CStr(CP.Addon.Execute(reCaptchaProcessGuid))
                End If
                If (Not String.IsNullOrEmpty(reCaptchaResponse)) Then
                    '
                    ' -- recaptcha error
                    response.errorMessage = "The Recaptcha text did not match. Please try again."
                    response.ProcessedOk = False
                Else
                    '
                    ' -- recapcha ok
                    '
                    If True Then
                        Dim donationAmount As Double = CP.Utils.EncodeNumber(donationRequest.donationAmount)
                        If (donationAmount = 0) Then
                            donationAmount = CP.Utils.EncodeNumber(donationRequest.donationAmount)
                        End If
                        Dim donationAmountString As String = FormatCurrency(donationAmount)
                        response.ProcessedOk = True
                        With donationRequest
                            If (donationAmount <= 0) Then
                                response.errorMessage = donationErrorAmount
                                response.ProcessedOk = False
                            ElseIf (.DFFirstName = "") Then
                                response.errorMessage = donationErrorFirstName
                                response.ProcessedOk = False
                            ElseIf (.DFLastName = "") Then
                                response.errorMessage = donationErrorLastname
                                response.ProcessedOk = False
                            ElseIf (.DFPhone = "") Then

                                response.errorMessage = donationErrorPhone
                                response.ProcessedOk = False
                            ElseIf (.DFAddress = "") Then
                                response.errorMessage = donationErrorAddress
                                response.ProcessedOk = False
                            ElseIf (.DFEmail = "") Then
                                response.errorMessage = donationErrorEmail
                                response.ProcessedOk = False

                            ElseIf (.DFZip = "") Then
                                response.errorMessage = donationErrorZip
                                response.ProcessedOk = False
                                'ElseIf Not verifyUserAndAccount(CP, donationDetails, donationAccountID, donationPersonId, errMessage) Then
                            ElseIf Not verifyUserAndAccount(CP, donationRequest, donationAccountID, donationPersonId, errMessage) Then
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
                                Select Case CP.Utils.EncodeInteger(.DFType)
                                    Case 1
                                        itemGuid = itemGuidOnce1
                                    Case 2
                                        itemGuid = itemGuidMonthly2
                                    Case 3
                                        itemGuid = itemGuidQuarterly3
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
                                    '
                                    ' -- create new account
                                    newAccountName = .DFFirstName & " " & .DFLastName
                                    CP.Utils.AppendLog("createAccount.log", "newAccountName: " & newAccountName)
                                    donationAccountID = eCom.createAccount(CP, response.errorMessage, CP.User.Id, newAccountName)
                                    response.ProcessedOk = String.IsNullOrEmpty(response.errorMessage)
                                    If response.ProcessedOk Then
                                        '
                                        ' -- set current use as primary and billing
                                        eCom.setAccountPrimaryContact(CP, response.errorMessage, donationAccountID, CP.User.Id)
                                        response.ProcessedOk = String.IsNullOrEmpty(response.errorMessage)
                                        '
                                        eCom.setAccountBillingContact(CP, response.errorMessage, donationAccountID, CP.User.Id)
                                        response.ProcessedOk = String.IsNullOrEmpty(response.errorMessage)
                                    End If
                                End If
                                '
                                '
                                If response.ProcessedOk Then
                                    locOrderID = eCom.createOrder(CP, response.errorMessage)
                                    response.ProcessedOk = String.IsNullOrEmpty(response.errorMessage)
                                    '
                                    eCom.setOrderAccount(CP, response.errorMessage, locOrderID, donationAccountID)
                                    response.ProcessedOk = String.IsNullOrEmpty(response.errorMessage)
                                End If
                                '
                                '
                                If response.ProcessedOk Then
                                    locOrderDetailID = eCom.addOrderItem(CP, response.errorMessage, locOrderID, locItemID)
                                    response.ProcessedOk = String.IsNullOrEmpty(response.errorMessage)
                                    If (response.ProcessedOk) Then
                                        Call CP.Db.ExecuteSQL("update orderdetails set unitPriceOverride=" & CP.Db.EncodeSQLNumber(donationAmount) & " where id=" & locOrderDetailID)
                                    End If
                                End If
                                '
                                '   payment for order if no errors
                                '
                                Dim onDemandMethod As New Contensive.Addons.aoAccountBilling.apiClass.OnDemandMethodStructApiVersion
                                onDemandMethod.useAch = False
                                onDemandMethod.CreditCardNumber = .DFcardNo
                                onDemandMethod.CreditCardExpiration = (New Date(CP.Doc.GetInteger("DFcardYr"), CP.Doc.GetInteger("DFcardExp"), 1)).ToShortDateString
                                onDemandMethod.SecurityCode = .DFcardCVV
                                onDemandMethod.FirstName = .DFFirstName
                                onDemandMethod.LastName = .DFLastName
                                response.ProcessedOk = eCom.payOrder(CP, response.errorMessage, locOrderID, onDemandMethod, 0, "Taxicab, Limousine and Paratransit Association Donation")
                                If response.ProcessedOk Then
                                    '
                                    '   login user
                                    '
                                    CP.User.LoginByID(CP.User.Id.ToString)
                                    ''
                                    'eCom.setAccountPrimaryContact(CP, response.errorMessage, donationAccountID, CP.User.Id)
                                    'response.ProcessedOk = String.IsNullOrEmpty(response.errorMessage)
                                    ''
                                    'eCom.setAccountBillingContact(CP, response.errorMessage, donationAccountID, CP.User.Id)
                                    'response.ProcessedOk = String.IsNullOrEmpty(response.errorMessage)
                                End If
                                '
                                If response.ProcessedOk Then
                                    '
                                    '   insert Donation
                                    '
                                    If cs.Insert("Donations") Then
                                        donationID = cs.GetInteger("id")
                                        cs.SetField("name", "Donation made " & Date.Today & " by " & .DFFirstName & " " & .DFLastName)
                                        cs.SetField("firstName", .DFFirstName)
                                        cs.SetField("middleName", .DFMiddleName)
                                        cs.SetField("lastName", .DFLastName)
                                        cs.SetField("address", .DFAddress)
                                        cs.SetField("city", .DFCity)
                                        cs.SetField("state", .DFState)
                                        cs.SetField("zip", .DFZip)
                                        cs.SetField("phone", .DFPhone)
                                        cs.SetField("email", .DFEmail)
                                        cs.SetField("amount", donationAmount.ToString())
                                        cs.SetField("processorReference", "Order #" & locOrderID)
                                        cs.SetField("processorResponse", "Processed OK")
                                        cs.SetField("memberID", CP.User.Id.ToString)
                                        cs.SetField("visitID", CP.Visit.Id.ToString)
                                        cs.SetField("accountid", donationAccountID.ToString())
                                    End If
                                    cs.Close()
                                    '
                                    ' populate thank you page
                                    '
                                    Dim regDate As Date = Date.Now()
                                    Dim completedDate As String = regDate.ToString("MMMM" & " " & "dd" & ", " & "yyyy")
                                    response.name = .DFFirstName & " " & .DFLastName & "</br>" & "Address:" & " " & .DFAddress & "</br>" & "City:" & " " & .DFCity & "</br>" & "State/Province:" & " " & .DFState & "</br> " & "Zip/Postal Code:" & " " & .DFZip & "</br> " & "Email:" & " " & .DFEmail
                                    response.completedDate = completedDate
                                    response.donationType = CP.Utils.EncodeInteger(.DFType)
                                    response.donationAmount = donationAmountString
                                    response.myDFPaymentType = CStr(.DFPaymentType)
                                    Dim newcardNumber As String = .DFcardNo
                                    If (.DFcardNo.Length > 4) Then
                                        newcardNumber = newcardNumber.Substring(newcardNumber.Length - 4, 4)
                                    End If
                                    response.paymentHolderName = "Cardholder Name:" & " " & .DFcardName & "</br>" & " " & newcardNumber & "</br>" & " " & .DFcardType
                                    '
                                    '   send notifications
                                    '
                                    copy += "First Name: " & .DFFirstName & br
                                    copy += "Last Name: " & .DFLastName & br
                                    copy += "Amount Paid: " & donationAmountString & br
                                    copy += "Payment Date: " & Date.Today & br
                                    copy += "Account Number: " & Right(.DFcardNo, 4) & br
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
                                    If csDon.Open("people", "email = " & CP.Db.EncodeSQLText(.DFEmail)) Then
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
                                        Call csDon.SetField("Name", .DFFirstName & " " & .DFLastName)
                                        Call csDon.SetField("email", .DFEmail)
                                        donUserId = csDon.GetInteger("id")
                                    End If
                                    Call csDon.Close()
                                    '
                                    CP.Email.sendSystem("Dontaion Form Auto Responder", copy, donUserId)
                                    CP.Email.sendSystem("Donation Form Notification", copy)
                                End If
                            End If
                        End With
                    End If
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
        Private Shared Function verifyUserAndAccount(ByVal CP As BaseClasses.CPBaseClass, donationDetails As DonationRequestViewModel, ByRef returnDonationAccountID As Integer, ByRef returnDonationPersonId As Integer, ByRef returnErrMessage As String) As Boolean
            Dim result As Boolean = True
            '
            Try
                Dim cs As BaseClasses.CPCSBaseClass = CP.CSNew()
                Dim clearFlag As Boolean = False
                Dim donationUserName As String = ""
                Dim eCommerce As New aoAccountBilling.apiClass
                Dim newAccountMessage As String = "Donations created account. "
                Dim ecommerceErrorMessage As String = ""
                Dim Name As String = CP.Doc.GetText("DFFirstName") & " " & CP.Doc.GetText("DFLastName")
                '
                If Not (CP.User.IsAuthenticated()) Then
                    If (CP.User.IsRecognized()) Then
                        CP.User.Logout()
                    End If
                    '
                    ' create new account for this user
                    '
                    returnDonationAccountID = 0
                    ' 20181226 - allow duplicate email accounts
                    If cs.Insert("people") Then
                        returnDonationPersonId = cs.GetInteger("id")
                        cs.SetField("Name", donationDetails.DFName)
                        cs.SetField("lastName", donationDetails.DFLastName)
                        cs.SetField("firstName", donationDetails.DFFirstName)
                        cs.SetField("email", donationDetails.DFEmail)
                        cs.SetField("BillName", donationDetails.DFName)

                    End If
                    cs.Close()
                    CP.User.LoginByID(returnDonationPersonId.ToString)
                    'If cs.Open("people", "email=" & CP.Db.EncodeSQLText(donationDetails.DFEmail)) Then
                    '    ' this email is in user
                    '    '
                    '    returnErrMessage = "Please login or select a different email address"
                    '    cs.Close()
                    '    Return False
                    'Else
                    '    '
                    '    ' ok to create a new donation user
                    '    '
                    '    cs.Close()


                    '    '
                    '    If cs.Insert("people") Then
                    '        returnDonationPersonId = cs.GetInteger("id")
                    '        cs.SetField("Name", donationDetails.DFName)
                    '        cs.SetField("lastName", donationDetails.DFLastName)
                    '        cs.SetField("firstName", donationDetails.DFFirstName)
                    '        cs.SetField("email", donationDetails.DFEmail)
                    '        cs.SetField("BillName", donationDetails.DFName)

                    '    End If
                    '    cs.Close()
                    '    CP.User.LoginByID(returnDonationPersonId.ToString)

                    'End If
                Else
                    '
                    ' authenticated - return this user's accountid
                    '
                    returnDonationPersonId = CP.User.Id
                    If cs.Open("People", "id=" & returnDonationPersonId, "", True, "accountID") Then
                        returnDonationAccountID = cs.GetInteger("accountID")
                    End If
                    cs.Close()
                    ' returnDonationAccountID = cs.GetInteger("accountID")
                    '
                    '   verify account record exists
                    '
                    If Not cs.Open("Accounts", "id=" & returnDonationAccountID, "", True, "id") Then
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
                                Call cs.SetField("BillName", donationDetails.DFName)
                            End If
                            Call cs.Close()
                            ' CP.Db.ExecuteSQL("update abaccounts set memberId=" & returnDonationPersonId & ",billingmemberId=" & returnDonationPersonId & " where id=" & returnDonationAccountID)
                            eCommerce.setAccountBillingContact(CP, ecommerceErrorMessage, returnDonationAccountID, CP.User.Id)
                            eCommerce.setAccountPrimaryContact(CP, ecommerceErrorMessage, returnDonationAccountID, CP.User.Id)
                        End If
                    End If
                Else
                    '
                    ' -- Account provided, verify it is ok
                    Dim accountStatus As aoAccountBilling.apiClass.AccountStatusStructAPIVersion = eCommerce.getAccountStatus(CP, returnErrMessage, returnDonationAccountID)
                    If (String.IsNullOrEmpty(returnErrMessage)) Then
                        If (Not accountStatus.exists) Then
                            returnDonationAccountID = 0
                        ElseIf (accountStatus.closed) Then
                            returnErrMessage = "Your account is closed."
                            result = False
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
