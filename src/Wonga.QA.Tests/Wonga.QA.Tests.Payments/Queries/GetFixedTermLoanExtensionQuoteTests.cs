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
    public class GetFixedTermLoanExtensionQuoteTests
    {
        [Test]
        public void TenDayLoanQuoteOnDayFiveToExtendForTenDays()
        {
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
        }
    }
}
