using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db.Blacklist;
using Wonga.QA.Tests.Core;
using Wonga.QA.Framework.Db;

namespace Wonga.QA.Tests.Risk.Checkpoints
{
	[Parallelizable(TestScope.All), AUT(AUT.Za,AUT.Wb)]
	class CheckpointApplicationElementNotOnBlacklistTests
	{
        //private const RiskMask TestMask = RiskMask.TESTBlacklist;
	    private RiskMask _testMask;
		private string _internationalCode;
		private const string InternationalCodeZa = "+27";

		[FixtureSetUp]
		public void FixtureSetUp()
		{
			switch (Config.AUT)
			{
				case AUT.Za:
					{
						_internationalCode = InternationalCodeZa;
					    _testMask = RiskMask.TESTBlacklist;
					}
					break;
                case AUT.Wb:
                    {
                        //_internationalCode = InternationalCodeZa;
                        _testMask = RiskMask.TESTBlacklistSME;
                    }
                    break;

				default:
					{
						throw new NotImplementedException(Config.AUT.ToString());
					}
			}
		}

		[Test, AUT(AUT.Za,AUT.Wb)]
        [JIRA("SME-131")]
		public void CheckpointApplicationElementNotOnBlacklistAccept()
		{
            switch (Config.AUT)
            {
                case AUT.Ca:
                    {
                        var customer = CustomerBuilder.New().WithEmployer(_testMask).WithBankAccountNumber(Get.GetBankAccountNumber()).Build();
                        var application = ApplicationBuilder.New(customer).Build();
                        Assert.IsNotNull(application);

                    }
                    break;
                case AUT.Wb:
                    {
                        var mainApplicant = CustomerBuilder.New().WithMiddleName(_testMask).Build();
                        var organisation = OrganisationBuilder.New(mainApplicant).Build();
                        var application = ApplicationBuilder.New(mainApplicant, organisation).WithExpectedDecision(ApplicationDecisionStatus.Accepted).Build();
                        Assert.IsNotNull(application);

                        var riskWorkflows = Application.GetWorkflowsForApplication(application.Id, RiskWorkflowTypes.MainApplicant);
                        Assert.AreEqual(riskWorkflows.Count, 1, "There should be 1 risk workflow");
                        Assert.Contains(Application.GetExecutedCheckpointDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId, RiskCheckpointStatus.Verified), Get.EnumToString(RiskCheckpointDefinitionEnum.ApplicationDataBlacklistCheck));
                    }
                    break;
            }
		}

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-131")]
        public void CheckpointApplicationElementOnBlacklistDeclined()
        {
            //cannot use randomemail since max lenght is 5
            const string blacklistEmail = "i_am_blacklisted@gmail.com";
            var db = Drive.Db;
            db.AddEmailToBlacklist(blacklistEmail);

            var mainApplicant = CustomerBuilder.New().WithMiddleName(_testMask).WithEmailAddress(blacklistEmail).Build();
            var organisation = OrganisationBuilder.New(mainApplicant).Build();
            var application = ApplicationBuilder.New(mainApplicant, organisation).WithExpectedDecision(ApplicationDecisionStatus.Declined).Build();
            Assert.IsNotNull(application);

            var riskWorkflows = Application.GetWorkflowsForApplication(application.Id, RiskWorkflowTypes.MainApplicant);
            Assert.AreEqual(riskWorkflows.Count, 1, "There should be 1 risk workflow");
            Assert.Contains(Application.GetExecutedCheckpointDefinitionsForRiskWorkflow(riskWorkflows[0].WorkflowId, RiskCheckpointStatus.Failed), Get.EnumToString(RiskCheckpointDefinitionEnum.ApplicationDataBlacklistCheck));
        }

		[Test, AUT(AUT.Za)]
		public void CheckpointApplicationElementNotOnBlacklistMobilePhoneNumberPresent()
		{
			var mobilePhoneNumber = Get.GetMobilePhone();
			var customer = CustomerBuilder.New()
				.WithEmployer(_testMask)
				.WithBankAccountNumber(Get.GetBankAccountNumber())
				.WithMobileNumber(mobilePhoneNumber).Build();
			
			var formattedMobilePhoneNumber = _internationalCode + mobilePhoneNumber.Remove(0,1);

			var blacklistEntity = new BlackListEntity{MobilePhone = formattedMobilePhoneNumber, ExternalId =  Guid.NewGuid()};
			Drive.Db.Blacklist.BlackLists.InsertOnSubmit(blacklistEntity);
			blacklistEntity.Submit();

            ApplicationBuilder.New(customer).WithExpectedDecision(ApplicationDecisionStatus.Declined).Build();
		}

		[Test, AUT(AUT.Za)]
		public void CheckpointApplicationElementNotOnBlacklistBankAccountPresent()
		{
			var bankAccountNumber = Get.GetBankAccountNumber();
			var customer = CustomerBuilder.New().WithEmployer(_testMask).WithBankAccountNumber(bankAccountNumber).Build();
			var blacklistEntity = new BlackListEntity { BankAccount = bankAccountNumber.ToString(), ExternalId = Guid.NewGuid() };
			Drive.Db.Blacklist.BlackLists.InsertOnSubmit(blacklistEntity);
			blacklistEntity.Submit();

            ApplicationBuilder.New(customer).WithExpectedDecision(ApplicationDecisionStatus.Declined).Build();
		}
	}
}
