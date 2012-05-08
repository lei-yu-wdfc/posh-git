using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db.BankGateway;
using Wonga.QA.Framework.Mocks;
using Wonga.QA.Tests.BankGateway.Enums;
using Wonga.QA.Tests.BankGateway.Helpers;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.BankGateway
{
    [TestFixture, AUT(AUT.Ca), FeatureSwitch(FeatureSwitchConstants.BmoFeatureSwitchKey)]
    public class BankGatewayCaBmo
    {
        private readonly dynamic _bgTrans = Drive.Data.BankGateway.Db.Transactions;
        private readonly dynamic _bgAckTypes = Drive.Data.BankGateway.Db.AcknowledgeTypes;
        private readonly dynamic _bgAck = Drive.Data.BankGateway.Db.Acknowledges;

        [Test, AUT(AUT.Ca), JIRA("CA-1914")]
        public void WhenCustomerEntersInstitutionNumber001ThenBankGatewayShouldRouteTransactionToBmo()
        {
            var customer = CustomerBuilder.New().
                                WithInstitutionNumber("001").
                                WithBranchNumber("00022").
                                Build();
            var application = ApplicationBuilder.New(customer).Build();

            Do.Until(() => _bgTrans.FindAll(_bgTrans.ApplicationId == application.Id &&
                                    _bgTrans.BankIntegrationId == (int)BankGatewayIntegrationId.Bmo).Single());
        }

        [Test, AUT(AUT.Ca), JIRA("CA-1914")]
        public void WhenCustomerEntersValidBmoBankAccountNumberThenBankGatewayShouldUpdateTransactionAsPaid()
        {
            var customer = CustomerBuilder.New().
                                WithInstitutionNumber("001").
                                WithBranchNumber("00022").
                                Build();
            var application = ApplicationBuilder.New(customer).Build();

            Do.Until(() => _bgTrans.FindAll(_bgTrans.ApplicationId == application.Id &&
                                    _bgTrans.BankIntegrationId == (int)BankGatewayIntegrationId.Bmo &&
                                    _bgTrans.TransactionStatus == (int)BankGatewayTransactionStatus.Paid).Single());
        }

        [Test, AUT(AUT.Ca), JIRA("CA-1914")]
        public void WhenCustomerEntersInValidBmoBankAccountNumberThenBankGatewayShouldUpdateTransactionAsFailed()
        {
            var customer = CustomerBuilder.New().
                                WithInstitutionNumber("001").
                                WithBranchNumber("00022").
                                Build();

            BmoResponseBuilder.New().
                ForBankAccountNumber(customer.BankAccountNumber).
                RejectTransaction();

            var application = ApplicationBuilder.New(customer).Build();

            Do.Until(() => _bgTrans.FindAll(_bgTrans.ApplicationId == application.Id &&
                                    _bgTrans.BankIntegrationId == (int)BankGatewayIntegrationId.Bmo &&
                                    _bgTrans.TransactionStatus == (int)BankGatewayTransactionStatus.Failed).Single());
        }

        [Test, AUT(AUT.Ca), JIRA("CA-1914"), Ignore("This will fail until the correct file formats are added")]
        public void WhenBmoReturnsAnInvalidFileThenBankGatewayShouldUpdateTransactionAsFailed()
        {
            //TODO: This will fail until the correct file formats are added... 

            var customer = CustomerBuilder.New().
                                WithInstitutionNumber("001").
                                WithBranchNumber("00022").
                                Build();

            BmoResponseBuilder.New().
                ForBankAccountNumber(customer.BankAccountNumber).
                RejectFile();

            var application = ApplicationBuilder.New(customer).Build();

            Do.Until(() => _bgTrans.FindAll(_bgTrans.ApplicationId == application.Id &&
                                    _bgTrans.BankIntegrationId == (int)BankGatewayIntegrationId.Bmo &&
                                    _bgTrans.TransactionStatus == (int)BankGatewayTransactionStatus.Failed).Single());
        }

        [Test, AUT(AUT.Ca), JIRA("CA-1914")]
        public void WhenBmoReturnsAnAckFileDeft220ThenBankGatewayShouldRecordTheFileAndTransactionNumberInTheAcksTable()
        {
            var customer = CustomerBuilder.New().
                                WithInstitutionNumber("001").
                                WithBranchNumber("00022").
                                Build();
            var application = ApplicationBuilder.New(customer).Build();

            var ackType = _bgAckTypes.FindAll(_bgAckTypes.BankIntegrationId == (int)BankGatewayIntegrationId.Bmo && _bgAckTypes.Name == "DEFT220").Single();

            var transaction = Do.Until(() => _bgTrans.FindAll(_bgTrans.ApplicationId == application.Id &&
                                 _bgTrans.BankIntegrationId == (int)BankGatewayIntegrationId.Bmo &&
                                 _bgTrans.TransactionStatus == (int)BankGatewayTransactionStatus.Paid).Single());

            Do.Until(() => _bgAck.FindAll(_bgAck.TransactionID == transaction.TransactionId && _bgAck.AcknowledgeTypeID == ackType.AcknowledgeTypeId).Single());
        }

        [Test, AUT(AUT.Ca), JIRA("CA-1914")]
        public void PositiveFileAcknowledgementShouldBePersisted()
        {
            var ackType = Drive.Db.BankGateway.AcknowledgeTypes.Single(a => a.BankIntegrationId == (int)BankGatewayIntegrationId.Bmo && a.Name == "DEFT200");

            var previousAck = Drive.Db.BankGateway.Acknowledges.
                Where(a => a.AcknowledgeTypeID == ackType.AcknowledgeTypeId).
                OrderByDescending(a => a.AcknowledgeId).Take(1).SingleOrDefault();

            var previousAckId = previousAck != null ? previousAck.AcknowledgeId : -1;

            Do.Until(() =>
            {
                var latestAck = Drive.Db.BankGateway.Acknowledges.Where(
                    a => a.AcknowledgeTypeID == ackType.AcknowledgeTypeId).
                        OrderByDescending(a => a.AcknowledgeId).Take(1).Single();

                Assert.IsFalse(latestAck.HasError);

                return latestAck.AcknowledgeId != previousAckId;
            });
        }

        [Test, AUT(AUT.Ca), JIRA("CA-1914")]
        public void NegativeFileAcknowledgementShouldBePersistedAndAllTransactionsRejected()
        {
            Guid applicationIdAccepted;
            Guid applicationIdRejected;

            using (new BmoBatchSending())
            {
                // Rejected
                var customer = CustomerBuilder.New().
                                    WithInstitutionNumber("001").
                                    WithBranchNumber("00022").
                                    Build();
                var setup = BmoResponseBuilder.New().
                                    ForBankAccountNumber(customer.BankAccountNumber).
                                    RejectFile();

                applicationIdRejected = ApplicationBuilder.New(customer).Build().Id;

                // Accepted
                customer = CustomerBuilder.New().
                    WithInstitutionNumber("001").
                    WithBranchNumber("00022").
                    Build();

                applicationIdAccepted = ApplicationBuilder.New(customer).Build().Id;
            }

            var ackTypeSettled = Drive.Db.BankGateway.AcknowledgeTypes.Single(a => a.BankIntegrationId == (int)BankGatewayIntegrationId.Bmo && a.Name == "DEFT220");
            var ackTypeRejected = Drive.Db.BankGateway.AcknowledgeTypes.Single(a => a.BankIntegrationId == (int)BankGatewayIntegrationId.Bmo && a.Name == "DEFT210-211-260");

            Framework.Db.BankGateway.TransactionEntity transaction = null;

            // Accepted: expect fail transaction, NO settlement ack, NO reject ack
            Do.Until(() => transaction = Drive.Db.BankGateway.Transactions.Single(t => t.ApplicationId == applicationIdAccepted && t.BankIntegrationId == (int)BankGatewayIntegrationId.Bmo && t.TransactionStatus == (int)BankGatewayTransactionStatus.Failed));
            Do.With.Timeout(TimeSpan.FromSeconds(2)).While(() => Drive.Db.BankGateway.Acknowledges.Single(t => t.TransactionID == transaction.TransactionId && t.AcknowledgeTypeID == ackTypeSettled.AcknowledgeTypeId));
            Do.With.Timeout(TimeSpan.FromSeconds(2)).While(() => Drive.Db.BankGateway.Acknowledges.Single(t => t.TransactionID == transaction.TransactionId && t.AcknowledgeTypeID == ackTypeRejected.AcknowledgeTypeId));

            // Rejected: expect failed transaction, NO settlement ack, NO reject ack
            Do.Until(() => transaction = Drive.Db.BankGateway.Transactions.Single(t => t.ApplicationId == applicationIdRejected && t.BankIntegrationId == 2 && t.TransactionStatus == 5));
            Do.With.Timeout(TimeSpan.FromSeconds(2)).While(() => Drive.Db.BankGateway.Acknowledges.Single(t => t.TransactionID == transaction.TransactionId && t.AcknowledgeTypeID == ackTypeSettled.AcknowledgeTypeId));
            Do.With.Timeout(TimeSpan.FromSeconds(2)).While(() => Drive.Db.BankGateway.Acknowledges.Single(t => t.TransactionID == transaction.TransactionId && t.AcknowledgeTypeID == ackTypeRejected.AcknowledgeTypeId));
        }

        [Test, AUT(AUT.Ca), JIRA("CA-1914")]
        public void SettlementInvalidCrossreferenceNumberShouldPersistAck()
        {
            var customer = CustomerBuilder.New().
                                WithInstitutionNumber("001").
                                WithBranchNumber("00022").
                                Build();

            var customResponseOverride = new XElement("SettlementReportDetail",
                new XElement("Segments", new XAttribute("SenderReference", "INVALID DATA")));

            var setup = BmoResponseBuilder.New().
                                ForBankAccountNumber(customer.BankAccountNumber).
                                CustomOverride(customResponseOverride);

            var applicationId = ApplicationBuilder.New(customer).Build().Id;

            var ackType = Drive.Db.BankGateway.AcknowledgeTypes.Single(a => a.BankIntegrationId == 2 && a.Name == "DEFT220");

            var pendingTransaction = Do.Until(() => Drive.Db.BankGateway.Transactions.Single(
                t => t.ApplicationId == applicationId && t.BankIntegrationId == (int)BankGatewayIntegrationId.Bmo && t.TransactionStatus == (int)BankGatewayTransactionStatus.InProgress));

            // Transaction should NOT go into success status - we corrupted the response with invalid data
            Do.With.Timeout(TimeSpan.FromSeconds(5)).While(() => Drive.Db.BankGateway.Transactions.Single(
                t => t.ApplicationId == applicationId && t.BankIntegrationId == (int)BankGatewayIntegrationId.Bmo && t.TransactionStatus == (int)BankGatewayTransactionStatus.Paid));
        }

        [Test, AUT(AUT.Ca), JIRA("CA-1914")]
        public void SettlementValidAndInvalidCrossreferenceNumberShouldProcessValid()
        {
            Guid applicationWithInvalidResponseFromBank;
            Guid applicationWithValidResponseFromBank;

            using (new BmoBatchSending())
            {
                var customer = CustomerBuilder.New().
                    WithInstitutionNumber("001").
                    WithBranchNumber("00022").
                    Build();
                var customResponseOverride = new XElement("SettlementReportDetail",
                                                          new XElement("Segments", new XAttribute("SenderReference", "INVALID DATA")));
                var setup = BmoResponseBuilder.New().
                    ForBankAccountNumber(customer.BankAccountNumber).
                    CustomOverride(customResponseOverride);
                applicationWithInvalidResponseFromBank = ApplicationBuilder.New(customer).Build().Id;

                customer = CustomerBuilder.New().
                    WithInstitutionNumber("001").
                    WithBranchNumber("00022").
                    Build();
                applicationWithValidResponseFromBank = ApplicationBuilder.New(customer).Build().Id;
            }

            var ackType = Drive.Db.BankGateway.AcknowledgeTypes.Single(a => a.BankIntegrationId == (int)BankGatewayIntegrationId.Bmo && a.Name == "DEFT220");
            Framework.Db.BankGateway.TransactionEntity transaction = null;

            Do.Until(() => transaction = Drive.Db.BankGateway.Transactions.Single(t => t.ApplicationId == applicationWithValidResponseFromBank &&
                t.BankIntegrationId == (int)BankGatewayIntegrationId.Bmo && t.TransactionStatus == (int)BankGatewayTransactionStatus.Paid));
            Do.Until(() => Drive.Db.BankGateway.Acknowledges.Single(t => t.TransactionID == transaction.TransactionId && t.AcknowledgeTypeID == ackType.AcknowledgeTypeId));
        }

        [Test, AUT(AUT.Ca), JIRA("CA-1914")]
        public void SettlementSendPaymentsInOneBatchShouldUpdateTransactionStatusAndPersistAck()
        {
            var applicationIds = new List<Guid>();

            using (new BmoBatchSending())
            {
                for (int i = 0; i < 3; i++)
                {
                    var customer = CustomerBuilder.New().
                        WithInstitutionNumber("001").
                        WithBranchNumber("00022").
                        Build();

                    applicationIds.Add(ApplicationBuilder.New(customer).Build().Id);
                }
            }

            var ackType = Drive.Db.BankGateway.AcknowledgeTypes.Single(a => a.BankIntegrationId == (int)BankGatewayIntegrationId.Bmo && a.Name == "DEFT220");

            foreach (var applicationId in applicationIds)
            {
                Framework.Db.BankGateway.TransactionEntity transaction = null;

                Do.Until(() => transaction = Drive.Db.BankGateway.Transactions.Single(t => t.ApplicationId == applicationId &&
                    t.BankIntegrationId == (int)BankGatewayIntegrationId.Bmo && t.TransactionStatus == (int)BankGatewayTransactionStatus.Paid));
                Do.Until(() => Drive.Db.BankGateway.Acknowledges.Single(t => t.TransactionID == transaction.TransactionId && t.AcknowledgeTypeID == ackType.AcknowledgeTypeId));
            }
        }

        [Test, AUT(AUT.Ca), JIRA("CA-1914")]
        public void SettlementSendPaymentsAcceptedAndRejectedInOneBatchShouldUpdateTransactionStatusAndPersistAck()
        {
            Guid applicationIdAccepted;
            Guid applicationIdRejected;

            using (new BmoBatchSending())
            {
                // Rejected
                var customer = CustomerBuilder.New().
                                    WithInstitutionNumber("001").
                                    WithBranchNumber("00022").
                                    Build();
                var setup = BmoResponseBuilder.New().
                                    ForBankAccountNumber(customer.BankAccountNumber).
                                    RejectTransaction();

                applicationIdRejected = ApplicationBuilder.New(customer).Build().Id;

                // Accepted
                customer = CustomerBuilder.New().
                    WithInstitutionNumber("001").
                    WithBranchNumber("00022").
                    Build();

                applicationIdAccepted = ApplicationBuilder.New(customer).Build().Id;
            }

            var ackTypeSettled = Drive.Db.BankGateway.AcknowledgeTypes.Single(a => a.BankIntegrationId == 2 && a.Name == "DEFT220");
            var ackTypeRejected = Drive.Db.BankGateway.AcknowledgeTypes.Single(a => a.BankIntegrationId == 2 && a.Name == "DEFT210-211-260");

            Framework.Db.BankGateway.TransactionEntity transaction = null;

            // Accepted: expect successful transaction, settlement ack, NO reject ack
            Do.Until(() => transaction = Drive.Db.BankGateway.Transactions.Single(t => t.ApplicationId == applicationIdAccepted &&
                t.BankIntegrationId == (int)BankGatewayIntegrationId.Bmo && t.TransactionStatus == (int)BankGatewayTransactionStatus.Paid));
            Do.Until(() => Drive.Db.BankGateway.Acknowledges.Single(t => t.TransactionID == transaction.TransactionId && t.AcknowledgeTypeID == ackTypeSettled.AcknowledgeTypeId));
            Do.With.Timeout(TimeSpan.FromSeconds(2)).While(() => Drive.Db.BankGateway.Acknowledges.Single(t => t.TransactionID == transaction.TransactionId && t.AcknowledgeTypeID == ackTypeRejected.AcknowledgeTypeId));

            // Rejected: expect failed transaction, NO settlement ack, reject ack
            Do.Until(() => transaction = Drive.Db.BankGateway.Transactions.Single(t => t.ApplicationId == applicationIdRejected && t.BankIntegrationId == (int)BankGatewayIntegrationId.Bmo && t.TransactionStatus == (int)BankGatewayTransactionStatus.Failed));
            Do.Until(() => Drive.Db.BankGateway.Acknowledges.Single(t => t.TransactionID == transaction.TransactionId && t.AcknowledgeTypeID == ackTypeRejected.AcknowledgeTypeId));
            Do.With.Timeout(TimeSpan.FromSeconds(2)).While(() => Drive.Db.BankGateway.Acknowledges.Single(t => t.TransactionID == transaction.TransactionId && t.AcknowledgeTypeID == ackTypeSettled.AcknowledgeTypeId));
        }

        [Test, AUT(AUT.Ca), JIRA("CA-1914")]
        public void RejectedValidAndInvalidCrossreferenceNumberShouldProcessValid()
        {
            Guid applicationWithInvalidResponseFromBank;
            Guid applicationWithValidResponseFromBank;

            using (new BmoBatchSending())
            {
                // Rejected transaction with invalid crossreference number
                var customer = CustomerBuilder.New().
                    WithInstitutionNumber("001").
                    WithBranchNumber("00022").
                    WithSurname("Wonga").WithForename("Canada Inc").
                    Build();
                var customResponseOverride = new[]
						{
							new XElement("RejectedTransactionsReportDetail",
											new XElement("Segments", new XAttribute("CrossReferenceNumber", "INVALID DATA"))),
							new XElement("SettlementReportDetail", new XAttribute("LogicalRecordNumber", -1))
						};
                var setup = BmoResponseBuilder.New().
                    ForBankAccountNumber(customer.BankAccountNumber).
                    CustomOverride(customResponseOverride);
                applicationWithInvalidResponseFromBank = ApplicationBuilder.New(customer).Build().Id;

                // Rejected transaction with valid crossreference number
                customer = CustomerBuilder.New().
                    WithInstitutionNumber("001").
                    WithBranchNumber("00022").
                    WithSurname("Surname").WithForename("Forename").
                    Build();
                setup = BmoResponseBuilder.New().
                    ForBankAccountNumber(customer.BankAccountNumber).
                    RejectTransaction();
                applicationWithValidResponseFromBank = ApplicationBuilder.New(customer).Build().Id;
            }

            var ackType = Drive.Db.BankGateway.AcknowledgeTypes.Single(a => a.BankIntegrationId == (int)BankGatewayIntegrationId.Bmo && a.Name == "DEFT210-211-260");
            Framework.Db.BankGateway.TransactionEntity transaction = null;

            Do.Until(() => transaction = Drive.Db.BankGateway.Transactions.Single(t => t.ApplicationId == applicationWithValidResponseFromBank && t.BankIntegrationId == (int)BankGatewayIntegrationId.Bmo && t.TransactionStatus == (int)BankGatewayTransactionStatus.Failed));
            Do.Until(() => Drive.Db.BankGateway.Acknowledges.Single(t => t.TransactionID == transaction.TransactionId && t.AcknowledgeTypeID == ackType.AcknowledgeTypeId));
        }

        [Test, AUT(AUT.Ca), JIRA("CA-1914")]
        public void RejectedInvalidCrossreferenceNumberShouldPersistAck()
        {
            var customer = CustomerBuilder.New().
                                WithInstitutionNumber("001").
                                WithBranchNumber("00022").
                                WithSurname("Wonga").WithForename("Canada Inc").
                                Build();

            var customResponseOverride = new[]
					{
						new XElement("RejectedTransactionsReportDetail",
						             new XElement("Segments", new XAttribute("CrossReferenceNumber", "INVALID DATA"))),
						new XElement("SettlementReportDetail", new XAttribute("LogicalRecordNumber", -1))
					};

            var setup = BmoResponseBuilder.New().
                ForBankAccountNumber(customer.BankAccountNumber).
                CustomOverride(customResponseOverride);

            var applicationId = ApplicationBuilder.New(customer).Build().Id;

            var ackType = Drive.Db.BankGateway.AcknowledgeTypes.Single(a => a.BankIntegrationId == 2 && a.Name == "DEFT210-211-260");

            var pendingTransaction = Do.Until(() => Drive.Db.BankGateway.Transactions.Single(
                t => t.ApplicationId == applicationId && t.BankIntegrationId == (int)BankGatewayIntegrationId.Bmo && t.TransactionStatus == (int)BankGatewayTransactionStatus.InProgress));

            // Transaction should NOT go into success status - we corrupted the response with invalid data
            Do.With.Timeout(TimeSpan.FromSeconds(5)).While(() => Drive.Db.BankGateway.Transactions.Single(
                t => t.ApplicationId == applicationId && t.BankIntegrationId == (int)BankGatewayIntegrationId.Bmo && t.TransactionStatus == (int)BankGatewayTransactionStatus.Paid));
        }

        [Test, AUT(AUT.Ca), JIRA("CA-1914")]
        public void WhenMultipleBmoTransactionsAreHandledTogetherThenBankGatewayShouldHandleThemAsABatch()
        {
            var applicationIds = new List<Guid>();

            using (new BmoBatchSending())
            {
                for (int i = 0; i < 3; i++)
                {
                    var customer = CustomerBuilder.New().
                        WithInstitutionNumber("001").
                        WithBranchNumber("00022").
                        Build();

                    applicationIds.Add(ApplicationBuilder.New(customer).Build().Id);
                }

                var transactions = new List<TransactionEntity>();

                foreach (var applicationId in applicationIds)
                {
                    TransactionEntity transaction = null;
                    Do.Until(() => transaction = Drive.Db.BankGateway.Transactions.Single(
                                t => t.ApplicationId == applicationId &&
                                     t.BankIntegrationId == (int)BankGatewayIntegrationId.Bmo &&
                                     t.TransactionStatus == (int)BankGatewayTransactionStatus.New));

                    if (transaction != null)
                    {
                        transactions.Add(transaction);
                    }
                }

                Assert.AreEqual(applicationIds.Count, transactions.Count);
            }

            var ackType = Drive.Db.BankGateway.AcknowledgeTypes.Single(a => a.BankIntegrationId == (int)BankGatewayIntegrationId.Bmo && a.Name == "DEFT220");

            foreach (var applicationId in applicationIds)
            {
                TransactionEntity transaction = null;

                Do.Until(() => transaction = Drive.Db.BankGateway.Transactions.Single(
                            t => t.ApplicationId == applicationId &&
                                 t.BankIntegrationId == (int)BankGatewayIntegrationId.Bmo &&
                                 t.TransactionStatus == (int)BankGatewayTransactionStatus.Paid));
            }
        }

        [Test, AUT(AUT.Ca), JIRA("CA-1914")]
        public void WhenLoanGoesToBmoThenItShouldRecieveSettlementReport200ToIndicateASuccessfulCashOut()
        {
            const int loanTerm = 15;
            const decimal loanAmount = 100;

            var customer = CustomerBuilder.New().
                                WithInstitutionNumber("001").
                                WithBranchNumber("00022").
                                ForProvince(ProvinceEnum.ON).
                                Build();

            var application = ApplicationBuilder.New(customer).WithLoanTerm(loanTerm).WithLoanAmount(loanAmount).Build();

            TransactionEntity transaction = WaitBankGatewayFunctions.WaitForStatusOfTransaction(application.Id);

            WaitBankGatewayFunctions.WaitForAckForTransaction(transaction);
        }

        [Test, AUT(AUT.Ca), JIRA("CA-1965"), FeatureSwitch(FeatureSwitchConstants.BmoFeatureSwitchKey)]
        public void RejectedSendPaymentShouldUpdateTransactionStatusAndPersistAck()
        {
            var customer = CustomerBuilder.New().
                    WithInstitutionNumber("001").
                    WithBranchNumber("00022").
                    Build();

            var setup = BmoResponseBuilder.New().
                    ForBankAccountNumber(customer.BankAccountNumber).
                    RejectTransaction();

            var application = ApplicationBuilder.New(customer).Build();

            Framework.Db.BankGateway.TransactionEntity transaction = null;

            var ackType = Drive.Db.BankGateway.AcknowledgeTypes.Single(a => a.BankIntegrationId == (int)BankGatewayIntegrationId.Bmo && a.Name == "DEFT210-211-260");

            Do.Until(() => transaction = Drive.Db.BankGateway.Transactions.Single(t => t.ApplicationId == application.Id && t.BankIntegrationId == (int)BankGatewayIntegrationId.Bmo && t.TransactionStatus == (int)BankGatewayTransactionStatus.Failed));
            Do.Until(() => Drive.Db.BankGateway.Acknowledges.Single(t => t.TransactionID == transaction.TransactionId && t.AcknowledgeTypeID == ackType.AcknowledgeTypeId));
        }
    }
}
