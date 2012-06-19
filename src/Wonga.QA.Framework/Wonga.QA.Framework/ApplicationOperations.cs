﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

            MoveApplicationTransactionDates(application, span);

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

        public static void RewindApplicationDates(Application application, TimeSpan span)
        {
            var paymentsAppsTab = Drive.Data.Payments.Db.Applications;
            dynamic applicationEntity =
                paymentsAppsTab.FindAll(paymentsAppsTab.ExternalId == application.Id).Single();

            var riskAppTab = Drive.Data.Risk.Db.RiskApplications;
            dynamic riskApplication = riskAppTab.FindAll(riskAppTab.ApplicationId == application.Id).Single();

            ApplicationOperations.RewindApplicationDates(applicationEntity, riskApplication, span);
        }

        public static void Rewind(Guid applicationId, int absoluteDays)
        {
            var appTab = Drive.Data.Payments.Db.Applications;
            var fixTermLoanAppTab = Drive.Data.Payments.Db.FixedTermLoanApplications;

            // Rewinds a Loans Dates
            var application = appTab.FindAll(appTab.ExternalId == applicationId).Single();
            var fixTermLoanAppTabRow =
                fixTermLoanAppTab.FindAll(fixTermLoanAppTab.ApplicationId == application.ApplicationId).Single();

            if (fixTermLoanAppTabRow.NextDueDate == null)
            {
                throw new Exception("Rewind: FixedTermLoanApplication.NextDueDate is null");
            }

            var riskAppTab = Drive.Data.Risk.Db.RiskApplications;
            dynamic riskApplication = riskAppTab.FindAll(riskAppTab.ApplicationId == applicationId).Single();

            var duration = new TimeSpan(absoluteDays, 0, 0, 0);

            RewindApplicationDates(application, riskApplication,duration);
        }

        public static void UpdateNextDueDate(dynamic fixedApp, TimeSpan span)
        {
            var dt = DateTime.UtcNow;
            var fixedTermLoanAppTab = Drive.Data.Payments.Db.FixedTermLoanApplications;

            try
            {
                fixedApp.NextDueDate = (dt += span);
                fixedTermLoanAppTab.Update(fixedApp);
            }
            catch (Exception)
            {
                // Retry in case of deadlocks
                Thread.Sleep(1000);
                fixedApp.NextDueDate = (dt += span);
                fixedTermLoanAppTab.Update(fixedApp);
            }
        }

        public static void MoveAcceptedOnDate(dynamic app, TimeSpan span)
        {
            var appTab = Drive.Data.Payments.Db.Applications;

            try
            {
                app.AcceptedOn += span;
                appTab.Update(app);
            }
            catch (Exception)
            {
                // Retry in case of deadlocks
                Thread.Sleep(1000);
                app.AcceptedOn += span;
                appTab.Update(app);
            }
        }

        public static void RewindToDayOfLoanTerm(Guid applicationId, int dayOfLoanTerm)
        {
            var daysToRewind = GetAbsoluteDaysToRewind(dayOfLoanTerm);

            Rewind(applicationId, daysToRewind);
        }

        public static int GetAbsoluteDaysToRewind(int dayOfLoanToMakeRepayment)
        {
            int daysUntilStartOfLoan = GetNumberOfDaysUntilStartOfLoan();

            int daysToRewind = daysUntilStartOfLoan + dayOfLoanToMakeRepayment - 1;
            return daysToRewind;
        }

        public static int GetNumberOfDaysUntilStartOfLoan(DateTime? loanCreatedOn = null)
        {
            switch (Config.AUT)
            {
                case AUT.Ca:
                    {
                        if (!loanCreatedOn.HasValue)
                            loanCreatedOn = DateTime.Now;

                        // In CA loans start the day after the loan is created
                        DateTime firstDayOfLoan = GetNextWorkingDay(new Date(loanCreatedOn.Value.AddDays(1)));

                        return (int)(firstDayOfLoan.Subtract(loanCreatedOn.Value)).TotalDays;
                    }
                default:
                    {
                        return 0;
                    }
            }
        }

        public static Date GetNextWorkingDay(Date date)
        {
            switch (Config.AUT)
            {
                case AUT.Za:
                    {
                        if (date.DateTime.DayOfWeek == DayOfWeek.Sunday) date.DateTime = date.DateTime.AddDays(1);
                        while (IsHoliday(date)) date.DateTime = date.DateTime.AddDays(1);
                        return date;
                    }

                default:
                    {
                        if (date.DateTime.DayOfWeek == DayOfWeek.Saturday) date.DateTime = date.DateTime.AddDays(2);
                        if (date.DateTime.DayOfWeek == DayOfWeek.Sunday) date.DateTime = date.DateTime.AddDays(1);
                        while (IsHoliday(date)) date.DateTime = date.DateTime.AddDays(1);
                        return date;
                    }
            }
        }

        public static bool IsHoliday(Date date)
        {
            // can't seem to also include IsBankHoliday column... works fine without it checked with the dates in the CalendarDates Table
            var calendarDatesTab = Drive.Data.Payments.Db.CalendarDates;
            return calendarDatesTab.FindAll(calendarDatesTab.Date == date).Any();
            
            
            //new DbDriver().Payments.CalendarDates.Any(a => a.IsBankHoliday && a.Date == date);
        }

        public static void MoveApplicationTransactionDates(dynamic application, TimeSpan span)
        {
            var transactionsTab = Drive.Data.Payments.Db.Transactions;
            var transactions = transactionsTab.FindAllByApplicationId(application.ApplicationId);

            foreach (var transaction in transactions)
            {
                transaction.CreatedOn -= span;
                transaction.PostedOn -= span;
                transactionsTab.Update(transaction);
            }
        }
    }
}
