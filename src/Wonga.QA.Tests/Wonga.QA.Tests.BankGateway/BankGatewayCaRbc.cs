using System;
using System.Collections.Generic;
using System.Linq;
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
    public class BankGatewayCaRbc
    {
        private readonly dynamic _bgTrans = Drive.Data.BankGateway.Db.Transactions;
        private readonly dynamic _bgFiles = Drive.Data.BankGateway.Db.Files;
        private readonly dynamic _bgServiceTypes = Drive.Data.BankGateway.Db.ServiceTypes;
        private readonly dynamic _bgAckTypes = Drive.Data.BankGateway.Db.AcknowledgeTypes;
        private readonly dynamic _bgAck = Drive.Data.BankGateway.Db.Acknowledges;

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
        public void WhenMultipleCustomersEnterTransactionsTheTransactionsShouldBeBatchTogetherAndSentToRbcInOneFile()
        {
            var previousFileId =
                _bgFiles.FindAll(_bgFiles.ServiceTypeId ==
                                 (_bgServiceTypes.FindByBankIntegrationId((int) BankGatewayIntegrationId.Rbc).
                                     ServiceTypeId)).OrderByDescending(_bgFiles.FileId).First().FileId;

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
                                                    _bgTrans.TransactionStatus == (int)BankGatewayTransactionStatus.New &&
                                                    _bgTrans.BankIntegrationId == (int)BankGatewayIntegrationId.Rbc).Single());
                }
            }

            foreach (var applicationId in applicationIds)
            {
                Do.Until(() => _bgTrans.FindAll(_bgTrans.ApplicationId == applicationId &&
                                _bgTrans.TransactionStatus == (int)BankGatewayTransactionStatus.Paid &&
                                _bgTrans.BankIntegrationId == (int)BankGatewayIntegrationId.Rbc).Single());
            }

            var numberNewFile =
                _bgFiles.FindAll(_bgFiles.ServiceTypeId ==
                                 (_bgServiceTypes.FindByBankIntegrationId((int) BankGatewayIntegrationId.Rbc).
                                     ServiceTypeId) && _bgFiles.FileId > previousFileId).Count();

            Assert.AreEqual(numberNewFile, 1);
        }

        [Test, AUT(AUT.Ca), JIRA("CA-2207"), Parallelizable, FeatureSwitch(FeatureSwitchConstants.RbcFeatureSwitchKey)]
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

        [Test, AUT(AUT.Ca), JIRA("CA-2207"), Parallelizable, FeatureSwitch(FeatureSwitchConstants.RbcFeatureSwitchKey)]
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

        [Test, AUT(AUT.Ca), JIRA("CA-2098"), Parallelizable, FeatureSwitch(FeatureSwitchConstants.RbcFeatureSwitchKey)]
        public void WhenCustomerEntersAnApplicationAndRbcRejectsTheTransactionThenBankGatewayShouldUpdateTransactionAsFailed()
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

        [Test, AUT(AUT.Ca), JIRA("CA-2098"), Parallelizable, FeatureSwitch(FeatureSwitchConstants.RbcFeatureSwitchKey)]
        public void WhenCustomerEntersAnApplicationAndRbcWarnsTheTransactionThenBankGatewayShouldUpdateTransactionAsSuccess()
        {
            var customer = CustomerBuilder.New().
                        WithInstitutionNumber("003").
                        WithBranchNumber("00022").
                Build();

            RbcResponseBuilder.New().
                ForBankAccountNumber(customer.BankAccountNumber).
                Warn();


            var application = ApplicationBuilder.New(customer).Build();

            Do.Until(() => _bgTrans.FindAll(_bgTrans.ApplicationId == application.Id &&
                                            _bgTrans.BankIntegrationId == (int)BankGatewayIntegrationId.Rbc &&
                                            _bgTrans.TransactionStatus == (int)BankGatewayTransactionStatus.Paid).
                               Single());
        }

        [Test, AUT(AUT.Ca), JIRA("CA-2073"), Parallelizable, FeatureSwitch(FeatureSwitchConstants.RbcFeatureSwitchKey)]
        public void WhenRbcReturnsAnAllInputRecordsReportFileThenBankGatewayShouldRecordTheFileAndTransactionNumberInTheAcksTable()
        {
            var customer = CustomerBuilder.New().
                                WithInstitutionNumber("003").
                                WithBranchNumber("00022").
                                Build();
            var application = ApplicationBuilder.New(customer).Build();

            var transaction = Do.Until(() => _bgTrans.FindAll(_bgTrans.ApplicationId == application.Id &&
                                 _bgTrans.BankIntegrationId == (int)BankGatewayIntegrationId.Rbc &&
                                 _bgTrans.TransactionStatus == (int)BankGatewayTransactionStatus.Paid).Single());

            var ackType = _bgAckTypes.FindAll(_bgAckTypes.BankIntegrationId == (int)BankGatewayIntegrationId.Rbc && _bgAckTypes.Name == "RPT0902P").Single();

            Do.Until(() => _bgAck.FindAll(_bgAck.TransactionID == transaction.TransactionId && _bgAck.AcknowledgeTypeID == ackType.AcknowledgeTypeId).Single());
        }

        [Test, AUT(AUT.Ca), JIRA("CA-2073"), Parallelizable, FeatureSwitch(FeatureSwitchConstants.RbcFeatureSwitchKey)]
        public void WhenRbcReturnsReturnedItemsRecordsReportFileThenBankGatewayShouldRecordTheFileAndTransactionNumberInTheAcksTable()
        {
            var customer = CustomerBuilder.New().
                                WithInstitutionNumber("003").
                                WithBranchNumber("00022").
                                Build();

            RbcResponseBuilder.New().
                ForBankAccountNumber(customer.BankAccountNumber).
                Reject();

            var application = ApplicationBuilder.New(customer).Build();

            var transaction = Do.Until(() => _bgTrans.FindAll(_bgTrans.ApplicationId == application.Id &&
                                 _bgTrans.BankIntegrationId == (int)BankGatewayIntegrationId.Rbc &&
                                 _bgTrans.TransactionStatus == (int)BankGatewayTransactionStatus.Failed).Single());

            var ackType = _bgAckTypes.FindAll(_bgAckTypes.BankIntegrationId == (int)BankGatewayIntegrationId.Rbc && _bgAckTypes.Name == "RPT0901P").Single();

            Do.Until(() => _bgAck.FindAll(_bgAck.TransactionID == transaction.TransactionId && _bgAck.AcknowledgeTypeID == ackType.AcknowledgeTypeId).Single());
        }

        [Test, AUT(AUT.Ca), JIRA("CA-2098"), FeatureSwitch(FeatureSwitchConstants.RbcFeatureSwitchKey)]
        public void WhenRbcReturnsAnAllInputRecordAndAReturnedItemsFileThenBankGatewayShouldUpdateTransactionAsPaidAndFailed()
        {
            var applicationIdOne = new Guid();
            var applicationIdTwo = new Guid();

            using (new RbcBatchSending())
            {
                var customerOne = CustomerBuilder.New().
                    WithInstitutionNumber("003").
                    WithBranchNumber("00022").
                    Build();

                applicationIdOne = ApplicationBuilder.New(customerOne).Build().Id;

                var customerTwo = CustomerBuilder.New().
                    WithInstitutionNumber("003").
                    WithBranchNumber("00022").
                    Build();

                RbcResponseBuilder.New().
                    ForBankAccountNumber(customerTwo.BankAccountNumber).
                    Reject();

                applicationIdTwo = ApplicationBuilder.New(customerTwo).Build().Id;
            }

            var ackTypeAllInputRecords = _bgAckTypes.FindAll(_bgAckTypes.BankIntegrationId == (int)BankGatewayIntegrationId.Rbc && _bgAckTypes.Name == "RPT0902P").Single();
            var ackTypeReturnedItems = _bgAckTypes.FindAll(_bgAckTypes.BankIntegrationId == (int)BankGatewayIntegrationId.Rbc && _bgAckTypes.Name == "RPT0901P").Single();

            var paidTransaction = Do.Until(() => _bgTrans.FindAll(_bgTrans.ApplicationId == applicationIdOne &&
                                 _bgTrans.BankIntegrationId == (int)BankGatewayIntegrationId.Rbc &&
                                 _bgTrans.TransactionStatus == (int)BankGatewayTransactionStatus.Paid).Single());

            Do.Until(() => _bgAck.FindAll(_bgAck.TransactionID == paidTransaction.TransactionId && _bgAck.AcknowledgeTypeID == ackTypeAllInputRecords.AcknowledgeTypeId).Single());

            var failedTransaction = Do.Until(() => _bgTrans.FindAll(_bgTrans.ApplicationId == applicationIdTwo &&
                                 _bgTrans.BankIntegrationId == (int)BankGatewayIntegrationId.Rbc &&
                                 _bgTrans.TransactionStatus == (int)BankGatewayTransactionStatus.Failed).Single());

            Do.Until(() => _bgAck.FindAll(_bgAck.TransactionID == failedTransaction.TransactionId && _bgAck.AcknowledgeTypeID == ackTypeReturnedItems.AcknowledgeTypeId).Single());
        }

    }
}
