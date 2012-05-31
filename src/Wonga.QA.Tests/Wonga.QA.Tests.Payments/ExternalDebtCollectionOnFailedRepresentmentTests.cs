using System;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Msmq;
using Wonga.QA.Tests.Core;
using Wonga.QA.Tests.Payments.Helpers;

namespace Wonga.QA.Tests.Payments
{
    [Parallelizable(TestScope.All)]
    public class ExternalDebtCollectionOnFailedRepresentmentTests
    {
    	private const string FeatureSwitchKey = "FeatureSwitch.MoveLoanToDcaOnFailedRepresentment";
    	private static bool _bankGatewayIsinTestMode;
    	private readonly dynamic _debtCollection = Drive.Data.Payments.Db.DebtCollection;
    	private readonly dynamic _externalDebtCollectionSaga = Drive.Data.OpsSagas.Db.ExternalDebtCollectionOnFailedRepresentmentSagaEntity;

    	[FixtureSetUp]
        public static void FixtureSetUp()
        {
            _bankGatewayIsinTestMode = ConfigurationFunctions.GetBankGatewayTestMode();

            ConfigurationFunctions.SetBankGatewayTestMode(false);
        }

        [FixtureTearDown]
        public static void FixtureTearDown()
        {
            ConfigurationFunctions.SetBankGatewayTestMode(_bankGatewayIsinTestMode);
        }

		[Test, AUT(AUT.Ca), JIRA("CA-2285"), FeatureSwitch(FeatureSwitchKey)]
		public void WhenLoanClosedThenShouldNotMoveApplicationToDca()
		{
			var customer = CustomerBuilder.New().Build();
			var application = ApplicationBuilder.New(customer).Build();

			application.PutApplicationIntoArrears();

			// Validate that external debt collection finished
			Do.Until(() => _externalDebtCollectionSaga.FindByApplicationId(application.Id));

			Drive.Msmq.Payments.Send(new IApplicationClosedEvent
			                         	{
			                         		AccountId = customer.Id,
											ApplicationId = application.Id,
											ClosedOn = DateTime.UtcNow
			                         	});

    		// Validate that external debt collection finished
			Do.Until(() => _externalDebtCollectionSaga.FindByApplicationId(application.Id) == null);

			// And no debt collection record was created
			Assert.AreEqual(0, _debtCollection.FindAll(_debtCollection.Applications.ExternalId == application.Id).Count());
		}

		[Test, AUT(AUT.Ca), JIRA("CA-2285"), FeatureSwitch(FeatureSwitchKey)]
		public void When31DaysPassedAndRepresentmentFailedThenShouldMoveApplicationToDca()
		{
			var customer = CustomerBuilder.New().Build();
			var application = ApplicationBuilder.New(customer).Build();

			application.PutApplicationIntoArrears();

			Drive.Msmq.Payments.Send(new IRepresentmentFailedEvent
			                         	{
			                         		AccountId = customer.Id,
											ApplicationId = application.Id,
											CreatedOn = DateTime.UtcNow,
											RepresentmentAttempt = 1
			                         	});

			// Trigger 31 days timeout of debt collection agency
			var debtCollectionSaga = Do.Until(() =>
				_externalDebtCollectionSaga.
				FindByApplicationId(application.Id));

			Drive.Msmq.Payments.Send(new TimeoutMessage{ SagaId = debtCollectionSaga.Id});

			// Wait for debt collection to be created
			Do.Until(() => _debtCollection.FindAll(_debtCollection.Applications.ExternalId == application.Id).Single());
		}
	}
}