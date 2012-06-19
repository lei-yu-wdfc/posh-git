using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Msmq;
using Wonga.QA.Tests.Core;
using CreateFixedTermLoanApplicationCommand = Wonga.QA.Framework.Api.CreateFixedTermLoanApplicationZaCommand;
using PaymentTransactionEnum = Wonga.QA.Framework.Msmq.PaymentTransactionEnum;
using SignApplicationCommand = Wonga.QA.Framework.Api.SignApplicationCommand;

namespace Wonga.QA.Tests.Payments
{
	[TestFixture, Parallelizable(TestScope.All)]
	public class MoveApplicationToDcaTests
	{
		private Application _application;
        private dynamic _debtCollections = Drive.Data.Payments.Db.DebtCollection;
        private dynamic _externalDebtCollectionSagas = Drive.Data.OpsSagas.Db.ExternalDebtCollectionSagaEntity;
	    private dynamic _transactions = Drive.Data.Payments.Db.Transactions;


		[Test, AUT(AUT.Za), JIRA("ZA-2256")]
		public void MoveApplicationToDca_AfterDcaDelayPeriod()
		{
			var customer = CustomerBuilder.New().Build();
			_application =
				ApplicationBuilder.New(customer).WithExpectedDecision(ApplicationDecisionStatus.Accepted).WithLoanAmount(100).
					Build().PutIntoArrears();

			//Force dca to timeout immmediately
			var externalDebtCollectionSagaEntities = Do.Until(() => _externalDebtCollectionSagas.FindAllByApplicationId(_application.Id).Single());

			new MsmqDriver().Payments.Send(new TimeoutMessage()
			{
				SagaId = externalDebtCollectionSagaEntities.Id,
			});

			var debtCollection = Do.Until(() => _debtCollections.FindAll(_debtCollections.Applications.ExternalId == _application.Id).Single());

			Assert.AreEqual(debtCollection.MovedToAgency, true);
		}

		[Test, AUT(AUT.Za), JIRA("ZA-2256"), DependsOn("MoveApplicationToDca_AfterDcaDelayPeriod")]
		public void MoveApplicationToDca_AfterDcaDelayPeriod_SuspendInterstTransactionIsPosted()
		{
			var loanApp = Do.Until(() => Drive.Data.Payments.Db.Applications.FindAllByExternalId(_application.Id).Single());
			var suspendInterstTransaction = Do.Until(() => _transactions.FindAll(_transactions.ApplicationId == loanApp.ApplicationId && 
                                                           _transactions.Type == PaymentTransactionEnum.SuspendInterestAccrual.ToString()).FirstOrDefault());

			Assert.IsNotNull(suspendInterstTransaction);
		}
	}
}