using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Api.Requests.Payments.Queries;
using Wonga.QA.Framework.Api.Requests.Payments.Queries.Uk;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Payments.Queries
{
	[TestFixture]
	[Parallelizable(TestScope.All)]
	public class GetFixedTermLoanCalculationQueryTests
	{
		public const int FixedTermLoanProductId = 1;

		[Test, AUT(AUT.Ca), JIRA("CA-1843")]
		[Row(11)]
		[Row(45)]
		[Row(31)]
		public void GetFixedTermLoanCalculationQuery_ShouldReturnTermBetweenMinAndMaxProductTerm_WhenNoPromoCodeIsSpecified(int term)
		{
			var product = Drive.Data.Payments.Db.Products.FindByProductId(FixedTermLoanProductId);
			
			var	res = Drive.Api.Queries.Post(new GetFixedTermLoanCalculationQuery
			   	                            	{
			   	                            		Term = term,
													LoanAmount = 123.45
			   	                            	});

			int actualTerm = Convert.ToInt32(res.Values["Term"].First());
			
			Assert.GreaterThanOrEqualTo(product.TermMax, actualTerm);
			Assert.LessThanOrEqualTo(product.TermMin, actualTerm);
		}

        [Test, AUT(AUT.Uk), Owner(Owner.MohammadRashid), JIRA("UKWEB-1125"), Pending("Pending on pushing issue8 branch of Payments/Marketing/Ops to master")]
        public void GetFixedTermLoanCalculation_WhenTransmissionFeeDiscountIsSupplied()
        {
            var product = Drive.Data.Payments.Db.Products.FindByProductId(FixedTermLoanProductId);

            var res = Drive.Api.Queries.Post(new GetFixedTermLoanCalculationQuery
            {
                Term = 2,
                LoanAmount = 123.45,
                TransmissionFeeDiscount = 50
            });

            decimal transmissionFee = Convert.ToDecimal(res.Values["TransmissionFee"].First());
            Assert.AreEqual(transmissionFee, (decimal)2.75);
        }

        [Test, AUT(AUT.Uk), Owner(Owner.MohammadRashid), JIRA("UKWEB-1123"), Pending("Pending on pushing issue8 branch of Payments/Marketing/Ops to master")]
        public void GetFixedTermLoanOffer_WhenTransmissionFeeDiscountIsSupplied()
        {
            var res = Drive.Api.Queries.Post(new GetFixedTermLoanOfferUkQuery
            {
                AccountId = Guid.NewGuid(),
                TransmissionFeeDiscount = 50
            });

            decimal transmissionFee = Convert.ToDecimal(res.Values["TransmissionFee"].First());
            Assert.AreEqual(transmissionFee, (decimal)2.75);
        }

	}
}
