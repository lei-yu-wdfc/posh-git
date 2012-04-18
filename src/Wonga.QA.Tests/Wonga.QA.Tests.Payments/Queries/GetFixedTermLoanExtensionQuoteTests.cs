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
        public void NewLoanAbleToExtend()
        {
            var accountId = Guid.NewGuid();
            var bankAccountId = Guid.NewGuid();
            var paymentCardId = Guid.NewGuid();
            var appId = Guid.NewGuid();
            const decimal trustRating = 400.00M;

            var setupData = new ExtendLoanFunctions();

            setupData.NewLoanAbleToExtendSetup(appId, paymentCardId, bankAccountId, accountId, trustRating);

            var response = Drive.Api.Queries.Post(new GetFixedTermLoanExtensionQuoteUkQuery() {ApplicationId = appId});
            Assert.AreEqual("109.86", response.Values["TotalAmountDueToday"].Single(), "TotalAmountDueToday incorrect");
        }
    }
}
