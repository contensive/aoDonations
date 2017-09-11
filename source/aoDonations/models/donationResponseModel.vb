
Option Strict On
Option Explicit On
'
Namespace Contensive.Addons.aoDonations
    '
    '
    Public Class donationResponseModel
        '
        ' if processedOk is false, the errorMessage is a user message to be displayed on the UI
        '
        Public ProcessedOk As Boolean = False
        Public errorMessage As String
        '
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
        '
        ' Public errorList As New List(Of String)
        '
    End Class
End Namespace
