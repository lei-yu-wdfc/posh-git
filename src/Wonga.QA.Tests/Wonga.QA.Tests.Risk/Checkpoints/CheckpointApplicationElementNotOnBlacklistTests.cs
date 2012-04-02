using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Data.Enums.Risk;
using Wonga.QA.Framework.Db.Blacklist;
using Wonga.QA.Framework.Db.Extensions;
using Wonga.QA.Tests.Core;
using Wonga.QA.Framework.Db;

namespace Wonga.QA.Tests.Risk.Checkpoints
{
    [Parallelizable(TestScope.All), AUT(AUT.Za, AUT.Wb)]
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

        [Test, AUT(AUT.Za, AUT.Wb)]
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

                        var riskWorkflows = Drive.Db.GetWorkflowsForApplication(application.Id, RiskWorkflowTypes.MainApplicant);
                        Assert.AreEqual(riskWorkflows.Count, 1, "There should be 1 risk workflow");
						Assert.Contains(Drive.Db.GetExecutedCheckpointDefinitionNamesForRiskWorkflow(riskWorkflows[0].WorkflowId, RiskCheckpointStatus.Verified), Get.EnumToString(RiskCheckpointDefinitionEnum.ApplicationDataBlacklistCheck));
                    }
                    break;
            }
        }

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-131")]
        public void CheckpointApplicationElementOnBlacklistDeclined()
        {
            //cannot use randomemail since max lenght is 5
            var blacklistSurname = Get.RandomString(10);
            var db = Drive.Db;
            db.AddSurnameToBlacklist(blacklistSurname);

            var mainApplicant = CustomerBuilder.New().WithMiddleName(_testMask).WithSurname(blacklistSurname).Build();
            var organisation = OrganisationBuilder.New(mainApplicant).Build();
            var application = ApplicationBuilder.New(mainApplicant, organisation).WithExpectedDecision(ApplicationDecisionStatus.Declined).Build();
            Assert.IsNotNull(application);

            var riskWorkflows = Drive.Db.GetWorkflowsForApplication(application.Id, RiskWorkflowTypes.MainApplicant);
            Assert.AreEqual(riskWorkflows.Count, 1, "There should be 1 risk workflow");
			Assert.Contains(Drive.Db.GetExecutedCheckpointDefinitionNamesForRiskWorkflow(riskWorkflows[0].WorkflowId, RiskCheckpointStatus.Failed), Get.EnumToString(RiskCheckpointDefinitionEnum.ApplicationDataBlacklistCheck));
        }

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-1170")]
        public void CheckpointApplicationGuarantorNotOnBlacklistAccept()
        {
            var mainApplicant = CustomerBuilder.New().WithMiddleName(RiskMask.TESTNoCheck).Build();
            var organisation = OrganisationBuilder.New(mainApplicant).Build();

            var guarantorList = new List<Customer>
                                    {
                                        new Customer(Guid.NewGuid(), Get.RandomEmail(), Get.RandomString(8), Get.RandomString(8), Get.GetDoB(),
                                                     Get.GetMobilePhone())
                                            {
                                                MiddleName = _testMask.ToString(),
                                            }
                                    };

            var applicationBuilder = ApplicationBuilder.New(mainApplicant, organisation) as BusinessApplicationBuilder;
            applicationBuilder.WithGuarantors(guarantorList);
            var application = applicationBuilder.Build();

            applicationBuilder.BuildGuarantors();
            applicationBuilder.SignApplicationForSecondaryDirectors();

            Assert.IsNotNull(application);

            var riskDb = Drive.Db.Risk;
            var riskApplicationEntity = Do.Until(() => riskDb.RiskApplications.SingleOrDefault(p => p.ApplicationId == application.Id));
            Assert.IsNotNull(riskApplicationEntity, "Risk application should exist");

            var riskAccountEntity = Do.Until(() => riskDb.RiskAccounts.SingleOrDefault(p => p.AccountId == riskApplicationEntity.AccountId));
            Assert.IsNotNull(riskAccountEntity, "Risk account should exist");

            var socialDetailsEntity = Do.Until(() => riskDb.SocialDetails.SingleOrDefault(p => p.AccountId == riskApplicationEntity.AccountId));
            Assert.IsNotNull(socialDetailsEntity, "Risk Social details should exist");

            //Wait for the Risk data - this will be externalized somewhere 

            Do.Until(
                () =>
                Drive.Db.Risk.RiskWorkflows.Any(
                    p =>
                    p.ApplicationId == application.Id &&
                    (RiskWorkflowTypes) p.WorkflowType == RiskWorkflowTypes.Guarantor));

            var guarantorRiskWorkflow = Drive.Db.GetWorkflowsForApplication(application.Id, RiskWorkflowTypes.Guarantor);
            Assert.AreEqual(guarantorRiskWorkflow.Count, 1, "There should be 1 risk workflow");
            Do.Until(() => Drive.Db.Risk.WorkflowCheckpoints.Any(p => p.RiskWorkflowId == guarantorRiskWorkflow[0].RiskWorkflowId && p.CheckpointStatus != 0));

			Assert.Contains(Drive.Db.GetExecutedCheckpointDefinitionNamesForRiskWorkflow(guarantorRiskWorkflow[0].WorkflowId, RiskCheckpointStatus.Verified), Get.EnumToString(RiskCheckpointDefinitionEnum.ApplicationDataBlacklistCheck));
        }

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-1170")]
        public void CheckpointApplicationGuarantorOnBlacklistDecline()
        {
            //add a specific name to blacklist
            var blacklistSurname = Get.RandomString(10);
            var db = Drive.Db;
            db.AddSurnameToBlacklist(blacklistSurname);

            //use that name for guarantor
            var mainApplicant = CustomerBuilder.New().WithMiddleName(RiskMask.TESTNoCheck).Build();
            var organisation = OrganisationBuilder.New(mainApplicant).Build();

            var guarantorList = new List<Customer>
                                    {
                                        new Customer(Guid.NewGuid(), Get.RandomEmail(), Get.RandomString(8), blacklistSurname, Get.GetDoB(),
                                                     Get.GetMobilePhone())
                                            {
                                                MiddleName = _testMask.ToString(),
                                            }
                                    };

            var applicationBuilder = ApplicationBuilder.New(mainApplicant, organisation) as BusinessApplicationBuilder;
            applicationBuilder.WithGuarantors(guarantorList);
            var application = applicationBuilder.Build();

            applicationBuilder.BuildGuarantors();
            applicationBuilder.SignApplicationForSecondaryDirectors();

            Assert.IsNotNull(application);

            var riskDb = Drive.Db.Risk;
            var riskApplicationEntity = Do.Until(() => riskDb.RiskApplications.SingleOrDefault(p => p.ApplicationId == application.Id));
            Assert.IsNotNull(riskApplicationEntity, "Risk application should exist");

            var riskAccountEntity = Do.Until(() => riskDb.RiskAccounts.SingleOrDefault(p => p.AccountId == riskApplicationEntity.AccountId));
            Assert.IsNotNull(riskAccountEntity, "Risk account should exist");

            var socialDetailsEntity = Do.Until(() => riskDb.SocialDetails.SingleOrDefault(p => p.AccountId == riskApplicationEntity.AccountId));
            Assert.IsNotNull(socialDetailsEntity, "Risk Social details should exist");

            Do.Until(
                () =>
                Drive.Db.Risk.RiskWorkflows.Any(
                    p =>
                    p.ApplicationId == application.Id &&
                    (RiskWorkflowTypes) p.WorkflowType == RiskWorkflowTypes.Guarantor));
            var guarantorRiskWorkflow = Drive.Db.GetWorkflowsForApplication(application.Id, RiskWorkflowTypes.Guarantor);
            Assert.AreEqual(guarantorRiskWorkflow.Count, 1, "There should be 1 risk workflow");
            Do.Until(() => Drive.Db.Risk.WorkflowCheckpoints.Any(p => p.RiskWorkflowId == guarantorRiskWorkflow[0].RiskWorkflowId && p.CheckpointStatus != 0));

			Assert.Contains(Drive.Db.GetExecutedCheckpointDefinitionNamesForRiskWorkflow(guarantorRiskWorkflow[0].WorkflowId, RiskCheckpointStatus.Failed), Get.EnumToString(RiskCheckpointDefinitionEnum.ApplicationDataBlacklistCheck));
        }

        [Test, AUT(AUT.Za)]
        public void CheckpointApplicationElementNotOnBlacklistMobilePhoneNumberPresent()
        {
            var mobilePhoneNumber = Get.GetMobilePhone();
            var customer = CustomerBuilder.New()
                .WithEmployer(_testMask)
                .WithBankAccountNumber(Get.GetBankAccountNumber())
                .WithMobileNumber(mobilePhoneNumber).Build();

            var formattedMobilePhoneNumber = _internationalCode + mobilePhoneNumber.Remove(0, 1);

            var blacklistEntity = new BlackListEntity { MobilePhone = formattedMobilePhoneNumber, ExternalId = Guid.NewGuid() };
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
