using System;
using System.Collections.Generic;
using System.Dynamic;
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
    [Parallelizable(TestScope.All)]
    class CheckpointApplicationElementNotOnBlacklistTests
    {
        private RiskMask _testMask;
        private const string InternationalCodeZa = "+27";

        [FixtureSetUp]
        public void FixtureSetUp()
        {
            _testMask = Config.AUT == AUT.Wb ? RiskMask.TESTBlacklistSME : RiskMask.TESTBlacklist;
        }

        [Test, AUT(AUT.Za, AUT.Wb, AUT.Uk)]
		[JIRA("SME-131", "SME-1363","UK-847")]
        public void ApplicationElementNotOnBlackList_LoanIsAccepted()
        {
            switch (Config.AUT)
            {
                 case AUT.Uk:
            		{
            			var dateOfBirth = Get.GetDoB();
                        var bankAccountNumber = Get.RandomLong(10000000, 99999999).ToString();

                        var customer = CustomerBuilder.New()
							.WithEmployer(_testMask)
							.WithDateOfBirth(dateOfBirth)
							.WithBankAccountNumber(bankAccountNumber)
							.Build();

                        var application = ApplicationBuilder.New(customer).Build();
                        Assert.IsNotNull(application);

                        var riskWorkflows = Drive.Db.GetWorkflowsForApplication(application.Id, RiskWorkflowTypes.MainApplicant);
                        Assert.AreEqual(riskWorkflows.Count, 1, "There should be 1 risk workflow");
                        Assert.Contains(Drive.Db.GetExecutedCheckpointDefinitionNamesForRiskWorkflow(riskWorkflows[0].WorkflowId, RiskCheckpointStatus.Verified), Get.EnumToString(RiskCheckpointDefinitionEnum.ApplicationDataBlacklistCheck));
                    }
                    break;
                 case AUT.Za:
                    {
                        var dateOfBirth = Get.GetDoB();
                        var nationalNumber = Get.GetNationalNumber(dateOfBirth, true);
                        var bankAccountNumber = Get.GetBankAccountNumber();

                        var customer = CustomerBuilder.New()
                            .WithEmployer(_testMask)
                            .WithDateOfBirth(dateOfBirth)
                            .WithNationalNumber(nationalNumber)
                            .WithBankAccountNumber(bankAccountNumber)
                            .Build();

                        var application = ApplicationBuilder.New(customer).Build();
                        Assert.IsNotNull(application);

                        var riskWorkflows = Drive.Db.GetWorkflowsForApplication(application.Id, RiskWorkflowTypes.MainApplicant);
                        Assert.AreEqual(riskWorkflows.Count, 1, "There should be 1 risk workflow");
                        Assert.Contains(Drive.Db.GetExecutedCheckpointDefinitionNamesForRiskWorkflow(riskWorkflows[0].WorkflowId, RiskCheckpointStatus.Verified), Get.EnumToString(RiskCheckpointDefinitionEnum.ApplicationDataBlacklistCheck));
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
        [JIRA("SME-131","SME-1363")]
        public void ApplicationElementIsOnBlackList_LoanIsDeclined()
        {
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

        [Test, AUT(AUT.Za)]
        public void ApplicationElementIsOnBlackList_MobilePhone_LoanIsDeclined()
        {
            var mobilePhoneNumber = Get.GetMobilePhone();
            var customer = CustomerBuilder.New()
                .WithEmployer(_testMask)
                .WithBankAccountNumber(Get.GetBankAccountNumber())
                .WithMobileNumber(mobilePhoneNumber).Build();

            var formattedMobilePhoneNumber = InternationalCodeZa + mobilePhoneNumber.Remove(0, 1);

            var blacklistEntity = new BlackListEntity { MobilePhone = formattedMobilePhoneNumber, ExternalId = Guid.NewGuid() };
            Drive.Db.Blacklist.BlackLists.InsertOnSubmit(blacklistEntity);
            blacklistEntity.Submit();

            ApplicationBuilder.New(customer).WithExpectedDecision(ApplicationDecisionStatus.Declined).Build();
        }

        [Test, AUT(AUT.Uk)]
        [JIRA("UK-847"), Description("Scenario 1: Name on BlackList: Declined")]
        public void ApplicationElementIsOnBlackList_ApplicantName_LoanIsDeclined()
        {
            var blacklistSurname = Get.RandomString(10);
            var db = Drive.Db;
            db.AddSurnameToBlacklist(blacklistSurname);

            var customer = CustomerBuilder.New().WithSurname(blacklistSurname).WithEmployer(_testMask).Build();
            var application = ApplicationBuilder.New(customer).WithExpectedDecision(ApplicationDecisionStatus.Declined).Build();
            var riskWorkflows = Drive.Db.GetWorkflowsForApplication(application.Id, RiskWorkflowTypes.MainApplicant);
            Assert.AreEqual(riskWorkflows.Count, 1, "There should be 1 risk workflow");
            Assert.Contains(Drive.Db.GetExecutedCheckpointDefinitionNamesForRiskWorkflow(riskWorkflows[0].WorkflowId, RiskCheckpointStatus.Failed), Get.EnumToString(RiskCheckpointDefinitionEnum.ApplicationDataBlacklistCheck));
        }

        [Test, AUT(AUT.Za, AUT.Uk)]
        [JIRA("UK-847"), Description("Scenario 1: Name on BlackList: Accepted")]
        public void ApplicationElementIsOnBlackList_ApplicantName_LoanIsApproved()
        {
            var blacklistSurname = Get.RandomString(10);
            var db = Drive.Data.Blacklist.Db.Blacklist;
            db.DeleteAllByLastName(blacklistSurname);

            var customer = CustomerBuilder.New().WithSurname(blacklistSurname).WithEmployer(_testMask).Build();
            var application = ApplicationBuilder.New(customer).WithExpectedDecision(ApplicationDecisionStatus.Accepted).Build();
            var riskWorkflows = Drive.Db.GetWorkflowsForApplication(application.Id, RiskWorkflowTypes.MainApplicant);
            Assert.AreEqual(riskWorkflows.Count, 1, "There should be 1 risk workflow");
            Assert.Contains(Drive.Db.GetExecutedCheckpointDefinitionNamesForRiskWorkflow(riskWorkflows[0].WorkflowId, RiskCheckpointStatus.Verified), Get.EnumToString(RiskCheckpointDefinitionEnum.ApplicationDataBlacklistCheck));
        }

        [Test, AUT(AUT.Uk)]
        [Pending("If anyone finds out WHY this fails tell me:) - inside Blacklist there is: MobilePhone = customerDetails.MobilePhone.Formatted - that could be it. Since i dont have a working local deploy i cant check.")]
        [JIRA("UK-847"), Description("Scenario 3: HomePhone on BlackList: Declined")]
        public void ApplicationElementIsOnBlackList_HomePhone_LoanIsDeclined()
        {
            var blacklistPhone = Get.GetPhone();
            var db = Drive.Data.Blacklist.Db.Blacklist;

            dynamic bl = new ExpandoObject();
            bl.ExternalId = Guid.NewGuid();
            bl.HomePhone = blacklistPhone;
            db.Insert(bl);

            var customer = CustomerBuilder.New().WithPhoneNumber(blacklistPhone).WithEmployer(_testMask).Build();
            var application = ApplicationBuilder.New(customer).WithExpectedDecision(ApplicationDecisionStatus.Declined).Build();
            var riskWorkflows = Drive.Db.GetWorkflowsForApplication(application.Id, RiskWorkflowTypes.MainApplicant);
            Assert.AreEqual(riskWorkflows.Count, 1, "There should be 1 risk workflow");
            Assert.Contains(Drive.Db.GetExecutedCheckpointDefinitionNamesForRiskWorkflow(riskWorkflows[0].WorkflowId, RiskCheckpointStatus.Failed), Get.EnumToString(RiskCheckpointDefinitionEnum.ApplicationDataBlacklistCheck));
        }

        [Test, AUT(AUT.Za, AUT.Uk)]
        [JIRA("UK-847"), Description("Scenario 3: HomePhone on BlackList: Accepted")]
        public void ApplicationElementIsOnBlackList_HomePhone_LoanIsApproved()
        {
            var blacklistPhone = Get.GetPhone();
            var db = Drive.Data.Blacklist.Db.Blacklist;
            db.DeleteAllByHomePhone(blacklistPhone);

            var customer = CustomerBuilder.New().WithPhoneNumber(blacklistPhone).WithEmployer(_testMask).Build();
            var application = ApplicationBuilder.New(customer).WithExpectedDecision(ApplicationDecisionStatus.Accepted).Build();
            var riskWorkflows = Drive.Db.GetWorkflowsForApplication(application.Id, RiskWorkflowTypes.MainApplicant);
            Assert.AreEqual(riskWorkflows.Count, 1, "There should be 1 risk workflow");
            Assert.Contains(Drive.Db.GetExecutedCheckpointDefinitionNamesForRiskWorkflow(riskWorkflows[0].WorkflowId, RiskCheckpointStatus.Verified), Get.EnumToString(RiskCheckpointDefinitionEnum.ApplicationDataBlacklistCheck));
        }

        [Test, AUT(AUT.Uk)]
        [Pending("Same as above")]
        [JIRA("UK-847"), Description("Scenario 4: MobilePhone on BlackList: Declined")]
        public void ApplicationElementIsOnBlackList_UKMobilePhone_LoanIsDeclined()
        {
            var blacklistPhone = Get.GetMobilePhone();
            var db = Drive.Data.Blacklist.Db.Blacklist;

            dynamic bl = new ExpandoObject();
            bl.ExternalId = Guid.NewGuid();
            bl.MobilePhone = blacklistPhone;
            db.Insert(bl);

            var customer = CustomerBuilder.New().WithMobileNumber(blacklistPhone).WithEmployer(_testMask).Build();
            var application = ApplicationBuilder.New(customer).WithExpectedDecision(ApplicationDecisionStatus.Declined).Build();
            var riskWorkflows = Drive.Db.GetWorkflowsForApplication(application.Id, RiskWorkflowTypes.MainApplicant);
            Assert.AreEqual(riskWorkflows.Count, 1, "There should be 1 risk workflow");
            Assert.Contains(Drive.Db.GetExecutedCheckpointDefinitionNamesForRiskWorkflow(riskWorkflows[0].WorkflowId, RiskCheckpointStatus.Failed), Get.EnumToString(RiskCheckpointDefinitionEnum.ApplicationDataBlacklistCheck));
        }

        [Test, AUT(AUT.Za, AUT.Uk)]
        [JIRA("UK-847"), Description("Scenario 4: MobilePhone on BlackList: Accepted")]
        public void ApplicationElementIsOnBlackList_MobilePhone_LoanIsApproved()
        {
            var blacklistPhone = Get.GetMobilePhone();
            var db = Drive.Data.Blacklist.Db.Blacklist;
            db.DeleteAllByMobilePhone(blacklistPhone);

            var customer = CustomerBuilder.New().WithMobileNumber(blacklistPhone).WithEmployer(_testMask).Build();
            var application = ApplicationBuilder.New(customer).WithExpectedDecision(ApplicationDecisionStatus.Accepted).Build();
            var riskWorkflows = Drive.Db.GetWorkflowsForApplication(application.Id, RiskWorkflowTypes.MainApplicant);
            Assert.AreEqual(riskWorkflows.Count, 1, "There should be 1 risk workflow");
            Assert.Contains(Drive.Db.GetExecutedCheckpointDefinitionNamesForRiskWorkflow(riskWorkflows[0].WorkflowId, RiskCheckpointStatus.Verified), Get.EnumToString(RiskCheckpointDefinitionEnum.ApplicationDataBlacklistCheck));
        }

        [Test, AUT(AUT.Za)]
        [Description("Not working for UK since the stupid bank account is constant")]
        public void ApplicationElementIsOnBlackList_BankAccount_LoanIsDeclined()
        {
            var bankAccountNumber = Get.GetBankAccountNumber();
            var db = Drive.Data.Blacklist.Db.Blacklist;

            db.Insert(new BlackListEntity()
            {
                ExternalId = Guid.NewGuid(),
                BankAccount = bankAccountNumber
            });

            var customer = CustomerBuilder.New().WithEmployer(_testMask).WithBankAccountNumber(bankAccountNumber).Build();
            var application = ApplicationBuilder.New(customer).WithExpectedDecision(ApplicationDecisionStatus.Declined).Build();
            var riskWorkflows = Drive.Db.GetWorkflowsForApplication(application.Id, RiskWorkflowTypes.MainApplicant);
            Assert.AreEqual(riskWorkflows.Count, 1, "There should be 1 risk workflow");
            Assert.Contains(
                Drive.Db.GetExecutedCheckpointDefinitionNamesForRiskWorkflow(riskWorkflows[0].WorkflowId,
                                                                             RiskCheckpointStatus.Failed),
                Get.EnumToString(RiskCheckpointDefinitionEnum.ApplicationDataBlacklistCheck));
        }

        [Test, AUT(AUT.Za)]
        [Description("Not working for UK since the stupid bank account is constant")]
        public void ApplicationElementIsOnBlackList_BankAccount_LoanIsApproved()
        {
            var bankAccountNumber = Get.GetBankAccountNumber();
            var db = Drive.Data.Blacklist.Db.Blacklist;
            db.DeleteAllByBankAccount(bankAccountNumber);

            var customer = CustomerBuilder.New().WithEmployer(_testMask).WithBankAccountNumber(bankAccountNumber).Build();
            var application = ApplicationBuilder.New(customer).WithExpectedDecision(ApplicationDecisionStatus.Declined).Build();
            var riskWorkflows = Drive.Db.GetWorkflowsForApplication(application.Id, RiskWorkflowTypes.MainApplicant);
            Assert.AreEqual(riskWorkflows.Count, 1, "There should be 1 risk workflow");
            Assert.Contains(Drive.Db.GetExecutedCheckpointDefinitionNamesForRiskWorkflow(riskWorkflows[0].WorkflowId, RiskCheckpointStatus.Failed),
                            Get.EnumToString(RiskCheckpointDefinitionEnum.ApplicationDataBlacklistCheck));
        }

        
        #region SME specific

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-1170", "SME-1363")]
        public void CheckpointApplicationGuarantorNotOnBlacklistAccept()
        {
            var mainApplicant = CustomerBuilder.New().WithMiddleName(RiskMask.TESTNoCheck).Build();
            var organisation = OrganisationBuilder.New(mainApplicant).Build();

            var guarantorList = new List<CustomerBuilder>
                                    {
                                        CustomerBuilder.New().WithMiddleName(_testMask),
                                    };

            var applicationBuilder = ApplicationBuilder.New(mainApplicant, organisation) as BusinessApplicationBuilder;
            applicationBuilder.WithGuarantors(guarantorList);
            var application = applicationBuilder.Build();

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
                    (RiskWorkflowTypes)p.WorkflowType == RiskWorkflowTypes.Guarantor));

            var guarantorRiskWorkflow = Drive.Db.GetWorkflowsForApplication(application.Id, RiskWorkflowTypes.Guarantor);
            Assert.AreEqual(guarantorRiskWorkflow.Count, 1, "There should be 1 risk workflow");
            Do.Until(() => Drive.Db.Risk.WorkflowCheckpoints.Any(p => p.RiskWorkflowId == guarantorRiskWorkflow[0].RiskWorkflowId && p.CheckpointStatus != 0));

            Assert.Contains(Drive.Db.GetExecutedCheckpointDefinitionNamesForRiskWorkflow(guarantorRiskWorkflow[0].WorkflowId, RiskCheckpointStatus.Verified), Get.EnumToString(RiskCheckpointDefinitionEnum.ApplicationDataBlacklistCheck));
        }

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-1170", "SME-1363")]
        public void CheckpointApplicationGuarantorOnBlacklistDecline()
        {
            //add a specific name to blacklist
            var blacklistSurname = Get.RandomString(10);
            var db = Drive.Db;
            db.AddSurnameToBlacklist(blacklistSurname);

            //use that name for guarantor
            var mainApplicant = CustomerBuilder.New().WithMiddleName(RiskMask.TESTNoCheck).Build();
            var organisation = OrganisationBuilder.New(mainApplicant).Build();

            var guarantorList = new List<CustomerBuilder>
                                    {
                                        CustomerBuilder.New().WithSurname(blacklistSurname).WithMiddleName(_testMask),
                                    };
            var applicationBuilder = ApplicationBuilder.New(mainApplicant, organisation) as BusinessApplicationBuilder;
            applicationBuilder.WithGuarantors(guarantorList);
            var application = applicationBuilder.Build();

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
                    (RiskWorkflowTypes)p.WorkflowType == RiskWorkflowTypes.Guarantor));
            var guarantorRiskWorkflow = Drive.Db.GetWorkflowsForApplication(application.Id, RiskWorkflowTypes.Guarantor);
            Assert.AreEqual(guarantorRiskWorkflow.Count, 1, "There should be 1 risk workflow");
            Do.Until(() => Drive.Db.Risk.WorkflowCheckpoints.Any(p => p.RiskWorkflowId == guarantorRiskWorkflow[0].RiskWorkflowId && p.CheckpointStatus != 0));

            Assert.Contains(Drive.Db.GetExecutedCheckpointDefinitionNamesForRiskWorkflow(guarantorRiskWorkflow[0].WorkflowId, RiskCheckpointStatus.Failed), Get.EnumToString(RiskCheckpointDefinitionEnum.ApplicationDataBlacklistCheck));
        }

        #endregion
    }
}
