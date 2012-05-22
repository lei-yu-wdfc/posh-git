using System;
using System.Collections.Generic;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db.BankGateway;
using Wonga.QA.Framework.Mocks;
using Wonga.QA.Tests.BankGateway.Enums;
using Wonga.QA.Tests.BankGateway.Helpers;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.BankGateway
{
    [TestFixture, AUT(AUT.Ca)]
    [Parallelizable(TestScope.Self)]
    public class BankGatewayCa
    {
        private readonly dynamic _bgTrans = Drive.Data.BankGateway.Db.Transactions;
        private readonly dynamic _opsSagasScotiaCashOutToBeSent = Drive.Data.OpsSagas.Db.ScotiaCashOutTransactionsToBeSent;

        [Test, AUT(AUT.Ca), JIRA("CA-1913"), Parallelizable]
        public void WhenCustomerLoanIsFundedBankGatewayShouldUpdateCashOutTransactionToStatusOfPaid()
        {
            var customer = CustomerBuilder.New().
                Build();
            var application = ApplicationBuilder.New(customer).Build();

            Do.Until(() => _bgTrans.FindAll(_bgTrans.ApplicationId == application.Id &&
                                            _bgTrans.TransactionStatus == (int) BankGatewayTransactionStatus.Paid).
                               Single());

        }

        [Test, AUT(AUT.Ca), JIRA("CA-1913"), Parallelizable]
        public void WhenCustomerLoanIsRepaidBankGatewayShouldUpdateCashInTransactionToStatusOfPaid()
        {
            var customer = CustomerBuilder.New().
                Build();
            var application = ApplicationBuilder.New(customer).Build();

            Do.Until(() => _bgTrans.FindAll(_bgTrans.ApplicationId == application.Id &&
                                            _bgTrans.TransactionStatus == (int)BankGatewayTransactionStatus.Paid).
                               Single());

            application.RepayOnDueDate();

            Assert.IsTrue(_bgTrans.GetCount(_bgTrans.ApplicationId == application.Id) == 2);

            Do.Until(() => _bgTrans.FindAll(_bgTrans.ApplicationId == application.Id &&
                                            _bgTrans.TransactionStatus == (int)BankGatewayTransactionStatus.Paid).
                               OrderByDescending(_bgTrans.TransactionId).First());
        }

        [Test, AUT(AUT.Ca), JIRA("CA-1913"), Parallelizable]
        public void
            WhenCustomerEntersAnApplicationWithAnInvalidBankAccountThenBankGatewayShouldUpdateTransactionAsFailed()
        {
            var customer = CustomerBuilder.New().
                Build();

            ScotiaResponseBuilder.New().
                ForBankAccountNumber(customer.BankAccountNumber).
                Reject();

            var application = ApplicationBuilder.New(customer).Build();

            Do.Until(() => _bgTrans.FindAll(_bgTrans.ApplicationId == application.Id &&
                                            _bgTrans.TransactionStatus == (int) BankGatewayTransactionStatus.Failed).
                               Single());
        }

        [Test, AUT(AUT.Ca), JIRA("CA-1913"), FeatureSwitch(FeatureSwitchConstants.BmoFeatureSwitchKey, true), Parallelizable]
        public void WhenCustomerEntersAnApplicationWithInstitionCode001ThenBankGatewayShouldRouteTransactionToScotia()
        {
            var customer = CustomerBuilder.New().
                WithInstitutionNumber("001").
                WithBranchNumber("00022").
                Build();
            var application = ApplicationBuilder.New(customer).Build();

            Do.Until(() => _bgTrans.FindAll(_bgTrans.ApplicationId == application.Id &&
                                            _bgTrans.TransactionStatus == (int) BankGatewayTransactionStatus.Paid)).
                Single();

        }

        [Test, AUT(AUT.Ca), JIRA("CA-1913"), FeatureSwitch(FeatureSwitchConstants.BmoFeatureSwitchKey, true), Parallelizable]
        public void WhenCustomerEntersAnApplicationWithInstitionCode003ThenBankGatewayShouldRouteTransactionToScotia()
        {
            var customer = CustomerBuilder.New().
                WithInstitutionNumber("003").
                WithBranchNumber("00022").
                Build();
            var application = ApplicationBuilder.New(customer).Build();

            Do.Until(() => _bgTrans.FindAll(_bgTrans.ApplicationId == application.Id &&
                                            _bgTrans.TransactionStatus == (int) BankGatewayTransactionStatus.Paid)).
                Single();

        }

        [Test, AUT(AUT.Ca), JIRA("CA-1913"), FeatureSwitch(FeatureSwitchConstants.BmoFeatureSwitchKey), Parallelizable]
        public void WhenCustomerEntersInstitionNumber002ThenBankGatewayShouldRouteTransactionToScotia()
        {
            var customer = CustomerBuilder.New().
                WithInstitutionNumber("002").
                WithBranchNumber("00018").
                Build();
            var application = ApplicationBuilder.New(customer).Build();

            Do.Until(() => _bgTrans.FindAll(_bgTrans.ApplicationId == application.Id &&
                                            _bgTrans.BankIntegrationId == (int) BankGatewayIntegrationId.Scotia).Single());

        }

        [Test, AUT(AUT.Ca), JIRA("CA-1914")]
        public void WhenMultipleScotiaTransactionsAreHandledTogetherThenBankGatewayShouldHandleThemAsABatch()
        {
            var applicationIds = new List<Guid>();

            using (new ScotiaBatchSending())
            {
                for (var i = 0; i < 3; i++)
                {
                    var customer = CustomerBuilder.New().
                        Build();

                    applicationIds.Add(ApplicationBuilder.New(customer).Build().Id);
                }

                var transactions = new List<TransactionEntity>();

                foreach (var applicationId in applicationIds)
                {
                    var id = applicationId;

                    TransactionEntity transaction = Do.Until(() => _bgTrans.FindAll(_bgTrans.ApplicationId == id &&
                                    _bgTrans.BankIntegrationId == (int)BankGatewayIntegrationId.Scotia &&
                                    _bgTrans.TransactionStatus == (int)BankGatewayTransactionStatus.New).Single());

                    if (transaction != null)
                    {
                        transactions.Add(transaction);
                    }
                }

                Assert.AreEqual(applicationIds.Count, transactions.Count);

                Do.Until(() => _opsSagasScotiaCashOutToBeSent.GetCount() == 3);
            }

            Do.With.Timeout(1).Until(() => _opsSagasScotiaCashOutToBeSent.GetCount() == 0);
        }
    }
}