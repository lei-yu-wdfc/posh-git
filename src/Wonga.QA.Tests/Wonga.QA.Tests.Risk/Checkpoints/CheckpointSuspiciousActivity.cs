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
    public class CheckpointSuspiciousActivity
    {
        [Test, AUT(AUT.Uk), JIRA("UK-845")]
        public void LnSuspiciousActivityDeclined()
        {
            int suspiciousDuration = Get.RandomInt(1,
                Convert.ToInt16(Drive.Db.Ops.ServiceConfigurations.Single(a =>
                    a.Key == Get.EnumToString(ServiceConfigurationKeys.RiskSuspiciousPrevApplicationDuration)).Value));

            int suspiciousDaysSinceLastLoan = Get.RandomInt(1,
                Convert.ToInt16(Drive.Db.Ops.ServiceConfigurations.Single(a =>
                    a.Key == Get.EnumToString(ServiceConfigurationKeys.RiskSuspiciousDaysSinceLastApplication)).Value));

            Customer cust = CustomerBuilder.New()
                .Build();

            Application L0app = ApplicationBuilder.New(cust)
                .WithLoanTerm(suspiciousDuration)
                .Build();

            L0app.RepayOnDueDate();

            Drive.Db.RewindToDayOfLoanTerm(L0app.Id, suspiciousDaysSinceLastLoan);

            cust.UpdateEmployer(Get.EnumToString(RiskMask.TESTNoSuspiciousApplicationActivity));

            Application LnApp = ApplicationBuilder.New(cust)
                .WithExpectedDecision(ApplicationDecisionStatus.Declined)
                .Build();

            Assert.AreEqual(LnApp.FailedCheckpoint, Get.EnumToString(RiskCheckpointDefinitionEnum.SuspiciousActivityCheck));
        }

        [Test, AUT(AUT.Uk), JIRA("UK-845")]
        public void LnSuspiciousActivityAcceptedDueToUnsuspiciousDuration()
        {
            int unsuspiciousDuration = Convert.ToInt16(Drive.Db.Ops.ServiceConfigurations.Single(a =>
                a.Key == Get.EnumToString(ServiceConfigurationKeys.RiskSuspiciousPrevApplicationDuration)).Value) + 1;

            int suspiciousDaysSinceLastLoan = Get.RandomInt(1,
                Convert.ToInt16(Drive.Db.Ops.ServiceConfigurations.Single(a =>
                    a.Key == Get.EnumToString(ServiceConfigurationKeys.RiskSuspiciousDaysSinceLastApplication)).Value));

            Customer cust = CustomerBuilder.New()
                .Build();

            Application L0app = ApplicationBuilder.New(cust)
                .WithLoanTerm(unsuspiciousDuration)
                .Build();

            L0app.RepayOnDueDate();

            Drive.Db.RewindToDayOfLoanTerm(L0app.Id, suspiciousDaysSinceLastLoan);

            cust.UpdateEmployer(Get.EnumToString(RiskMask.TESTNoSuspiciousApplicationActivity));

            Application LnApp = ApplicationBuilder.New(cust)
                .WithExpectedDecision(ApplicationDecisionStatus.Accepted)
                .Build();
        }

        [Test, AUT(AUT.Uk), JIRA("UK-845")]
        public void LnSuspiciousActivityAcceptedDueToUnsuspiciousDaysSinceLastLoan()
        {
            int unsuspiciousDuration = Get.RandomInt(1,
                Convert.ToInt16(Drive.Db.Ops.ServiceConfigurations.Single(a =>
                    a.Key == Get.EnumToString(ServiceConfigurationKeys.RiskSuspiciousPrevApplicationDuration)).Value));

            int suspiciousDaysSinceLastLoan = Convert.ToInt16(Drive.Db.Ops.ServiceConfigurations.Single(
                a => a.Key == Get.EnumToString(ServiceConfigurationKeys.RiskSuspiciousDaysSinceLastApplication)).Value) + 1;

            Customer cust = CustomerBuilder.New()
                .Build();

            Application L0app = ApplicationBuilder.New(cust)
                .WithLoanTerm(unsuspiciousDuration)
                .Build();

            L0app.RepayOnDueDate();

            Drive.Db.RewindToDayOfLoanTerm(L0app.Id, suspiciousDaysSinceLastLoan);

            cust.UpdateEmployer(Get.EnumToString(RiskMask.TESTNoSuspiciousApplicationActivity));

            Application LnApp = ApplicationBuilder.New(cust)
                .WithExpectedDecision(ApplicationDecisionStatus.Accepted)
                .Build();
        }

        [Test, AUT(AUT.Uk), JIRA("UK-845")]
        public void LnSuspiciousActivityAccepted()
        {
            int unsuspiciousDuration = Convert.ToInt16(Drive.Db.Ops.ServiceConfigurations.Single(a =>
                a.Key == Get.EnumToString(ServiceConfigurationKeys.RiskSuspiciousPrevApplicationDuration)).Value) + 1;

            int suspiciousDaysSinceLastLoan = Convert.ToInt16(Drive.Db.Ops.ServiceConfigurations.Single(a =>
                a.Key == Get.EnumToString(ServiceConfigurationKeys.RiskSuspiciousDaysSinceLastApplication)).Value) + 1;

            Customer cust = CustomerBuilder.New()
                .Build();

            Application L0app = ApplicationBuilder.New(cust)
                .WithLoanTerm(unsuspiciousDuration)
                .Build();

            L0app.RepayOnDueDate();

            Drive.Db.RewindToDayOfLoanTerm(L0app.Id, suspiciousDaysSinceLastLoan);

            cust.UpdateEmployer(Get.EnumToString(RiskMask.TESTNoSuspiciousApplicationActivity));

            Application LnApp = ApplicationBuilder.New(cust)
                .WithExpectedDecision(ApplicationDecisionStatus.Accepted)
                .Build();
        }
    }
}
