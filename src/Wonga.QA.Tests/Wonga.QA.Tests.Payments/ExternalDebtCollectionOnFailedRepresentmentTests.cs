using System;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Mocks;
using Wonga.QA.Framework.Msmq;
using Wonga.QA.Framework.Msmq.Messages.Payments.InternalMessages.Messages;
using Wonga.QA.Framework.Msmq.Messages.Payments.PublicMessages;
using Wonga.QA.Tests.BankGateway.Enums;
using Wonga.QA.Tests.Core;
using Wonga.QA.Tests.Payments.Enums;
using Wonga.QA.Tests.Payments.Helpers.Ca;
using IncomeFrequencyEnum = Wonga.QA.Framework.Api.Enums.IncomeFrequencyEnum;

namespace Wonga.QA.Tests.Payments
{
    [AUT(AUT.Ca), Parallelizable(TestScope.Descendants)]
    public class ExternalDebtCollectionOnFailedRepresentmentTests
    {
    	private const string FeatureSwitchKey = "FeatureSwitch.MoveLoanToDcaOnFailedRepresentment";
    	private readonly dynamic _debtCollection = Drive.Data.Payments.Db.DebtCollection;
    	private readonly dynamic _externalDebtCollectionSaga = Drive.Data.OpsSagas.Db.ExternalDebtCollectionOnFailedRepresentmentSagaEntity;

        private readonly dynamic _opsSagasMultipleRepresentmentsInArrearsSagaEntity =
                                                    Drive.Data.OpsSagas.Db.MultipleRepresentmentsInArrearsSagaEntity;
        private readonly dynamic _bgTrans = Drive.Data.BankGateway.Db.Transactions;
        private readonly dynamic _inArrearsNoticeSaga = Drive.Data.OpsSagas.Db.InArrearsNoticeSagaEntity;
        private readonly dynamic _paymentApplications = Drive.Data.Payments.Db.Applications;

		[Test, AUT(AUT.Ca), JIRA("CA-2285"), FeatureSwitch(FeatureSwitchKey)]
		public void WhenLoanClosedThenShouldNotMoveApplicationToDca()
		{
			var customer = CustomerBuilder.New().Build();
			var application = ApplicationBuilder.New(customer).Build();

			application.PutIntoArrears();

			// Validate that external debt collection finished
			Do.Until(() => _externalDebtCollectionSaga.FindByApplicationId(application.Id));

			Drive.Msmq.Payments.Send(new IApplicationClosed
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

			application.PutIntoArrears();

			Drive.Msmq.Payments.Send(new IRepresentmentFailed
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

        [Test, AUT(AUT.Ca), JIRA("CA-2351"), FeatureSwitch(FeatureSwitchKey)]
        public void WhenASuccessfulRepresentmentIsMadeForACustomerInArrearsOnThereNextPayDateThenApplicationShouldNotBeMovedToDca()
        {
            const int loanTerm = 10;
            var customer = CustomerBuilder.New().WithIncomeFrequency(IncomeFrequencyEnum.Monthly).Build();
            var application = ApplicationBuilder.New(customer).WithLoanTerm(loanTerm).Build();

            var nextPayDateForRepresentmentOne = CalculateNextPayDateFunctionsCa.CalculateNextPayDate(DateTime.Today, Convert.ToDateTime(customer.GetNextPayDate()),
                                                                                        (PaymentFrequency)(Convert.ToInt32(customer.GetIncomeFrequency())));
            var numOfDaysToNextPayDateForRepresentmentOne = (int)nextPayDateForRepresentmentOne.Subtract(DateTime.Today).TotalDays;

            application.PutIntoArrears((uint)numOfDaysToNextPayDateForRepresentmentOne);
            TimeoutInArrearsNoticeSaga(application.Id, numOfDaysToNextPayDateForRepresentmentOne);

            TimeoutMultipleRepresentmentsInArrearsSagaEntity(application.Id);

            var currentDateForRepresentmentTwo = Convert.ToDateTime(customer.GetNextPayDate());

            var nextPayDateForRepresentmentTwo = CalculateNextPayDateFunctionsCa.CalculateNextPayDate(currentDateForRepresentmentTwo, Convert.ToDateTime(customer.GetNextPayDate()),
                                                                            (PaymentFrequency)(Convert.ToInt32(customer.GetIncomeFrequency())));
            var numOfDaysToNextPayDateForRepresentmentTwo = (int)nextPayDateForRepresentmentTwo.Subtract(currentDateForRepresentmentTwo).TotalDays;

            ScotiaResponseBuilder.New().
                ForBankAccountNumber(customer.BankAccountNumber).
                Reject();

            TimeoutInArrearsNoticeSaga(application.Id, numOfDaysToNextPayDateForRepresentmentTwo);
            application.RewindApplicationFurther((uint)numOfDaysToNextPayDateForRepresentmentTwo);
            TimeoutMultipleRepresentmentsInArrearsSagaEntity(application.Id);

            Do.Until(() => (int)_bgTrans.GetCount(_bgTrans.ApplicationId == application.Id &&
                                                   _bgTrans.TransactionStatus ==
                                                   (int)BankGatewayTransactionStatus.Paid) == 2);

            //Check number of days in arrears in greater than 30...

            //Verify that application is NOT in DCA...
        }

        [Test, AUT(AUT.Ca), JIRA("CA-2351"), FeatureSwitch(FeatureSwitchKey)]
        public void WhenAFailedRepresentmentIsMadeForACustomerOnThereNextPayDateGreaterThan30DaysInArrearsThenApplicationShouldBeMovedToDca()
        {
            const int loanTerm = 10;
            var customer = CustomerBuilder.New().WithIncomeFrequency(IncomeFrequencyEnum.Monthly).Build();
            var application = ApplicationBuilder.New(customer).WithLoanTerm(loanTerm).Build();

            var nextPayDateForRepresentmentOne = CalculateNextPayDateFunctionsCa.CalculateNextPayDate(DateTime.Today, Convert.ToDateTime(customer.GetNextPayDate()),
                                                                                        (PaymentFrequency)(Convert.ToInt32(customer.GetIncomeFrequency())));
            var numOfDaysToNextPayDateForRepresentmentOne = (int)nextPayDateForRepresentmentOne.Subtract(DateTime.Today).TotalDays;

            application.PutIntoArrears((uint)numOfDaysToNextPayDateForRepresentmentOne);
            TimeoutInArrearsNoticeSaga(application.Id, numOfDaysToNextPayDateForRepresentmentOne);
            RewindExternalDebtCollectionOnFailedRepresentmentSagaEntity(application.Id, numOfDaysToNextPayDateForRepresentmentOne);

            TimeoutMultipleRepresentmentsInArrearsSagaEntity(application.Id);

            var currentDateForRepresentmentTwo = Convert.ToDateTime(customer.GetNextPayDate());

            var nextPayDateForRepresentmentTwo = CalculateNextPayDateFunctionsCa.CalculateNextPayDate(currentDateForRepresentmentTwo, Convert.ToDateTime(customer.GetNextPayDate()),
                                                                            (PaymentFrequency)(Convert.ToInt32(customer.GetIncomeFrequency())));
            var numOfDaysToNextPayDateForRepresentmentTwo = (int)nextPayDateForRepresentmentTwo.Subtract(currentDateForRepresentmentTwo).TotalDays;

            TimeoutInArrearsNoticeSaga(application.Id, numOfDaysToNextPayDateForRepresentmentTwo);
            application.RewindApplicationFurther((uint)numOfDaysToNextPayDateForRepresentmentTwo);
            RewindExternalDebtCollectionOnFailedRepresentmentSagaEntity(application.Id, numOfDaysToNextPayDateForRepresentmentTwo);

            ScotiaResponseBuilder.New().
                ForBankAccountNumber(customer.BankAccountNumber).
                Reject();

            TimeoutMultipleRepresentmentsInArrearsSagaEntity(application.Id);

            Do.Until(() => (int)_bgTrans.GetCount(_bgTrans.ApplicationId == application.Id &&
                                       _bgTrans.TransactionStatus ==
                                       (int)BankGatewayTransactionStatus.Failed) == 2);

            var appId = _paymentApplications.FindByExternalId(application.Id).ApplicationId;
            Do.Until(() => _debtCollection.FindByApplicationId(appId));
        }

        [Test, AUT(AUT.Ca), JIRA("CA-2351"), FeatureSwitch(FeatureSwitchKey), ExpectedException(typeof(DoException))]
        public void WhenASuccessfulRepresentmentIsMadeForACustomerOnThereNextPayDateGreaterThan30DaysInArrearsThenApplicationShouldNotBeMovedToDca()
        {
            const int loanTerm = 10;
            var customer = CustomerBuilder.New().WithIncomeFrequency(IncomeFrequencyEnum.Monthly).Build();
            var application = ApplicationBuilder.New(customer).WithLoanTerm(loanTerm).Build();

            var nextPayDateForRepresentmentOne = CalculateNextPayDateFunctionsCa.CalculateNextPayDate(DateTime.Today, Convert.ToDateTime(customer.GetNextPayDate()),
                                                                                        (PaymentFrequency)(Convert.ToInt32(customer.GetIncomeFrequency())));
            var numOfDaysToNextPayDateForRepresentmentOne = (int)nextPayDateForRepresentmentOne.Subtract(DateTime.Today).TotalDays;

            application.PutIntoArrears((uint)numOfDaysToNextPayDateForRepresentmentOne);
            TimeoutInArrearsNoticeSaga(application.Id, numOfDaysToNextPayDateForRepresentmentOne);
            RewindExternalDebtCollectionOnFailedRepresentmentSagaEntity(application.Id, numOfDaysToNextPayDateForRepresentmentOne);

            TimeoutMultipleRepresentmentsInArrearsSagaEntity(application.Id);

            Do.Until(() => (int)_bgTrans.GetCount(_bgTrans.ApplicationId == application.Id &&
                           _bgTrans.TransactionStatus ==
                           (int)BankGatewayTransactionStatus.Paid) == 2);

            var currentDateForRepresentmentTwo = Convert.ToDateTime(customer.GetNextPayDate());

            var nextPayDateForRepresentmentTwo = CalculateNextPayDateFunctionsCa.CalculateNextPayDate(currentDateForRepresentmentTwo, Convert.ToDateTime(customer.GetNextPayDate()),
                                                                            (PaymentFrequency)(Convert.ToInt32(customer.GetIncomeFrequency())));
            var numOfDaysToNextPayDateForRepresentmentTwo = (int)nextPayDateForRepresentmentTwo.Subtract(currentDateForRepresentmentTwo).TotalDays;

            TimeoutInArrearsNoticeSaga(application.Id, numOfDaysToNextPayDateForRepresentmentTwo);
            application.RewindApplicationFurther((uint)numOfDaysToNextPayDateForRepresentmentTwo);
            RewindExternalDebtCollectionOnFailedRepresentmentSagaEntity(application.Id,
                                                                        numOfDaysToNextPayDateForRepresentmentTwo);

            TimeoutMultipleRepresentmentsInArrearsSagaEntity(application.Id);

            Do.Until(() => (int)_bgTrans.GetCount(_bgTrans.ApplicationId == application.Id &&
                                       _bgTrans.TransactionStatus ==
                                       (int)BankGatewayTransactionStatus.Paid) == 3);

            var appId = _paymentApplications.FindByExternalId(application.Id).ApplicationId;
            Do.Until(() => _debtCollection.FindByApplicationId(appId));
        }

        [Test, AUT(AUT.Ca), JIRA("CA-2351"), FeatureSwitch(FeatureSwitchKey), ExpectedException(typeof(DoException))]
        public void WhenAFailedRepresentmentIsMadeForACustomerOnThereNextPayDateLessThan30DaysInArrearsThenApplicationShouldNotBeMovedToDca()
        {
            const int loanTerm = 10;
            var customer = CustomerBuilder.New().WithIncomeFrequency(IncomeFrequencyEnum.Weekly).Build();
            var application = ApplicationBuilder.New(customer).WithLoanTerm(loanTerm).Build();

            var nextPayDateForRepresentmentOne = CalculateNextPayDateFunctionsCa.CalculateNextPayDate(DateTime.Today, Convert.ToDateTime(customer.GetNextPayDate()),
                                                                                        (PaymentFrequency)(Convert.ToInt32(customer.GetIncomeFrequency())));
            var numOfDaysToNextPayDateForRepresentmentOne = (int)nextPayDateForRepresentmentOne.Subtract(DateTime.Today).TotalDays;

            application.PutIntoArrears((uint)numOfDaysToNextPayDateForRepresentmentOne);
            TimeoutInArrearsNoticeSaga(application.Id, numOfDaysToNextPayDateForRepresentmentOne);
            RewindExternalDebtCollectionOnFailedRepresentmentSagaEntity(application.Id, numOfDaysToNextPayDateForRepresentmentOne);

            TimeoutMultipleRepresentmentsInArrearsSagaEntity(application.Id);

            Do.Until(() => (int)_bgTrans.GetCount(_bgTrans.ApplicationId == application.Id &&
                           _bgTrans.TransactionStatus ==
                           (int)BankGatewayTransactionStatus.Paid) == 2);

            var currentDateForRepresentmentTwo = Convert.ToDateTime(customer.GetNextPayDate());

            var nextPayDateForRepresentmentTwo = CalculateNextPayDateFunctionsCa.CalculateNextPayDate(currentDateForRepresentmentTwo, Convert.ToDateTime(customer.GetNextPayDate()),
                                                                            (PaymentFrequency)(Convert.ToInt32(customer.GetIncomeFrequency())));
            var numOfDaysToNextPayDateForRepresentmentTwo = (int)nextPayDateForRepresentmentTwo.Subtract(currentDateForRepresentmentTwo).TotalDays;

            TimeoutInArrearsNoticeSaga(application.Id, numOfDaysToNextPayDateForRepresentmentTwo);
            application.RewindApplicationFurther((uint)numOfDaysToNextPayDateForRepresentmentTwo);
            RewindExternalDebtCollectionOnFailedRepresentmentSagaEntity(application.Id,
                                                                        numOfDaysToNextPayDateForRepresentmentTwo);

            ScotiaResponseBuilder.New().
                ForBankAccountNumber(customer.BankAccountNumber).
                Reject();

            TimeoutMultipleRepresentmentsInArrearsSagaEntity(application.Id);

            Do.Until(() => (int)_bgTrans.GetCount(_bgTrans.ApplicationId == application.Id &&
                                       _bgTrans.TransactionStatus ==
                                       (int)BankGatewayTransactionStatus.Failed) == 2);

            var appId = _paymentApplications.FindByExternalId(application.Id).ApplicationId;
            Do.Until(() => _debtCollection.FindByApplicationId(appId));
        }

        [Test, AUT(AUT.Ca), JIRA("CA-2351"), FeatureSwitch(FeatureSwitchKey)]
        public void WhenAFailedRepresentmentIsMadeForACustomerOnThereNextPayDateLessThan30DaysInArrearsThenApplicationShouldBeMovedToDcaOnDay30OfBeingInArrears()
        {
            const int loanTerm = 10;
            var customer = CustomerBuilder.New().WithIncomeFrequency(IncomeFrequencyEnum.Weekly).Build();
            var application = ApplicationBuilder.New(customer).WithLoanTerm(loanTerm).Build();

            var nextPayDateForRepresentmentOne = CalculateNextPayDateFunctionsCa.CalculateNextPayDate(DateTime.Today, Convert.ToDateTime(customer.GetNextPayDate()),
                                                                                        (PaymentFrequency)(Convert.ToInt32(customer.GetIncomeFrequency())));
            var numOfDaysToNextPayDateForRepresentmentOne = (int)nextPayDateForRepresentmentOne.Subtract(DateTime.Today).TotalDays;

            application.PutIntoArrears((uint)numOfDaysToNextPayDateForRepresentmentOne);
            TimeoutInArrearsNoticeSaga(application.Id, numOfDaysToNextPayDateForRepresentmentOne);
            RewindExternalDebtCollectionOnFailedRepresentmentSagaEntity(application.Id, numOfDaysToNextPayDateForRepresentmentOne);

            TimeoutMultipleRepresentmentsInArrearsSagaEntity(application.Id);

            Do.Until(() => (int)_bgTrans.GetCount(_bgTrans.ApplicationId == application.Id &&
                           _bgTrans.TransactionStatus ==
                           (int)BankGatewayTransactionStatus.Paid) == 2);

            var currentDateForRepresentmentTwo = Convert.ToDateTime(customer.GetNextPayDate());

            var nextPayDateForRepresentmentTwo = CalculateNextPayDateFunctionsCa.CalculateNextPayDate(currentDateForRepresentmentTwo, Convert.ToDateTime(customer.GetNextPayDate()),
                                                                            (PaymentFrequency)(Convert.ToInt32(customer.GetIncomeFrequency())));
            var numOfDaysToNextPayDateForRepresentmentTwo = (int)nextPayDateForRepresentmentTwo.Subtract(currentDateForRepresentmentTwo).TotalDays;

            TimeoutInArrearsNoticeSaga(application.Id, numOfDaysToNextPayDateForRepresentmentTwo);
            application.RewindApplicationFurther((uint)numOfDaysToNextPayDateForRepresentmentTwo);
            RewindExternalDebtCollectionOnFailedRepresentmentSagaEntity(application.Id,
                                                                        numOfDaysToNextPayDateForRepresentmentTwo);

            ScotiaResponseBuilder.New().
                ForBankAccountNumber(customer.BankAccountNumber).
                Reject();

            TimeoutMultipleRepresentmentsInArrearsSagaEntity(application.Id);

            Do.Until(() => (int)_bgTrans.GetCount(_bgTrans.ApplicationId == application.Id &&
                                       _bgTrans.TransactionStatus ==
                                       (int)BankGatewayTransactionStatus.Failed) == 2);

            var debtCollectionSaga = Do.Until(() => _externalDebtCollectionSaga.FindByApplicationId(application.Id));

            Drive.Msmq.Payments.Send(new TimeoutMessage { SagaId = debtCollectionSaga.Id });

            var appId = _paymentApplications.FindByExternalId(application.Id).ApplicationId;
            Do.Until(() => _debtCollection.FindByApplicationId(appId));
        }

        private void RewindExternalDebtCollectionOnFailedRepresentmentSagaEntity(Guid applicationGuid, int numOfDays)
        {
            var wentIntoArrearsOnLocalTime = Do.Until(() => _externalDebtCollectionSaga.FindByApplicationId(applicationGuid).WentIntoArrearsOnLocalTime);
            wentIntoArrearsOnLocalTime -= new TimeSpan(numOfDays, 0, 0, 0);
            _externalDebtCollectionSaga.UpdateByApplicationId(ApplicationId: applicationGuid, WentIntoArrearsOnLocalTime: wentIntoArrearsOnLocalTime);
        }

        private void TimeoutMultipleRepresentmentsInArrearsSagaEntity(Guid applicationGuid)
        {
            var multipleRepresentmentSaga = Do.Until(() => _opsSagasMultipleRepresentmentsInArrearsSagaEntity.FindByApplicationId(applicationGuid));
            Drive.Msmq.Payments.Send(new TimeoutMessage { SagaId = multipleRepresentmentSaga.Id });
        }

        private String GetNumberOfRepresentmentsSent(Guid applicationGuid)
        {
            var multipleRepresentmentSaga = Do.Until(() => _opsSagasMultipleRepresentmentsInArrearsSagaEntity.FindByApplicationId(applicationGuid));
            return multipleRepresentmentSaga.RepresentmentsSent.ToString();
        }

        private String GetNextRepresentmentDate(Guid applicationGuid)
        {
            var multipleRepresentmentSaga = Do.Until(() => _opsSagasMultipleRepresentmentsInArrearsSagaEntity.FindByApplicationId(applicationGuid));
            return multipleRepresentmentSaga.NextRepresentmentDate.ToString();
        }

        private Decimal CurrentRepresentmentAmount(Guid applicationGuid)
        {
            var multipleRepresentmentSaga = Do.Until(() => _opsSagasMultipleRepresentmentsInArrearsSagaEntity.FindByApplicationId(applicationGuid));
            return Math.Round(Convert.ToDecimal(multipleRepresentmentSaga.LastRepresentmentAmount), 2, MidpointRounding.AwayFromZero);
        }

        private void TimeoutInArrearsNoticeSaga(Guid applicationGuid, int numberOfDaysInArrears)
        {
            var inArrearsNoticeSaga =
                Do.Until(() => _inArrearsNoticeSaga.FindByApplicationId(applicationGuid));

            for (var i = 0; i < numberOfDaysInArrears; i++)
            {
                Drive.Msmq.Payments.Send(new TimeoutMessage { SagaId = inArrearsNoticeSaga.Id });
            }
        }
	}
}