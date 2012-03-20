﻿using System.Globalization;
using System.Threading;
using Gallio.Framework.Assertions;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;
using System.Linq;
using System;
using Wonga.QA.Framework.Db;

namespace Wonga.QA.Tests.Ui
{
    /// <summary>
    /// Slider tests for Za
    /// </summary>
    /// 
    class SliderTests : UiTest
    {
        private int _amountMax;
        private int _amountMin;
        private int _termMax;
        private int _termMin;
        private string _repaymentDate;
        private ApiResponse _response;
        private DateTime _actualDate;


        [SetUp, JIRA("QA-149")]
        public void GetInitialValues()
        {
            ApiRequest request;
            switch (Config.AUT)
            {
                case AUT.Uk:
                    request = new GetFixedTermLoanOfferUkQuery();
                    break;
                case AUT.Za:
                    request = new GetFixedTermLoanOfferZaQuery();
                    break;
                case AUT.Ca:
                    request = new GetFixedTermLoanOfferCaQuery();
                    break;
                case AUT.Wb:
                    request = new GetBusinessFixedInstallmentLoanOfferWbUkQuery();
                    break;
                default:
                    throw new NotImplementedException();
            }


            _response = Driver.Api.Queries.Post(request);
            _amountMax = (int)Decimal.Parse(_response.Values["AmountMax"].Single(), CultureInfo.InvariantCulture);
            _amountMin = (int)Double.Parse(_response.Values["AmountMin"].Single(), CultureInfo.InvariantCulture);
            _termMax = Int32.Parse(_response.Values["TermMax"].Single(), CultureInfo.InvariantCulture);
            _termMin = Int32.Parse(_response.Values["TermMin"].Single(), CultureInfo.InvariantCulture);
        }

        [Test, AUT(AUT.Za), JIRA("QA-149")]
        public void MovingSlidersRepaymentDateShouldBeCorrect()
        {
            var page = Client.Home();
            int randomDuration = _termMin + (new Random()).Next(_termMax - _termMin);
            page.Sliders.HowLong = randomDuration.ToString(CultureInfo.InvariantCulture);

            string[] dateArray = page.Sliders.GetRepaymentDate.Split(' ');
            string day = Char.IsDigit(dateArray[1].ElementAt(1)) ? dateArray[1].Remove(2, 2) : dateArray[1].Remove(1, 2);
            _repaymentDate = day + " " + dateArray[2] + " " + dateArray[3];

            _actualDate = DateTime.Now.AddDays(randomDuration);
            Assert.AreEqual(_repaymentDate, String.Format(CultureInfo.InvariantCulture, "{0:d MMM yyyy}", _actualDate));
        }

        [Test, AUT(AUT.Za), JIRA("QA-149")]
        public void MovingSlidersLoanSummaryShouldBeCorrect()
        {
            var page = Client.Home();
            int randomAmount = _amountMin + (new Random()).Next(_amountMax - _amountMin);
            int randomDuration = _termMin + (new Random()).Next(_termMax - _termMin);

            page.Sliders.HowMuch = randomAmount.ToString(CultureInfo.InvariantCulture);
            page.Sliders.HowLong = randomDuration.ToString(CultureInfo.InvariantCulture);

            _response = Driver.Api.Queries.Post(new GetFixedTermLoanCalculationZaQuery { LoanAmount = randomAmount, Term = randomDuration });

            string totalRepayable = _response.Values["TotalRepayable"].Single();
            Assert.AreEqual(page.Sliders.GetTotalToRepay.Remove(0, 1), totalRepayable);
        }

        [Test, AUT(AUT.Za), JIRA("QA-149")]
        public void MovingSlidersBeyondMaxIsNotAllowedByFrontEnd()
        {
            var page = Client.Home();
            int amountBiggerThanMax = _amountMax + 1000;
            page.Sliders.HowMuch = amountBiggerThanMax.ToString(CultureInfo.InvariantCulture);
            Assert.AreEqual(_amountMax.ToString(CultureInfo.InvariantCulture), page.Sliders.GetTotalAmount.Remove(0, 1));
        }

        [Test, AUT(AUT.Ca)]
        public void VariableInterestisCalculatedCorrectly()
        {
            var homePage = Client.Home();
            homePage.Sliders.HowMuch = "100";
            homePage.Sliders.HowLong = "30";
            Console.WriteLine("Sliders: {0} for {1}", homePage.Sliders.HowLong, homePage.Sliders.HowLong);
            Thread.Sleep(500);
            //0.5 sec pause

            Assert.AreEqual(homePage.Sliders.GetTotalToRepay, "$121.00");
            //maximum charge is 21$ for each 100$ borrowed for 30 days.
        }

        [Test, AUT(AUT.Ca, AUT.Za, AUT.Wb), JIRA("QA-156", "QA-238")]
        public void DefaultAmountSliderValueShouldBeCorrectL0()
        {
            var page = Client.Home();
            switch (Config.AUT)
            {
                case AUT.Za:
                    Assert.AreEqual(page.Sliders.HowMuch, "1335");
                    break;
                case AUT.Ca:
                    Assert.AreEqual(page.Sliders.HowMuch, "265");
                    break;
                case AUT.Wb:
                    Assert.AreEqual(page.Sliders.HowMuch, "9,000");
                    break;
            }

        }

        [Test, AUT(AUT.Ca, AUT.Za), JIRA("QA-156", "QA-238")]
        public void DefaultAmountSliderValueShouldBeCorrectLn()
        {
            var loginPage = Client.Login();
            string email = Data.RandomEmail();
            Customer customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            Application application = ApplicationBuilder.New(customer)
                .Build();
            application.RepayOnDueDate();
            loginPage.LoginAs(email);

            var page = Client.Home();
            switch (Config.AUT)
            {
                case AUT.Za:
                    Assert.AreEqual(page.Sliders.HowMuch, "1335");
                    break;
                case AUT.Ca:
                    Assert.AreEqual(page.Sliders.HowMuch, "265");
                    break;
            }
        }

        [Test, AUT(AUT.Ca), JIRA("QA-241", "QA-159")]
        public void DefaultDurationSliderValueShouldBeCorrectL0()
        {
            var page = Client.Home();
            switch (Config.AUT)
            {
                case AUT.Za:
                    string[] dateArray = page.Sliders.GetRepaymentDate.Split(' ');
                    string day = Char.IsDigit(dateArray[1].ElementAt(1)) ? dateArray[1].Remove(2, 2) : dateArray[1].Remove(1, 2);
                    _repaymentDate = day + " " + dateArray[2] + " " + dateArray[3];

                    var today = DateTime.Today;
                    var nextPayDate = today.Day <= 25
                                           ? new DateTime(today.Year, today.Month, 25)
                                           : new DateTime(today.Year, today.Month, 25).AddMonths(1);
                    var expectedDate = Driver.Db.GetPreviousWorkingDay(new Date(nextPayDate));
                    Assert.AreEqual(String.Format("{0:d MMM yyyy}", expectedDate.DateTime), _repaymentDate);
                    break;
                case AUT.Ca:
                    Assert.AreEqual(page.Sliders.HowLong, "11");
                    break;
            }

        }

        [Test, AUT(AUT.Ca), JIRA("QA-241", "QA-159")]
        public void DefaultDurationSliderValueShouldBeCorrectLn()
        {
            var loginPage = Client.Login();
            string email = Data.RandomEmail();
            Customer customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            Application application = ApplicationBuilder.New(customer)
                .Build();
            application.RepayOnDueDate();
            loginPage.LoginAs(email);

            var page = Client.Home();
            switch (Config.AUT)
            {
                case AUT.Za:
                    string[] dateArray = page.Sliders.GetRepaymentDate.Split(' ');
                    string day = Char.IsDigit(dateArray[1].ElementAt(1)) ? dateArray[1].Remove(2, 2) : dateArray[1].Remove(1, 2);
                    _repaymentDate = day + " " + dateArray[2] + " " + dateArray[3];

                    var today = DateTime.Today;
                    var nextPayDate = today.Day <= 25
                                           ? new DateTime(today.Year, today.Month, 25)
                                           : new DateTime(today.Year, today.Month, 25).AddMonths(1);
                    var expectedDate = Driver.Db.GetPreviousWorkingDay(new Date(nextPayDate));
                    Assert.AreEqual(String.Format("{0:d MMM yyyy}", expectedDate.DateTime), _repaymentDate);
                    break;
                case AUT.Ca:
                    Assert.AreEqual(page.Sliders.HowLong, "11");
                    break;
            }

        }

        [Test, AUT(AUT.Ca, AUT.Za), JIRA("QA-237", "QA-153")]
        public void ChangingAmountBeyondMinIsNotAllowedByFrontEnd()
        {
            var product = Driver.Db.Payments.Products.FirstOrDefault();
            int minAmountValue = (int)product.AmountMin;
            int setAmountValue = minAmountValue - 1;
            var page = Client.Home();
            page.Sliders.HowMuch = setAmountValue.ToString(CultureInfo.InvariantCulture);
            page.Sliders.HowLong = "10"; //To lost focus
            page.Help.HelpTriggerClick();
            Assert.AreEqual(minAmountValue.ToString(CultureInfo.InvariantCulture), page.Sliders.HowMuch);
        }

        [Test, AUT(AUT.Ca), JIRA("QA-239", "QA-158")]
        public void MaxDurationSliderValueShouldBeCorrectL0()
        {
            var product = Driver.Db.Payments.Products.FirstOrDefault();
            int maxLoanDuration = product.TermMax;
            int setLoanDuration = maxLoanDuration + 1;
            var page = Client.Home();
            page.Sliders.HowLong = setLoanDuration.ToString(CultureInfo.InvariantCulture);
            page.Sliders.HowMuch = "10"; //To lost focus
            page.Help.HelpTriggerClick();
            Assert.AreEqual(maxLoanDuration.ToString(CultureInfo.InvariantCulture), page.Sliders.HowLong);
        }

        [Test, AUT(AUT.Ca), JIRA("QA-239", "QA-158")]
        public void MaxDurationSliderValueShouldBeCorrectLn()
        {
            var loginPage = Client.Login();
            string email = Data.RandomEmail();
            Customer customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            Application application = ApplicationBuilder.New(customer)
                .Build();
            application.RepayOnDueDate();
            loginPage.LoginAs(email);

            var page = Client.Home();
            var product = Driver.Db.Payments.Products.FirstOrDefault();
            int maxLoanDuration = product.TermMax;
            int setLoanDuration = maxLoanDuration + 1;

            page.Sliders.HowLong = setLoanDuration.ToString(CultureInfo.InvariantCulture);
            page.Sliders.HowMuch = "10"; //To lost focus
            page.Help.HelpTriggerClick();
            Assert.AreEqual(maxLoanDuration.ToString(CultureInfo.InvariantCulture), page.Sliders.HowLong);
        }

        [Test, AUT(AUT.Ca, AUT.Za), JIRA("QA-157")]
        public void MinDurationSliderValueShouldBeCorrectL0()
        {
            var product = Driver.Db.Payments.Products.FirstOrDefault();
            int minDurationValue = (int)product.TermMin;
            int setDurationValue = minDurationValue - 1;
            var page = Client.Home();
            page.Sliders.HowLong = setDurationValue.ToString(CultureInfo.InvariantCulture);
            page.Sliders.HowMuch = "400"; // to lost focus
            Thread.Sleep(2000); // wait some time to changes apply, with out this row it's fail
            Assert.AreEqual(minDurationValue.ToString(CultureInfo.InvariantCulture), page.Sliders.HowLong);
        }

        [Test, AUT(AUT.Ca, AUT.Za), JIRA("QA-157")]
        public void MinDurationSliderValueShouldBeCorrectLn()
        {
            var loginPage = Client.Login();
            string email = Data.RandomEmail();
            Customer customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            Application application = ApplicationBuilder.New(customer)
                .Build();
            application.RepayOnDueDate();
            loginPage.LoginAs(email);

            var product = Driver.Db.Payments.Products.FirstOrDefault();
            int minDurationValue = (int)product.TermMin;
            int setDurationValue = minDurationValue - 1;
            var page = Client.Home();
            page.Sliders.HowLong = setDurationValue.ToString(CultureInfo.InvariantCulture);
            page.Sliders.HowMuch = "400"; // to lost focus
            Thread.Sleep(2000); // wait some time to changes apply, with out this row it's fail
            Assert.AreEqual(minDurationValue.ToString(CultureInfo.InvariantCulture), page.Sliders.HowLong);
        }

         [Test, AUT(AUT.Ca, AUT.Za), JIRA("QA-194")]
        public void WhanCustomerWithLiveLoanTriesTakeLoanSlidersShouldBeBlocked()
        {
            var loginPage = Client.Login();
            string email = Data.RandomEmail();
            Customer customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            Application application = ApplicationBuilder.New(customer)
                .Build();
            loginPage.LoginAs(email);
            Assert.Throws<Exception>(() => { var page = Client.Home(); });
        }

         [Test, AUT(AUT.Ca, AUT.Za), JIRA("QA-155")]
         public void MaxAmountSliderValueShouldBeCorrectLn()
         {
             var loginPage = Client.Login();
             string email = Data.RandomEmail();
             Customer customer = CustomerBuilder.New().WithEmailAddress(email).Build();
             Application application = ApplicationBuilder.New(customer)
                 .Build();
             application.RepayOnDueDate();
             loginPage.LoginAs(email);
             var homePage = Client.Home();
             var response = Driver.Db.Risk.RiskAccounts.FirstOrDefault();
             int creditLimit = (int)response.CreditLimit;
             int moreThanCreditLimit = (int)response.CreditLimit + 100;
             homePage.Sliders.HowMuch = moreThanCreditLimit.ToString(CultureInfo.InvariantCulture);
             homePage.Sliders.HowLong = "10"; //to lost focus
             Thread.Sleep(1000);
             Assert.AreEqual(creditLimit.ToString(), homePage.Sliders.HowMuch);

         }
    }
}
