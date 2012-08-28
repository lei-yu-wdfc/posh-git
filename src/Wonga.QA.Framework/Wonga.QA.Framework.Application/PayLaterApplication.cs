using System;
using Wonga.QA.Framework.Application.Operations.PayLater;
using Wonga.QA.Framework.Application.Queries;

namespace Wonga.QA.Framework.Application
{
	public class PayLaterApplication : ApplicationBase
	{
		public PayLaterApplication(Guid accountId) : base(accountId)
		{
		}

		public override void Repay()
		{
			var totalAmount = ApplicationQueries.PayLater.GetAmountToRepay(Id);
			Repay(totalAmount);
		}

		public override void Repay(Decimal amount)
		{
			throw new NotImplementedException();
		}

		public void PutFirstInstallmentIntoArrears()
		{
			PayLaterApplicationOperations.Arrears.PutInstallmentIntoArrears(0);
		}

		public void PutSecondInstallmentIntoArrears()
		{
			PayLaterApplicationOperations.Arrears.PutInstallmentIntoArrears(1);
		}

		public void PutThirdInstallmentIntoArrears()
		{
			PayLaterApplicationOperations.Arrears.PutInstallmentIntoArrears(2);
		}


	}
}
