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
	[AUT(AUT.Za)]
	public partial class WorkflowTests
	{
		private Dictionary<string, string> _originalServiceConfiguration = new Dictionary<string, string>();
		
		[FixtureSetUp]
		public void FixtureSetUp()
		{
			switch (Config.AUT)
			{
				case (AUT.Za):
					{
						var mock = Drive.Db.GetServiceConfiguration("Mocks.HyphenAHVWebServiceEnabled");

						_originalServiceConfiguration.Add("Mocks.HyphenAHVWebServiceEnabled", mock.Value);
						Drive.Db.SetServiceConfiguration("Mocks.HyphenAHVWebServiceEnabled", "false");
					}
					break;

				default:
					{
						return;
					}
			}
		}

		[FixtureTearDown]
		public void FixtureTearDown()
		{
			Drive.Db.SetServiceConfigurations(_originalServiceConfiguration);
		}

		[Test, AUT(AUT.Za), Explicit]
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

			ApplicationBuilder.New(customer).WithIovationBlackBox("Allow").WithExpectedDecision(ApplicationDecisionStatus.ReadyToSign).Build();
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
							.WithBankAccountNumber(Get.GetBankAccountNumber())
							.WithPostcodeInAddress(Get.GetPostcode())
							.Build();
					}
					break;

				default:
					{
						throw new NotImplementedException(Config.AUT.ToString());
					}
			}
			
		}

		#endregion
	}
}
