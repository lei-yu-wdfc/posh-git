using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Data;
using Wonga.QA.Framework.Data.Enums.Risk;
using Wonga.QA.Framework.Db.Extensions;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Risk.Checkpoints
{
    [Parallelizable(TestScope.All)]
    public class CheckpointSuspiciousActivity
    {
        [Test, AUT(AUT.Uk), JIRA("UK-845"), Description("Scenario 1: Declined")]
        public void LnSuspiciousActivityDeclined()
        {
            var suspiciousDuration = Get.RandomInt(1,
                Convert.ToInt16(Drive.Db.Ops.ServiceConfigurations.Single(a =>
                    a.Key == Get.EnumToString(ServiceConfigurationKeys.RiskSuspiciousPrevApplicationDuration)).Value));

            var suspiciousDaysSinceLastLoan = Get.RandomInt(1,
                Convert.ToInt16(Drive.Db.Ops.ServiceConfigurations.Single(a =>
                    a.Key == Get.EnumToString(ServiceConfigurationKeys.RiskSuspiciousDaysSinceLastApplication)).Value));

            var customer = CustomerBuilder.New()
                .Build();

            var l0Application = ApplicationBuilder.New(customer)
                .WithLoanTerm(suspiciousDuration)
                .Build();

            l0Application.RepayOnDueDate();

            Drive.Db.RewindToDayOfLoanTerm(l0Application.Id, suspiciousDaysSinceLastLoan);

            customer.UpdateEmployer(Get.EnumToString(RiskMask.TESTNoSuspiciousApplicationActivity));

            Application lnApplication = ApplicationBuilder.New(customer)
                .WithExpectedDecision(ApplicationDecisionStatus.Declined)
                .Build();

            Assert.AreEqual(lnApplication.FailedCheckpoint, Get.EnumToString(RiskCheckpointDefinitionEnum.SuspiciousActivityCheck));
        }

        [Test, AUT(AUT.Uk), JIRA("UK-845"), Description("Scenario 2: Accepted")]
        public void LnSuspiciousActivityAcceptedDueToUnsuspiciousDuration()
        {
            var unsuspiciousDuration = Convert.ToInt16(Drive.Db.Ops.ServiceConfigurations.Single(a =>
                a.Key == Get.EnumToString(ServiceConfigurationKeys.RiskSuspiciousPrevApplicationDuration)).Value) + 1;

            var suspiciousDaysSinceLastLoan = Get.RandomInt(1,
                Convert.ToInt16(Drive.Db.Ops.ServiceConfigurations.Single(a =>
                    a.Key == Get.EnumToString(ServiceConfigurationKeys.RiskSuspiciousDaysSinceLastApplication)).Value));

            var customer = CustomerBuilder.New()
                .Build();

            var l0Application = ApplicationBuilder.New(customer)
                .WithLoanTerm(unsuspiciousDuration)
                .Build();

            l0Application.RepayOnDueDate();

            Drive.Db.RewindToDayOfLoanTerm(l0Application.Id, suspiciousDaysSinceLastLoan);

            customer.UpdateEmployer(Get.EnumToString(RiskMask.TESTNoSuspiciousApplicationActivity));

            var lnApplication = ApplicationBuilder.New(customer)
                .WithExpectedDecision(ApplicationDecisionStatus.Accepted)
                .Build();
        }

        [Test, AUT(AUT.Uk), JIRA("UK-845"), Description("Scenario 3: Accepted"),Category(TestCategories.CoreTest)]
        public void LnSuspiciousActivityAcceptedDueToUnsuspiciousDaysSinceLastLoan()
        {
            var unsuspiciousDuration = Get.RandomInt(1,
                Convert.ToInt16(Drive.Db.Ops.ServiceConfigurations.Single(a =>
                    a.Key == Get.EnumToString(ServiceConfigurationKeys.RiskSuspiciousPrevApplicationDuration)).Value));

            var suspiciousDaysSinceLastLoan = Convert.ToInt16(Drive.Db.Ops.ServiceConfigurations.Single(
                a => a.Key == Get.EnumToString(ServiceConfigurationKeys.RiskSuspiciousDaysSinceLastApplication)).Value) + 2;

            var customer = CustomerBuilder.New()
                .Build();

            var l0Application = ApplicationBuilder.New(customer)
                .WithLoanTerm(unsuspiciousDuration)
                .Build();

            l0Application.RepayOnDueDate();

            Drive.Db.RewindToDayOfLoanTerm(l0Application.Id, suspiciousDaysSinceLastLoan);

            customer.UpdateEmployer(Get.EnumToString(RiskMask.TESTNoSuspiciousApplicationActivity));

            var lnApplication = ApplicationBuilder.New(customer)
                .WithExpectedDecision(ApplicationDecisionStatus.Accepted)
                .Build();
        }

        [Test, AUT(AUT.Uk), JIRA("UK-845"), Description("Scenario 4: Accepted")]
        public void LnSuspiciousActivityAccepted()
        {
            var unsuspiciousDuration = Convert.ToInt16(Drive.Db.Ops.ServiceConfigurations.Single(a =>
                a.Key == Get.EnumToString(ServiceConfigurationKeys.RiskSuspiciousPrevApplicationDuration)).Value) + 1;

            var suspiciousDaysSinceLastLoan = Convert.ToInt16(Drive.Db.Ops.ServiceConfigurations.Single(a =>
                a.Key == Get.EnumToString(ServiceConfigurationKeys.RiskSuspiciousDaysSinceLastApplication)).Value) + 1;

            var customer = CustomerBuilder.New()
                .Build();

            var l0Application = ApplicationBuilder.New(customer)
                .WithLoanTerm(unsuspiciousDuration)
                .Build();

            l0Application.RepayOnDueDate();

            Drive.Db.RewindToDayOfLoanTerm(l0Application.Id, suspiciousDaysSinceLastLoan);

            customer.UpdateEmployer(Get.EnumToString(RiskMask.TESTNoSuspiciousApplicationActivity));

            var lnApplication = ApplicationBuilder.New(customer)
                .WithExpectedDecision(ApplicationDecisionStatus.Accepted)
                .Build();
        }
    }
}
