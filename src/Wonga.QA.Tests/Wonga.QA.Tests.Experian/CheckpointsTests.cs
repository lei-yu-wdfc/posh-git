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

namespace Wonga.QA.Tests.Experian
{
    [Parallelizable(TestScope.All)]
    public class CheckpointsTests
    {
        [Test, AUT(AUT.Wb)]
        [JIRA("SME-575"), Description("Experian -> This test creates a loan for the unknown customer that is alive and with no consumer bureau data, then checks the risk checkpoint")]
        [Pending("Experian not implemented yet?")]
        public void TestExperianUnknownApplicant_LoanIsApproved()
        {
            const String forename = "Unknown";
            const String surname = "Customer";
            var application = CreateApplicationWithAsserts(RiskMiddlenameMask.TESTExperianApplicantIsNotDeceased, forename,
                                                           surname, Data.GetDoB());

            Assert.Contains(Application.GetExecutedCheckpointDefinitions(application.Id, CheckpointStatus.Verified), Data.EnumToString(CheckpointDefinitionEnum.ApplicantIsAlive));
            AssertCreditBureauUsed(application, CreditBureauEnum.Experian);
        }

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-575"), Description("Experian -> This test creates a loan for the Kathleen customer that is alive, then checks the risk checkpoint")]
        [Pending("Experian not implemented yet?")]
        public void TestExperianApplicantIsNotDeceased_LoanIsApproved()
        {
            const String forename = "kathleen";
            const String surname = "bridson";
            var application = CreateApplicationWithAsserts(RiskMiddlenameMask.TESTExperianApplicantIsNotDeceased, forename,
                                                           surname, Data.GetDoB());
            Assert.Contains(Application.GetExecutedCheckpointDefinitions(application.Id, CheckpointStatus.Verified), Data.EnumToString(CheckpointDefinitionEnum.ApplicantIsAlive));
            AssertCreditBureauUsed(application, CreditBureauEnum.Experian);
        }

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-575"), Description("Experian -> This test creates a loan for the customer that is dead, then checks the risk checkpoint")]
        [Pending("Experian not implemented yet?")]
        public void TestExperianApplicantIsDeceased_LoanIsDeclined()
        {
            const String forename = "Johnny";
            const String surname = "DeadGuy";
            var application = CreateApplicationWithAsserts(RiskMiddlenameMask.TESTExperianApplicantIsNotDeceased, forename,
                                                           surname, Data.GetDoB());
            Assert.Contains(Application.GetExecutedCheckpointDefinitions(application.Id, CheckpointStatus.Failed), Data.EnumToString(CheckpointDefinitionEnum.ApplicantIsAlive));
            AssertCreditBureauUsed(application, CreditBureauEnum.Experian);
        }

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-584"), Description("Experian -> This test creates a loan for a customer that is not CIFAS flagged, then checks the risk checkpoint")]
        [Pending("Experian not implemented yet?")]
        public void TestExperianApplicantIsNotCifasFlagged_LoanIsApproved()
        {
            const String forename = "kathleen";
            const String surname = "nicole";
            var application = CreateApplicationWithAsserts(RiskMiddlenameMask.TESTExperianApplicationElementNotCIFASFlagged, forename,
                                                           surname, Data.GetDoB());
            Assert.Contains(Application.GetExecutedCheckpointDefinitions(application.Id, CheckpointStatus.Verified), Data.EnumToString(CheckpointDefinitionEnum.CIFASFraudCheck));
            AssertCreditBureauUsed(application, CreditBureauEnum.Experian);
        }

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-584"), Description("Experian -> This test creates a loan for a customer that IS CIFAS flagged, then checks the risk checkpoint")]
        [Pending("Experian not implemented yet?")]
        public void TestExperianApplicantIsCifasFlagged_LoanIsDeclined()
        {
            const String forename = "laura";
            const String surname = "insolvent";
            var application = CreateApplicationWithAsserts(RiskMiddlenameMask.TESTExperianApplicationElementNotCIFASFlagged, forename,
                                                           surname, Data.GetDoB());
            Assert.Contains(Application.GetExecutedCheckpointDefinitions(application.Id, CheckpointStatus.Failed), Data.EnumToString(CheckpointDefinitionEnum.CIFASFraudCheck));
            AssertCreditBureauUsed(application, CreditBureauEnum.Experian);
        }

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-638"), Description("Experian -> This test creates a loan for the solvent customer, then checks the risk checkpoint")]
        [Pending("Experian not implemented yet?")]
        public void TestExperianApplicantIsSolvent_LoanIsApproved()
        {
            const String forename = "kathleen";
            const String surname = "nicole";
            var application = CreateApplicationWithAsserts(RiskMiddlenameMask.TESTExperianApplicantIsSolvent, forename,
                                                           surname, Data.GetDoB());
            Assert.Contains(Application.GetExecutedCheckpointDefinitions(application.Id, CheckpointStatus.Verified), Data.EnumToString(CheckpointDefinitionEnum.CustomerIsSolvent));
            AssertCreditBureauUsed(application, CreditBureauEnum.Experian);
        }

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-638"), Description("Experian -> This test creates a loan for the insolvent customer, then checks the risk checkpoint")]
        [Pending("Experian not implemented yet?")]
        public void TestExperianApplicantIsInsolvent_LoanIsDeclined()
        {
            const String forename = "laura";
            const String surname = "insolvent";
            var application = CreateApplicationWithAsserts(RiskMiddlenameMask.TESTExperianApplicantIsSolvent, forename,
                                                           surname, Data.GetDoB());
            Assert.Contains(Application.GetExecutedCheckpointDefinitions(application.Id, CheckpointStatus.Failed), Data.EnumToString(CheckpointDefinitionEnum.CustomerIsSolvent));
            AssertCreditBureauUsed(application, CreditBureauEnum.Experian);
        }

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-644"), Description("Experian -> This test creates a loan for a customer with the correct date of birth, then checks the risk checkpoint")]
        [Pending("Experian not implemented yet?")]
        public void TestExperianDateOfBirthIsCorrect_LoanIsApproved()
        {
            const String forename = "kathleen";
            const String surname = "bridson";
            var dateOfBirth = new Date(new DateTime(1992, 1, 24), DateFormat.Date);
            var application = CreateApplicationWithAsserts(RiskMiddlenameMask.TESTExperianCustomerDateOfBirthIsCorrect, forename,
                                                           surname, dateOfBirth);
            Assert.Contains(Application.GetExecutedCheckpointDefinitions(application.Id, CheckpointStatus.Verified), Data.EnumToString(CheckpointDefinitionEnum.DateOfBirthIsCorrect));
            AssertCreditBureauUsed(application, CreditBureauEnum.Experian);
        }

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-644"), Description("Experian -> This test creates a loan for a customer with the incorrect date of birth, then checks the risk checkpoint")]
        [Pending("Experian not implemented yet?")]
        public void TestExperianDateOfBirthIsIncorrect_LoanIsDeclined()
        {
            const String forename = "kathleen";
            const String surname = "bridson";
            var dateOfBirth = new Date(new DateTime(1990, 3, 21), DateFormat.Date);
            var application = CreateApplicationWithAsserts(RiskMiddlenameMask.TESTExperianCustomerDateOfBirthIsCorrect, forename,
                                                           surname, dateOfBirth);
            Assert.Contains(Application.GetExecutedCheckpointDefinitions(application.Id, CheckpointStatus.Failed), Data.EnumToString(CheckpointDefinitionEnum.DateOfBirthIsCorrect));
            AssertCreditBureauUsed(application, CreditBureauEnum.Experian);
        }

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-644"), Description("Experian -> This test creates a loan for a customer with the not provided date of birth, then checks the risk checkpoint")]
        [Pending("Experian not implemented yet?")]
        public void TestExperianDateOfBirthIsNotProvided_LoanIsApproved()
        {
            const String forename = "kathleen";
            const String surname = "bridson";
            var dateOfBirth = new Date(new DateTime(1973, 5, 11), DateFormat.Date);
            var application = CreateApplicationWithAsserts(RiskMiddlenameMask.TESTExperianCustomerDateOfBirthIsCorrect, forename,
                                                           surname, dateOfBirth);
            Assert.Contains(Application.GetExecutedCheckpointDefinitions(application.Id, CheckpointStatus.Verified), Data.EnumToString(CheckpointDefinitionEnum.DateOfBirthIsCorrect));
            AssertCreditBureauUsed(application, CreditBureauEnum.Experian);
        }

        private static void AssertCreditBureauUsed(Application application, CreditBureauEnum creditBureauUsed)
        {
            var db = new DbDriver();
            var riskApplicationEntity = db.Risk.RiskApplications.SingleOrDefault(r => r.ApplicationId == application.Id);
            Assert.IsNotNull(riskApplicationEntity, "The risk application should exist");
            Assert.IsNotNull(riskApplicationEntity.CreditBureauUsed, "One credit bureau should have been used");
            Assert.AreEqual((CreditBureauEnum)riskApplicationEntity.CreditBureauUsed, creditBureauUsed, "The credit bureau used should be the same");
        }
        private static Application CreateApplicationWithAsserts(RiskMiddlenameMask middlenameMask, String forename, String surname, Date dateOfBirth)
        {
            var customer = CustomerBuilder.New().WithMiddleName(middlenameMask.ToString()).WithForename(forename).
                                     WithSurname(surname).WithDateOfBirth(dateOfBirth).Build();
            var organization = OrganisationBuilder.New().WithPrimaryApplicant(customer).Build();
            var application = ApplicationBuilder.New(customer, organization).Build();
            Assert.IsNotNull(application);

            var riskDb = Driver.Db.Risk;
            var riskApplicationEntity = Do.Until(() => riskDb.RiskApplications.SingleOrDefault(p => p.ApplicationId == application.Id));
            Assert.IsNotNull(riskApplicationEntity, "Risk application should exist");

            var riskAccountEntity = Do.Until(() => riskDb.RiskAccounts.SingleOrDefault(p => p.AccountId == riskApplicationEntity.AccountId));
            Assert.IsNotNull(riskAccountEntity, "Risk account should exist");

            var socialDetailsEntity = Do.Until(() => riskDb.SocialDetails.SingleOrDefault(p => p.AccountId == riskApplicationEntity.AccountId));
            Assert.IsNotNull(socialDetailsEntity, "Risk Social details should exist");

            return application;
        }
    }
}
