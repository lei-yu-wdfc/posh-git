using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        private const int TransactionStatusPaid = 4;
		private const int TransactionStatusFailed = 5;
		private const int BankIntegrationIdScotia = 1;
        private const int BankIntegrationIdBmo = 2;
		private static readonly Random Random = new Random(Environment.TickCount);

		[Test, AUT(AUT.Ca), JIRA("CA-1931"), FeatureSwitch(Constants.BmoFeatureSwitchKey)]
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
					t.TransactionStatus == TransactionStatusPaid));
		}

		[Test, AUT(AUT.Ca), JIRA("CA-1931"), FeatureSwitch(Constants.BmoFeatureSwitchKey)]
		public void SendPaymentMessageWithRealAccountShouldBeRoutedToBmoAndRejected()
		{	
			var customer = CustomerBuilder.New().
								WithInstitutionNumber("001").
								WithBranchNumber("00022").
								WithSurname("Wonga").WithForename("Canada Inc").
								Build();

		    BmoResponseBuilder.New().
		        ForBankAccountNumber(customer.BankAccountNumber).
		        RejectTransaction();

			var application = ApplicationBuilder.New(customer).Build();

			Do.Until(() => Drive.Db.BankGateway.Transactions.Single(
				t => t.ApplicationId == application.Id &&
					t.BankIntegrationId == BankIntegrationIdBmo &&
					t.TransactionStatus == TransactionStatusFailed));
		}

        [Test, AUT(AUT.Ca), JIRA("CA-1931"), FeatureSwitch(Constants.BmoFeatureSwitchKey)]
		public void SendPaymentMessageWithRealAccountShouldBeRoutedToBmoAndRejectedFile()
		{
			var customer = CustomerBuilder.New().
								WithInstitutionNumber("001").
								WithBranchNumber("00022").
								WithSurname("Wonga").WithForename("Canada Inc").
								Build();

            BmoResponseBuilder.New().
                ForBankAccountNumber(customer.BankAccountNumber).
                RejectFile();

			var application = ApplicationBuilder.New(customer).Build();

			Do.Until(() => Drive.Db.BankGateway.Transactions.Single(
				t => t.ApplicationId == application.Id &&
					t.BankIntegrationId == BankIntegrationIdBmo &&
					t.TransactionStatus == TransactionStatusFailed));
		}

        [Test, AUT(AUT.Ca), JIRA("CA-1931"), FeatureSwitch(Constants.BmoFeatureSwitchKey)]
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

            Do.Until(
                () =>
                transaction =
                Drive.Db.BankGateway.Transactions.Single(
                    t =>
                    t.ApplicationId == application.Id && t.TransactionStatus == TransactionStatusPaid &&
                    t.BankIntegrationId == BankIntegrationIdBmo));

            Do.Until(
                () =>
                Drive.Db.BankGateway.Acknowledges.Single(
                    t =>
                    t.TransactionID == transaction.TransactionId && t.AcknowledgeTypeID == ackType.AcknowledgeTypeId));
		}

        [Test, AUT(AUT.Ca), JIRA("CA-1931"), FeatureSwitch(Constants.BmoFeatureSwitchKey)]
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

        [Test, AUT(AUT.Ca), JIRA("CA-1931"), FeatureSwitch(Constants.BmoFeatureSwitchKey)]
		public void SendPaymentMessageShouldBeRoutedToScotiaAndRejected()
		{
			var customer = CustomerBuilder.New().
								WithInstitutionNumber("002").
								WithBranchNumber("00018").
								Build();

            ScotiaResponseBuilder.New().
                ForBankAccountNumber(customer.BankAccountNumber).
                Reject();

			var application = ApplicationBuilder.New(customer).Build();

			Do.Until(() => Drive.Db.BankGateway.Transactions.Single(
				t => t.ApplicationId == application.Id &&
					t.BankIntegrationId == BankIntegrationIdScotia &&
					t.TransactionStatus == TransactionStatusFailed ));
		}

        [Test, AUT(AUT.Ca), JIRA("CA-1931"), FeatureSwitch(Constants.BmoFeatureSwitchKey)]
		public void PositiveFileAcknowledgementShouldBePersisted()
		{
			var ackType = Drive.Db.BankGateway.AcknowledgeTypes.Single(a => a.BankIntegrationId == BankIntegrationIdBmo && a.Name == "DEFT200");
			
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

        [Test, AUT(AUT.Ca), JIRA("CA-1931"), Ignore("Requires fix in Mock to not send any reports after ack file rejected."), FeatureSwitch(Constants.BmoFeatureSwitchKey)]
		public void NegativeFileAcknowledgementShouldBePersistedAndAllTransactionsRejected()
		{
			Guid applicationIdAccepted = Guid.Empty;
			Guid applicationIdRejected = Guid.Empty;

			using (new BankGatewayBmoSendBatch())
			{
				// Rejected
				var customer = CustomerBuilder.New().
									WithInstitutionNumber("001").
									WithBranchNumber("00022").
									WithSurname("Wonga").WithForename("Canada Inc").
									Build();
			    BmoResponseBuilder.New().
			        ForBankAccountNumber(customer.BankAccountNumber).
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

            var ackTypeSettled =
                Drive.Db.BankGateway.AcknowledgeTypes.Single(
                    a => a.BankIntegrationId == BankIntegrationIdBmo && a.Name == "DEFT220");

            var ackTypeRejected =
                Drive.Db.BankGateway.AcknowledgeTypes.Single(
                    a => a.BankIntegrationId == BankIntegrationIdBmo && a.Name == "DEFT210-211-260");

			Framework.Db.BankGateway.TransactionEntity transaction = null;

			// Accepted: expect fail transaction, NO settlement ack, NO reject ack
            Do.Until(
                () =>
                transaction =
                Drive.Db.BankGateway.Transactions.Single(
                    t =>
                    t.ApplicationId == applicationIdAccepted && t.BankIntegrationId == BankIntegrationIdBmo &&
                    t.TransactionStatus == TransactionStatusFailed));

            Do.With.Timeout(TimeSpan.FromSeconds(2)).While(
                () =>
                Drive.Db.BankGateway.Acknowledges.Single(
                    t =>
                    t.TransactionID == transaction.TransactionId &&
                    t.AcknowledgeTypeID == ackTypeSettled.AcknowledgeTypeId));

            Do.With.Timeout(TimeSpan.FromSeconds(2)).While(
                () =>
                Drive.Db.BankGateway.Acknowledges.Single(
                    t =>
                    t.TransactionID == transaction.TransactionId &&
                    t.AcknowledgeTypeID == ackTypeRejected.AcknowledgeTypeId));


			// Rejected: expect failed transaction, NO settlement ack, NO reject ack
            Do.Until(
                () =>
                transaction =
                Drive.Db.BankGateway.Transactions.Single(
                    t =>
                    t.ApplicationId == applicationIdRejected && t.BankIntegrationId == BankIntegrationIdBmo &&
                    t.TransactionStatus == TransactionStatusFailed));

            Do.With.Timeout(TimeSpan.FromSeconds(2)).While(
                () =>
                Drive.Db.BankGateway.Acknowledges.Single(
                    t =>
                    t.TransactionID == transaction.TransactionId &&
                    t.AcknowledgeTypeID == ackTypeSettled.AcknowledgeTypeId));

            Do.With.Timeout(TimeSpan.FromSeconds(2)).While(
                () =>
                Drive.Db.BankGateway.Acknowledges.Single(
                    t =>
                    t.TransactionID == transaction.TransactionId &&
                    t.AcknowledgeTypeID == ackTypeRejected.AcknowledgeTypeId));
		}

        [Test, AUT(AUT.Ca), JIRA("CA-1931"), FeatureSwitch(Constants.BmoFeatureSwitchKey)]
		public void SettlementSendPaymentShouldUpdateTransactionStatusAndPersistAck()
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

            Do.Until(
                () =>
                transaction =
                Drive.Db.BankGateway.Transactions.Single(
                    t => t.ApplicationId == application.Id && t.BankIntegrationId == BankIntegrationIdBmo && t.TransactionStatus == TransactionStatusPaid));

            Do.Until(
                () =>
                Drive.Db.BankGateway.Acknowledges.Single(
                    t =>
                    t.TransactionID == transaction.TransactionId && t.AcknowledgeTypeID == ackType.AcknowledgeTypeId));

		}

        [Test, AUT(AUT.Ca), JIRA("CA-1931"), FeatureSwitch(Constants.BmoFeatureSwitchKey)]
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

				Do.Until(() => transaction = Drive.Db.BankGateway.Transactions.Single(t => t.ApplicationId == applicationId && t.BankIntegrationId == BankIntegrationIdBmo && t.TransactionStatus == TransactionStatusPaid));
				Do.Until(() => Drive.Db.BankGateway.Acknowledges.Single(t => t.TransactionID == transaction.TransactionId && t.AcknowledgeTypeID == ackType.AcknowledgeTypeId));
			}
		}

        [Test, AUT(AUT.Ca), JIRA("CA-1931"), FeatureSwitch(Constants.BmoFeatureSwitchKey)]
		public void SettlementSendPaymentsAcceptedAndRejectedInOneBatchShouldUpdateTransactionStatusAndPersistAck()
		{
			Guid applicationIdAccepted = Guid.Empty;
			Guid applicationIdRejected = Guid.Empty;

			using (new BankGatewayBmoSendBatch())
			{
				// Rejected
				var customer = CustomerBuilder.New().
									WithInstitutionNumber("001").
									WithBranchNumber("00022").
									WithSurname("Wonga").WithForename("Canada Inc").
									Build();
			    BmoResponseBuilder.New().
			        ForBankAccountNumber(customer.BankAccountNumber).
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

            var ackTypeSettled =
                Drive.Db.BankGateway.AcknowledgeTypes.Single(
                    a => a.BankIntegrationId == BankIntegrationIdBmo && a.Name == "DEFT220");

            var ackTypeRejected =
                Drive.Db.BankGateway.AcknowledgeTypes.Single(
                    a => a.BankIntegrationId == BankIntegrationIdBmo && a.Name == "DEFT210-211-260");

			Framework.Db.BankGateway.TransactionEntity transaction = null;

			// Accepted: expect successful transaction, settlement ack, NO reject ack
            Do.Until(
                () =>
                transaction =
                Drive.Db.BankGateway.Transactions.Single(
                    t =>
                    t.ApplicationId == applicationIdAccepted && t.BankIntegrationId == BankIntegrationIdBmo &&
                    t.TransactionStatus == TransactionStatusPaid));

            Do.Until(
                () =>
                Drive.Db.BankGateway.Acknowledges.Single(
                    t =>
                    t.TransactionID == transaction.TransactionId &&
                    t.AcknowledgeTypeID == ackTypeSettled.AcknowledgeTypeId));

            Do.With.Timeout(TimeSpan.FromSeconds(2)).While(
                () =>
                Drive.Db.BankGateway.Acknowledges.Single(
                    t =>
                    t.TransactionID == transaction.TransactionId &&
                    t.AcknowledgeTypeID == ackTypeRejected.AcknowledgeTypeId));


			// Rejected: expect failed transaction, NO settlement ack, reject ack
            Do.Until(
                () =>
                transaction =
                Drive.Db.BankGateway.Transactions.Single(
                    t =>
                    t.ApplicationId == applicationIdRejected && t.BankIntegrationId == BankIntegrationIdBmo &&
                    t.TransactionStatus == TransactionStatusFailed));

            Do.Until(
                () =>
                Drive.Db.BankGateway.Acknowledges.Single(
                    t =>
                    t.TransactionID == transaction.TransactionId &&
                    t.AcknowledgeTypeID == ackTypeRejected.AcknowledgeTypeId));

            Do.With.Timeout(TimeSpan.FromSeconds(2)).While(
                () =>
                Drive.Db.BankGateway.Acknowledges.Single(
                    t =>
                    t.TransactionID == transaction.TransactionId &&
                    t.AcknowledgeTypeID == ackTypeSettled.AcknowledgeTypeId));
		}

        [Test, AUT(AUT.Ca), JIRA("CA-1931")]
        public void SendPaymentMessageShouldBeProcessed()
        {
            var customer = CustomerBuilder.New().
                                WithInstitutionNumber("002").
                                WithBranchNumber("00018").
                                Build();

            var application = ApplicationBuilder.New(customer).Build();

            Do.Until(() => Drive.Db.BankGateway.Transactions.Single(
                t => t.ApplicationId == application.Id &&
                    t.TransactionStatus == TransactionStatusPaid));
        }

        [Test, AUT(AUT.Ca), JIRA("CA-1931")]
        public void SendPaymentMessageShouldBeRejected()
        {
            var customer = CustomerBuilder.New().
                                WithInstitutionNumber("002").
                                WithBranchNumber("00018").
                                Build();

            ScotiaResponseBuilder.New().
                ForBankAccountNumber(customer.BankAccountNumber).
                Reject();

            var application = ApplicationBuilder.New(customer).Build();

            Do.Until(() => Drive.Db.BankGateway.Transactions.Single(
                t => t.ApplicationId == application.Id &&
                    t.TransactionStatus == TransactionStatusFailed));
        }
	}
}
