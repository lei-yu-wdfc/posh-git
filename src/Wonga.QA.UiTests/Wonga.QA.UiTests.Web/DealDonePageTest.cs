﻿using System;
using System.Globalization;
using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Api.Requests.Payments.Queries;
using Wonga.QA.Framework.Api.Requests.Payments.Queries.Za;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.UiTests.Web
{
    [Parallelizable(TestScope.All)]
    class DealDonePageTest : UiTest
    {
        private int _amountMax;
        private int _amountMin;
        private int _termMax;
        private int _termMin;
        private ApiResponse _response;
        private DateTime _actualDate;

        [SetUp, JIRA("QA-192")]
        public void GetInitialValues()
        {
            _amountMin = (int)Drive.Data.Payments.Db.Products.All().FirstOrDefault().AmountMin;
            _amountMax = int.Parse(Drive.Data.Ops.Db.ServiceConfigurations.FindBy(Key: "Payments.DefaultCreditLimit").Value);
            _termMax = (int)Drive.Data.Payments.Db.Products.All().FirstOrDefault().TermMax;
            _termMin = (int)Drive.Data.Payments.Db.Products.All().FirstOrDefault().TermMin;
        }

        [Test, AUT(AUT.Za, AUT.Ca), JIRA("QA-192")]
        public void CorrectDataShouldBeDisplayedOnApplicationSuccessPage()
        {
            string firstName = Get.RandomString(3, 10);
            string lastName = Get.RandomString(3, 10);
            string middleName = Get.RandomString(3, 10);
            int randomAmount = _amountMin + (new Random()).Next(_amountMax - _amountMin);
            int randomDuration = _termMin + (new Random()).Next(_termMax - _termMin);
            var journey = JourneyFactory.GetL0Journey(Client.Home())
                .WithEmployerName(Get.EnumToString(RiskMask.TESTEmployedMask))
                .WithAmount(randomAmount).WithDuration(randomDuration)
                .WithFirstName(firstName).WithLastName(lastName).WithMiddleName(middleName);
            var acceptedPage = journey.Teleport<AcceptedPage>() as AcceptedPage;
            switch (Config.AUT)
            {
                #region case Za
                case AUT.Za:
                    string fullName = firstName + " " + lastName;
                    string fullmidlname = firstName + " " + middleName + " " + lastName;
                    Assert.AreEqual(fullName, acceptedPage.GetNameInLoanAgreement);
                    Assert.AreEqual(fullmidlname, acceptedPage.GetNameInDirectDebit);

                    acceptedPage.SignAgreementConfirm();
                    acceptedPage.SignDirectDebitConfirm();
                    var dealDoneZa = acceptedPage.Submit() as DealDonePage;
                    // Check date
                    DateTime _dateZa = DateTime.Parse(dealDoneZa.GetRepaymentDate());
                    _actualDate = DateTime.Now.AddDays(randomDuration);
                    Assert.AreEqual(String.Format(CultureInfo.InvariantCulture, "{0:d MMM yyyy}", _dateZa), String.Format(CultureInfo.InvariantCulture, "{0:d MMM yyyy}", _actualDate));
                    // Check amount
                    _response = Drive.Api.Queries.Post(new GetFixedTermLoanCalculationZaQuery { LoanAmount = randomAmount, Term = randomDuration });
                    string totalRepayableZa = _response.Values["TotalRepayable"].Single();
                    Assert.AreEqual(dealDoneZa.GetRapaymentAmount().Remove(0, 1), totalRepayableZa);
                    break;
                #endregion
                #region case Ca GetFixedTermLoanCalculationQuery don't work for Ca Wonga.QA.Framework.Api.Exceptions.ValidatorException: Could not process query
                case AUT.Ca:
                    acceptedPage.SignConfirmCaL0(DateTime.Now.ToString("d MMM yyyy"), firstName, lastName);
                    var dealDoneCa = acceptedPage.Submit() as DealDonePage;
                    // Check data
                    DateTime _dateCa = DateTime.Parse(dealDoneCa.GetRepaymentDate());
                    _actualDate = DateTime.Now.AddDays(randomDuration + 1);
                    Assert.AreEqual(String.Format(CultureInfo.InvariantCulture, "{0:d MMM yyyy}", _actualDate), String.Format(CultureInfo.InvariantCulture, "{0:d MMM yyyy}", _dateCa));
                    // Check amount
                    _response = Drive.Api.Queries.Post(new GetFixedTermLoanCalculationQuery { LoanAmount = randomAmount, Term = randomDuration });
                    string totalRepayable = _response.Values["TotalRepayable"].Single();
                    Assert.AreEqual(dealDoneCa.GetRapaymentAmount().Remove(0, 1), totalRepayable);
                    break;
                #endregion
            }

        }


    }
}
