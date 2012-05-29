using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Cs;
using Wonga.QA.Framework.Msmq;
using Wonga.QA.Tests.Core;
using Wonga.QA.Tests.Payments.Helpers;

namespace Wonga.QA.Tests.Payments.Queries
{
    [TestFixture]
    [Parallelizable(TestScope.All)]
    public class GetFixedTermLoanExtensionQuoteTests
    {
        [Test, AUT(AUT.Uk)]
        public void TenDayLoanQuoteOnDayFiveToExtendForTenDays()
        {
            var promiseDate = new Date(DateTime.UtcNow.AddDays(5));
            var day1ExtendDate = promiseDate.DateTime.AddDays(1).Date;
            var accountId = Guid.NewGuid();
            var bankAccountId = Guid.NewGuid();
            var paymentCardId = Guid.NewGuid();
            var appId = Guid.NewGuid();
            const decimal trustRating = 400.00M;

            var setupData = new ExtendLoanFunctions();

            setupData.TenDayLoanQuoteOnDayFiveSetup(appId, paymentCardId, bankAccountId, accountId, trustRating);

            var response = Drive.Api.Queries.Post(new GetFixedTermLoanExtensionQuoteUkQuery() {ApplicationId = appId});
            Assert.AreEqual(appId.ToString(), response.Values["ApplicationId"].Single(), "ApplicationId incorrect");
            Assert.AreEqual("1", response.Values["SliderMinDays"].Single(), "SliderMinDays incorrect");
            Assert.AreEqual("30", response.Values["SliderMaxDays"].Single(), "SliderMaxDays incorrect");
            Assert.AreEqual("100.00", response.Values["CurrentPrincipleAmount"].Single(), "CurrentPrincipleAmount incorrect");
            Assert.AreEqual("110.70", response.Values["TotalAmountDueToday"].Single(), "TotalAmountDueToday incorrect");
            Assert.AreEqual("10.70", response.Values["ExtensionPartPaymentAmount"].Single(), "ExtensionPartPaymentAmount incorrect");
            Assert.AreEqual("10", response.Values["LoanExtensionFee"].Single(), "LoanExtensionFee incorrect");

            Assert.AreEqual(day1ExtendDate, DateTime.Parse(response.Values["ExtensionDate"].ToArray()[0]).Date, "First Day Extend Date Incorrec" );
        }

        [Test]
        [AUT(AUT.Uk), JIRA("UK-1351")]
        public void Query_ShouldReturnAllDataRequiredForLoanExtension_WhenCustomerHasApplicationAcceptedMoreThanOneDayAgo()
        {
            var promiseDate = new Date(DateTime.UtcNow.AddDays(4));
            Customer customer = CustomerBuilder.New().Build();
            var application = ApplicationBuilder.New(customer).WithPromiseDate(promiseDate).Build();
            Drive.Data.Payments.Db.Applications.UpdateByExternalId(ExternalId: application.Id, AcceptedOn: DateTime.UtcNow.AddDays((-10)));

            var query = new CsGetFixedTermLoanExtensionQuoteQuery { ApplicationId = application.Id};            

            var response = Drive.Cs.Queries.Post(query);
           
            Assert.IsTrue(response.Values["ApplicationId"].SingleOrDefault(i => i == application.Id.ToString()) != null);
            Assert.IsTrue(response.Values["SliderMinDays"].SingleOrDefault(i => i == 1.ToString()) != null);
            Assert.IsTrue(response.Values["SliderMaxDays"].SingleOrDefault(i => i == 30.ToString()) != null);
            Assert.IsTrue(response.Values["TotalAmountDueToday"].SingleOrDefault(isDecimal) != null);
            Assert.IsTrue(response.Values["CurrentPrincipleAmount"].SingleOrDefault(isDecimal) != null);
            Assert.IsTrue(response.Values["ExtensionPartPaymentAmount"].SingleOrDefault(isDecimal) != null);
            Assert.IsTrue(response.Values["LoanExtensionFee"].SingleOrDefault(isDecimal) != null);
            Assert.IsTrue(response.Values["IsExtendable"].ElementAt(0)=="true" );

            for( var pos=1;pos<30;pos++ )
            {
                Assert.IsTrue(isDecimal( response.Values["FutureInterestAndFees"].ToArray()[pos]));
                Assert.IsTrue(isDecimal(response.Values["TotalAmountDueOnExtensionDate"].ToArray()[pos]));
                Assert.IsTrue(isDate(response.Values["ExtensionDate"].ToArray()[pos]));
                Assert.IsTrue(isDate(response.Values["ExtensionDate"].ToArray()[pos]));
            }

           
        }

        [Test]
        [AUT(AUT.Uk), JIRA("UK-1351")]
        public void Query_ShouldReturnNoQuotes_WhenNextDueDateIsInMoreThanOneWeek()
        {
            Customer customer = CustomerBuilder.New().Build();
            var application = ApplicationBuilder.New(customer).WithPromiseDate(new Date(DateTime.UtcNow.AddDays(14))).Build();

            var query = new CsGetFixedTermLoanExtensionQuoteQuery { ApplicationId = application.Id };

            var response = Drive.Cs.Queries.Post(query);

            Assert.IsTrue(response.Values["ApplicationId"].SingleOrDefault(i => i == application.Id.ToString()) != null);
            Assert.IsTrue(response.Values["SliderMinDays"].SingleOrDefault(i => i == 1.ToString()) != null);
            Assert.IsTrue(response.Values["SliderMaxDays"].SingleOrDefault(i => i == 30.ToString()) != null);
            Assert.IsTrue(response.Values["TotalAmountDueToday"].SingleOrDefault(isDecimal) != null);
            Assert.IsTrue(response.Values["CurrentPrincipleAmount"].SingleOrDefault(isDecimal) != null);
            Assert.IsTrue(response.Values["ExtensionPartPaymentAmount"].SingleOrDefault(isDecimal) != null);
            Assert.IsTrue(response.Values["LoanExtensionFee"].SingleOrDefault(isDecimal) != null);
            Assert.IsTrue(response.Values["IsExtendable"].ElementAt(0) == "false");
            Assert.IsTrue(response.Values["ErrorMessage"].ElementAt(0) == "DueDateTooFarInFuture");

            Assert.IsTrue(response.Values["FutureInterestAndFees"].ToList().Count==0, "No quote data should be returned");
            Assert.IsTrue(response.Values["TotalAmountDueOnExtensionDate"].ToList().Count==0, "No quote data should be returned");
            Assert.IsTrue(response.Values["ExtensionDate"].ToList().Count==0, "No quote data should be returned");
          
        }

        bool isDecimal(string val)
        {
            decimal test=0;
            return decimal.TryParse(val, out test);
        }

        public bool isDate(string val)
        {
            DateTime test;
            return DateTime.TryParse(val, out test);
        }


    }
}
