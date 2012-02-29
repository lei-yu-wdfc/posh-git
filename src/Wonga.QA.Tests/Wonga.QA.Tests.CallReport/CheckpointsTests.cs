using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.CallReport
{
    [Parallelizable(TestScope.All)]
    public class CheckpointsTests
    {
        [Test, AUT(AUT.Wb)]
        [JIRA("SME-575"), Description("CallReport -> This test creates a loan for the unknown customer that is alive and with no consumer bureau data, then checks the risk checkpoint")]
        public void TestCallReportUnknownApplicant_LoanIsApproved()
        {
            const String forename = "unknown";
            const String surname = "customer";

            var customer = CustomerBuilder.New().WithMiddleName(RiskMiddlenameMask.TESTApplicantIsNotDeceased.ToString()).WithForename(forename).WithSurname(surname).Build();
            var organization = OrganisationBuilder.New().WithPrimaryApplicant(customer).Build();
            var application = ApplicationBuilder.New(customer, organization).Build();
            Assert.IsNotNull(application);

            var riskDb = Driver.Db.Risk;
            var riskApplicationEntity = Do.Until(() => riskDb.RiskApplications.SingleOrDefault(p => p.ApplicationId == application.Id));
            Assert.IsNotNull(riskApplicationEntity, "Risk application should exist");

            var riskAccountEntity = Do.Until(() => riskDb.RiskAccounts.SingleOrDefault(p => p.AccountId == riskApplicationEntity.AccountId));
            Assert.IsNotNull(riskAccountEntity, "Risk account should exist");

            var socialDetailsEntity = Do.Until(() => riskDb.SocialDetails.SingleOrDefault(p => p.AccountId == riskApplicationEntity.AccountId));
            Assert.IsNotNull(socialDetailsEntity, "Social details should exist");

            Assert.Contains(Application.GetExecutedCheckpointDefinitions(application.Id, CheckpointStatus.Verified), Data.EnumToString(CheckpointDefinitionEnum.ApplicantIsAlive));
            AssertCreditBureauUsed(application, CreditBureauEnum.CallReport);
        }

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-575"), Description("CallReport -> This test creates a loan for the Kathleen customer that is alive according to call report, then checks the risk checkpoint")]
        public void TestCallReportApplicantIsNotDeceased_LoanIsApproved()
        {
            const String forename = "kathleen";
            const String surname = "bridson";

            var customer = CustomerBuilder.New().WithMiddleName(RiskMiddlenameMask.TESTApplicantIsNotDeceased.ToString()).WithForename(forename).WithSurname(surname).Build();
            var organization = OrganisationBuilder.New().WithPrimaryApplicant(customer).Build();
            var application = ApplicationBuilder.New(customer, organization).Build();
            Assert.IsNotNull(application);

            var riskDb = Driver.Db.Risk;
            var riskApplicationEntity = Do.Until(() => riskDb.RiskApplications.SingleOrDefault(p => p.ApplicationId == application.Id));
            Assert.IsNotNull(riskApplicationEntity, "Risk application should exist");

            var riskAccountEntity = Do.Until(() => riskDb.RiskAccounts.SingleOrDefault(p => p.AccountId == riskApplicationEntity.AccountId));
            Assert.IsNotNull(riskAccountEntity, "Risk account should exist");

            var socialDetailsEntity = Do.Until(() => riskDb.SocialDetails.SingleOrDefault(p => p.AccountId == riskApplicationEntity.AccountId));
            Assert.IsNotNull(socialDetailsEntity, "Social details should exist");

            Assert.Contains(Application.GetExecutedCheckpointDefinitions(application.Id, CheckpointStatus.Verified), Data.EnumToString(CheckpointDefinitionEnum.ApplicantIsAlive));
            AssertCreditBureauUsed(application, CreditBureauEnum.CallReport);
        }

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-575"), Description("CallReport -> This test creates a loan for the customer that is dead according to call report, then checks the risk checkpoint")]
        public void TestCallReportApplicantIsDeceased_LoanIsDeclined()
        {
            const String forename = "Johnny";
            const String surname = "DeadGuy";

            var customer = CustomerBuilder.New().WithMiddleName(RiskMiddlenameMask.TESTApplicantIsNotDeceased.ToString()).WithForename(forename).WithSurname(surname).Build();
            var organization = OrganisationBuilder.New().WithPrimaryApplicant(customer).Build();
            var application = ApplicationBuilder.New(customer, organization).WithExpectedDecision(ApplicationDecisionStatusEnum.Declined).Build();
            Assert.IsNotNull(application);

            var riskDb = Driver.Db.Risk;
            var riskApplicationEntity = Do.Until(() => riskDb.RiskApplications.SingleOrDefault(p => p.ApplicationId == application.Id));
            Assert.IsNotNull(riskApplicationEntity, "Risk application should exist");

            var riskAccountEntity = Do.Until(() => riskDb.RiskAccounts.SingleOrDefault(p => p.AccountId == riskApplicationEntity.AccountId));
            Assert.IsNotNull(riskAccountEntity, "Risk account should exist");

            var socialDetailsEntity = Do.Until(() => riskDb.SocialDetails.SingleOrDefault(p => p.AccountId == riskApplicationEntity.AccountId));
            Assert.IsNotNull(socialDetailsEntity, "Social details should exist");

            Assert.Contains(Application.GetExecutedCheckpointDefinitions(application.Id, CheckpointStatus.Failed), Data.EnumToString(CheckpointDefinitionEnum.ApplicantIsAlive));
            AssertCreditBureauUsed(application, CreditBureauEnum.CallReport);
        }

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-584"), Description("CallReport -> This test creates a loan for a customer that is not CIFAS flagged, then checks the risk checkpoint")]
        public void TestCallReportApplicantIsNotCifasFlagged_LoanIsApproved()
        {
            const String forename = "kathleen";
            const String surname = "nicole";

            var customer = CustomerBuilder.New().WithMiddleName(RiskMiddlenameMask.TESTApplicationElementNotCIFASFlagged.ToString()).WithForename(forename).WithSurname(surname).Build();
            var organization = OrganisationBuilder.New().WithPrimaryApplicant(customer).Build();
            var application = ApplicationBuilder.New(customer, organization).Build();
            Assert.IsNotNull(application);

            var riskDb = Driver.Db.Risk;
            var riskApplicationEntity = Do.Until(() => riskDb.RiskApplications.SingleOrDefault(p => p.ApplicationId == application.Id));
            Assert.IsNotNull(riskApplicationEntity, "Risk application should exist");

            var riskAccountEntity = Do.Until(() => riskDb.RiskAccounts.SingleOrDefault(p => p.AccountId == riskApplicationEntity.AccountId));
            Assert.IsNotNull(riskAccountEntity, "Risk account should exist");

            var socialDetailsEntity = Do.Until(() => riskDb.SocialDetails.SingleOrDefault(p => p.AccountId == riskApplicationEntity.AccountId));
            Assert.IsNotNull(socialDetailsEntity, "Social details should exist");

            Assert.Contains(Application.GetExecutedCheckpointDefinitions(application.Id, CheckpointStatus.Verified), Data.EnumToString(CheckpointDefinitionEnum.CIFASFraudCheck));
            AssertCreditBureauUsed(application, CreditBureauEnum.CallReport);
        }

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-584"), Description("CallReport -> This test creates a loan for a customer that IS CIFAS flagged, then checks the risk checkpoint")]
        public void TestCallReportApplicantIsCifasFlagged_LoanIsDeclined()
        {
            const String forename = "laura";
            const String surname = "insolvent";

            var customer = CustomerBuilder.New().WithMiddleName(RiskMiddlenameMask.TESTApplicationElementNotCIFASFlagged.ToString()).WithForename(forename).WithSurname(surname).Build();
            var organization = OrganisationBuilder.New().WithPrimaryApplicant(customer).Build();
            var application = ApplicationBuilder.New(customer, organization).WithExpectedDecision(ApplicationDecisionStatusEnum.Declined).Build();
            Assert.IsNotNull(application);

            var riskDb = Driver.Db.Risk;
            var riskApplicationEntity = Do.Until(() => riskDb.RiskApplications.SingleOrDefault(p => p.ApplicationId == application.Id));
            Assert.IsNotNull(riskApplicationEntity, "Risk application should exist");


            var riskAccountEntity = Do.Until(() => riskDb.RiskAccounts.SingleOrDefault(p => p.AccountId == riskApplicationEntity.AccountId));
            Assert.IsNotNull(riskAccountEntity, "Risk account should exist");

            var socialDetailsEntity = Do.Until(() => riskDb.SocialDetails.SingleOrDefault(p => p.AccountId == riskApplicationEntity.AccountId));
            Assert.IsNotNull(socialDetailsEntity, "Social details should exist");

            Assert.Contains(Application.GetExecutedCheckpointDefinitions(application.Id, CheckpointStatus.Failed), Data.EnumToString(CheckpointDefinitionEnum.CIFASFraudCheck));
            AssertCreditBureauUsed(application, CreditBureauEnum.CallReport);
        }

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-638"), Description("CallReport -> This test creates a loan for the solvent customer, then checks the risk checkpoint")]
        public void TestCallReportApplicantIsSolvent_LoanIsApproved()
        {
            const String forename = "kathleen";
            const String surname = "nicole";

            var customer = CustomerBuilder.New().WithMiddleName(RiskMiddlenameMask.TESTApplicantIsSolvent.ToString()).WithForename(forename).WithSurname(surname).Build();
            var organization = OrganisationBuilder.New().WithPrimaryApplicant(customer).Build();
            var application = ApplicationBuilder.New(customer, organization).Build();
            Assert.IsNotNull(application);

            var riskDb = Driver.Db.Risk;
            var riskApplicationEntity = Do.Until(() => riskDb.RiskApplications.SingleOrDefault(p => p.ApplicationId == application.Id));
            Assert.IsNotNull(riskApplicationEntity, "Risk application should exist");

            var riskAccountEntity = Do.Until(() => riskDb.RiskAccounts.SingleOrDefault(p => p.AccountId == riskApplicationEntity.AccountId));
            Assert.IsNotNull(riskAccountEntity, "Risk account should exist");

            var socialDetailsEntity = Do.Until(() => riskDb.SocialDetails.SingleOrDefault(p => p.AccountId == riskApplicationEntity.AccountId));
            Assert.IsNotNull(socialDetailsEntity, "Social details should exist");

            Assert.Contains(Application.GetExecutedCheckpointDefinitions(application.Id, CheckpointStatus.Verified), Data.EnumToString(CheckpointDefinitionEnum.CustomerIsSolvent));
            AssertCreditBureauUsed(application, CreditBureauEnum.CallReport);
        }

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-638"), Description("CallReport -> This test creates a loan for the insolvent customer, then checks the risk checkpoint")]
        public void TestCallReportApplicantIsInsolvent_LoanIsDeclined()
        {
            const String forename = "laura";
            const String surname = "insolvent";

            var customer = CustomerBuilder.New().WithMiddleName(RiskMiddlenameMask.TESTApplicantIsSolvent.ToString()).WithForename(forename).WithSurname(surname).Build();
            var organization = OrganisationBuilder.New().WithPrimaryApplicant(customer).Build();
            var application = ApplicationBuilder.New(customer, organization).WithExpectedDecision(ApplicationDecisionStatusEnum.Declined).Build();
            Assert.IsNotNull(application);

            var riskDb = Driver.Db.Risk;
            var riskApplicationEntity = Do.Until(() => riskDb.RiskApplications.SingleOrDefault(p => p.ApplicationId == application.Id));
            Assert.IsNotNull(riskApplicationEntity, "Risk application should exist");

            var riskAccountEntity = Do.Until(() => riskDb.RiskAccounts.SingleOrDefault(p => p.AccountId == riskApplicationEntity.AccountId));
            Assert.IsNotNull(riskAccountEntity, "Risk account should exist");

            var socialDetailsEntity = Do.Until(() => riskDb.SocialDetails.SingleOrDefault(p => p.AccountId == riskApplicationEntity.AccountId));
            Assert.IsNotNull(socialDetailsEntity, "Social details should exist");

            Assert.Contains(Application.GetExecutedCheckpointDefinitions(application.Id, CheckpointStatus.Failed), Data.EnumToString(CheckpointDefinitionEnum.CustomerIsSolvent));
            AssertCreditBureauUsed(application, CreditBureauEnum.CallReport);
        }

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-644"), Description("CallReport -> This test creates a loan for a customer with the correct date of birth, then checks the risk checkpoint")]
        public void TestCallReportDateOfBirthIsCorrect_LoanIsApproved()
        {
            const String forename = "kathleen";
            const String surname = "bridson";
            var dateOfBirth = new Date(new DateTime(1992, 1, 24), DateFormat.Date);

            var customer = CustomerBuilder.New().WithMiddleName(RiskMiddlenameMask.TESTCustomerDateOfBirthIsCorrect.ToString()).WithForename(forename).WithSurname(surname).WithDateOfBirth(dateOfBirth).Build();
            var organization = OrganisationBuilder.New().WithPrimaryApplicant(customer).Build();
            var application = ApplicationBuilder.New(customer, organization).Build();
            Assert.IsNotNull(application);

            var riskDb = Driver.Db.Risk;
            var riskApplicationEntity = Do.Until(() => riskDb.RiskApplications.SingleOrDefault(p => p.ApplicationId == application.Id));
            Assert.IsNotNull(riskApplicationEntity, "Risk application should exist");

            var riskAccountEntity = Do.Until(() => riskDb.RiskAccounts.SingleOrDefault(p => p.AccountId == riskApplicationEntity.AccountId));
            Assert.IsNotNull(riskAccountEntity, "Risk account should exist");

            var socialDetailsEntity = Do.Until(() => riskDb.SocialDetails.SingleOrDefault(p => p.AccountId == riskApplicationEntity.AccountId));
            Assert.IsNotNull(socialDetailsEntity, "Social details should exist");

            Assert.Contains(Application.GetExecutedCheckpointDefinitions(application.Id, CheckpointStatus.Verified), Data.EnumToString(CheckpointDefinitionEnum.DateOfBirthIsCorrect));
            AssertCreditBureauUsed(application, CreditBureauEnum.CallReport);
        }

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-644"), Description("CallReport -> This test creates a loan for a customer with the incorrect date of birth, then checks the risk checkpoint")]
        public void TestCallReportDateOfBirthIsIncorrect_LoanIsDeclined()
        {
            const String forename = "kathleen";
            const String surname = "bridson";
            var dateOfBirth = new Date(new DateTime(1990, 3, 21), DateFormat.Date);

            var customer = CustomerBuilder.New().WithMiddleName(RiskMiddlenameMask.TESTCustomerDateOfBirthIsCorrect.ToString()).WithForename(forename).WithSurname(surname).WithDateOfBirth(dateOfBirth).Build();
            var organization = OrganisationBuilder.New().WithPrimaryApplicant(customer).Build();
            var application = ApplicationBuilder.New(customer, organization).WithExpectedDecision(ApplicationDecisionStatusEnum.Declined).Build();
            Assert.IsNotNull(application);

            var riskDb = Driver.Db.Risk;
            var riskApplicationEntity = Do.Until(() => riskDb.RiskApplications.SingleOrDefault(p => p.ApplicationId == application.Id));
            Assert.IsNotNull(riskApplicationEntity, "Risk application should exist");

            var riskAccountEntity = Do.Until(() => riskDb.RiskAccounts.SingleOrDefault(p => p.AccountId == riskApplicationEntity.AccountId));
            Assert.IsNotNull(riskAccountEntity, "Risk account should exist");

            var socialDetailsEntity = Do.Until(() => riskDb.SocialDetails.SingleOrDefault(p => p.AccountId == riskApplicationEntity.AccountId));
            Assert.IsNotNull(socialDetailsEntity, "Social details should exist");

            Assert.Contains(Application.GetExecutedCheckpointDefinitions(application.Id, CheckpointStatus.Failed), Data.EnumToString(CheckpointDefinitionEnum.DateOfBirthIsCorrect));
            AssertCreditBureauUsed(application, CreditBureauEnum.CallReport);
        }

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-644"), Description("CallReport -> This test creates a loan for a customer with the not provided date of birth, then checks the risk checkpoint")]
        [Pending("IS THIS DECLINED OR APPROVED?? IfDateOfBirthIsNotProvidedByCallReport_LoanIsApproved() ")]
        public void TestCallReportDateOfBirthNotProvided_LoanIsApproved()
        {
            const String forename = "kathleen";
            const String surname = "bridson";
            var dateOfBirth = new Date(new DateTime(1973, 5, 11), DateFormat.Date);

            var customer = CustomerBuilder.New().WithMiddleName(RiskMiddlenameMask.TESTCustomerDateOfBirthIsCorrect.ToString()).WithForename(forename).WithSurname(surname).WithDateOfBirth(dateOfBirth).Build();
            var organization = OrganisationBuilder.New().WithPrimaryApplicant(customer).Build();
            var application = ApplicationBuilder.New(customer, organization).Build();
            Assert.IsNotNull(application);

            var riskDb = Driver.Db.Risk;
            var riskApplicationEntity = Do.Until(() => riskDb.RiskApplications.SingleOrDefault(p => p.ApplicationId == application.Id));
            Assert.IsNotNull(riskApplicationEntity, "Risk application should exist");

            var riskAccountEntity = Do.Until(() => riskDb.RiskAccounts.SingleOrDefault(p => p.AccountId == riskApplicationEntity.AccountId));
            Assert.IsNotNull(riskAccountEntity, "Risk account should exist");

            var socialDetailsEntity = Do.Until(() => riskDb.SocialDetails.SingleOrDefault(p => p.AccountId == riskApplicationEntity.AccountId));
            Assert.IsNotNull(socialDetailsEntity, "Social details should exist");

            Assert.Contains(Application.GetExecutedCheckpointDefinitions(application.Id, CheckpointStatus.Failed), Data.EnumToString(CheckpointDefinitionEnum.DateOfBirthIsCorrect));
            AssertCreditBureauUsed(application, CreditBureauEnum.CallReport);
        }

        private void AssertCreditBureauUsed(Application application, CreditBureauEnum creditBureauUsed)
        {
            var db = new DbDriver();
            var riskApplicationEntity = db.Risk.RiskApplications.SingleOrDefault(r => r.ApplicationId == application.Id);
            Assert.IsNotNull(riskApplicationEntity, "The risk application should exist");
            Assert.IsNotNull(riskApplicationEntity.CreditBureauUsed, "One credit bureau should have been used");
            Assert.AreEqual((CreditBureauEnum)riskApplicationEntity.CreditBureauUsed, creditBureauUsed, "The credit bureau used should be the same");
        }
    }
}
