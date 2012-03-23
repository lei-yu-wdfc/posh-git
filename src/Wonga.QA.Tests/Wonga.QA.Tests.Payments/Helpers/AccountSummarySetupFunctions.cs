using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db.Payments;
using Wonga.QA.Framework.Msmq;

namespace Wonga.QA.Tests.Payments.Helpers
{
    public class AccountSummarySetupFunctions
    {

        public void Scenario01Setup(Guid accountId, Guid appId, decimal trustRating)
        {
            var bankAccountId = Guid.NewGuid();
            var paymentCardId = Guid.NewGuid();

            // Create Application 
            CreateFixedTermLoanApplication(appId, accountId, bankAccountId, paymentCardId);

            // Check App Exists in DB
            Guid id = appId;
            Do.Until(() => Drive.Db.Payments.Applications.Single(a => a.ExternalId == id));

            Drive.Msmq.Payments.Send(new SignApplicationCommand() { AccountId = accountId, ApplicationId = appId, CreatedOn = DateTime.Now.AddHours(-1) });
            Drive.Msmq.Payments.Send(new IApplicationAcceptedEvent() { AccountId = accountId, ApplicationId = appId, CreatedOn = DateTime.Now.AddHours(-1) });

            // Check App Exists in DB
            Do.Until(() => Drive.Db.Payments.Applications.Single(a => a.ExternalId == id && a.SignedOn != null && a.AcceptedOn != null));

            // Go to DB and set Application to closed
            ApplicationEntity app = Drive.Db.Payments.Applications.Single(a => a.ExternalId == id);
            app.ClosedOn = DateTime.UtcNow.AddDays(-1);
            app.Submit(true);

        }

        public void Scenario02Setup(Guid appId, Guid paymentCardId, Guid bankAccountId, Guid accountId, decimal trustRating)
        {
            const string extendMinLoanDays = "7";
            const string extendLoanDaysBeforeDueDate = "30";

            // Create Account so that time zone can be looked up
            Drive.Msmq.Payments.Send(new IAccountCreatedEvent() { AccountId = accountId });

            // Override setting in ServiceConfig Db for ExtendDaysMins
            var cfg1 = Drive.Db.Ops.ServiceConfigurations.Single(a => a.Key == "Payments.ExtendLoanMinDays");
            cfg1.Value = extendMinLoanDays;
            cfg1.Submit();

            // Override setting in ServiceConfig Db for ExtendLoanDaysBeforeDueDate
            var cfg2 = Drive.Db.Ops.ServiceConfigurations.Single(a => a.Key == "Payments.ExtendLoanDaysBeforeDueDate");
            cfg2.Value = extendLoanDaysBeforeDueDate;
            cfg2.Submit();

            // Create Application 
            CreateFixedTermLoanApplication(appId, accountId, bankAccountId, paymentCardId);

            Drive.Msmq.Payments.Send(new SignApplicationCommand() { AccountId = accountId, ApplicationId = appId, CreatedOn = DateTime.Now.AddHours(-1) });
            Drive.Msmq.Payments.Send(new IApplicationAcceptedEvent() { AccountId = accountId, ApplicationId = appId, CreatedOn = DateTime.Now.AddHours(-1) });

            // Check App Exists in DB
            Do.Until(
                () =>
                Drive.Db.Payments.Applications.Single(a => a.ExternalId == appId && a.SignedOn != null && a.AcceptedOn != null));

            var trnGuid1 = CreateLoanAdvanceTransaction(appId);
            var trnGuid2 = CreateTransmissionFeeTransaction(appId);

            Do.Until(() => Drive.Db.Payments.Transactions.Count(itm => itm.ExternalId == trnGuid1 || itm.ExternalId == trnGuid2) == 2);
        }

        public void Scenario03Setup(Guid appId, Guid paymentCardId, Guid bankAccountId, Guid accountId, decimal trustRating)
        {
            const string extendMinLoanDays = "-1";
            const string extendLoanDaysBeforeDueDate = "30";

            // Create Account so that time zone can be looked up
            Drive.Msmq.Payments.Send(new IAccountCreatedEvent() { AccountId = accountId });

            // Override setting in ServiceConfig Db for ExtendDaysMins
            var cfg1 = Drive.Db.Ops.ServiceConfigurations.Single(a => a.Key == "Payments.ExtendLoanMinDays");
            cfg1.Value = extendMinLoanDays;
            cfg1.Submit();

            // Override setting in ServiceConfig Db for ExtendLoanDaysBeforeDueDate
            var cfg2 = Drive.Db.Ops.ServiceConfigurations.Single(a => a.Key == "Payments.ExtendLoanDaysBeforeDueDate");
            cfg2.Value = extendLoanDaysBeforeDueDate;
            cfg2.Submit();

            // Create Application 
            CreateFixedTermLoanApplication(appId, accountId, bankAccountId, paymentCardId);

            Drive.Msmq.Payments.Send(new SignApplicationCommand() { AccountId = accountId, ApplicationId = appId, CreatedOn = DateTime.Now.AddHours(-1) });
            Drive.Msmq.Payments.Send(new IApplicationAcceptedEvent() { AccountId = accountId, ApplicationId = appId, CreatedOn = DateTime.Now.AddHours(-1) });

            // Check App Exists in DB
            Do.Until(() => Drive.Db.Payments.Applications.Single(a => a.ExternalId == appId));

            var trnGuid1 = CreateLoanAdvanceTransaction(appId);
            var trnGuid2 = CreateTransmissionFeeTransaction(appId);

            Do.Until(() => Drive.Db.Payments.Transactions.Count(itm => itm.ExternalId == trnGuid1 || itm.ExternalId == trnGuid2) == 2);
        }

        public void Scenario04Setup(Guid paymentCardId, Guid appId, Guid bankAccountId, Guid accountId,decimal trustRating)
        {
            const string extendMinLoanDays = "-1";
            const string extendLoanDaysBeforeDueDate = "30";

            // Create Account so that time zone can be looked up
            Drive.Msmq.Payments.Send(new IAccountCreatedEvent() { AccountId = accountId });

            // Override setting in ServiceConfig Db for ExtendDaysMins
            var cfg1 = Drive.Db.Ops.ServiceConfigurations.Single(a => a.Key == "Payments.ExtendLoanMinDays");
            cfg1.Value = extendMinLoanDays;
            cfg1.Submit();

            // Override setting in ServiceConfig Db for ExtendLoanDaysBeforeDueDate
            var cfg2 = Drive.Db.Ops.ServiceConfigurations.Single(a => a.Key == "Payments.ExtendLoanDaysBeforeDueDate");
            cfg2.Value = extendLoanDaysBeforeDueDate;
            cfg2.Submit();

            // Create Application 
            CreateFixedTermLoanApplication(appId, accountId, bankAccountId, paymentCardId);

            Drive.Msmq.Payments.Send(new SignApplicationCommand() { AccountId = accountId, ApplicationId = appId, CreatedOn = DateTime.Now.AddHours(-1) });
            Drive.Msmq.Payments.Send(new IApplicationAcceptedEvent() { AccountId = accountId, ApplicationId = appId, CreatedOn = DateTime.Now.AddHours(-1) });

            // Check App Exists in DB
            Do.Until(() => Drive.Db.Payments.Applications.Single(a => a.ExternalId == appId));

            var trnGuid1 = CreateLoanAdvanceTransaction(appId);
            var trnGuid2 = CreateTransmissionFeeTransaction(appId);
            var trnGuid3 = CreateExtensionFeeTransaction(appId);
            var trnGuid4 = CreateExtensionFeeTransaction(appId);
            var trnGuid5 = CreateExtensionFeeTransaction(appId);

            // Check transactions have been created
            Do.Until(
                () =>
                Drive.Db.Payments.Transactions.Count(
                    itm =>
                    itm.ExternalId == trnGuid1 || itm.ExternalId == trnGuid2 || itm.ExternalId == trnGuid3 ||
                    itm.ExternalId == trnGuid4 || itm.ExternalId == trnGuid5) == 5);
        }

        public void Scenario05Setup(Guid paymentCardId, Guid appId, Guid bankAccountId, Guid accountId, decimal trustRating)
        {
            const string extendMinLoanDays = "7";
            const string extendLoanDaysBeforeDueDate = "30";


            // Create Account so that time zone can be looked up
            Drive.Msmq.Payments.Send(new IAccountCreatedEvent() { AccountId = accountId });

            // Override setting in ServiceConfig Db for ExtendDaysMins
            var cfg1 = Drive.Db.Ops.ServiceConfigurations.Single(a => a.Key == "Payments.ExtendLoanMinDays");
            cfg1.Value = extendMinLoanDays;
            cfg1.Submit();

            // Override setting in ServiceConfig Db for ExtendLoanDaysBeforeDueDate
            var cfg2 = Drive.Db.Ops.ServiceConfigurations.Single(a => a.Key == "Payments.ExtendLoanDaysBeforeDueDate");
            cfg2.Value = extendLoanDaysBeforeDueDate;
            cfg2.Submit();

            // Create Application 
            CreateFixedTermLoanApplication(appId, accountId, bankAccountId, paymentCardId);

            Drive.Msmq.Payments.Send(new SignApplicationCommand() { AccountId = accountId, ApplicationId = appId, CreatedOn = DateTime.Now.AddHours(-1) });
            Drive.Msmq.Payments.Send(new IApplicationAcceptedEvent() { AccountId = accountId, ApplicationId = appId, CreatedOn = DateTime.Now.AddHours(-1) });

            // Check App Exists in DB
            Do.Until(() => Drive.Db.Payments.Applications.Single(a => a.ExternalId == appId));

            var trnGuid1 = CreateLoanAdvanceTransaction(appId);
            var trnGuid2 = CreateTransmissionFeeTransaction(appId);
            var trnGuid3 = CreateExtensionFeeTransaction(appId);
            var trnGuid4 = CreateExtensionFeeTransaction(appId);
            var trnGuid5 = CreateExtensionFeeTransaction(appId);

            // Check transactions have been created
            Do.Until(
                () =>
                Drive.Db.Payments.Transactions.Count(
                    itm =>
                    itm.ExternalId == trnGuid1 || itm.ExternalId == trnGuid2 || itm.ExternalId == trnGuid3 ||
                    itm.ExternalId == trnGuid4 || itm.ExternalId == trnGuid5) == 5);
        }

        public void Scenario06Setup(Guid appId, Guid paymentCardId, Guid bankAccountId, Guid accountId, decimal trustRating)
        {
            const string extendMinLoanDays = "-1";
            const string extendLoanDaysBeforeDueDate = "30";

            // Create Account so that time zone can be looked up
            Drive.Msmq.Payments.Send(new IAccountCreatedEvent() { AccountId = accountId });

            // Override setting in ServiceConfig Db for ExtendDaysMins
            var cfg1 = Drive.Db.Ops.ServiceConfigurations.Single(a => a.Key == "Payments.ExtendLoanMinDays");
            cfg1.Value = extendMinLoanDays;
            cfg1.Submit();

            // Override setting in ServiceConfig Db for ExtendLoanDaysBeforeDueDate
            var cfg2 = Drive.Db.Ops.ServiceConfigurations.Single(a => a.Key == "Payments.ExtendLoanDaysBeforeDueDate");
            cfg2.Value = extendLoanDaysBeforeDueDate;
            cfg2.Submit();

            // Create Application 
            CreateFixedTermLoanApplication(appId, accountId, bankAccountId, paymentCardId);

            Drive.Msmq.Payments.Send(new SignApplicationCommand() { AccountId = accountId, ApplicationId = appId, CreatedOn = DateTime.Now.AddHours(-1) });
            Drive.Msmq.Payments.Send(new IApplicationAcceptedEvent() { AccountId = accountId, ApplicationId = appId, CreatedOn = DateTime.Now.AddHours(-1) });

            // Check App Exists in DB
            Do.Until(() => Drive.Db.Payments.Applications.Single(a => a.ExternalId == appId));

            var trnGuid1 = CreateLoanAdvanceTransaction(appId);
            var trnGuid2 = CreateTransmissionFeeTransaction(appId);

            // Check transactions have been created
            Do.Until(
                () => Drive.Db.Payments.Transactions.Count(itm => itm.ExternalId == trnGuid1 || itm.ExternalId == trnGuid2) == 2);
        }


        public void Scenario07Setup(Guid paymentCardId, Guid bankAccountId, Guid appId, Guid accountId, decimal trustRating)
        {
            const string extendMinLoanDays = "-1";
            const string extendLoanDaysBeforeDueDate = "30";

            const string extendLoanEnabled = "false";

            // Create Account so that time zone can be looked up
            Drive.Msmq.Payments.Send(new IAccountCreatedEvent() { AccountId = accountId });

            // Override setting in ServiceConfig Db for ExtendDaysMins
            var cfg1 = Drive.Db.Ops.ServiceConfigurations.Single(a => a.Key == "Payments.ExtendLoanMinDays");
            cfg1.Value = extendMinLoanDays;
            cfg1.Submit();

            // Override setting in ServiceConfig Db for ExtendLoanDaysBeforeDueDate
            var cfg2 = Drive.Db.Ops.ServiceConfigurations.Single(a => a.Key == "Payments.ExtendLoanDaysBeforeDueDate");
            cfg2.Value = extendLoanDaysBeforeDueDate;
            cfg2.Submit();

            // Override setting in ServiceConfig Db for ExtendDaysMins
            var cfg3 = Drive.Db.Ops.ServiceConfigurations.Single(a => a.Key == "Payments.ExtendLoanEnabled");
            cfg3.Value = extendLoanEnabled;
            cfg3.Submit();


            // Create Application 
            CreateFixedTermLoanApplication(appId, accountId, bankAccountId, paymentCardId);

            Drive.Msmq.Payments.Send(new SignApplicationCommand() { AccountId = accountId, ApplicationId = appId, CreatedOn = DateTime.Now.AddHours(-1) });
            Drive.Msmq.Payments.Send(new IApplicationAcceptedEvent() { AccountId = accountId, ApplicationId = appId, CreatedOn = DateTime.Now.AddHours(-1) });

            // Check App Exists in DB
            Do.Until(() => Drive.Db.Payments.Applications.Single(a => a.ExternalId == appId));

            var trnGuid1 = CreateLoanAdvanceTransaction(appId);
            var trnGuid2 = CreateTransmissionFeeTransaction(appId);

            // Check transactions have been created
            Do.Until(
                () => Drive.Db.Payments.Transactions.Count(itm => itm.ExternalId == trnGuid1 || itm.ExternalId == trnGuid2) == 2);
        }

        public void Scenario08Setup(Guid paymentCardId, Guid bankAccountId, Guid accountId, Guid appId)
        {

            const string extendMinLoanDays = "-1";
            const string extendLoanDaysBeforeDueDate = "30";

            const string extendLoanEnabled = "false";

            // Create Account so that time zone can be looked up
            Drive.Msmq.Payments.Send(new IAccountCreatedEvent() { AccountId = accountId });

            // Override setting in ServiceConfig Db for ExtendDaysMins
            var cfg1 = Drive.Db.Ops.ServiceConfigurations.Single(a => a.Key == "Payments.ExtendLoanMinDays");
            cfg1.Value = extendMinLoanDays;
            cfg1.Submit();

            // Override setting in ServiceConfig Db for ExtendLoanDaysBeforeDueDate
            var cfg2 = Drive.Db.Ops.ServiceConfigurations.Single(a => a.Key == "Payments.ExtendLoanDaysBeforeDueDate");
            cfg2.Value = extendLoanDaysBeforeDueDate;
            cfg2.Submit();

            // Override setting in ServiceConfig Db for ExtendDaysMins
            var cfg3 = Drive.Db.Ops.ServiceConfigurations.Single(a => a.Key == "Payments.ExtendLoanEnabled");
            cfg3.Value = extendLoanEnabled;
            cfg3.Submit();


            // Create Application 
            CreateFixedTermLoanApplication(appId, accountId, bankAccountId, paymentCardId);

            Drive.Msmq.Payments.Send(new SignApplicationCommand() { AccountId = accountId, ApplicationId = appId, CreatedOn = DateTime.Now.AddHours(-1) });
            Drive.Msmq.Payments.Send(new IApplicationAcceptedEvent() { AccountId = accountId, ApplicationId = appId, CreatedOn = DateTime.Now.AddHours(-1) });

            // Check App Exists in DB
            Do.Until(() => Drive.Db.Payments.Applications.Single(a => a.ExternalId == appId));

            var trnGuid1 = CreateLoanAdvanceTransaction(appId);
            var trnGuid2 = CreateTransmissionFeeTransaction(appId);

            // Check transactions have been created
            Do.Until(
                () => Drive.Db.Payments.Transactions.Count(itm => itm.ExternalId == trnGuid1 || itm.ExternalId == trnGuid2) == 2);

            // Go to DB and set Application to closed
            ApplicationEntity app = Drive.Db.Payments.Applications.Single(a => a.ExternalId == appId);
            app.ClosedOn = DateTime.UtcNow.AddDays(0);
            var fixAppId = app.ApplicationId;
            app.Submit(true);

            // Go to DB and set NextDueDate to today
            FixedTermLoanApplicationEntity fixApp =
                Drive.Db.Payments.FixedTermLoanApplications.Single(a => a.ApplicationId == fixAppId);
            fixApp.NextDueDate = DateTime.UtcNow.AddDays(0);
            fixApp.Submit(true);

        }

        public void Scenario09Setup(Guid requestId2, Guid requestId1, Guid accountId, Guid paymentCardId, Guid appId, Guid bankAccountId)
        {
            // Create Account so that time zone can be looked up
            Drive.Msmq.Payments.Send(new IAccountCreatedEvent() { AccountId = accountId });
            Do.Until(() => Drive.Db.Payments.AccountPreferences.Single(a => a.AccountId == accountId));

            // Create Application & Check it Exists in DB
            CreateFixedTermLoanApplication(appId, accountId, bankAccountId, paymentCardId);
            Do.Until(() => Drive.Db.Payments.Applications.Single(a => a.ExternalId == appId));

            // Set SignedOn + AcceptedOn & check statuses have been updated
            Drive.Msmq.Payments.Send(new SignApplicationCommand() { AccountId = accountId, ApplicationId = appId, CreatedOn = DateTime.Now.AddHours(-1) });
            Drive.Msmq.Payments.Send(new IApplicationAcceptedEvent() { AccountId = accountId, ApplicationId = appId, CreatedOn = DateTime.Now.AddHours(-1) });
            Do.Until(
                () =>
                Drive.Db.Payments.Applications.Single(a => a.ExternalId == appId && a.SignedOn != null && a.AcceptedOn != null));

            // Create transactions & Check transactions have been created
            var trnGuid1 = CreateLoanAdvanceTransaction(appId);
            var trnGuid2 = CreateTransmissionFeeTransaction(appId);
            Do.Until(
                () => Drive.Db.Payments.Transactions.Count(itm => itm.ExternalId == trnGuid1 || itm.ExternalId == trnGuid2) == 2);

            // Send command to create scheduled payment request
            Drive.Msmq.Payments.Send(new CreateScheduledPaymentRequestCommand() { ApplicationId = appId, RepaymentRequestId = requestId1, });
            Drive.Msmq.Payments.Send(new CreateScheduledPaymentRequestCommand() { ApplicationId = appId, RepaymentRequestId = requestId2, });

            Do.Until(() => Drive.Db.Payments.RepaymentRequests.Count(itm => itm.ExternalId == requestId1));
            Do.Until(() => Drive.Db.Payments.RepaymentRequests.Count(itm => itm.ExternalId == requestId2));

            // Go to DB and set Application NextDueDate to today.
            ApplicationEntity app = Drive.Db.Payments.Applications.Single(a => a.ExternalId == appId);
            FixedTermLoanApplicationEntity fixedApp =
                Drive.Db.Payments.FixedTermLoanApplications.Single(a => a.ApplicationId == app.ApplicationId);
            fixedApp.NextDueDate = DateTime.UtcNow.Date;
            fixedApp.Submit(true);
        }

        public void Scenario10Setup(Guid requestId1, Guid requestId2, Guid appId, Guid bankAccountId, Guid accountId, Guid paymentCardId)
        {
            // Create Account so that time zone can be looked up
            Drive.Msmq.Payments.Send(new IAccountCreatedEvent() { AccountId = accountId });
            Do.Until(() => Drive.Db.Payments.AccountPreferences.Single(a => a.AccountId == accountId));

            // Create Application & Check it Exists in DB
            CreateFixedTermLoanApplication(appId, accountId, bankAccountId, paymentCardId);
            Do.Until(() => Drive.Db.Payments.Applications.Single(a => a.ExternalId == appId));

            // Set SignedOn + AcceptedOn & check statuses have been updated
            Drive.Msmq.Payments.Send(new SignApplicationCommand() { AccountId = accountId, ApplicationId = appId, CreatedOn = DateTime.Now.AddHours(-1) });
            Drive.Msmq.Payments.Send(new IApplicationAcceptedEvent() { AccountId = accountId, ApplicationId = appId, CreatedOn = DateTime.Now.AddHours(-1) });
            Do.Until(
                () =>
                Drive.Db.Payments.Applications.Single(a => a.ExternalId == appId && a.SignedOn != null && a.AcceptedOn != null));

            // Create transactions & Check transactions have been created
            var trnGuid1 = CreateLoanAdvanceTransaction(appId);
            var trnGuid2 = CreateTransmissionFeeTransaction(appId);
            var trnGuid3 = CreateMissedPaymentChargeTransaction(appId);
            Do.Until(
                () =>
                Drive.Db.Payments.Transactions.Count(
                    itm => itm.ExternalId == trnGuid1 || itm.ExternalId == trnGuid2 || itm.ExternalId == trnGuid3) == 3);

            // Send command to create scheduled payment request
            Drive.Msmq.Payments.Send(new CreateScheduledPaymentRequestCommand() { ApplicationId = appId, RepaymentRequestId = requestId1, });
            Drive.Msmq.Payments.Send(new CreateScheduledPaymentRequestCommand() { ApplicationId = appId, RepaymentRequestId = requestId2, });

            Do.Until(() => Drive.Db.Payments.RepaymentRequests.Count(itm => itm.ExternalId == requestId1));
            Do.Until(() => Drive.Db.Payments.RepaymentRequests.Count(itm => itm.ExternalId == requestId2));

            // Go to DB and set Application NextDueDate to yesterday.
            ApplicationEntity app = Drive.Db.Payments.Applications.Single(a => a.ExternalId == appId);
            FixedTermLoanApplicationEntity fixedApp =
                Drive.Db.Payments.FixedTermLoanApplications.Single(a => a.ApplicationId == app.ApplicationId);
            fixedApp.NextDueDate = DateTime.UtcNow.Date.AddDays(-1);
            fixedApp.Submit(true);
        }

        public void Scenario11Setup(Guid requestId1, Guid requestId2, Guid appId, Guid bankAccountId, Guid accountId, Guid paymentCardId)
        {
            // Create Account so that time zone can be looked up
            Drive.Msmq.Payments.Send(new IAccountCreatedEvent() { AccountId = accountId });
            Do.Until(() => Drive.Db.Payments.AccountPreferences.Single(a => a.AccountId == accountId));

            // Create Application & Check it Exists in DB
            CreateFixedTermLoanApplication(appId, accountId, bankAccountId, paymentCardId);
            Do.Until(() => Drive.Db.Payments.Applications.Single(a => a.ExternalId == appId));

            // Set SignedOn + AcceptedOn & check statuses have been updated
            Drive.Msmq.Payments.Send(new SignApplicationCommand() { AccountId = accountId, ApplicationId = appId, CreatedOn = DateTime.Now.AddHours(-1) });
            Drive.Msmq.Payments.Send(new IApplicationAcceptedEvent() { AccountId = accountId, ApplicationId = appId, CreatedOn = DateTime.Now.AddHours(-1) });
            Do.Until(
                () =>
                Drive.Db.Payments.Applications.Single(a => a.ExternalId == appId && a.SignedOn != null && a.AcceptedOn != null));

            // Create transactions & Check transactions have been created
            var trnGuid1 = CreateLoanAdvanceTransaction(appId);
            var trnGuid2 = CreateTransmissionFeeTransaction(appId);
            var trnGuid3 = CreateMissedPaymentChargeTransaction(appId);
            Do.Until(
                () =>
                Drive.Db.Payments.Transactions.Count(
                    itm => itm.ExternalId == trnGuid1 || itm.ExternalId == trnGuid2 || itm.ExternalId == trnGuid3) == 3);

            // Send command to create scheduled payment request
            Drive.Msmq.Payments.Send(new CreateScheduledPaymentRequestCommand() { ApplicationId = appId, RepaymentRequestId = requestId1, });
            Drive.Msmq.Payments.Send(new CreateScheduledPaymentRequestCommand() { ApplicationId = appId, RepaymentRequestId = requestId2, });

            Do.Until(() => Drive.Db.Payments.RepaymentRequests.Count(itm => itm.ExternalId == requestId1));
            Do.Until(() => Drive.Db.Payments.RepaymentRequests.Count(itm => itm.ExternalId == requestId2));

            // Go to DB and set Application NextDueDate to yesterday.
            ApplicationEntity app = Drive.Db.Payments.Applications.Single(a => a.ExternalId == appId);
            FixedTermLoanApplicationEntity fixedApp = Drive.Db.Payments.FixedTermLoanApplications.Single(a => a.ApplicationId == app.ApplicationId);
            fixedApp.NextDueDate = DateTime.UtcNow.Date.AddDays(-3);
            fixedApp.Submit(true);
        }

        public void Scenario12Setup(Guid requestId1, Guid requestId2, Guid appId, Guid bankAccountId, Guid accountId, Guid paymentCardId)
        {
            // Create Account so that time zone can be looked up
            Drive.Msmq.Payments.Send(new IAccountCreatedEvent() { AccountId = accountId });
            Do.Until(() => Drive.Db.Payments.AccountPreferences.Single(a => a.AccountId == accountId));

            // Create Application & Check it Exists in DB
            CreateFixedTermLoanApplication(appId, accountId, bankAccountId, paymentCardId);
            Do.Until(() => Drive.Db.Payments.Applications.Single(a => a.ExternalId == appId));

            // Set SignedOn + AcceptedOn & check statuses have been updated
            Drive.Msmq.Payments.Send(new SignApplicationCommand() { AccountId = accountId, ApplicationId = appId, CreatedOn = DateTime.Now.AddHours(-1) });
            Drive.Msmq.Payments.Send(new IApplicationAcceptedEvent() { AccountId = accountId, ApplicationId = appId, CreatedOn = DateTime.Now.AddHours(-1) });
            Do.Until(
                () =>
                Drive.Db.Payments.Applications.Single(a => a.ExternalId == appId && a.SignedOn != null && a.AcceptedOn != null));

            // Create transactions & Check transactions have been created
            var trnGuid1 = CreateLoanAdvanceTransaction(appId);
            var trnGuid2 = CreateTransmissionFeeTransaction(appId);
            var trnGuid3 = CreateMissedPaymentChargeTransaction(appId);
            Do.Until(
                () =>
                Drive.Db.Payments.Transactions.Count(
                    itm => itm.ExternalId == trnGuid1 || itm.ExternalId == trnGuid2 || itm.ExternalId == trnGuid3) == 3);

            // Send command to create scheduled payment request
            Drive.Msmq.Payments.Send(new CreateScheduledPaymentRequestCommand() { ApplicationId = appId, RepaymentRequestId = requestId1, });
            Drive.Msmq.Payments.Send(new CreateScheduledPaymentRequestCommand() { ApplicationId = appId, RepaymentRequestId = requestId2, });

            Do.Until(() => Drive.Db.Payments.RepaymentRequests.Count(itm => itm.ExternalId == requestId1));
            Do.Until(() => Drive.Db.Payments.RepaymentRequests.Count(itm => itm.ExternalId == requestId2));

            // Go to DB and set Application NextDueDate to yesterday.
            ApplicationEntity app = Drive.Db.Payments.Applications.Single(a => a.ExternalId == appId);
            FixedTermLoanApplicationEntity fixedApp =
                Drive.Db.Payments.FixedTermLoanApplications.Single(a => a.ApplicationId == app.ApplicationId);
            fixedApp.NextDueDate = DateTime.UtcNow.Date.AddDays(-31);
            fixedApp.Submit(true);
        }

        public void Scenario13Setup(Guid requestId1, Guid requestId2, Guid appId, Guid bankAccountId, Guid accountId, Guid paymentCardId)
        {
            // Create Account so that time zone can be looked up
            Drive.Msmq.Payments.Send(new IAccountCreatedEvent() { AccountId = accountId });
            Do.Until(() => Drive.Db.Payments.AccountPreferences.Single(a => a.AccountId == accountId));

            // Create Application & Check it Exists in DB
            CreateFixedTermLoanApplication(appId, accountId, bankAccountId, paymentCardId);
            Do.Until(() => Drive.Db.Payments.Applications.Single(a => a.ExternalId == appId));

            // Set SignedOn + AcceptedOn & check statuses have been updated
            Drive.Msmq.Payments.Send(new SignApplicationCommand() { AccountId = accountId, ApplicationId = appId, CreatedOn = DateTime.Now.AddHours(-1) });
            Drive.Msmq.Payments.Send(new IApplicationAcceptedEvent() { AccountId = accountId, ApplicationId = appId, CreatedOn = DateTime.Now.AddHours(-1) });
            Do.Until(() => Drive.Db.Payments.Applications.Single(a => a.ExternalId == appId && a.SignedOn != null && a.AcceptedOn != null));

            // Create transactions & Check transactions have been created
            var trnGuid1 = CreateLoanAdvanceTransaction(appId);
            var trnGuid2 = CreateTransmissionFeeTransaction(appId);
            var trnGuid3 = CreateMissedPaymentChargeTransaction(appId);
            Do.Until(() => Drive.Db.Payments.Transactions.Count(itm => itm.ExternalId == trnGuid1 || itm.ExternalId == trnGuid2 || itm.ExternalId == trnGuid3) == 3);

            // Send command to create scheduled payment request
            Drive.Msmq.Payments.Send(new CreateScheduledPaymentRequestCommand() { ApplicationId = appId, RepaymentRequestId = requestId1, });
            Drive.Msmq.Payments.Send(new CreateScheduledPaymentRequestCommand() { ApplicationId = appId, RepaymentRequestId = requestId2, });

            Do.Until(() => Drive.Db.Payments.RepaymentRequests.Count(itm => itm.ExternalId == requestId1));
            Do.Until(() => Drive.Db.Payments.RepaymentRequests.Count(itm => itm.ExternalId == requestId2));

            // Go to DB and set Application NextDueDate to yesterday.
            ApplicationEntity app = Drive.Db.Payments.Applications.Single(a => a.ExternalId == appId);
            FixedTermLoanApplicationEntity fixedApp = Drive.Db.Payments.FixedTermLoanApplications.Single(a => a.ApplicationId == app.ApplicationId);
            fixedApp.NextDueDate = DateTime.UtcNow.Date.AddDays(-61);
            fixedApp.Submit(true);
        }

        public void Scenario14Setup(Guid requestId1, Guid requestId2, int applicationId, Guid accountId, Guid appId,
                                    Guid paymentCardId, Guid bankAccountId)
        {
            const string minDays = "0";

            var inArrearsMinDays = Drive.Db.Ops.ServiceConfigurations.Single(a => a.Key == "Payments.InArrearsMinDays");
            inArrearsMinDays.Value = minDays;
            inArrearsMinDays.Submit();

            // Create Account so that time zone can be looked up
            Drive.Msmq.Payments.Send(new IAccountCreatedEvent() { AccountId = accountId });
            Do.Until(() => Drive.Db.Payments.AccountPreferences.Single(a => a.AccountId == accountId));

            // Create Application & Check it Exists in DB
            CreateFixedTermLoanApplication(appId, accountId, bankAccountId, paymentCardId);
            Do.Until(() => Drive.Db.Payments.Applications.Single(a => a.ExternalId == appId));

            // Set SignedOn + AcceptedOn & check statuses have been updated
            Drive.Msmq.Payments.Send(new SignApplicationCommand() { AccountId = accountId, ApplicationId = appId, CreatedOn = DateTime.Now.AddHours(-1) });
            Drive.Msmq.Payments.Send(new IApplicationAcceptedEvent() { AccountId = accountId, ApplicationId = appId, CreatedOn = DateTime.Now.AddHours(-1) });
            Do.Until(
                () =>
                Drive.Db.Payments.Applications.Single(a => a.ExternalId == appId && a.SignedOn != null && a.AcceptedOn != null));

            // Create transactions & Check transactions have been created
            var trnGuid1 = CreateLoanAdvanceTransaction(appId);
            var trnGuid2 = CreateTransmissionFeeTransaction(appId);
            var trnGuid3 = CreateMissedPaymentChargeTransaction(appId);
            Do.Until(
                () =>
                Drive.Db.Payments.Transactions.Count(
                    itm => itm.ExternalId == trnGuid1 || itm.ExternalId == trnGuid2 || itm.ExternalId == trnGuid3) == 3);

            // Send command to create scheduled payment request
            Drive.Msmq.Payments.Send(new CreateScheduledPaymentRequestCommand() { ApplicationId = appId, RepaymentRequestId = requestId1, });
            Drive.Msmq.Payments.Send(new CreateScheduledPaymentRequestCommand() { ApplicationId = appId, RepaymentRequestId = requestId2, });

            Do.Until(() => Drive.Db.Payments.RepaymentRequests.Count(itm => itm.ExternalId == requestId1));
            Do.Until(() => Drive.Db.Payments.RepaymentRequests.Count(itm => itm.ExternalId == requestId2));

            // Go to DB and set Application NextDueDate to yesterday.
            ApplicationEntity app = Drive.Db.Payments.Applications.Single(a => a.ExternalId == appId);
            applicationId = app.ApplicationId;
            FixedTermLoanApplicationEntity fixedApp =
                Drive.Db.Payments.FixedTermLoanApplications.Single(a => a.ApplicationId == app.ApplicationId);
            fixedApp.NextDueDate = DateTime.UtcNow.Date.AddDays(-61);
            fixedApp.Submit(true);

            // Put application into arrears
            Drive.Msmq.Payments.Send(new AddArrearsCommand()
            {
                ApplicationId = applicationId,
                PaymentTransactionType = PaymentTransactionEnum.CardPayment,
                ReferenceId = Guid.NewGuid()
            });
            Do.Until(() => Drive.Db.Payments.Arrears.Single(a => a.ApplicationId == applicationId));

            // Create Repayment Arrangement
            var dateTimes = new DateTime[]
                                {
                                    DateTime.UtcNow.AddDays(10),
                                };
            Drive.Msmq.Payments.Send(new CreateRepaymentArrangementCommand()
            {
                ApplicationId = appId,
                Frequency = PaymentFrequencyEnum.Monthly,
                NumberOfMonths = 1,
                RepaymentDates = dateTimes,
                CreatedOn = DateTime.UtcNow
            });
            Do.Until(() => Drive.Db.Payments.RepaymentArrangements.Single(a => a.ApplicationId == applicationId));
        }

        private void CreateFixedTermLoanApplication(Guid appId, Guid accountId, Guid bankAccountId, Guid paymentCardId, int dueInDays = 10)
        {
            Drive.Msmq.Payments.Send(new CreateFixedTermLoanApplicationCommand()
            {
                ApplicationId = appId,
                AccountId = accountId,
                PromiseDate = DateTime.UtcNow.AddDays(dueInDays),
                BankAccountId = bankAccountId,
                PaymentCardId = paymentCardId,
                LoanAmount = 100.0M,
                Currency = CurrencyCodeIso4217Enum.GBP,
                CreatedOn = DateTime.UtcNow
            });
        }

        private Guid CreateLoanAdvanceTransaction(Guid appId)
        {
            var trnGuid1 = Guid.NewGuid();

            Drive.Msmq.Payments.Send(new CreateTransactionCommand()
            {
                ApplicationId = appId,
                ExternalId = trnGuid1,
                Amount = 100.00M,
                Type = PaymentTransactionEnum.CashAdvance,
                Currency = CurrencyCodeIso4217Enum.GBP,
                Mir = 30.0M,
                PostedOn = DateTime.Now,
                Scope = PaymentTransactionScopeEnum.Debit,
                Reference = "Test Cash Advance"
            });
            return trnGuid1;
        }

        private Guid CreateTransmissionFeeTransaction(Guid appId)
        {
            var trnGuid1 = Guid.NewGuid();

            Drive.Msmq.Payments.Send(new CreateTransactionCommand()
            {
                ApplicationId = appId,
                ExternalId = trnGuid1,
                Amount = 5.50M,
                Type = PaymentTransactionEnum.Fee,
                Currency = CurrencyCodeIso4217Enum.GBP,
                Mir = 30.0M,
                PostedOn = DateTime.Now,
                Scope = PaymentTransactionScopeEnum.Debit,
                Reference = "Test Transmission fee"
            });
            return trnGuid1;
        }

        private Guid CreateExtensionFeeTransaction(Guid appId)
        {
            var trnGuid1 = Guid.NewGuid();

            Drive.Msmq.Payments.Send(new CreateTransactionCommand()
            {
                ApplicationId = appId,
                ExternalId = trnGuid1,
                Amount = 20.00M,
                Type = PaymentTransactionEnum.LoanExtensionFee,
                Currency = CurrencyCodeIso4217Enum.GBP,
                Mir = 30.0M,
                PostedOn = DateTime.Now,
                Scope = PaymentTransactionScopeEnum.Debit,
                Reference = "Test Extension fee"
            });
            return trnGuid1;
        }

        private Guid CreateMissedPaymentChargeTransaction(Guid appId)
        {
            var trnGuid1 = Guid.NewGuid();

            Drive.Msmq.Payments.Send(new CreateTransactionCommand()
            {
                ApplicationId = appId,
                ExternalId = trnGuid1,
                Amount = 20.00M,
                Type = PaymentTransactionEnum.DefaultCharge,
                Currency = CurrencyCodeIso4217Enum.GBP,
                Mir = 30.0M,
                PostedOn = DateTime.Now,
                Scope = PaymentTransactionScopeEnum.Debit,
                Reference = "Test Missed Payment Charge"
            });
            return trnGuid1;
        }
    }
}
