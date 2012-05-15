using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Payments.Queries
{
	[TestFixture]
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
	}
}
