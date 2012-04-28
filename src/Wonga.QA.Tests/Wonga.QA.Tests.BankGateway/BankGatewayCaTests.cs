using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Mocks;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.BankGateway
{
	[TestFixture, AUT(AUT.Ca)]
	public class BankGatewayCaTests
	{
		private const int TransactionStatusInProgress = 3;
		private const int TransactionStatusSuccess = 4;
		private const int TransactionStatusFailed = 5;
		private const int BankIntegrationIdScotia = 1;
		private const int BankIntegrationIdBmo = 2;

		private static readonly Random Random = new Random(Environment.TickCount);

		[Test, AUT(AUT.Ca), JIRA("CA-1914"), FeatureSwitch(Constants.BmoFeatureSwitchKey)]
		public void SendPaymentMessageWithRealAccountShouldBeRoutedToBmo()
		{
			var customer = CustomerBuilder.New().
                                WithInstitutionNumber("001").
								WithBranchNumber("00022").
								WithBankAccountNumber(1641421).
								WithSurname("Wonga").WithForename("Canada Inc").
                                Build();
			var application = ApplicationBuilder.New(customer).Build();

			Do.Until(() => Drive.Db.BankGateway.Transactions.Single(
				t => t.ApplicationId == application.Id &&
					t.BankIntegrationId == BankIntegrationIdBmo &&
					t.TransactionStatus == TransactionStatusSuccess));
		}

		[Test, AUT(AUT.Ca), JIRA("CA-1914"), FeatureSwitch(Constants.BmoFeatureSwitchKey)]
		public void SendPaymentMessageWithRealAccountShouldBeRoutedToBmoAndRejected()
		{
			var bankAccountNumber = Random.Next(1000000, 9999999);
			
			var customer = CustomerBuilder.New().
								WithInstitutionNumber("001").
								WithBranchNumber("00022").
								WithBankAccountNumber(bankAccountNumber).
								WithSurname("Wonga").WithForename("Canada Inc").
								Build();

			var setup = BmoResponseBuilder.New().
								ForBankAccountNumber(bankAccountNumber).
								RejectTransaction();

			var application = ApplicationBuilder.New(customer).Build();

			Do.Until(() => Drive.Db.BankGateway.Transactions.Single(
				t => t.ApplicationId == application.Id &&
					t.BankIntegrationId == BankIntegrationIdBmo &&
					t.TransactionStatus == TransactionStatusFailed));
		}

        [Test, AUT(AUT.Ca), JIRA("CA-1914"), FeatureSwitch(Constants.BmoFeatureSwitchKey)]
		public void SendPaymentMessageWithRealAccountShouldBeRoutedToBmoAndRejectedFile()
		{
			var bankAccountNumber = Random.Next(1000000, 9999999);

			var customer = CustomerBuilder.New().
								WithInstitutionNumber("001").
								WithBranchNumber("00022").
								WithBankAccountNumber(bankAccountNumber).
								WithSurname("Wonga").WithForename("Canada Inc").
								Build();

			var setup = BmoResponseBuilder.New().
								ForBankAccountNumber(bankAccountNumber).
								RejectFile();

			var application = ApplicationBuilder.New(customer).Build();

			Do.Until(() => Drive.Db.BankGateway.Transactions.Single(
				t => t.ApplicationId == application.Id &&
					t.BankIntegrationId == BankIntegrationIdBmo &&
					t.TransactionStatus == TransactionStatusFailed));
		}

        [Test, AUT(AUT.Ca), JIRA("CA-1914"), FeatureSwitch(Constants.BmoFeatureSwitchKey)]
		public void SendPaymentMessageWithFakeAccountShouldBeRoutedToBmo()
		{
			var customer = CustomerBuilder.New().
								WithInstitutionNumber("001").
								WithBranchNumber("00022").
								WithBankAccountNumber(9999999).
								WithSurname("Surname").WithForename("Forename").
								Build();
			var application = ApplicationBuilder.New(customer).Build();

			Framework.Db.BankGateway.TransactionEntity transaction = null;

			var ackType = Drive.Db.BankGateway.AcknowledgeTypes.Single(a => a.BankIntegrationId == BankIntegrationIdBmo && a.Name == "DEFT220");

			Do.Until(() => transaction = Drive.Db.BankGateway.Transactions.Single(t => t.ApplicationId == application.Id &&
				t.TransactionStatus == TransactionStatusSuccess && t.BankIntegrationId == BankIntegrationIdBmo));
			Do.Until(() => Drive.Db.BankGateway.Acknowledges.Single(t => t.TransactionID == transaction.TransactionId && t.AcknowledgeTypeID == ackType.AcknowledgeTypeId));
		}

		[Test, AUT(AUT.Ca), JIRA("CA-1914"), FeatureSwitch(Constants.BmoFeatureSwitchKey)]
		public void SendPaymentMessageShouldBeRoutedToScotia()
		{
            var customer = CustomerBuilder.New().
                                WithInstitutionNumber("002").
                                WithBranchNumber("00018").
                                Build();
            var application = ApplicationBuilder.New(customer).Build();

			Do.Until(() => Drive.Db.BankGateway.Transactions.Single(
				t => t.ApplicationId == application.Id &&
					t.BankIntegrationId == BankIntegrationIdScotia));
		}

		[Test, AUT(AUT.Ca), JIRA("CA-1914"), FeatureSwitch(Constants.BmoFeatureSwitchKey)]
		public void SendPaymentMessageShouldBeRoutedToScotiaAndRejected()
		{
			var bankAccountNumber = Random.Next(1000000, 9999999);

			var customer = CustomerBuilder.New().
								WithBankAccountNumber(bankAccountNumber).
								WithInstitutionNumber("002").
								WithBranchNumber("00018").
								Build();

			var setup = ScotiaResponseBuilder.New().
								ForBankAccountNumber(bankAccountNumber).
								Reject();

			var application = ApplicationBuilder.New(customer).Build();

			Do.Until(() => Drive.Db.BankGateway.Transactions.Single(
				t => t.ApplicationId == application.Id &&
					t.BankIntegrationId == BankIntegrationIdScotia &&
					t.TransactionStatus == TransactionStatusFailed ));
		}

        [Test, AUT(AUT.Ca), JIRA("CA-1914"), FeatureSwitch(Constants.BmoFeatureSwitchKey)]
		public void PositiveFileAcknowledgementShouldBePersisted()
		{
			var ackType = Drive.Db.BankGateway.AcknowledgeTypes.Single(a => a.BankIntegrationId == 2 && a.Name == "DEFT200");
			
			var previousAck = Drive.Db.BankGateway.Acknowledges.
				Where(a => a.AcknowledgeTypeID == ackType.AcknowledgeTypeId).
				OrderByDescending(a => a.AcknowledgeId).Take(1).SingleOrDefault();

			var previousAckId = previousAck != null ? previousAck.AcknowledgeId : -1;

			SendPaymentMessageWithFakeAccountShouldBeRoutedToBmo();

			Do.Until(() =>
			         	{
			         		var latestAck = Drive.Db.BankGateway.Acknowledges.Where(
								a => a.AcknowledgeTypeID == ackType.AcknowledgeTypeId).
			         				OrderByDescending(a => a.AcknowledgeId).Take(1).Single();

							Assert.IsFalse(latestAck.HasError);

			         		return latestAck.AcknowledgeId != previousAckId;
			         	});
		}

        [Test, AUT(AUT.Ca), JIRA("CA-1914"), FeatureSwitch(Constants.BmoFeatureSwitchKey)]
		public void NegativeFileAcknowledgementShouldBePersistedAndAllTransactionsRejected()
		{
			Guid applicationIdAccepted = Guid.Empty;
			Guid applicationIdRejected = Guid.Empty;

			using (new BankGatewayBmoSendBatch())
			{
				// Rejected
				var bankAccountNumber = Random.Next(1000000, 9999999);
				var customer = CustomerBuilder.New().
									WithInstitutionNumber("001").
									WithBranchNumber("00022").
									WithBankAccountNumber(bankAccountNumber).
									WithSurname("Wonga").WithForename("Canada Inc").
									Build();
				var setup = BmoResponseBuilder.New().
									ForBankAccountNumber(bankAccountNumber).
									RejectFile();

				applicationIdRejected = ApplicationBuilder.New(customer).Build().Id;

				// Accepted
				customer = CustomerBuilder.New().
					WithInstitutionNumber("001").
					WithBranchNumber("00022").
					WithBankAccountNumber(9999999).
					WithSurname("Surname").WithForename("Forename").
					Build();

				applicationIdAccepted = ApplicationBuilder.New(customer).Build().Id;
			}

			var ackTypeSettled = Drive.Db.BankGateway.AcknowledgeTypes.Single(a => a.BankIntegrationId == BankIntegrationIdBmo && a.Name == "DEFT220");
			var ackTypeRejected = Drive.Db.BankGateway.AcknowledgeTypes.Single(a => a.BankIntegrationId == BankIntegrationIdBmo && a.Name == "DEFT210-211-260");

			Framework.Db.BankGateway.TransactionEntity transaction = null;

			// Accepted: expect fail transaction, NO settlement ack, NO reject ack
			Do.Until(() => transaction = Drive.Db.BankGateway.Transactions.Single(t => t.ApplicationId == applicationIdAccepted && t.BankIntegrationId == BankIntegrationIdBmo && t.TransactionStatus == TransactionStatusFailed));
			Do.With.Timeout(TimeSpan.FromSeconds(2)).While(() => Drive.Db.BankGateway.Acknowledges.Single(t => t.TransactionID == transaction.TransactionId && t.AcknowledgeTypeID == ackTypeSettled.AcknowledgeTypeId));
			Do.With.Timeout(TimeSpan.FromSeconds(2)).While(() => Drive.Db.BankGateway.Acknowledges.Single(t => t.TransactionID == transaction.TransactionId && t.AcknowledgeTypeID == ackTypeRejected.AcknowledgeTypeId));

			// Rejected: expect failed transaction, NO settlement ack, NO reject ack
			Do.Until(() => transaction = Drive.Db.BankGateway.Transactions.Single(t => t.ApplicationId == applicationIdRejected && t.BankIntegrationId == 2 && t.TransactionStatus == 5));
			Do.With.Timeout(TimeSpan.FromSeconds(2)).While(() => Drive.Db.BankGateway.Acknowledges.Single(t => t.TransactionID == transaction.TransactionId && t.AcknowledgeTypeID == ackTypeSettled.AcknowledgeTypeId));
			Do.With.Timeout(TimeSpan.FromSeconds(2)).While(() => Drive.Db.BankGateway.Acknowledges.Single(t => t.TransactionID == transaction.TransactionId && t.AcknowledgeTypeID == ackTypeRejected.AcknowledgeTypeId));
		}

        [Test, AUT(AUT.Ca), JIRA("CA-1914"), FeatureSwitch(Constants.BmoFeatureSwitchKey)]
		public void SettlementInvalidCrossreferenceNumberShouldPersistAck()
		{
			var bankAccountNumber = Random.Next(1000000, 9999999);
			var customer = CustomerBuilder.New().
								WithInstitutionNumber("001").
								WithBranchNumber("00022").
								WithBankAccountNumber(bankAccountNumber).
								WithSurname("Wonga").WithForename("Canada Inc").
								Build();
			
			var customResponseOverride = new XElement("SettlementReportDetail",
				new XElement("Segments", new XAttribute("SenderReference", "INVALID DATA")));

			var setup = BmoResponseBuilder.New().
								ForBankAccountNumber(bankAccountNumber).
								CustomOverride(customResponseOverride);

			var applicationId = ApplicationBuilder.New(customer).Build().Id;

			var ackType = Drive.Db.BankGateway.AcknowledgeTypes.Single(a => a.BankIntegrationId == 2 && a.Name == "DEFT220");

			var pendingTransaction = Do.Until(() => Drive.Db.BankGateway.Transactions.Single(
				t => t.ApplicationId == applicationId && t.BankIntegrationId == BankIntegrationIdBmo && t.TransactionStatus == TransactionStatusInProgress));

			// Transaction should NOT go into success status - we corrupted the response with invalid data
			Do.With.Timeout(TimeSpan.FromSeconds(5)).While(() => Drive.Db.BankGateway.Transactions.Single(
				t => t.ApplicationId == applicationId && t.BankIntegrationId == BankIntegrationIdBmo && t.TransactionStatus == TransactionStatusSuccess));
		}

		[Test, AUT(AUT.Ca), JIRA("CA-1914"), FeatureSwitch(Constants.BmoFeatureSwitchKey)]
		public void SettlementValidAndInvalidCrossreferenceNumberShouldProcessValid()
		{
			Guid applicationWithInvalidResponseFromBank;
			Guid applicationWithValidResponseFromBank;

			using (new BankGatewayBmoSendBatch())
			{
				var bankAccountNumber = Random.Next(1000000, 9999999);
				var customer = CustomerBuilder.New().
					WithInstitutionNumber("001").
					WithBranchNumber("00022").
					WithBankAccountNumber(bankAccountNumber).
					WithSurname("Wonga").WithForename("Canada Inc").
					Build();
				var customResponseOverride = new XElement("SettlementReportDetail",
				                                          new XElement("Segments", new XAttribute("SenderReference", "INVALID DATA")));
				var setup = BmoResponseBuilder.New().
					ForBankAccountNumber(bankAccountNumber).
					CustomOverride(customResponseOverride);
				applicationWithInvalidResponseFromBank = ApplicationBuilder.New(customer).Build().Id;

				customer = CustomerBuilder.New().
					WithInstitutionNumber("001").
					WithBranchNumber("00022").
					WithBankAccountNumber(9999999).
					WithSurname("Surname").WithForename("Forename").
					Build();
				applicationWithValidResponseFromBank = ApplicationBuilder.New(customer).Build().Id;
			}

			var ackType = Drive.Db.BankGateway.AcknowledgeTypes.Single(a => a.BankIntegrationId == BankIntegrationIdBmo && a.Name == "DEFT220");
			Framework.Db.BankGateway.TransactionEntity transaction = null;

			Do.Until(() => transaction = Drive.Db.BankGateway.Transactions.Single(t => t.ApplicationId == applicationWithValidResponseFromBank &&
				t.BankIntegrationId == BankIntegrationIdBmo && t.TransactionStatus == TransactionStatusSuccess));
			Do.Until(() => Drive.Db.BankGateway.Acknowledges.Single(t => t.TransactionID == transaction.TransactionId && t.AcknowledgeTypeID == ackType.AcknowledgeTypeId));
		}

        [Test, AUT(AUT.Ca), JIRA("CA-1914"), FeatureSwitch(Constants.BmoFeatureSwitchKey)]
		public void SettlementSendPaymentsInOneBatchShouldUpdateTransactionStatusAndPersistAck()
		{
			var applicationIds = new List<Guid>();

			using (new BankGatewayBmoSendBatch())
			{
				for (int i = 0; i < 3; i++)
				{
					var customer = CustomerBuilder.New().
						WithInstitutionNumber("001").
						WithBranchNumber("00022").
						WithBankAccountNumber(9999999).
						WithSurname("Surname").WithForename("Forename").
						Build();

					applicationIds.Add(ApplicationBuilder.New(customer).Build().Id);
				}
			}

			var ackType = Drive.Db.BankGateway.AcknowledgeTypes.Single(a => a.BankIntegrationId == BankIntegrationIdBmo && a.Name == "DEFT220");

			foreach (var applicationId in applicationIds)
			{
				Framework.Db.BankGateway.TransactionEntity transaction = null;

				Do.Until(() => transaction = Drive.Db.BankGateway.Transactions.Single(t => t.ApplicationId == applicationId &&
					t.BankIntegrationId == BankIntegrationIdBmo && t.TransactionStatus == TransactionStatusSuccess));
				Do.Until(() => Drive.Db.BankGateway.Acknowledges.Single(t => t.TransactionID == transaction.TransactionId && t.AcknowledgeTypeID == ackType.AcknowledgeTypeId));
			}
		}

        [Test, AUT(AUT.Ca), JIRA("CA-1914"), FeatureSwitch(Constants.BmoFeatureSwitchKey)]
		public void SettlementSendPaymentsAcceptedAndRejectedInOneBatchShouldUpdateTransactionStatusAndPersistAck()
		{
			Guid applicationIdAccepted = Guid.Empty;
			Guid applicationIdRejected = Guid.Empty;

			using (new BankGatewayBmoSendBatch())
			{
				// Rejected
				var bankAccountNumber = Random.Next(1000000, 9999999);
				var customer = CustomerBuilder.New().
									WithInstitutionNumber("001").
									WithBranchNumber("00022").
									WithBankAccountNumber(bankAccountNumber).
									WithSurname("Wonga").WithForename("Canada Inc").
									Build();
				var setup = BmoResponseBuilder.New().
									ForBankAccountNumber(bankAccountNumber).
									RejectTransaction();

				applicationIdRejected = ApplicationBuilder.New(customer).Build().Id;

				// Accepted
				customer = CustomerBuilder.New().
					WithInstitutionNumber("001").
					WithBranchNumber("00022").
					WithBankAccountNumber(9999999).
					WithSurname("Surname").WithForename("Forename").
					Build();

				applicationIdAccepted = ApplicationBuilder.New(customer).Build().Id;
			}

			var ackTypeSettled = Drive.Db.BankGateway.AcknowledgeTypes.Single(a => a.BankIntegrationId == 2 && a.Name == "DEFT220");
			var ackTypeRejected = Drive.Db.BankGateway.AcknowledgeTypes.Single(a => a.BankIntegrationId == 2 && a.Name == "DEFT210-211-260");

			Framework.Db.BankGateway.TransactionEntity transaction = null;

			// Accepted: expect successful transaction, settlement ack, NO reject ack
			Do.Until(() => transaction = Drive.Db.BankGateway.Transactions.Single(t => t.ApplicationId == applicationIdAccepted &&
				t.BankIntegrationId == BankIntegrationIdBmo && t.TransactionStatus == TransactionStatusSuccess));
			Do.Until(() => Drive.Db.BankGateway.Acknowledges.Single(t => t.TransactionID == transaction.TransactionId && t.AcknowledgeTypeID == ackTypeSettled.AcknowledgeTypeId));
			Do.With.Timeout(TimeSpan.FromSeconds(2)).While(() => Drive.Db.BankGateway.Acknowledges.Single(t => t.TransactionID == transaction.TransactionId && t.AcknowledgeTypeID == ackTypeRejected.AcknowledgeTypeId));

			// Rejected: expect failed transaction, NO settlement ack, reject ack
			Do.Until(() => transaction = Drive.Db.BankGateway.Transactions.Single(t => t.ApplicationId == applicationIdRejected && t.BankIntegrationId == BankIntegrationIdBmo && t.TransactionStatus == TransactionStatusFailed));
			Do.Until(() => Drive.Db.BankGateway.Acknowledges.Single(t => t.TransactionID == transaction.TransactionId && t.AcknowledgeTypeID == ackTypeRejected.AcknowledgeTypeId));
			Do.With.Timeout(TimeSpan.FromSeconds(2)).While(() => Drive.Db.BankGateway.Acknowledges.Single(t => t.TransactionID == transaction.TransactionId && t.AcknowledgeTypeID == ackTypeSettled.AcknowledgeTypeId));
		}

		[Test, AUT(AUT.Ca), JIRA("CA-1914"), FeatureSwitch(Constants.BmoFeatureSwitchKey)]
		public void RejectedValidAndInvalidCrossreferenceNumberShouldProcessValid()
		{
			Guid applicationWithInvalidResponseFromBank;
			Guid applicationWithValidResponseFromBank;

			using (new BankGatewayBmoSendBatch())
			{
				// Rejected transaction with invalid crossreference number
				var bankAccountNumber = Random.Next(1000000, 9999999);
				var customer = CustomerBuilder.New().
					WithInstitutionNumber("001").
					WithBranchNumber("00022").
					WithBankAccountNumber(bankAccountNumber).
					WithSurname("Wonga").WithForename("Canada Inc").
					Build();
				var customResponseOverride = new[]
						{
							new XElement("RejectedTransactionsReportDetail",
											new XElement("Segments", new XAttribute("CrossReferenceNumber", "INVALID DATA"))),
							new XElement("SettlementReportDetail", new XAttribute("LogicalRecordNumber", -1))
						};
				var setup = BmoResponseBuilder.New().
					ForBankAccountNumber(bankAccountNumber).
					CustomOverride(customResponseOverride);
				applicationWithInvalidResponseFromBank = ApplicationBuilder.New(customer).Build().Id;

				// Rejected transaction with valid crossreference number
				bankAccountNumber = Random.Next(1000000, 9999999);
				customer = CustomerBuilder.New().
					WithInstitutionNumber("001").
					WithBranchNumber("00022").
					WithBankAccountNumber(bankAccountNumber).
					WithSurname("Surname").WithForename("Forename").
					Build();
				setup = BmoResponseBuilder.New().
					ForBankAccountNumber(bankAccountNumber).
					RejectTransaction();
				applicationWithValidResponseFromBank = ApplicationBuilder.New(customer).Build().Id;
			}

			var ackType = Drive.Db.BankGateway.AcknowledgeTypes.Single(a => a.BankIntegrationId == BankIntegrationIdBmo && a.Name == "DEFT210-211-260");
			Framework.Db.BankGateway.TransactionEntity transaction = null;

			Do.Until(() => transaction = Drive.Db.BankGateway.Transactions.Single(t => t.ApplicationId == applicationWithValidResponseFromBank && t.BankIntegrationId == BankIntegrationIdBmo && t.TransactionStatus == TransactionStatusFailed));
			Do.Until(() => Drive.Db.BankGateway.Acknowledges.Single(t => t.TransactionID == transaction.TransactionId && t.AcknowledgeTypeID == ackType.AcknowledgeTypeId));
		}

		[Test, AUT(AUT.Ca), JIRA("CA-1914"), FeatureSwitch(Constants.BmoFeatureSwitchKey)]
		public void RejectedInvalidCrossreferenceNumberShouldPersistAck()
		{
			var bankAccountNumber = Random.Next(1000000, 9999999);
			var customer = CustomerBuilder.New().
								WithInstitutionNumber("001").
								WithBranchNumber("00022").
								WithBankAccountNumber(bankAccountNumber).
								WithSurname("Wonga").WithForename("Canada Inc").
								Build();

			var customResponseOverride = new[]
					{
						new XElement("RejectedTransactionsReportDetail",
						             new XElement("Segments", new XAttribute("CrossReferenceNumber", "INVALID DATA"))),
						new XElement("SettlementReportDetail", new XAttribute("LogicalRecordNumber", -1))
					};

			var setup = BmoResponseBuilder.New().
				ForBankAccountNumber(bankAccountNumber).
				CustomOverride(customResponseOverride);

			var applicationId = ApplicationBuilder.New(customer).Build().Id;

			var ackType = Drive.Db.BankGateway.AcknowledgeTypes.Single(a => a.BankIntegrationId == 2 && a.Name == "DEFT210-211-260");

			var pendingTransaction = Do.Until(() => Drive.Db.BankGateway.Transactions.Single(
				t => t.ApplicationId == applicationId && t.BankIntegrationId == BankIntegrationIdBmo && t.TransactionStatus == TransactionStatusInProgress));

			// Transaction should NOT go into success status - we corrupted the response with invalid data
			Do.With.Timeout(TimeSpan.FromSeconds(5)).While(() => Drive.Db.BankGateway.Transactions.Single(
				t => t.ApplicationId == applicationId && t.BankIntegrationId == BankIntegrationIdBmo && t.TransactionStatus == TransactionStatusSuccess));
		}
	}
}
