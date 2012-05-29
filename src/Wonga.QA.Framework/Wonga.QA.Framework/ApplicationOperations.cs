﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Data;
using Wonga.QA.Framework.Db.Extensions;
using Wonga.QA.Framework.Db.Payments;
using Wonga.QA.Framework.Db.Risk;

namespace Wonga.QA.Framework
{
    public static class ApplicationOperations
    {
        public static Application DaysFromStart(this Application app, int daysAgo)
        {
            ApplicationEntity application = Drive.Db.Payments.Applications.Single(a => a.ExternalId == app.Id);
            TimeSpan span = application.FixedTermLoanApplicationEntity.NextDueDate.Value - DateTime.Today.AddDays(app.LoanTerm - daysAgo);
            RiskApplicationEntity riskApplication = Drive.Db.Risk.RiskApplications.Single(r => r.ApplicationId == app.Id);

            Drive.Db.RewindApplicationDates(application, riskApplication, span);
            return app;
        }
        
        /// <summary>
        /// this is the new RewindApplicationDates method for Drive.Data
        /// </summary>
        /// <param name="application"></param>
        /// <param name="riskApp"></param>
        /// <param name="span"></param>
        public static void RewindApplicationDates(dynamic application, dynamic riskApp, TimeSpan span)
        {
            application.ApplicationDate -= span;
            application.SignedOn -= span;
            application.CreatedOn -= span;
            application.AcceptedOn -= span;

            var fixedTermLoanApplication =
                Drive.Data.Payments.Db.FixedTermLoanApplications.FindByApplicationId(application.ApplicationId);

            fixedTermLoanApplication.PromiseDate -= span;
            fixedTermLoanApplication.NextDueDate -= span;
            if (application.ClosedOn != null)
                application.ClosedOn -= span;

            Drive.Data.Payments.Db.Applications.Update(application);
            Drive.Data.Payments.Db.FixedTermLoanApplications.Update(fixedTermLoanApplication);

            var transactionsTab = Drive.Data.Payments.Db.Transactions;
            var transactions = transactionsTab.FindAllByApplicationId(application.ApplicationId);

            foreach(var transaction in transactions)
            {
                transaction.CreatedOn -= span;
                transaction.PostedOn -= span;
                transactionsTab.Update(transaction);
            }

            var arrearEntity = Drive.Data.Payments.Db.Arrears.FindByApplicationId(application.ApplicationId);

            if (arrearEntity != null)
            {
                arrearEntity.CreatedOn -= span;
                Drive.Data.Payments.Db.Arrears.Update(arrearEntity);
            }

            riskApp.ApplicationDate -= span;
            riskApp.PromiseDate -= span;
            if (riskApp.ClosedOn != null)
                riskApp.ClosedOn -= span;

            Drive.Data.Risk.Db.RiskApplications.Update(riskApp);
        }

        public static void RewindApplicationDates(dynamic application, TimeSpan span)
        {
            var paymentsAppsTab = Drive.Data.Payments.Db.Applications;
            dynamic applicationEntity =
                paymentsAppsTab.FindAll(paymentsAppsTab.ExternalId == application.Id).Single();

            var riskAppTab = Drive.Data.Risk.Db.RiskApplications;
            dynamic riskApplication = riskAppTab.FindAll(riskAppTab.ApplicationId == application.Id).Single();

            ApplicationOperations.RewindApplicationDates(applicationEntity, riskApplication, span);
        }
    }
}
