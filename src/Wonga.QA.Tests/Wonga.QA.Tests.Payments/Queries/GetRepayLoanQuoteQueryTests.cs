using System;
using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;
using Wonga.QA.Tests.Payments.Helpers;

namespace Wonga.QA.Tests.Payments.Queries
{
    [TestFixture]
    public class GetRepayLoanQuoteQueryTests
    {

        [Test, AUT(AUT.Uk)]
        public void RepayEarlyOnLoanStartDate()
        {
            const int dueInDays = 10;
            var promiseDate = new Date(DateTime.UtcNow.AddDays(dueInDays));
            var accountId = Guid.NewGuid();
            var bankAccountId = Guid.NewGuid();
            var paymentCardId = Guid.NewGuid();
            var appId = Guid.NewGuid();
            const decimal trustRating = 400.00M;

            var setupData = new RepayLoanFunctions();
            setupData.RepayEarlyOnLoanStartDate(appId, paymentCardId, bankAccountId, accountId, trustRating, dueInDays);

            //Call Api Query
            var response = Drive.Api.Queries.Post(new GetRepayLoanQuoteUkQuery() { ApplicationId = appId });
            var minRepayAmount = Drive.Db.Ops.ServiceConfigurations.Single(a => a.Key == "Payments.RepayLoanMinAmount").Value;

            Assert.AreEqual(appId.ToString(), response.Values["ApplicationId"].Single(), "ApplicationId incorrect");
            Assert.AreEqual(minRepayAmount, response.Values["SliderMinAmount"].Single(), "SliderMinAmount incorrect");
            Assert.AreEqual("105.50", response.Values["SliderMaxAmount"].Single(), "SliderMaxAmount incorrect");
            Assert.AreEqual("10", response.Values["DaysToDueDate"].Single(), "DaysToDueDate incorrect");
            
            // Check array
            Assert.AreEqual(110.41M,decimal.Parse(response.Values["Amount"].ToArray()[0]));

            

        }
    }
}
