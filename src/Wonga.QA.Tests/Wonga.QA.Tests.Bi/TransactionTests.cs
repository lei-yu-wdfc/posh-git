using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db.Payments;
using Wonga.QA.Framework.Msmq;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Bi
{
	[TestFixture]
	class TransactionTests
	{
		[Test, AUT(AUT.Za)]
		public void Transaction_StoredInTransactionTable_CashAdvance()
		{
			var typeName = "CashAdvance";

			var customer = CustomerBuilder.New().Build();
			var application = ApplicationBuilder.New(customer).Build();
			var transaction = GetTransactionFromPayments(application, typeName);
			WaitForTransactionExistsInBi(transaction);
		}

		[Test, AUT(AUT.Za)]
		public void Transaction_StoredInTransactionTable_InitiationFee()
		{
			var typeName = "InitiationFee";

			var customer = CustomerBuilder.New().Build();
			var application = ApplicationBuilder.New(customer).Build();
			var transaction = GetTransactionFromPayments(application, typeName);
			WaitForTransactionExistsInBi(transaction);
		}

		[Test, AUT(AUT.Za)]
		public void Transaction_StoredInTransactionTable_ServiceFee()
		{
			var typeName = "ServiceFee";

			var customer = CustomerBuilder.New().Build();
			var application = ApplicationBuilder.New(customer).Build();
			var transaction = GetTransactionFromPayments(application, typeName);
			WaitForTransactionExistsInBi(transaction);
		}

		[Test, AUT(AUT.Za)]
		public void Transaction_StoredInTransactionTable_SuspendInterestAccrual()
		{
			var typeName = "SuspendInterestAccrual";

			var customer = CustomerBuilder.New().Build();
			var application = ApplicationBuilder.New(customer).Build();
			var transaction = GetTransactionFromPayments(application, typeName);
			WaitForTransactionExistsInBi(transaction);
		}

		[Test, AUT(AUT.Za)]
		public void Transaction_StoredInTransactionTable_ResumeInterestAccrual()
		{
			var typeName = "ResumeInterestAccrual";

			var customer = CustomerBuilder.New().Build();
			var application = ApplicationBuilder.New(customer).Build();
			var transaction = GetTransactionFromPayments(application, typeName);
			WaitForTransactionExistsInBi(transaction);
		}

		#region Helpers

		private TransactionEntity GetTransactionFromPayments(Application application, string type)
		{
			Do.Until(
				() =>
				Driver.Db.Payments.Applications.Single(a => a.ExternalId == application.Id).Transactions.Any(
					t => t.Type == type));

			return Driver.Db.Payments.Applications.Single(a => a.ExternalId == application.Id).Transactions.Last(t => t.Type == type);
		}

		private void WaitForTransactionExistsInBi(TransactionEntity transaction)
		{
			Do.Until(() => Driver.Db.Bi.Transactions.Single(t => t.TransactionNKey == transaction.ExternalId));
		}

		#endregion
	}
}
