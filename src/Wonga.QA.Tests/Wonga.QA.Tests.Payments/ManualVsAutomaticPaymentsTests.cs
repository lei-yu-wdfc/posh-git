using System;
using System.Globalization;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Cs.Requests.Payments.Csapi.Commands;
using Wonga.QA.Framework.Cs.Requests.Payments.Csapi.Queries;
using Wonga.QA.Framework.Msmq.Enums.Integration.Payments.Enums;
using Wonga.QA.Tests.Core;
using System.Linq;

namespace Wonga.QA.Tests.Payments
{
	[TestFixture, Parallelizable((TestScope.All))]
	public class ManualVsAutomaticPaymentsTests
	{
		private static readonly dynamic Transactions = Drive.Data.Payments.Db.Transactions;

		[Test, AUT(AUT.Uk), JIRA("UKOPS-488"), Owner(Owner.ShaneMcHugh)]
		public void ShouldCreateAutomaticTransactionWhenApplicationIsDueToday()
		{
			var customer = CustomerBuilder.New().Build();
			var application = ApplicationBuilder.New(customer).Build();

			application.MakeDueToday();

			CheckDbForTransactionEntry(application, "Automatic Ping (Card)");
			CheckTransactionsQuery(application, "Automatic Ping (Card)");
		}

		[Test, AUT(AUT.Uk), JIRA("UKOPS-488"), Owner(Owner.ShaneMcHugh)]
		public void ShouldCreateManualTransactionWhenCsRepayWithPaymentCardCommandIsValid()
		{
			var customer = CustomerBuilder.New().Build();
			var application = ApplicationBuilder.New(customer).Build();

			Guid paymentId = Guid.NewGuid();
			Drive.Cs.Commands.Post(new CsRepayWithPaymentCardCommand
			{
				AccountId = application.AccountId,
				Amount = 100.00m,
				SalesforceUser = "csUser",
				CV2 = "111",
				Currency = "GBP",
				PaymentCardId = customer.GetPaymentCard(),
				PaymentId = paymentId
			});

			CheckDbForTransactionEntry(application, "Payment card repayment from CS");
			CheckTransactionsQuery(application, "Payment card repayment from CS");
		}

		[Test, AUT(AUT.Uk), JIRA("UKOPS-488"), Owner(Owner.ShaneMcHugh)]
		public void ShouldCreateManualTransactionWhenCsRepayWithExternalCardCommandIsValid()
		{
			var customer = CustomerBuilder.New().Build();
			var application = ApplicationBuilder.New(customer).Build();

			Guid paymentId = Guid.NewGuid();
			Drive.Cs.Commands.Post(new CsRepayWithExternalCardCommand
				{
					AccountId = application.AccountId,
					AddressLine1 = "line 1",
					AddressLine2 = "line 2",
					Amount = 100.00m,
					CardNumber = "5411111111111111",
					CardType = "visaDebit",
					Country = "UK",
					County = "county",
					SalesforceUser = "csUser",
					Currency = "GBP",
					CV2 = "121",
					ExpiryDate = new DateTime(DateTime.Now.Year + 1, 1, 31),
					HolderName = "holder name",
					PostCode = "12345",
					Town = "town",
					PaymentId = paymentId
				});

			CheckDbForTransactionEntry(application, "External payment card repayment from CS");
			CheckTransactionsQuery(application, "External payment card repayment from CS");
		}

		#region Helpers#

		private void CheckDbForTransactionEntry(dynamic application, string reference)
		{
			int appId = ApplicationOperations.GetAppInternalId(application);
			int scope = (int)PaymentTransactionScopeEnum.Credit;
			string type = PaymentTransactionEnum.CardPayment.ToString();
			Do.Until(() => Transactions.FindAllBy(Reference: reference, ApplicationId: appId, Scope: scope, Type: type, Amount: application.GetDueDateBalance()));
		}

		private void CheckTransactionsQuery(dynamic application, string reference)
		{
			Do.Until(
				() =>
				Drive.Cs.Queries.Post(new GetTransactionsQuery() { ApplicationGuid = application.Id }).Values["Reference"].
				Last().ToString(CultureInfo.InvariantCulture).Equals(reference));
		}

		#endregion helpers#
	}


}
