using System;
using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Payments
{
    class GetInstallmentQuoteTest
    {
        [Test, AUT(AUT.Uk), JIRA("UK-1111"), Pending("Commented out until query is added to the API")]
        public void GetInstallmentQuoteMethod()
        {
            //const decimal totalValue = 300.0m;
            //const string cust = "5C09C656-721A-4CDD-8EF8-410CC5343DE3";

            //ApiResponse parm = Drive.Api.Queries.Post(new GetInstallmentQuoteQuery {MerchantId = cust, TotalValue = totalValue, PromoCodeId = null});

            //Assert.IsNotNull(parm);
            //Assert.AreEqual(20.34m, Decimal.Parse(parm.Values["TransactionFee"].Single()));
            //Assert.AreEqual(100.0m, Decimal.Parse(parm.Values["MonthlyPaymentAmount"].Single()));
        }

        [Test, AUT(AUT.Uk), JIRA("UK-111"), Pending("Commented out until query is added to the API")]
        public void GetInstallmentQuoteHandlerWithTotalValueLessOrEqualsZeroReturnsError()
        {
            //const string guidId = "5C09C656-721A-4CDD-8EF8-410CC5343DE3";
            //const decimal totalValue = -100.0m;

            //try
            //{
            //    ApiResponse parm = Drive.Api.Queries.Post(new GetInstallmentQuoteQuery {MerchantId = guidId, TotalValue = totalValue, PromoCodeId = null});
            //    Assert.AreEqual(10.0m, Decimal.Parse(parm.Values["TransactionFee"].Single()));
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e);
            //}
        }
    }
}
