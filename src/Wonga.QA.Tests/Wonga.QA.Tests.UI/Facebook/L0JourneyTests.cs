using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading;
using Gallio.Framework.Assertions;
using MbUnit.Framework;
using OpenQA.Selenium;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Helpers;
using Wonga.QA.Framework.UI.Mappings;
using Wonga.QA.Framework.UI.UiElements.Pages;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;
using Wonga.QA.Framework.UI.UiElements.Pages.Wb;
using Wonga.QA.Tests.Core;
using Wonga.QA.Framework.UI;

namespace Wonga.QA.Tests.Ui.Facebook
{
    [Parallelizable(TestScope.All)]
    class L0JourneyTests : UiTest
    {
        [Test, AUT(AUT.Uk)]
        public void L0Journey()
        {
            var journey = JourneyFactory.GetL0Journey(Client.Home());
            var mySummary = journey.ApplyForLoan(200, 10)
                .FillPersonalDetails(Get.EnumToString(RiskMask.TESTEmployedMask))
                .FillAddressDetails()
                .FillAccountDetails()
                .FillBankDetails()
                .FillCardDetails()
                .WaitForAcceptedPage()
                .FillAcceptedPage().CurrentPage as DealDonePage;
        }

        [Test, AUT(AUT.Ca, AUT.Za, AUT.Uk), JIRA("QA-181"), Pending("ZA-2512")]
        public void L0JourneyCustomerOnCurrentAddressPageDoesNotEnterSomeRequiredFieldsWarningMessageDisplayed()
        {
            var journey = JourneyFactory.GetL0Journey(Client.Home());
            var addressDetailsPage = journey.ApplyForLoan(200, 10)
                                      .FillPersonalDetails(Get.EnumToString(RiskMask.TESTEmployedMask))
                                      .CurrentPage as AddressDetailsPage;

            switch (Config.AUT)
            {
                #region case Za
                case AUT.Za:
                    addressDetailsPage.HouseNumber = "25";
                    addressDetailsPage.Street = "high road";
                    addressDetailsPage.Town = "Kuku";
                    addressDetailsPage.County = "Province";
                    addressDetailsPage.AddressPeriod = "2 to 3 years";
                    Assert.IsTrue(addressDetailsPage.IsPostcodeWarningOccurred());
                    addressDetailsPage.PostCode = Get.GetPostcode();
                    addressDetailsPage.HouseNumber = "";
                    Assert.IsTrue(addressDetailsPage.IsHouseNumberWarningOccurred());
                    addressDetailsPage.HouseNumber = "25";
                    addressDetailsPage.Street = "";
                    Assert.IsTrue(addressDetailsPage.IsStreetWarningOccurred());
                    addressDetailsPage.Street = "high road";
                    addressDetailsPage.Town = "";
                    Assert.IsTrue(addressDetailsPage.IsTownWarningOccurred());
                    addressDetailsPage.Town = "Kuku";
                    addressDetailsPage.County = "";
                    Assert.IsTrue(addressDetailsPage.IsCountyWarningOccurred());
                    addressDetailsPage.County = "Province";
                    addressDetailsPage.AddressPeriod = "--- Please select ---";
                    Assert.IsTrue(addressDetailsPage.IsAddressPeriodWarningOccurred());
                    break;
                #endregion
                #region case Ca
                case AUT.Ca:
                    addressDetailsPage.Street = "Edward";
                    addressDetailsPage.Town = "Hearst";
                    addressDetailsPage.PostCode = "V4F3A9";
                    addressDetailsPage.AddressPeriod = "2 to 3 years";
                    Assert.IsTrue(addressDetailsPage.IsHouseNumberWarningOccurred());
                    addressDetailsPage.HouseNumber = "1403";
                    addressDetailsPage.Street = "";
                    Assert.IsTrue(addressDetailsPage.IsStreetWarningOccurred());
                    addressDetailsPage.Street = "Edward";
                    addressDetailsPage.Town = "";
                    Assert.IsTrue(addressDetailsPage.IsTownWarningOccurred());
                    addressDetailsPage.Town = "Hearst";
                    addressDetailsPage.PostCode = "";
                    Assert.IsTrue(addressDetailsPage.IsPostcodeWarningOccurred());
                    addressDetailsPage.PostCode = "V4F3A9";
                    addressDetailsPage.AddressPeriod = "--- Please select ---";
                    Assert.IsTrue(addressDetailsPage.IsAddressPeriodWarningOccurred());
                    break;
                #endregion
                #region case Uk
                case AUT.Uk:
                    addressDetailsPage.PostCodeLookup = "SW6 6PN";
                    addressDetailsPage.LookupByPostCode();
                    addressDetailsPage.GetAddressesDropDown();
                    Do.Until(() => addressDetailsPage.SelectedAddress = "93 Harbord Street, LONDON SW6 6PN");
                    Do.Until(() => addressDetailsPage.HouseNumber = "93");
                    Assert.IsTrue(addressDetailsPage.IsAddressPeriodWarningOccurred());
                    addressDetailsPage.AddressPeriod = "3 to 4 years";
                    addressDetailsPage.HouseNumber = "";
                    Assert.IsTrue(addressDetailsPage.IsHouseNumberWarningOccurred());
                    addressDetailsPage.HouseNumber = "93";
                    addressDetailsPage.Street = "";
                    Assert.IsTrue(addressDetailsPage.IsStreetWarningOccurred());
                    addressDetailsPage.Street = "Harbord Street";
                    addressDetailsPage.Town = "";
                    Assert.IsTrue(addressDetailsPage.IsTownWarningOccurred());
                    addressDetailsPage.Town = "LONDON";
                    addressDetailsPage.PostcodeInForm = "";
                    Assert.IsTrue(addressDetailsPage.IsPostcodeWarningOccurred());
                    break;
                #endregion

            }
        }

        [Test, AUT(AUT.Uk), JIRA("UK-969", "UKWEB-250"), MultipleAsserts, Pending("Test is in development. Also waiting for functionality implementation.")]
        public void L0PreAgreementPartonAccountSetupPageTest()
        {
            var loginPage = Client.Login();
            string email = Get.RandomEmail();
            Console.WriteLine("email={0}", email);

            // L0 journey
            var journeyL0 = JourneyFactory.GetL0Journey(Client.Home());
            journeyL0.ApplyForLoan(200, 10)
                .FillPersonalDetailsWithEmail(Get.EnumToString(RiskMask.TESTEmployedMask), email)
                .FillAddressDetails();

            var accountSetupPage = new AccountDetailsPage(this.Client);

            Assert.IsTrue(accountSetupPage.IsSecciLinkVisible());
            Assert.IsTrue(accountSetupPage.IsTermsAndConditionsLinkVisible());
            Assert.IsTrue(accountSetupPage.IsExplanationLinkVisible());

            //string baseWindowHdl = Client.Driver.CurrentWindowHandle;

            //Check SECCI popup window
            accountSetupPage.ClickSecciLink();
            // TBD: check header and values and close the pop-up
            //Assert.Contains(accountSetupPage.SecciPopupWindowContent(), "150");
            // end of TBD: check header and values and close the pop-up
            accountSetupPage.ClosePopupWindow();

            Assert.Contains(accountSetupPage.GetTermsAndConditionsTitle(), "Wonga.com Loan Conditions");
            accountSetupPage.ClosePopupWindow();

            Assert.Contains(accountSetupPage.GetExplanationTitle(), "Important information about your loan");
            accountSetupPage.ClosePopupWindow();

            // Manually check that loan agreement and SECCI emails are sent
            Console.WriteLine("Manually check that that loan agreement and SECCI emails are sent for user={0}", email);
        }

    }
}
