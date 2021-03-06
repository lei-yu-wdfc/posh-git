﻿using System.Globalization;
using System.Threading;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Api.Requests.Payments.Queries.Ca;
using Wonga.QA.Framework.Api.Requests.Payments.Queries.Uk;
using Wonga.QA.Framework.Api.Requests.Payments.Queries.Wb.Uk;
using Wonga.QA.Framework.Api.Requests.Payments.Queries.Za;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db.Extensions;
using Wonga.QA.Framework.Helpers;
using Wonga.QA.Framework.Old;
using Wonga.QA.Framework.UI;
using Wonga.QA.Framework.UI.UiElements.Pages;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;
using Wonga.QA.Framework.UI.UiElements.Pages.Wb;
using Wonga.QA.Tests.Core;
using System.Linq;
using System;

namespace Wonga.QA.UiTests.Web
{
    /// <summary>
    /// Slider tests for Za
    /// </summary>
    /// 
    [Parallelizable(TestScope.All)]
    class SliderTestsBase : UiTest
    {
        private int _amountMax;
        private int _amountMin;
        private int _amountDefault;
        private int _termMax;
        private int _termMin;
        private int _termDefault;
        private string _repaymentDate;
        private ApiResponse _response;
        //private DateTime _actualDate;

        private const int DefaultLoanTerm = 11;
        private const int MinimumMaxLoanTerm = 30;

        private const int _maxSliderValue = 419;
        private const int _minSliderValue = -419;

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


            _response = Drive.Api.Queries.Post(request);
            _amountMax = (int)Decimal.Parse(_response.Values["AmountMax"].Single(), CultureInfo.InvariantCulture);
            _amountMin = (int)Double.Parse(_response.Values["AmountMin"].Single(), CultureInfo.InvariantCulture);
            _amountDefault =
                (int)Decimal.Parse(_response.Values["AmountDefault"].Single(), CultureInfo.InvariantCulture);
            _termMax = Int32.Parse(_response.Values["TermMax"].Single(), CultureInfo.InvariantCulture);
            _termMin = Int32.Parse(_response.Values["TermMin"].Single(), CultureInfo.InvariantCulture);
            _termDefault = Int32.Parse(_response.Values["TermDefault"].Single(), CultureInfo.InvariantCulture);
        }

        #region Helpers

        private DateTime GetExpectedDefaultPromiseDateL0()
        {
            DateTime defaultPromiseDate;
            var now = DateTime.Today;

            switch (Config.AUT)
            {
                case AUT.Za:
                    {
                        var iMonth = DateTime.UtcNow.Month - 1;

                        var payDayPerMonths = Drive.Db.Ops.ServiceConfigurations.Single(a => a.Key == "Payments.PayDayPerMonth").Value.Split(',');
                        var sliderTermAddDays = Drive.Db.Ops.ServiceConfigurations.Single(a => a.Key == "Payments.SliderTermAddDays").Value;

                        var payDayPerMonth = Int32.Parse(payDayPerMonths[iMonth]);
                        var minimumTerm = Int32.Parse(sliderTermAddDays);

                        defaultPromiseDate = new DateTime(now.Year, now.Month, payDayPerMonth);
                        var expectedTermDefault = defaultPromiseDate.Subtract(now).Days;

                        //Check if we should snap to the next month's payday
                        if (expectedTermDefault < minimumTerm)
                        {
                            iMonth = iMonth + 1 >= 12 ? 1 : iMonth + 1;
                            payDayPerMonth = Int32.Parse(payDayPerMonths[iMonth]);
                            defaultPromiseDate = defaultPromiseDate.AddMonths(1);
                            defaultPromiseDate = new DateTime(defaultPromiseDate.Year, defaultPromiseDate.Month, payDayPerMonth);
                        }

                        defaultPromiseDate = Drive.Db.GetPreviousWorkingDay(new Date(defaultPromiseDate));
                    }
                    break;
                case AUT.Ca:
                    {
                        //CA starts all loans from the next working day
                        int daysTillStartOfLoan = DateHelper.GetNumberOfDaysUntilStartOfLoanForCa(now);
                        defaultPromiseDate = now.AddDays(DefaultLoanTerm + daysTillStartOfLoan);
                    }
                    break;

                default:
                    {
                        defaultPromiseDate = now.AddDays(DefaultLoanTerm);
                    }
                    break;
            }
            return defaultPromiseDate;
        }

        private int GetExpectedMinTerm()
        {
            return Drive.Db.Payments.Products.FirstOrDefault().TermMin;
        }

        private int GetExpectedMaxTermL0()
        {
            int maxTerm = 0;

            switch (Config.AUT)
            {
                case AUT.Za:
                    {
                        var promiseDate = GetExpectedDefaultPromiseDateL0();
                        var iMonth = promiseDate.Month - 1;

                        var payDayPlusToMaxTerm = Int32.Parse(Drive.Db.Ops.ServiceConfigurations.Single(a => a.Key == "Payments.PayDayPlusToMaxTerm").Value.Split(',')[iMonth]);

                        maxTerm = (promiseDate.AddDays(payDayPlusToMaxTerm) - DateTime.Today).Days;

                        if (maxTerm < MinimumMaxLoanTerm) maxTerm = MinimumMaxLoanTerm;
                    }
                    break;

                default:
                    {
                        return Drive.Db.Payments.Products.FirstOrDefault().TermMax;
                    }
            }

            return maxTerm;
        }

        #endregion

        [Test, AUT(AUT.Za, AUT.Ca), JIRA("QA-282")]
        [Category(TestCategories.SmokeTest)]
        public void ChooseLoanAmountAndDurationViaPlusMinusButtons()
        {
            var homePage = Client.Home();

            #region clicks

            for (int i = 0; i < Get.RandomInt(20); i++)
            {
                homePage.Sliders.ClickAmountPlusButton();
            }
            for (int i = 0; i < Get.RandomInt(20); i++)
            {
                homePage.Sliders.ClickAmountMinusButton();
            }
            for (int i = 0; i < Get.RandomInt(10); i++)
            {
                homePage.Sliders.ClickDurationPlusButton();
            }
            for (int i = 0; i < Get.RandomInt(10); i++)
            {
                homePage.Sliders.ClickDurationMinusButton();
            }

            #endregion

            var termCustomerEnter = Int32.Parse(homePage.Sliders.HowLong);
            var amountCustomerEnter = Int32.Parse(homePage.Sliders.HowMuch);

            string _totalAmount = homePage.Sliders.GetTotalAmount;

            Assert.AreEqual(amountCustomerEnter.ToString(), _totalAmount.Remove(0, 1));


            string[] dateArray = homePage.Sliders.GetRepaymentDate.Split(' ');
            string day = Char.IsDigit(dateArray[1].ElementAt(1)) ? dateArray[1].Remove(2, 2) : dateArray[1].Remove(1, 2);
            _repaymentDate = day + " " + dateArray[2] + " " + dateArray[3];
            DateTime expectedDate;
            switch (Config.AUT)
            {
                case AUT.Za:
                    expectedDate = DateTime.UtcNow.AddDays(termCustomerEnter);
                    break;
                case AUT.Ca:
                    expectedDate =
                        DateTime.UtcNow.AddDays(termCustomerEnter + DateHelper.GetNumberOfDaysUntilStartOfLoanForCa());
                    break;
                default:
                    throw new NotImplementedException();
            }
            Assert.AreEqual(String.Format("{0:d MMM yyyy}", expectedDate), _repaymentDate);

            var personalDetailsPage = homePage.Sliders.Apply() as PersonalDetailsPage;
            string personalDetailPageAmount = personalDetailsPage.GetTotalAmount;

            Assert.AreEqual(amountCustomerEnter.ToString(), personalDetailPageAmount.Remove(0, 1));

            dateArray = personalDetailsPage.GetRepaymentDate.Split(' ');
            day = Char.IsDigit(dateArray[1].ElementAt(1)) ? dateArray[1].Remove(2, 2) : dateArray[1].Remove(1, 2);
            _repaymentDate = day + " " + dateArray[2] + " " + dateArray[3];

            Assert.AreEqual(String.Format("{0:d MMM yyyy}", expectedDate), _repaymentDate);


        }


        [Test, AUT(AUT.Za, AUT.Ca), JIRA("QA-150")]// Pending removed
        // SmokeTest - return when test is enabled
        public void CustomerTypesValidValuesIntoAmountAndDurationFields()
        {
            this.GetInitialValues();
            var termCustomerEnter = Get.RandomInt(_termMin, _termMax);
            var amountCustomerEnter = Get.RandomInt(_amountMin, _amountMax);

            var homePage = Client.Home();

            homePage.Sliders.HowLong = termCustomerEnter.ToString();
            homePage.Sliders.HowMuch = amountCustomerEnter.ToString();

            string _totalAmount = homePage.Sliders.GetTotalAmount;

            Assert.AreEqual(amountCustomerEnter.ToString(), _totalAmount.Remove(0, 1));


            string[] dateArray = homePage.Sliders.GetRepaymentDate.Split(' ');
            string day = Char.IsDigit(dateArray[1].ElementAt(1)) ? dateArray[1].Remove(2, 2) : dateArray[1].Remove(1, 2);
            _repaymentDate = day + " " + dateArray[2] + " " + dateArray[3];
            DateTime expectedDate;
            switch (Config.AUT)
            {
                case AUT.Za:
                    expectedDate = DateTime.UtcNow.AddDays(termCustomerEnter);
                    break;
                case AUT.Ca:
                    expectedDate =
                        DateTime.UtcNow.AddDays(termCustomerEnter + DateHelper.GetNumberOfDaysUntilStartOfLoanForCa());
                    break;
                default:
                    throw new NotImplementedException();
            }
            Assert.AreEqual(String.Format("{0:d MMM yyyy}", expectedDate), _repaymentDate);

            var personalDetailsPage = homePage.Sliders.Apply() as PersonalDetailsPage;
            string personalDetailPageAmount = personalDetailsPage.GetTotalAmount;

            Assert.AreEqual(amountCustomerEnter.ToString(), personalDetailPageAmount.Remove(0, 1));

            dateArray = personalDetailsPage.GetRepaymentDate.Split(' ');
            day = Char.IsDigit(dateArray[1].ElementAt(1)) ? dateArray[1].Remove(2, 2) : dateArray[1].Remove(1, 2);
            _repaymentDate = day + " " + dateArray[2] + " " + dateArray[3];

            Assert.AreEqual(String.Format("{0:d MMM yyyy}", expectedDate), _repaymentDate);


        }

        [Test, AUT(AUT.Ca, AUT.Za, AUT.Wb), JIRA("QA-156", "QA-238", "QA-295"), Category(TestCategories.SmokeTest)]
        public void DefaultAmountSliderValueShouldBeCorrectL0()
        {
            this.GetInitialValues();
            var page = Client.Home();
            switch (Config.AUT)
            {
                case AUT.Za:
                    Assert.AreEqual(page.Sliders.HowMuch, _amountDefault.ToString(CultureInfo.InvariantCulture));
                    break;
                case AUT.Ca:
                    Assert.AreEqual(page.Sliders.HowMuch, _amountDefault.ToString(CultureInfo.InvariantCulture));
                    break;
                case AUT.Wb:
                    var defaultWbAmount =
                        Drive.Data.Ops.Db.ServiceConfigurations.FindByKey("Payments.Wb.DefaultLoanAmount").Value.
                            ToString();
                    Assert.AreEqual(page.Sliders.HowMuch.Replace(",", ""), defaultWbAmount);
                    break;
            }

        }

        [Test, AUT(AUT.Ca, AUT.Za, AUT.Wb), JIRA("QA-156", "QA-238", "QA-295")]
        public void DefaultAmountSliderValueShouldBeCorrectLn()
        {
            this.GetInitialValues();
            string email = Get.RandomEmail();
            if (Config.AUT.Equals(AUT.Wb))
            {
                Customer customer = CustomerBuilder.New().WithEmailAddress(email).Build();
                Organisation organisation = OrganisationBuilder.New(customer).Build();
                ApplicationBuilder.New(customer, organisation).WithExpectedDecision(ApplicationDecisionStatus.Accepted).
                    Build();
            }
            else
            {
                Customer customer = CustomerBuilder.New().WithEmailAddress(email).Build();
                ApplicationBuilder.New(customer).Build().RepayOnDueDate();
            }
            var loginPage = Client.Login();
            loginPage.LoginAs(email);

            var page = Client.Home();
            switch (Config.AUT)
            {
                case AUT.Za:
                    Assert.AreEqual(page.Sliders.HowMuch, _amountDefault.ToString(CultureInfo.InvariantCulture));
                    break;
                case AUT.Ca:
                    Assert.AreEqual(page.Sliders.HowMuch, _amountDefault.ToString(CultureInfo.InvariantCulture));
                    break;
                case AUT.Wb:
                    var defaultWbAmount =
                        Drive.Data.Ops.Db.ServiceConfigurations.FindByKey("Payments.Wb.DefaultLoanAmount").Value.
                            ToString();
                    Assert.AreEqual(page.Sliders.HowMuch.Replace(",", ""), defaultWbAmount);
                    break;
            }
        }

        [Test, AUT(AUT.Ca, AUT.Wb), JIRA("QA-241", "QA-159", "QA-296"), Category(TestCategories.SmokeTest)]
        public void DefaultDurationSliderValueShouldBeCorrectL0()
        {
            var page = Client.Home();
            switch (Config.AUT)
            {
                case AUT.Ca:
                    string[] dateArray = page.Sliders.GetRepaymentDate.Split(' ');
                    string day = Char.IsDigit(dateArray[1].ElementAt(1))
                                     ? dateArray[1].Remove(2, 2)
                                     : dateArray[1].Remove(1, 2);
                    _repaymentDate = day + " " + dateArray[2] + " " + dateArray[3];

                    var expectedDate = GetExpectedDefaultPromiseDateL0();
                    Assert.AreEqual(String.Format("{0:d MMM yyyy}", expectedDate), _repaymentDate);
                    break;
                case AUT.Wb:
                    Assert.AreEqual(page.Sliders.HowLong, "16");
                    break;
            }

        }

        [Test, AUT(AUT.Ca, AUT.Za, AUT.Wb), JIRA("QA-241", "QA-159", "QA-296")]
        public void DefaultDurationSliderValueShouldBeCorrectLn()
        {
            string email = Get.RandomEmail();
            if (Config.AUT.Equals(AUT.Wb))
            {
                Customer customer = CustomerBuilder.New().WithEmailAddress(email).Build();
                Organisation organisation = OrganisationBuilder.New(customer).Build();
                ApplicationBuilder.New(customer, organisation).WithExpectedDecision(ApplicationDecisionStatus.Accepted).
                    Build();
            }
            else
            {
                Customer customer = CustomerBuilder.New().WithEmailAddress(email).Build();
                ApplicationBuilder.New(customer).Build().RepayOnDueDate();
            }
            var loginPage = Client.Login();
            loginPage.LoginAs(email);

            var page = Client.Home();
            switch (Config.AUT)
            {
                case AUT.Za:
                    string[] dateArray = page.Sliders.GetRepaymentDate.Split(' ');
                    string day = Char.IsDigit(dateArray[1].ElementAt(1))
                                     ? dateArray[1].Remove(2, 2)
                                     : dateArray[1].Remove(1, 2);
                    _repaymentDate = day + " " + dateArray[2] + " " + dateArray[3];

                    var today = DateTime.Today;
                    var nextPayDate = today.Day <= 20
                                          ? new DateTime(today.Year, today.Month, 25)
                                          : new DateTime(today.Year, today.Month, 25).AddMonths(1);
                    var expectedDate = Drive.Db.GetPreviousWorkingDay(new Date(nextPayDate));
                    Assert.AreEqual(String.Format("{0:d MMM yyyy}", expectedDate.DateTime), _repaymentDate);
                    break;
                case AUT.Ca:
                    Assert.AreEqual(page.Sliders.HowLong, DefaultLoanTerm.ToString());
                    break;
                case AUT.Wb:
                    Assert.AreEqual(page.Sliders.HowLong, "16");
                    break;
            }

        }

        [Test, AUT(AUT.Ca, AUT.Za), JIRA("QA-239", "QA-158"), Category(TestCategories.SmokeTest)]
        public void MaxDurationSliderValueShouldBeCorrectL0()
        {
            int maxLoanDuration = GetExpectedMaxTermL0();
            int setLoanDuration = maxLoanDuration + 1;
            var page = Client.Home();
            page.Sliders.HowLong = setLoanDuration.ToString(CultureInfo.InvariantCulture);
            Assert.AreEqual(maxLoanDuration.ToString(CultureInfo.InvariantCulture), page.Sliders.HowLong);
        }

        [Test, AUT(AUT.Za, AUT.Ca), JIRA("QA-154", "QA-284"), Category(TestCategories.SmokeTest)]
        public void MaxAmountSliderValueShouldBeCorrectL0()
        {
            var serviceConfigurations = Drive.Data.Ops.Db.ServiceConfigurations;
            var defaultCreditLimit =
                serviceConfigurations.Find(serviceConfigurations.Key.Like("Risk.DefaultCreditLimit")).Value;
            int setAmount = Int32.Parse(defaultCreditLimit) + 100;

            var page = Client.Home();
            page.Sliders.HowMuch = setAmount.ToString(CultureInfo.InvariantCulture);
            Assert.AreEqual(defaultCreditLimit, page.Sliders.GetTotalAmount.Remove(0, 1));

        }

        [Test, AUT(AUT.Ca, AUT.Za), JIRA("QA-155", "QA-285"), Category(TestCategories.SmokeTest)]
        public void MaxAmountSliderValueShouldBeCorrectLn()
        {
            var riskAccounts = Drive.Data.Risk.Db.RiskAccounts;
            var loginPage = Client.Login();
            string email = Get.RandomEmail();
            Customer customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            Application application = ApplicationBuilder.New(customer)
                .Build();
            application.RepayOnDueDate();
            loginPage.LoginAs(email);

            var page = Client.Home();

            var creditLimit = riskAccounts.FindByAccountId(customer.Id).CreditLimit;
            var setAmount = creditLimit + 100;
            page.Sliders.HowMuch = setAmount.ToString(CultureInfo.InvariantCulture);
            page.Sliders.HowLong = "10";
            Assert.AreEqual(creditLimit.ToString(), page.Sliders.GetTotalAmount.Remove(0, 1) + ".00");
        }

        [Test, AUT(AUT.Ca, AUT.Za), JIRA("QA-157"), Category(TestCategories.SmokeTest)]
        public void MinDurationSliderValueShouldBeCorrectL0()
        {
            int minDurationValue = GetExpectedMinTerm();
            int setDurationValue = minDurationValue - 1;
            var page = Client.Home();
            page.Sliders.HowLong = setDurationValue.ToString(CultureInfo.InvariantCulture);
            page.Sliders.HowMuch = "200";
            Thread.Sleep(2000);
            Assert.AreEqual(minDurationValue.ToString(CultureInfo.InvariantCulture), page.Sliders.HowLong);
        }

        [Test, AUT(AUT.Ca, AUT.Za), JIRA("QA-194"), Category(TestCategories.SmokeTest)]
        public void WhanCustomerWithLiveLoanTriesTakeLoanSlidersShouldBeBlocked()
        {
            var loginPage = Client.Login();
            string email = Get.RandomEmail();
            Customer customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            Application application = ApplicationBuilder.New(customer)
                .Build();
            loginPage.LoginAs(email);
            var page = Client.Home();
            Assert.IsFalse(page.Sliders.IsSubmitButtonPresent());
        }


        [Test, AUT(AUT.Ca, AUT.Za), JIRA("QA-152"), Category(TestCategories.SmokeTest), MultipleAsserts]
        public void CustomerTriesEnterSomeRubbishDataToFieldsThenAmountsShouldntBeChanged()
        {
            this.GetInitialValues();
            var page = Client.Home();
            switch (Config.AUT)
            {

                case (AUT.Ca):

                    #region enter an empty string

                    page.Sliders.LoanAmount.Clear();
                    page.Sliders.LoanDuration.Clear();
                    Assert.AreEqual(_amountDefault.ToString(CultureInfo.InvariantCulture), page.Sliders.HowMuch);
                    Assert.AreEqual(_termDefault.ToString(CultureInfo.InvariantCulture), page.Sliders.HowLong);

                    #endregion

                    #region enter negative values

                    page.Sliders.HowMuch = "-200";
                    page.Sliders.HowLong = "-10";
                    Assert.AreEqual("200".ToString(CultureInfo.InvariantCulture), page.Sliders.HowMuch);
                    Assert.AreEqual("10".ToString(CultureInfo.InvariantCulture), page.Sliders.HowLong);

                    #endregion

                    #region enter mixed data

                    page.Sliders.HowMuch = "kjh2-dsf0sdf0";
                    page.Sliders.HowLong = "dfg1dfg-0df";
                    Assert.AreEqual("200".ToString(CultureInfo.InvariantCulture), page.Sliders.HowMuch);
                    Assert.AreEqual("10".ToString(CultureInfo.InvariantCulture), page.Sliders.HowLong);

                    #endregion

                    #region enter bigger than max possible values

                    page.Sliders.HowMuch = "5000";
                    page.Sliders.HowLong = "1000";
                    Assert.AreEqual(_amountMax.ToString(CultureInfo.InvariantCulture), page.Sliders.HowMuch);
                    Assert.AreEqual(_termMax.ToString(CultureInfo.InvariantCulture), page.Sliders.HowLong);

                    #endregion

                    break;

                case (AUT.Za):

                    #region enter an empty string

                    page.Sliders.LoanAmount.Clear();
                    page.Sliders.LoanDuration.Clear();
                    Assert.AreEqual(_amountDefault.ToString(CultureInfo.InvariantCulture), page.Sliders.HowMuch);
                    Do.With.Message("How long slider is empty").Until(() => page.Sliders.HowLong != String.Empty);
                    Assert.AreEqual(_termDefault.ToString(CultureInfo.InvariantCulture), page.Sliders.HowLong);

                    #endregion

                    #region enter negative values

                    page.Sliders.HowMuch = "-200";
                    page.Sliders.HowLong = "-10";
                    Assert.AreEqual("200".ToString(CultureInfo.InvariantCulture), page.Sliders.HowMuch);
                    Assert.AreEqual("10".ToString(CultureInfo.InvariantCulture), page.Sliders.HowLong);

                    #endregion

                    #region enter mixed data

                    page.Sliders.HowMuch = "kjh2-dsf0sdf0";
                    page.Sliders.HowLong = "dfg1dfg-0df";
                    Assert.AreEqual("200".ToString(CultureInfo.InvariantCulture), page.Sliders.HowMuch);
                    Assert.AreEqual("10".ToString(CultureInfo.InvariantCulture), page.Sliders.HowLong);

                    #endregion

                    #region enter bigger than max possible values

                    page.Sliders.HowMuch = "5000";
                    page.Sliders.HowLong = "1000";
                    Assert.AreEqual(_amountMax.ToString(CultureInfo.InvariantCulture), page.Sliders.HowMuch);
                    Assert.AreEqual(_termMax.ToString(CultureInfo.InvariantCulture), page.Sliders.HowLong);

                    #endregion

                    break;
            }




        }

        [Test, AUT(AUT.Ca, AUT.Za), JIRA("QA-283"), Category(TestCategories.SmokeTest)]
        public void CustomerTryToChooseLoanAountAndDurationBiggerThanMaxAndTakeLoan()
        {
            string firstName = Get.RandomString(3, 10);
            string lastName = Get.RandomString(3, 10);
            var serviceConfigurations = Drive.Data.Ops.Db.ServiceConfigurations;
            var defaultCreditLimit =
                serviceConfigurations.Find(serviceConfigurations.Key.Like("Risk.DefaultCreditLimit")).Value;
            int setAmount = Int32.Parse(defaultCreditLimit) + 100;
            var defaultTermLimit = GetExpectedMaxTermL0();
            int setTerm = defaultTermLimit + 10;

            var journey = JourneyFactory.GetL0Journey(Client.Home())
                .WithEmployerName(Get.EnumToString(RiskMask.TESTEmployedMask))
                .WithFirstName(firstName).WithLastName(lastName);
            var page = journey.CurrentPage as HomePage;
            page.Sliders.HowMuch = setAmount.ToString();
            page.Sliders.HowLong = setTerm.ToString();
            Assert.AreEqual(defaultCreditLimit.ToString(CultureInfo.InvariantCulture), page.Sliders.HowMuch);
            Thread.Sleep(1000);
            Assert.AreEqual(defaultTermLimit.ToString(CultureInfo.InvariantCulture), page.Sliders.HowLong);
            journey.CurrentPage = page.Sliders.Apply() as PersonalDetailsPage;
            var acceptedPage = journey.Teleport<AcceptedPage>() as AcceptedPage;
            switch (Config.AUT)
            {
                case AUT.Ca:
                    acceptedPage.SignConfirmCaL0(DateTime.Now.ToString("d MMM yyyy"), firstName, lastName);
                    break;

                case AUT.Za:
                    acceptedPage.SignAgreementConfirm();
                    acceptedPage.SignDirectDebitConfirm();
                    break;
            }
            var dealDone = acceptedPage.Submit();
        }
    }
}
