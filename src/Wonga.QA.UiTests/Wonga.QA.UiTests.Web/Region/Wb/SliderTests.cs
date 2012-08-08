using System.Globalization;
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
using Wonga.QA.Framework.UI;
using Wonga.QA.Framework.UI.UiElements.Pages;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;
using Wonga.QA.Framework.UI.UiElements.Pages.Wb;
using Wonga.QA.Tests.Core;
using System.Linq;
using System;

namespace Wonga.QA.UiTests.Web.Region.Wb
{
    [Parallelizable(TestScope.All), AUT(AUT.Wb)]
    public class SliderTests : UiTest
    {
        private int _amountMax;
        private int _amountMin;
        private int _amountDefault;
        private int _termMax;
        private int _termMin;
        private int _termDefault;
        //private string _repaymentDate;
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

        [Test, JIRA("QA-292"), Pending("Sliders need fix")]
        public void ChooseLoanAmountAndDurationViaSlidersMotionWb()
        {
            this.GetInitialValues();
            var homePage = Client.Home();
            homePage.Sliders.MoveAmountSlider = Get.RandomInt(_amountMin, _amountMax);
            homePage.Sliders.MoveDurationSlider = Get.RandomInt(_termMin, _termMax);

            Assert.IsNull(homePage.Sliders.GetTotalToRepay);

            var nextPage = homePage.Sliders.Apply() as EligibilityQuestionsPage;
            Assert.IsNotNull(nextPage);
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

        [Test, JIRA("SME-1581"), Owner(Owner.EugeneVlokh)]
        public void CheckThatMaxLoanAmountIsCorrect()
        {
            var homePage = Client.Home();
            homePage.Sliders.MoveAmountSlider = _maxSliderValue;

            Assert.AreEqual("15,000", homePage.Sliders.LoanAmount.GetValue());

        }

        [Test, JIRA("SME-1563"), Owner(Owner.EugeneVlokh)]
        [Pending("Wonga.QA.Framework.Core.Config does not contain a definition for WbRiskBasedPricingEnabled")]
        public void WhenCustomerUsesSlidersThenIncludeCostsAndRepaymentAmount()
        {

            var riskPricingEnabled = Do.Until(() => Drive.Db.Ops.ServiceConfigurations.Single(a => a.Key == "Payments.Wb.RiskBasedPricingEnabled"));
            //Config.WbRiskBasedPricingEnabled.RiskBasedPricingEnabled = bool.Parse(riskPricingEnabled.Value);

            var homePage = Client.Home();
            var sliders = homePage.Sliders;
            sliders.MoveAmountSlider = Get.RandomInt(_minSliderValue, _maxSliderValue);
            sliders.MoveDurationSlider = Get.RandomInt(_minSliderValue, _maxSliderValue);

            Assert.AreEqual("£" + sliders.LoanAmount.GetValue(), sliders.GetTotalAmount);
            Assert.IsNotNull(sliders.GetTotalFees);
        }
    }
}
