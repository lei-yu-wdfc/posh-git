using System;

namespace Wonga.QA.Framework.Application.Operations.Consumer
{
	public sealed class ConsumerApplicationOperationsTimeShift
	{
		public void TimeShiftApplicationDates(ApplicationBase application, TimeSpan span)
		{
			TimeShiftApplicationEntityPayments(application.Id, span);
			TimeShiftApplicationEntityRisk(application.Id, span);
		}

		private void TimeShiftApplicationEntityPayments(Guid applicationGuid, TimeSpan span)
		{
			var paymentsApplicationId = (int)Drive.Data.Payments.Db.Applications.FindByExternalId(applicationGuid).ApplicationId;

			TimeShiftPaymentsApplicationEntity(paymentsApplicationId, span);
			TimeShiftPaymentsFixedTermLoanApplicationEntity(paymentsApplicationId, span);
			TimeShiftPaymentsTransactionEntities(paymentsApplicationId, span);
			TimeShiftPaymentsArrearsEntities(paymentsApplicationId, span);
		}

		private void TimeShiftPaymentsApplicationEntity(int paymentsApplicationId, TimeSpan span)
		{
			var appEntity = Drive.Data.Payments.Db.Applications.FindByApplicationId(paymentsApplicationId);
			appEntity.ApplicationDate -= span;
			appEntity.SignedOn -= span;
			appEntity.CreatedOn -= span;
			appEntity.AcceptedOn -= span;
			if (appEntity.ClosedOn != null)
				appEntity.ClosedOn -= span;
			Drive.Data.Payments.Db.Applications.Update(appEntity);
		}

		private void TimeShiftPaymentsFixedTermLoanApplicationEntity(int paymentsApplicationId, TimeSpan span)
		{
			var fixedTermAppEntity = Drive.Data.Payments.Db.FixedTermLoanApplications.FindByApplicationId(paymentsApplicationId);
			fixedTermAppEntity.PromiseDate -= span;
			fixedTermAppEntity.NextDueDate -= span;
			Drive.Data.Payments.Db.FixedTermLoanApplications.Update(fixedTermAppEntity);
		}

		private void TimeShiftPaymentsTransactionEntities(int paymentsApplicationId, TimeSpan span)
		{
			var transactions = Drive.Data.Payments.Db.Transactions.FindAllByApplicationId(paymentsApplicationId);

			foreach (var transaction in transactions)
			{
				transaction.CreatedOn -= span;
				transaction.PostedOn -= span;
				Drive.Data.Payments.Db.Transactions.Update(transaction);
			}
		}

		private void TimeShiftPaymentsArrearsEntities(int paymentsApplicationId, TimeSpan span)
		{
			var arrearEntity = Drive.Data.Payments.Db.Arrears.FindByApplicationId(paymentsApplicationId);

			if (arrearEntity != null)
			{
				arrearEntity.CreatedOn -= span;
				Drive.Data.Payments.Db.Arrears.Update(arrearEntity);
			}
		}

		private void TimeShiftApplicationEntityRisk(Guid applicationGuid, TimeSpan span)
		{
			var riskAppEntity = Drive.Data.Risk.Db.RiskApplications.FindByApplicationId(applicationGuid);

			riskAppEntity.ApplicationDate -= span;
			riskAppEntity.PromiseDate -= span;
			if (riskAppEntity.ClosedOn != null)
				riskAppEntity.ClosedOn -= span;

			Drive.Data.Risk.Db.RiskApplications.Update(riskAppEntity);
		}
	}
}
