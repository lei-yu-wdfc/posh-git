using System;
using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api.Requests.Payments.Queries.Wb.Uk;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Payments
{
	[TestFixture]
    [Parallelizable(TestScope.All)]
	public class GetBusinessFixedInstallmentLoanOfferTests
	{
		[Test, AUT(AUT.Wb), JIRA("SME-889")]
		public void GetBusinessFixedInstallmentLoanOffer()
		{
            //This returns stuff from payments.Products
			var response = Drive.Api.Queries.Post(new GetBusinessFixedInstallmentLoanOfferWbUkQuery());

			Assert.IsNotNull(response);
			Assert.AreEqual("3000.00", response.Values["AmountMin"].SingleOrDefault(),"Expected AmountMin is incorrect.");
			Assert.AreEqual("10000.00", response.Values["AmountMax"].SingleOrDefault(), "Expected AmountMax is incorrect.");
			Assert.AreEqual("9000", response.Values["AmountDefault"].SingleOrDefault(), "Expected AmountDefault is incorrect.");
			Assert.AreEqual("1", response.Values["TermMin"].SingleOrDefault(), "Expected TermMin is incorrect."); 
			Assert.AreEqual("52", response.Values["TermMax"].SingleOrDefault(), "Expected TermMax is incorrect.");
			Assert.AreEqual("16", response.Values["TermDefault"].SingleOrDefault(), "Expected TermDefault is incorrect");
		}

		[Test, AUT(AUT.Wb), JIRA("SME-889")]
		public void GetBusinessFixedInstallmentLoanOfferAccountIsNull()
		{
			var response = Drive.Api.Queries.Post(new GetBusinessFixedInstallmentLoanOfferWbUkQuery{ AccountId = Guid.NewGuid()});

			Assert.IsNotNull(response);
			Assert.AreEqual("3000.00", response.Values["AmountMin"].SingleOrDefault(), "Expected AmountMin is incorrect.");
			Assert.AreEqual("10000.00", response.Values["AmountMax"].SingleOrDefault(), "Expected AmountMax is incorrect.");
			Assert.AreEqual("9000", response.Values["AmountDefault"].SingleOrDefault(), "Expected AmountDefault is incorrect.");
			Assert.AreEqual("1", response.Values["TermMin"].SingleOrDefault(), "Expected TermMin is incorrect.");
			Assert.AreEqual("52", response.Values["TermMax"].SingleOrDefault(), "Expected TermMax is incorrect.");
			Assert.AreEqual("16", response.Values["TermDefault"].SingleOrDefault(), "Expected TermDefault is incorrect");
		}
		
	}
}
