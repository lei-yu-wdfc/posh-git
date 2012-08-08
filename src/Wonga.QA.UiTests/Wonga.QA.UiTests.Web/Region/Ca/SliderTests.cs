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

namespace Wonga.QA.UiTests.Web.Region.Ca
{
    [Parallelizable(TestScope.All), AUT(AUT.Ca)]
    public class SliderTests : UiTest
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

        //private const int DefaultLoanTerm = 11;
        //private const int MinimumMaxLoanTerm = 30;

        //private const int _maxSliderValue = 419;
        //private const int _minSliderValue = -419;

        public void GetInitialValues()
        {
            ApiRequest request = new GetFixedTermLoanOfferCaQuery();

            _response = Drive.Api.Queries.Post(request);
            _amountMax = (int)Decimal.Parse(_response.Values["AmountMax"].Single(), CultureInfo.InvariantCulture);
            _amountMin = (int)Double.Parse(_response.Values["AmountMin"].Single(), CultureInfo.InvariantCulture);
            _amountDefault =
                (int)Decimal.Parse(_response.Values["AmountDefault"].Single(), CultureInfo.InvariantCulture);
            _termMax = Int32.Parse(_response.Values["TermMax"].Single(), CultureInfo.InvariantCulture);
            _termMin = Int32.Parse(_response.Values["TermMin"].Single(), CultureInfo.InvariantCulture);
            _termDefault = Int32.Parse(_response.Values["TermDefault"].Single(), CultureInfo.InvariantCulture);
        }

        [Test, Category(TestCategories.SmokeTest)]
        public void VariableInterestisCalculatedCorrectly()
        {
            var homePage = Client.Home();
            homePage.Sliders.HowMuch = "100";
            homePage.Sliders.HowLong = "30";

            Assert.AreEqual(homePage.Sliders.GetTotalToRepay, "$121.00");
            //maximum charge is 21$ for each 100$ borrowed for 30 days.
        }

        //Pending("Wierd selenium problem") fixed in ZA =>  once new sliders been enabled!
        [Test, JIRA("QA-149"), Pending("Wierd selenium problem")]
        public void ChooseLoanAmountAndDurationViaSlidersMotion()
        {
            var homePage = Client.Home();

            homePage.Sliders.MoveAmountSlider = Get.RandomInt(-100, 100);
            homePage.Sliders.MoveDurationSlider = Get.RandomInt(-100, 100);

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

        [Test, AUT(AUT.Ca), JIRA("QA-237", "QA-153"), Category(TestCategories.SmokeTest)]
        public void ChangingAmountBeyondMinIsNotAllowedByFrontEnd()
        {
            var product = Drive.Db.Payments.Products.FirstOrDefault();
            int minAmountValue = (int)product.AmountMin;
            int setAmountValue = minAmountValue - 1;
            var page = Client.Home();
            page.Sliders.HowMuch = setAmountValue.ToString(CultureInfo.InvariantCulture);

            Assert.AreEqual(minAmountValue.ToString(CultureInfo.InvariantCulture), page.Sliders.HowMuch);
        }

        [Test, AUT(AUT.Ca), JIRA("QA-239", "QA-158")]
        public void MaxDurationSliderValueShouldBeCorrectLn()
        {
            var loginPage = Client.Login();
            string email = Get.RandomEmail();
            Customer customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            Application application = ApplicationBuilder.New(customer)
                .Build();
            application.RepayOnDueDate();
            loginPage.LoginAs(email);

            var page = Client.Home();
            var product = Drive.Db.Payments.Products.FirstOrDefault();
            int maxLoanDuration = product.TermMax;
            int setLoanDuration = maxLoanDuration + 1;

            page.Sliders.HowLong = setLoanDuration.ToString(CultureInfo.InvariantCulture);
            page.Sliders.HowMuch = "10"; //To lost focus
            Assert.AreEqual(maxLoanDuration.ToString(CultureInfo.InvariantCulture), page.Sliders.HowLong);
        }

        [Test, AUT(AUT.Ca), JIRA("QA-157")]
        public void MinDurationSliderValueShouldBeCorrectLn()
        {
            var loginPage = Client.Login();
            string email = Get.RandomEmail();
            Customer customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            Application application = ApplicationBuilder.New(customer)
                .Build();
            application.RepayOnDueDate();
            loginPage.LoginAs(email);

            var product = Drive.Db.Payments.Products.FirstOrDefault();
            int minDurationValue = (int)product.TermMin;
            int setDurationValue = minDurationValue - 1;
            var page = Client.Home();
            page.Sliders.HowLong = setDurationValue.ToString(CultureInfo.InvariantCulture);
            page.Sliders.HowMuch = "400"; // to lost focus
            Thread.Sleep(2000); // wait some time to changes apply, with out this row it's fail
            Assert.AreEqual(minDurationValue.ToString(CultureInfo.InvariantCulture), page.Sliders.HowLong);
        }
    }
}
