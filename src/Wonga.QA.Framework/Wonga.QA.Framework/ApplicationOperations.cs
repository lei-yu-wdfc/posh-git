using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    }
}
