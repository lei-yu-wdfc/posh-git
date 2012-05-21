using System;
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
        /*
        /// <summary>
        /// this is the new RewindApplicationDates method for Drive.Data
        /// </summary>
        /// <param name="db"></param>
        /// <param name="application"></param>
        /// <param name="riskApp"></param>
        /// <param name="span"></param>
        public static void RewindApplicationDates(this DataDriver db, ApplicationEntity application, RiskApplicationEntity riskApp, TimeSpan span)
        {
            application.ApplicationDate -= span;
            application.SignedOn -= span;
            application.CreatedOn -= span;
            application.AcceptedOn -= span;
            application.FixedTermLoanApplicationEntity.PromiseDate -= span;
            application.FixedTermLoanApplicationEntity.NextDueDate -= span;
            if (application.ClosedOn != null)
                application.ClosedOn -= span;
            application.Submit(true);

            application.Transactions.ForEach(t => t.CreatedOn -= span);
            application.Transactions.ForEach(t => t.PostedOn -= span);
            application.Transactions.ForEach(t => t.Submit(true));

            if (application.ArrearEntity != null)
            {
                application.ArrearEntity.CreatedOn -= span;
                application.ArrearEntity.Submit(true);
            }

            riskApp.ApplicationDate -= span;
            riskApp.PromiseDate -= span;
            if (riskApp.ClosedOn != null)
                riskApp.ClosedOn -= span;
            riskApp.Submit(true);
        }*/
    }
}
