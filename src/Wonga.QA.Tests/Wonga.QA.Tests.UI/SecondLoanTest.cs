using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using MbUnit.Framework;
using OpenQA.Selenium;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Helpers;
using Wonga.QA.Framework.UI;
using Wonga.QA.Framework.UI.Mappings;
using Wonga.QA.Framework.UI.UiElements;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;
using Wonga.QA.Framework.UI.UiElements.Pages.Wb;
using Wonga.QA.Tests.Core;
using Wonga.QA.Framework.UI.UiElements.Pages;


namespace Wonga.QA.Tests.Ui
{
    [Parallelizable(TestScope.All)]
    class SecondLoanTest : UiTest
    {
        [Test, AUT(AUT.Za), JIRA("QA-195"), Category(TestCategories.SmokeTest)] // add AUT.Ca when .RepayOnDueDate() will be work
        public void InformationAboutSecondLoanShouldBeDisplayed()
        {
            string actualRepaymentDate;
            DateTime date;
            string email = Get.RandomEmail();
            string name = Get.GetName();
            string surname = Get.RandomString(10);
            Customer customer = CustomerBuilder
                .New()
                .WithEmailAddress(email)
                .WithForename(name)
                .WithSurname(surname)
                .Build();
            Application application = ApplicationBuilder.New(customer).Build();
            application.RepayOnDueDate();

            var loginPage = Client.Login();
            var mySummaryPage = loginPage.LoginAs(email);
            var journey = JourneyFactory.GetLnJourney(Client.Home());
            switch (Config.AUT)
            {
                case AUT.Za:
                    var pageZa = journey.WithAmount(500).WithDuration(20)
                        .Teleport<DealDonePage>()as DealDonePage;
                    Assert.AreEqual("R660.45", pageZa.GetRapaymentAmount());
                    date = DateTime.Now.AddDays(20);
                    actualRepaymentDate = String.Format("{0:d MMMM yyyy}", date);
                    Assert.AreEqual(actualRepaymentDate, pageZa.GetRepaymentDate());
                    var summaryZa = pageZa.ContinueToMyAccount() as MySummaryPage;
                    Assert.AreEqual("R660.45", summaryZa.GetTotalToRepay);
                    switch (date.Day % 10)
                    {
                        case 1:
                            actualRepaymentDate = (date.Day > 10 && date.Day < 20)
                                                        ? String.Format("{0:dddd d\\t\\h MMM yyyy}", date)
                                                        : String.Format("{0:dddd d\\s\\t MMM yyyy}", date);
                            break;
                        case 2:
                            actualRepaymentDate = (date.Day > 10 && date.Day < 20)
                                                        ? String.Format("{0:dddd d\\t\\h MMM yyyy}", date)
                                                        : String.Format("{0:dddd d\\n\\d MMM yyyy}", date);
                            break;
                        case 3:
                            actualRepaymentDate = (date.Day > 10 && date.Day < 20)
                                                        ? String.Format("{0:dddd d\\t\\h MMM yyyy}", date)
                                                        : String.Format("{0:dddd d\\r\\d MMM yyyy}", date);
                            break;
                        default:
                            actualRepaymentDate = String.Format("{0:dddd d\\t\\h MMM yyyy}", date);
                            break;
                    }
                    Assert.AreEqual(actualRepaymentDate, summaryZa.GetPromisedRepayDate);
                    var lastApplication = customer.GetApplications().Single(a => !a.IsClosed);
                    var applicationEntity = Do.Until(() => Drive.Data.Payments.Db.Applications.FindByExternalId(lastApplication.Id));
                    var fixedTermApplicationEntity = Do.Until(() => Drive.Data.Payments.Db.FixedTermLoanApplications.FindByApplicationId(applicationEntity.ApplicationId));
                    Assert.AreEqual(String.Format("{0:d MMMM yyyy}", date), String.Format("{0:d MMMM yyyy}", fixedTermApplicationEntity.PromiseDate));
                    Assert.AreEqual("500.00", fixedTermApplicationEntity.LoanAmount.ToString());
                    break;

                case AUT.Ca:
                    var pageCa = journey.WithAmount(200).WithDuration(20)
                        .Teleport<DealDonePage>() as DealDonePage;
                    Assert.AreEqual("$234.00", pageCa.GetRapaymentAmount());
                    date = DateTime.Now.AddDays(DateHelper.GetNumberOfDaysUntilStartOfLoanForCa()+20);
                    actualRepaymentDate = String.Format("{0:d MMMM yyyy}", date);
                    Assert.AreEqual(actualRepaymentDate, pageCa.GetRepaymentDate());
                    var summaryCa = pageCa.ContinueToMyAccount() as MySummaryPage;
                    Assert.AreEqual("$234.00", summaryCa.GetTotalToRepay);
                    switch (date.Day % 10)
                    {
                        case 1:
                            actualRepaymentDate = (date.Day > 10 && date.Day < 20)
                                                        ? String.Format("{0:ddd d\\t\\h MMM yyyy}", date)
                                                        : String.Format("{0:ddd d\\s\\t MMM yyyy}", date);
                            break;
                        case 2:
                            actualRepaymentDate = (date.Day > 10 && date.Day < 20)
                                                        ? String.Format("{0:ddd d\\t\\h MMM yyyy}", date)
                                                        : String.Format("{0:ddd d\\n\\d MMM yyyy}", date);
                            break;
                        case 3:
                            actualRepaymentDate = (date.Day > 10 && date.Day < 20)
                                                        ? String.Format("{0:ddd d\\t\\h MMM yyyy}", date)
                                                        : String.Format("{0:ddd d\\r\\d MMM yyyy}", date);
                            break;
                        default:
                            actualRepaymentDate = String.Format("{0:ddd d\\t\\h MMM yyyy}", date);
                            break;
                    }
                    Assert.AreEqual(actualRepaymentDate, summaryCa.GetPromisedRepayDate);
                    var lastApplication2 = customer.GetApplications().Single(a=>!a.IsClosed);
                    var applicationEntity2 = Do.Until(() => Drive.Data.Payments.Db.Applications.FindByExternalId(lastApplication2.Id));
                    var fixedTermApplicationEntity2 = Do.Until(() => Drive.Data.Payments.Db.FixedTermLoanApplications.FindByApplicationId(applicationEntity2.ApplicationId));
                    Assert.AreEqual(String.Format("{0:d MMMM yyyy}", date), String.Format("{0:d MMMM yyyy}", fixedTermApplicationEntity2.PromiseDate));
                    Assert.AreEqual("200.00", fixedTermApplicationEntity2.LoanAmount.ToString());
                    break;
            }
        }
    }
}
