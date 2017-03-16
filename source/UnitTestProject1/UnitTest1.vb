
Option Strict On
Option Explicit On

Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports Contensive.BaseClasses

Namespace Contensive.Addons.aoDonations

    <TestClass()> Public Class UnitTest1
        Public testAppName As String = "aoDonations"

        ''' <summary>
        ''' test:  test the donation view model
        ''' </summary>
        <TestMethod()> Public Sub donationViewModelTEst()
            Dim cp As New Contensive.Processor.CPClass
            cp.Context.appName = testAppName
            cp.getDoc(True)
            '
            ' assign
            cp.Doc.SetProperty("DFAddress", "1")
            cp.Doc.SetProperty("DFAddress2", "2")
            cp.Doc.SetProperty("DFFirstName", "3")
            cp.Doc.SetProperty("DFLastName", "4")
            cp.Doc.SetProperty("DFCity", "5")
            cp.Doc.SetProperty("State", "6")
            cp.Doc.SetProperty("DFZip", "7")
            cp.Doc.SetProperty("DFPhone", "8")
            cp.Doc.SetProperty("DFEmail", "9")
            cp.Doc.SetProperty("DFType", "10")
            cp.Doc.SetProperty("DFcardName", "11")
            cp.Doc.SetProperty("DFCardNumber", "12")
            cp.Doc.SetProperty("DFcardType", "13")
            cp.Doc.SetProperty("DFCardCVV", "14")
            cp.Doc.SetProperty("DFCardAddress", "15")
            cp.Doc.SetProperty("DFCardZip", "16")
            cp.Doc.SetProperty("DFChkAccount", "17")
            cp.Doc.SetProperty("DFChkAccountNo", "18")
            cp.Doc.SetProperty("DFChkRoutNo", "19")
            cp.Doc.SetProperty("DFPaymentType", "20")
            '
            ' act
            Dim donationDetails As New donationDetailsViewModel(cp)
            ''
            ''assert
            Assert.AreEqual("1", donationDetails.Address)
            Assert.AreEqual("2", donationDetails.Address2)
            Assert.AreEqual("3", donationDetails.firstName)
            Assert.AreEqual("4", donationDetails.lastName)
            Assert.AreEqual("5", donationDetails.City)
            Assert.AreEqual("6", donationDetails.State)
            Assert.AreEqual("7", donationDetails.Zip)
            Assert.AreEqual("8", donationDetails.Phone)
            Assert.AreEqual("9", donationDetails.Email)
            Assert.AreEqual(10, donationDetails.DonationType)
            Assert.AreEqual("11", donationDetails.cardName)
            Assert.AreEqual("12", donationDetails.cardNumber)
            Assert.AreEqual("13", donationDetails.cardType)
            Assert.AreEqual("14", donationDetails.cardCVV)
            Assert.AreEqual("15", donationDetails.cardAddress)
            Assert.AreEqual("16", donationDetails.cardZip)
            Assert.AreEqual("17", donationDetails.checkacctName)
            Assert.AreEqual("18", donationDetails.checkacctNumber)
            Assert.AreEqual("19", donationDetails.checkacctroutingNumber)
            Assert.AreEqual(20, donationDetails.DFPaymentType)

            cp.Dispose()
        End Sub
        ''' <summary>
        ''' test donation form valid case
        ''' </summary>
        <TestMethod()> Public Sub donationSubmitValidationTest()
            Dim cp As New Contensive.Processor.CPClass
            cp.Context.appName = testAppName
            cp.getDoc(True)
            Dim errorMessage As String = ""
            '
            ' assign
            '
            Dim donationDetails As donationDetailsViewModel = getValidDonationDetails(cp)
            '
            ' act
            Dim result As donationFormRequestModel = donationHandlerControllerAndView.processAndReturn(cp, errorMessage, donationDetails)
            '
            ' assert
            Assert.AreEqual(True, result.ProcessedOk)
            '
            cp.Dispose()
        End Sub
        ''' <summary>
        ''' test all donationform invalid cases
        ''' </summary>
        <TestMethod()> Public Sub donationSubmitInValidationTest()
            Dim cp As New Contensive.Processor.CPClass
            cp.Context.appName = testAppName
            cp.getDoc(True)
            Dim errorMessage As String = ""
            Dim donationDetails As donationDetailsViewModel
            Dim result As donationFormRequestModel
            '
            ' assign
            '
            donationDetails = getValidDonationDetails(cp)
            donationDetails.firstName = ""
            '
            ' act
            result = donationHandlerControllerAndView.processAndReturn(cp, errorMessage, donationDetails)
            '
            ' assert
            Assert.AreEqual(False, result.ProcessedOk)
            Assert.AreEqual(constantsModule.donationErrorFirstName, result.errorMessage)
            '
            ' assign
            '
            donationDetails = getValidDonationDetails(cp)
            donationDetails.lastName = ""
            '
            ' act
            result = donationHandlerControllerAndView.processAndReturn(cp, errorMessage, donationDetails)
            '
            ' assert
            Assert.AreEqual(False, result.ProcessedOk)
            Assert.AreEqual(constantsModule.donationErrorLastname, result.errorMessage)
            '
            ' assign
            '
            donationDetails = getValidDonationDetails(cp)
            donationDetails.Phone = ""
            '
            ' act
            result = donationHandlerControllerAndView.processAndReturn(cp, errorMessage, donationDetails)
            '
            ' assert
            Assert.AreEqual(False, result.ProcessedOk)
            Assert.AreEqual(constantsModule.donationErrorPhone, result.errorMessage)
            '
            ' assign
            '
            donationDetails = getValidDonationDetails(cp)
            donationDetails.Address = ""
            '
            ' act
            result = donationHandlerControllerAndView.processAndReturn(cp, errorMessage, donationDetails)
            '
            ' assert
            Assert.AreEqual(False, result.ProcessedOk)
            Assert.AreEqual(constantsModule.donationErrorAddress, result.errorMessage)
            '
            ' assign
            '
            donationDetails = getValidDonationDetails(cp)
            donationDetails.Email = ""
            '
            ' act
            result = donationHandlerControllerAndView.processAndReturn(cp, errorMessage, donationDetails)
            '
            ' assert
            Assert.AreEqual(False, result.ProcessedOk)
            Assert.AreEqual(constantsModule.donationErrorEmail, result.errorMessage)
            '
            ' assign
            '
            donationDetails = getValidDonationDetails(cp)
            donationDetails.Zip = ""
            '
            ' act
            result = donationHandlerControllerAndView.processAndReturn(cp, errorMessage, donationDetails)
            '
            ' assert
            Assert.AreEqual(False, result.ProcessedOk)
            Assert.AreEqual(constantsModule.donationErrorZip, result.errorMessage)
            '
            cp.Dispose()
        End Sub
        ''' <summary>
        ''' test the basic application -- use a unit test template
        ''' </summary>
        <TestMethod()> Public Sub helloWorldTest()
            Dim cp As New Contensive.Processor.CPClass
            cp.Context.appName = testAppName
            cp.getDoc(True)
            '
            Assert.AreEqual(testAppName, cp.Site.Name)
            '
            cp.Dispose()
            '
        End Sub
        ''' <summary>
        ''' create a donationDetails model that is valid
        ''' </summary>
        ''' <param name="cp"></param>
        ''' <returns></returns>
        Private Function getValidDonationDetails(cp As CPBaseClass) As donationDetailsViewModel
            Dim donationDetails As New donationDetailsViewModel(cp)
            donationDetails.firstName = "test"
            donationDetails.lastName = "tester"
            donationDetails.Name = donationDetails.firstName & " " & donationDetails.lastName
            donationDetails.Address = "here st"
            donationDetails.Address2 = "apt1"
            donationDetails.City = "ny"
            donationDetails.State = "va"
            donationDetails.Zip = "1234"
            donationDetails.Email = cp.Utils.GetRandomInteger.ToString
            donationDetails.Phone = "5678"
            donationDetails.cardZip = "123456"
            donationDetails.cardAddress = "here"
            donationDetails.cardType = "credit"
            donationDetails.cardName = "VISA"
            donationDetails.cardNumber = "1234"
            donationDetails.cardCVV = "123"
            Return donationDetails
        End Function

    End Class
End Namespace