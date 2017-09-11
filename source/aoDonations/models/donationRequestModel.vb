
Option Strict On
Option Explicit On
'
Namespace Contensive.Addons.aoDonations
    '
    Public Class donationRequestModel
        Public DFFirstName As String
        Public DFMiddleName As String
        Public DFLastName As String
        Public DFName As String
        Public DFAddress As String
        Public DFCity As String
        Public DFState As String
        Public DFZip As String
        Public DFPhone As String
        Public DFEmail As String
        Public DFType As String
        Public donateAmountOther As String
        Public donationAmount As String
        Public DFcardName As String
        Public DFcardNo As String
        Public DFcardType As String
        Public DFcardCVV As String
        Public DFPaymentType As String
        Public DFcardExp As String
        Public DFcardYr As String
        '
        Public Sub New(cp As Contensive.BaseClasses.CPBaseClass)
            DFFirstName = cp.Doc.GetText("DFFirstName")
            DFLastName = cp.Doc.GetText("DFLastName")
            DFName = cp.Doc.GetText("DFFirstName") & " " & cp.Doc.GetText("DFLastName")
            DFAddress = cp.Doc.GetText("DFAddress")
            DFCity = cp.Doc.GetText("DFCity")
            DFState = cp.Doc.GetText("DFState")
            DFZip = cp.Doc.GetText("DFZip")
            DFPhone = cp.Doc.GetText("DFPhone")
            DFEmail = cp.Doc.GetText("DFEmail")
            DFType = cp.Doc.GetText("DFType")
            DFcardName = cp.Doc.GetText("DFcardName")
            DFcardNo = cp.Doc.GetText("DFcardNo")
            DFcardType = cp.Doc.GetText("DFcardType")
            DFcardExp = cp.Doc.GetText("DFcardExp") & "/" & cp.Doc.GetText("DFcardYr")
            DFcardCVV = cp.Doc.GetText("DFCardCVV")
            DFPaymentType = cp.Doc.GetText("DFPaymentType")
            donationAmount = cp.Doc.GetText("donateAmt")
            donateAmountOther = cp.Doc.GetText("donateAmountOther")
            '
            cp.Utils.AppendLog("debug.log", "donationREquestModel, DFFirstName=[" & DFFirstName & "]")
            cp.Utils.AppendLog("debug.log", "donationREquestModel, donationAmount=[" & donationAmount & "],donateAmountOther=[" & donateAmountOther & "]")
        End Sub
        '


    End Class
End Namespace