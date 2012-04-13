using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Comms
{
	class CollectionsChaseEmailTest
	{
		[FixtureSetUp]
		public void FixtureSetUp()
		{
			
		}

		[FixtureTearDown]
		public void FixtureTearDown()
		{
			
		}

		[Test, JIRA("QA-206"), Pending]
		public void A2EmailIsSent()
		{
			var application = ApplicationBuilder.New(CustomerBuilder.New().Build()).Build();

			application.PutApplicationIntoArrears(1);
		}

		[Test, JIRA("QA-206"), Pending]
		public void A3EmailIsSent()
		{

		}

		[Test, JIRA("QA-206"), Pending]
		public void A4EmailIsSent()
		{

		}

		[Test, JIRA("QA-206"), Pending]
		public void A5EmailIsSent()
		{

		}

		[Test, JIRA("QA-206"), Pending]
		public void A6mailIsSent()
		{

		}

		[Test, JIRA("QA-206"), Pending]
		public void A7EmailIsSent()
		{

		}

		[Test, JIRA("QA-206"), Pending]
		public void A8EmailIsSent()
		{

		}

		[Test, JIRA("QA-206"), Pending]
		public void WhenInHardshipEmailIsNotSent()
		{

		}

		[Test, JIRA("QA-206"), Pending]
		public void WhenInDisputeEmailIsNotSent()
		{

		}

		#region Helpers

		#endregion
	}
}
