using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Threading;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db.Payments;
using Wonga.QA.Framework.Db.Risk;

namespace Wonga.QA.Framework.Db.Extensions
{
	public static partial class DbExtensions
	{
		public static void Rewind(this DbDriver db, Guid applicationId, int absoluteDays)
		{
			// Rewinds a Loans Dates
			ApplicationEntity application = db.Payments.Applications.Single(a => a.ExternalId == applicationId);
			RiskApplicationEntity riskApplication = db.Risk.RiskApplications.Single(r => r.ApplicationId == applicationId);

			if (application.FixedTermLoanApplicationEntity.NextDueDate == null)
			{
				throw new Exception("Rewind: FixedTermLoanApplication.NextDueDate is null");
			}

			var duration = new TimeSpan(absoluteDays, 0, 0, 0);

			db.RewindApplicationDates(application, riskApplication, duration);
		}

        public static void UpdateNextDueDate(this DbDriver db, FixedTermLoanApplicationEntity fixedApp, TimeSpan span)
        {
            var dt = DateTime.UtcNow;

            try
            {
                fixedApp.NextDueDate = (dt += span);
                fixedApp.Submit(true);
            }
            catch (Exception)
            {
               // Retry in case of deadlocks
               Thread.Sleep(1000);
               fixedApp.NextDueDate = (dt += span);
               fixedApp.Submit(true);
            }
        }

        public static void MoveAcceptedOnDate(this DbDriver db, ApplicationEntity app, TimeSpan span)
        {
            try
            {
                app.AcceptedOn += span;
                app.Submit(true);
            }
            catch (Exception)
            {
                // Retry in case of deadlocks
                Thread.Sleep(1000);
                app.AcceptedOn += span;
                app.Submit(true);
            }
        }

	    public static void RewindApplicationDates(this DbDriver db, ApplicationEntity application, RiskApplicationEntity riskApp, TimeSpan span)
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

			riskApp.ApplicationDate -= span;
			riskApp.PromiseDate -= span;
			if (riskApp.ClosedOn != null)
				riskApp.ClosedOn -= span;
			riskApp.Submit(true);
		}

		public static void RewindToDayOfLoanTerm(this DbDriver db, Guid applicationId, int dayOfLoanTerm)
		{
			var daysToRewind = db.GetAbsoluteDaysToRewind(dayOfLoanTerm);

			db.Rewind(applicationId, daysToRewind);
		}

		public static int GetAbsoluteDaysToRewind(this DbDriver db, int dayOfLoanToMakeRepayment)
		{
			int daysUntilStartOfLoan = db.GetNumberOfDaysUntilStartOfLoan();

			int daysToRewind = daysUntilStartOfLoan + dayOfLoanToMakeRepayment - 1;
			return daysToRewind;
		}

		public static int GetNumberOfDaysUntilStartOfLoan(this DbDriver db, DateTime? loanCreatedOn = null)
		{
			switch (Config.AUT)
			{
					case AUT.Ca:
					{
						if (!loanCreatedOn.HasValue)
							loanCreatedOn = DateTime.Now;

					    // In CA loans start the day after the loan is created
                        DateTime firstDayOfLoan = db.GetNextWorkingDay(new Date(loanCreatedOn.Value.AddDays(1)));

                        return (int)(firstDayOfLoan.Subtract(loanCreatedOn.Value)).TotalDays;
					}
					break;

				default:
					{
						return 0;
					}
					break;
			}
		}
	}
}
