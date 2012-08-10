using System;

namespace Wonga.QA.Framework.Application.Operations
{
	public class ApplicationOperationsTimeTravel
	{
		public void RewindApplicationDates(ApplicationBase application, TimeSpan span)
		{
			RewindApplicationEntityPayments(application.Id, span);
			RewindApplicationEntityRisk(application.Id, span);
		}

		private void RewindApplicationEntityPayments(Guid applicationGuid, TimeSpan span)
		{
			var paymentsApplicationId = (int)Drive.Data.Payments.Db.Applications.FindByExternalId(applicationGuid).ApplicationId;

			RewindPaymentsApplicationEntity(paymentsApplicationId, span);
			RewindPaymentsFixedTermLoanApplicationEntity(paymentsApplicationId, span);
			RewindPaymentsTransactionEntities(paymentsApplicationId, span);
			RewindPaymentsArrearsEntities(paymentsApplicationId, span);
		}

		private void RewindPaymentsApplicationEntity(int paymentsApplicationId, TimeSpan span)
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

		private void RewindPaymentsFixedTermLoanApplicationEntity(int paymentsApplicationId, TimeSpan span)
		{
			var fixedTermAppEntity = Drive.Data.Payments.Db.FixedTermLoanApplications.FindByApplicationId(paymentsApplicationId);
			fixedTermAppEntity.PromiseDate -= span;
			fixedTermAppEntity.NextDueDate -= span;
			Drive.Data.Payments.Db.FixedTermLoanApplications.Update(fixedTermAppEntity);
		}

		private void RewindPaymentsTransactionEntities(int paymentsApplicationId, TimeSpan span)
		{
			var transactions = Drive.Data.Payments.Db.Transactions.FindAllByApplicationId(paymentsApplicationId);

			foreach (var transaction in transactions)
			{
				transaction.CreatedOn -= span;
				transaction.PostedOn -= span;
				Drive.Data.Payments.Db.Transactions.Update(transaction);
			}
		}

		private void RewindPaymentsArrearsEntities(int paymentsApplicationId, TimeSpan span)
		{
			var arrearEntity = Drive.Data.Payments.Db.Arrears.FindByApplicationId(paymentsApplicationId);

			if (arrearEntity != null)
			{
				arrearEntity.CreatedOn -= span;
				Drive.Data.Payments.Db.Arrears.Update(arrearEntity);
			}
		}

		private void RewindApplicationEntityRisk(Guid applicationGuid, TimeSpan span)
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
