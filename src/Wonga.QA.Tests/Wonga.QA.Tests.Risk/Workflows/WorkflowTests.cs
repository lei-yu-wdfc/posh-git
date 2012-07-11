using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db;
using Wonga.QA.Framework.Db.Extensions;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Risk.Workflows
{
	[TestFixture, Category(TestCategories.CoreTest)]
    [AUT(AUT.Ca, AUT.Za)]
	public partial class WorkflowTests
	{
		[FixtureSetUp]
		public void FixtureSetUp()
		{
			switch (Config.AUT)
			{
				case (AUT.Za):
					{
						var hyphenAhvServiceEnabled = Drive.Data.Ops.GetServiceConfiguration<bool?>("Mocks.HyphenAHVWebServiceEnabled");
						var iovationMockEnabled = Drive.Data.Ops.GetServiceConfiguration<bool?>("Mocks.IovationEnabled");

						if( hyphenAhvServiceEnabled == null || hyphenAhvServiceEnabled == false || iovationMockEnabled == null || iovationMockEnabled == false)
						{
							Assert.Inconclusive("Test could not be ran due to service configuration");
						}
					}
					break;

				default:
					{
						return;
					}
			}
		}

		[Test, AUT(AUT.Za, AUT.Ca), Pending]
		public void WorkflowL0SingleWorkflowUsed()
		{
			var customer = CustomerBuilder.New().WithEmployer("Wonga").Build();
			var application =
				ApplicationBuilder.New(customer)
				.WithExpectedDecision(ApplicationDecisionStatus.Declined)
				.Build();

			var workflows = Drive.Db.GetWorkflowsForApplication(application.Id);

			Assert.AreEqual(1, workflows.Count);
		}

		[Test, AUT(AUT.Za), Pending]
		public void WorkflowApplicationL0Accepted()
		{
			var customer = BuildCustomerToBeAccepted();

			ApplicationBuilder.New(customer).WithExpectedDecision(ApplicationDecisionStatus.ReadyToSign).Build();
		}

		[Test, AUT(AUT.Za), Pending]
		public void WorkflowApplicationLnAccepted()
		{
			var customer = BuildCustomerToBeAccepted();
			customer.UpdateEmployer(RiskMask.TESTCustomerIsEmployed.ToString());
			ApplicationBuilder.New(customer).Build().RepayOnDueDate();

			customer.UpdateEmployer("Wonga");

			ApplicationBuilder.New(customer).Build();
		}

		#region Helpers

		private Customer BuildCustomerToBeAccepted()
		{
			switch (Config.AUT)
			{
				case(AUT.Za):
				{
					return
						CustomerBuilder.New()
						.WithEmployer("Wonga")
						.WithForename("ANITHA")
						.WithSurname("ESSACK")
						.WithDateOfBirth(new Date(new DateTime(1957, 12, 19)))
						.WithNationalNumber("5712190106083")
						.WithMobileNumber(Get.GetMobilePhone())
							.WithBankAccountNumber(Get.GetBankAccountNumber().ToString())
						.WithPostcodeInAddress(Get.GetPostcode())
						.Build();
				}
				default:
				{
					throw new NotImplementedException(Config.AUT.ToString());
				}
			}
			
		}

		#endregion
	}
}
