using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Api.Exceptions;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db.Ops;
using Wonga.QA.Framework.Msmq;
using Wonga.QA.Tests.Core;
using System.Threading;

namespace Wonga.QA.Tests.Payments.Command
{
	[TestFixture, Parallelizable(TestScope.All)]
	public class IncomingPartnerPaymentQueryTest
	{
		[FixtureSetUp]
		public void FixtureSetUp()
		{

		}

		[FixtureTearDown]
		public void FixtureTearDown()
		{

		}

		[Test, AUT(AUT.Za), JIRA("ZA-2570")]
		public void GetIncomingPartnerPaymentRequestParameters_Expect_Success()
		{
			//Arrange
			var query = new GetIncomingPartnerPaymentRequestParametersZaQuery();

			//Act
			var response = Drive.Api.Queries.Post(query);

			//Assert
			Assert.IsNotNull(response);
			Assert.IsNotEmpty(response.Values["SafeKey"].Single());
			Assert.IsNotEmpty(response.Values["ReceiptURL"].Single());
			Assert.IsNotEmpty(response.Values["FailURL"].Single());
			Assert.AreEqual("Auth", response.Values["TransactionType"].Single());
			Assert.AreEqual("ZAR", response.Values["CurrencyCode"].Single());
		}

	}
}