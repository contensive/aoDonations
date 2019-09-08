
Option Strict On
Option Explicit On

Namespace Contensive.Addons.aoDonations

    Public Module Constants
        '
        Public Const pnDonationVersion As String = "Donation Addon Version"
        Public Const donationVersion As Integer = 2
        '
        Public Const itemGuidOnce1 As String = "{F12533E8-F736-40A7-94E3-BCBF874D11DE}"
        Public Const itemGuidMonthly2 As String = "{D475BE89-1B7A-4AB1-B9E1-C8ED4768AE90}"
        Public Const itemGuidQuarterly3 As String = "{B5A437F1-A6EB-4B82-9ED0-089EA230D06F}"
        Public Const itemGuidAnnual4 As String = "{184FC137-64EB-4BD0-865F-97ECDA1B970E}"
        Public Const reCaptchaDisplayGuid = "{E9E51C6E-9152-4284-A44F-D3ABC423AB90}"
        Public Const reCaptchaProcessGuid = "{030AC5B0-F796-4EA4-B94C-986B1C29C16C}"
        '
        Public Const donationErrorFirstName As String = "In order to continue, please complete the required First Name field."
        Public Const donationErrorLastname As String = "In order to continue, please complete the required Last Name field."
        Public Const donationErrorPhone As String = "In order to continue, please complete the required Phone field."
        Public Const donationErrorAddress As String = "In order to continue, please complete the required Address field."
        Public Const donationErrorEmail As String = "In order to continue, please complete the required Email field."
        Public Const donationErrorZip As String = "In order to continue, please complete the required Zip field."
        Public Const donationErrorAmount As String = "In order to continue, please enter a donation amount."
    End Module
End Namespace
