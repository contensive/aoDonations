Option Strict On
Option Explicit On

Imports Contensive.BaseClasses
'
Namespace Contensive.Addons.aoDonations
    '
    '====================================================================================================
    ''' <summary>
    ''' Diagnostic
    ''' </summary>
    Public Class DiagnosticClass
        Inherits BaseClasses.AddonBaseClass
        '
        Public Overrides Function Execute(ByVal CP As BaseClasses.CPBaseClass) As Object
            Try
                If (CP.Site.GetInteger(pnDonationVersion, 0) <> donationVersion) Then Return "ERROR, version not updated. Try reinstalling collection"
                Return "OK, Donation Collection"
            Catch ex As Exception
                CP.Site.ErrorReport(ex)
                Return "ERROR, unexepected exception during diagnostic. [" & ex.ToString() & "]"
            End Try
        End Function
    End Class
End Namespace

