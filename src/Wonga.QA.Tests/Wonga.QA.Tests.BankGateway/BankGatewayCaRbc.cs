using System;
using System.Collections.Generic;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Mocks;
using Wonga.QA.Tests.BankGateway.Enums;
using Wonga.QA.Tests.BankGateway.Helpers;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.BankGateway
{
    [TestFixture]
    [Parallelizable(TestScope.Self)]
    public class BankGatewayCaRbc
    {
        private readonly dynamic _bgTrans = Drive.Data.BankGateway.Db.Transactions;

        [Test, AUT(AUT.Ca), JIRA("CA-1995"), FeatureSwitch(FeatureSwitchConstants.RbcFeatureSwitchKey), Parallelizable]
        public void WhenCustomerEntersInstitutionNumber003ThenBankGatewayShouldRouteTransactionToRbc()
        {
            var customer = CustomerBuilder.New().
                                WithInstitutionNumber("003").
                                WithBranchNumber("00022").
                                Build();
            var application = ApplicationBuilder.New(customer).Build();

            Do.Until(() => _bgTrans.FindAll(_bgTrans.ApplicationId == application.Id &&
                                    _bgTrans.BankIntegrationId == (int)BankGatewayIntegrationId.Rbc).Single());
        }

        [Test, AUT(AUT.Ca), JIRA("CA-1995"), FeatureSwitch(FeatureSwitchConstants.RbcFeatureSwitchKey)]
        public void WhenMultipleTransactionsAreBatchedHappyPath()
        {
            var applicationIds = new List<Guid>();

            using (new RbcBatchSending())
            {
                for (int i = 0; i < 3; i++)
                {
                    var customer = CustomerBuilder.New().
                        WithInstitutionNumber("003").
                        WithBranchNumber("00022").
                        Build();
                    var application = ApplicationBuilder.New(customer).Build();
                    applicationIds.Add(application.Id);

                    Do.Until(() => _bgTrans.FindAll(_bgTrans.ApplicationId == application.Id &&
                                                    _bgTrans.BankIntegrationId == (int)BankGatewayIntegrationId.Rbc).Single());
                }
            }

            // TODO: Add more solid assertions in here
        }

        
        [Test, AUT(AUT.Ca), JIRA("CA-2207"),Parallelizable]
        public void WhenCustomerLoanIsFundedBankGatewayShouldUpdateCashOutTransactionToStatusOfPaid()
        {
            var customer = CustomerBuilder.New().
                        WithInstitutionNumber("003").
                        WithBranchNumber("00022").
                        Build();
            var application = ApplicationBuilder.New(customer).Build();

            Do.Until(() => _bgTrans.FindAll(_bgTrans.ApplicationId == application.Id &&
                                            _bgTrans.BankIntegrationId == (int)BankGatewayIntegrationId.Rbc &&
                                            _bgTrans.TransactionStatus == (int)BankGatewayTransactionStatus.Paid).
                                Single());

        }

        [Test, AUT(AUT.Ca), JIRA("CA-2207"), Parallelizable]
        public void WhenCustomerLoanIsRepaidBankGatewayShouldUpdateCashInTransactionToStatusOfPaid()
        {
            var customer = CustomerBuilder.New().
                        WithInstitutionNumber("003").
                        WithBranchNumber("00022").
                        Build();
            var application = ApplicationBuilder.New(customer).Build();

            Do.Until(() => _bgTrans.FindAll(_bgTrans.ApplicationId == application.Id &&
                                            _bgTrans.BankIntegrationId == (int)BankGatewayIntegrationId.Rbc &&
                                            _bgTrans.TransactionStatus == (int)BankGatewayTransactionStatus.Paid).
                               Single());

            application.RepayOnDueDate();

            Assert.IsTrue(_bgTrans.GetCount(_bgTrans.ApplicationId == application.Id) == 2);

            Do.Until(() => _bgTrans.FindAll(_bgTrans.ApplicationId == application.Id &&
                                            _bgTrans.TransactionStatus == (int)BankGatewayTransactionStatus.Paid).
                               OrderByDescending(_bgTrans.TransactionId).First());
        }


        [Test, AUT(AUT.Ca), JIRA("CA-2207"), Parallelizable]
        public void WhenCustomerEntersAnApplicationWithAnInvalidBankAccountThenBankGatewayShouldUpdateTransactionAsFailed()
        {
            var customer = CustomerBuilder.New().
                        WithInstitutionNumber("003").
                        WithBranchNumber("00022").
                Build();

            RbcResponseBuilder.New().
                ForBankAccountNumber(customer.BankAccountNumber).
                Reject();

            var application = ApplicationBuilder.New(customer).Build();

            Do.Until(() => _bgTrans.FindAll(_bgTrans.ApplicationId == application.Id &&
                                            _bgTrans.BankIntegrationId == (int)BankGatewayIntegrationId.Rbc &&
                                            _bgTrans.TransactionStatus == (int)BankGatewayTransactionStatus.Failed).
                               Single());
        }
    }
}
