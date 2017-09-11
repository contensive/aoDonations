
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
            Dim donationDetails As New donationRequestModel(cp)
            ''
            ''assert
            Assert.AreEqual("1", donationDetails.DFAddress)
            'Assert.AreEqual("2", donationDetails.Address2)
            Assert.AreEqual("3", donationDetails.DFFirstName)
            Assert.AreEqual("4", donationDetails.DFLastName)
            Assert.AreEqual("5", donationDetails.DFCity)
            Assert.AreEqual("6", donationDetails.DFState)
            Assert.AreEqual("7", donationDetails.DFZip)
            Assert.AreEqual("8", donationDetails.DFPhone)
            Assert.AreEqual("9", donationDetails.DFEmail)
            Assert.AreEqual(10, donationDetails.DFType)
            Assert.AreEqual("11", donationDetails.DFcardName)
            Assert.AreEqual("12", donationDetails.DFcardNo)
            Assert.AreEqual("13", donationDetails.DFcardType)
            Assert.AreEqual("14", donationDetails.DFcardCVV)
            'Assert.AreEqual("15", donationDetails.cardAddress)
            'Assert.AreEqual("16", donationDetails.cardZip)
            'Assert.AreEqual("17", donationDetails.checkacctName)
            'Assert.AreEqual("18", donationDetails.checkacctNumber)
            'Assert.AreEqual("19", donationDetails.checkacctroutingNumber)
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
            Dim donationDetails As donationRequestModel = getValidDonationDetails(cp)
            '
            ' act
            Dim result As donationResponseModel = donationHandlerControllerAndView.processAndReturn(cp, errorMessage, donationDetails)
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
            Dim donationDetails As donationRequestModel
            Dim result As donationResponseModel
            '
            ' assign
            '
            donationDetails = getValidDonationDetails(cp)
            donationDetails.DFFirstName = ""
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
            donationDetails.DFLastName = ""
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
            donationDetails.DFPhone = ""
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
            donationDetails.DFAddress = ""
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
            donationDetails.DFEmail = ""
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
            donationDetails.DFZip = ""
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
        Private Function getValidDonationDetails(cp As CPBaseClass) As donationRequestModel
            Dim donationDetails As New donationRequestModel(cp)
            donationDetails.DFFirstName = "test"
            donationDetails.DFLastName = "tester"
            donationDetails.DFName = donationDetails.DFFirstName & " " & donationDetails.DFLastName
            donationDetails.DFAddress = "here st"
            'donationDetails.Address2 = "apt1"
            donationDetails.DFCity = "ny"
            donationDetails.DFState = "va"
            donationDetails.DFZip = "1234"
            donationDetails.DFEmail = cp.Utils.GetRandomInteger.ToString
            donationDetails.DFPhone = "5678"
            'donationDetails.cardZip = "123456"
            'donationDetails.cardAddress = "here"
            donationDetails.DFcardType = "credit"
            donationDetails.DFcardName = "VISA"
            donationDetails.DFcardNo = "1234"
            donationDetails.DFcardCVV = "123"
            Return donationDetails
        End Function

    End Class
End Namespace