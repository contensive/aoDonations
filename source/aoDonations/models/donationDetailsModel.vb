
Option Strict On
Option Explicit On
'
Namespace Contensive.Addons.aoDonations
    '
    Public Class donationDetailsViewModel

        Public firstName As String
        Public lastName As String
        Public Name As String
        Public Address As String
        Public Address2 As String
        Public City As String
        Public State As String
        Public Zip As String
        Public Phone As String
        Public Email As String
        Public DonationType As Integer
        Public cardName As String
        Public cardNumber As String
        Public cardType As String
        Public cardCVV As String
        Public cardAddress As String
        Public cardZip As String
        Public DFPaymentType As Integer
        Public checkacctName As String
        Public checkacctNumber As String
        Public checkacctroutingNumber As String
        '
        Public Sub New(cp As Contensive.BaseClasses.CPBaseClass)
            firstName = cp.Doc.GetText("DFFirstName")
            lastName = cp.Doc.GetText("DFLastName")
            Name = cp.Doc.GetText("DFFirstName") & " " & cp.Doc.GetText("DFLastName")
            Address = cp.Doc.GetText("DFAddress")
            Address2 = cp.Doc.GetText("DFAddress2")
            City = cp.Doc.GetText("DFCity")
            State = cp.Doc.GetText("State")
            Zip = cp.Doc.GetText("DFZip")
            Phone = cp.Doc.GetText("DFPhone")
            Email = cp.Doc.GetText("DFEmail")
            DonationType = cp.Doc.GetInteger("DFType")
            cardName = cp.Doc.GetText("DFcardName")
            cardNumber = cp.Doc.GetText("DFCardNumber")
            cardType = cp.Doc.GetText("DFcardType")
            'Dim cardExpiration = CP.Doc.GetText("DFCardExpMonth") & "/" & CP.Doc.GetText("DFCardExpYear")
            cardCVV = cp.Doc.GetText("DFCardCVV")
            cardAddress = cp.Doc.GetText("DFCardAddress")
            cardZip = cp.Doc.GetText("DFCardZip")
            DFPaymentType = cp.Doc.GetInteger("DFPaymentType")
            checkacctName = cp.Doc.GetText("DFChkAccount")
            checkacctNumber = cp.Doc.GetText("DFChkAccountNo")
            checkacctroutingNumber = cp.Doc.GetText("DFChkRoutNo")
        End Sub


    End Class
End Namespace