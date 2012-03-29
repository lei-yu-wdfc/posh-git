using System;
using System.Collections.Generic;
using System.Linq;
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

		public static void RewindApplicationDates(this DbDriver db, ApplicationEntity application, RiskApplicationEntity riskApp, TimeSpan span)
		{
			application.ApplicationDate -= span;
			application.SignedOn -= span;
			application.CreatedOn -= span;
			application.AcceptedOn -= span;
			application.FixedTermLoanApplicationEntity.PromiseDate -= span;
			application.FixedTermLoanApplicationEntity.NextDueDate -= span;
			application.Transactions.ForEach(t => t.PostedOn -= span);
			if (application.ClosedOn != null)
				application.ClosedOn -= span;
			application.Submit(true);

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

		public static int GetNumberOfDaysUntilStartOfLoan(this DbDriver db, DateTime? fromDate = null)
		{
			switch (Config.AUT)
			{
					case AUT.Ca:
					{
						if (!fromDate.HasValue)
							fromDate = DateTime.Now;

						var nextWorkingDay = db.GetNextWorkingDay(new Date(fromDate.Value));

						return (int)(nextWorkingDay.DateTime.Subtract(fromDate.Value)).TotalDays;
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
