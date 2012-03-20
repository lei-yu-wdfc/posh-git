using System;
using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db.Ops;
using Wonga.QA.Framework.Msmq;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.BankGateway
{
    [TestFixture, AUT(AUT.Uk), Parallelizable(TestScope.All)]
    public class HsbcCashOutTests
    {

        public bool _wasTestModeEnabled;

        [FixtureSetUp]
        public void TurnOffTestMode()
        {
            ServiceConfigurationEntity mode = Driver.Db.Ops.ServiceConfigurations.SingleOrDefault(e => e.Key == "BankGateway.IsTestMode");
            if (mode != null && Boolean.Parse(mode.Value))
            {
                mode.Value = false.ToString();
                mode.Submit();
                _wasTestModeEnabled = true;
            }
        }

        [FixtureTearDown]
        public void UnderTestModeChange()
        {
            if (_wasTestModeEnabled)
            {
                ServiceConfigurationEntity mode = Driver.Db.Ops.ServiceConfigurations.Single(e => e.Key == "BankGateway.IsTestMode");
                mode.Value = true.ToString();
                mode.Submit();
                _wasTestModeEnabled = false;
            }
        }

        [Test, JIRA("UK-495")]
        public void CashOutFileIsSent()
        {
            var accountId = CreateCustomerDetails().AccountId;
            var applicationId = Guid.NewGuid();

            Driver.Msmq.BankGateway.Send(new SendPaymentCommand
                                             {
                                                 AccountId = accountId,
                                                 Amount = 100.00M,
                                                 ApplicationId = applicationId,
                                                 BankAccount = "10032650",
                                                 BankCode = "161027",
                                                 Currency = CurrencyCodeIso4217Enum.GBP,
                                                 BankAccountType = "test",
                                                 SenderReference = Guid.NewGuid()
                                             });

            var transaction = Do.Until(() => Driver.Db.BankGateway.Transactions.Single(e => e.ApplicationId == applicationId && e.TransactionStatus == 3));

            var transactionReference = transaction.TransactionId.ToString();

            Driver.Msmq.Hsbc.Send(new FasterSecondResponseSuccessSagaUkCommand { FileName = "file name 1", RawContents = "Some raw contenst", TransactionReference = transactionReference });
            var baseResponseRecordEntity = Do.Until(() => Driver.Db.OpsSagasUk.BaseResponseRecordEntities.Single(x => x.TransactionReference == transactionReference));

            Driver.Msmq.Hsbc.Send(new TimeoutMessage { ClearTimeout = true, Expires = DateTime.UtcNow, SagaId = baseResponseRecordEntity.Id, State = null });

            Do.Until(() => Driver.Db.BankGateway.Transactions.Single(e => e.ApplicationId == applicationId).TransactionStatus == 4); //success

            Do.Until(() => Driver.Db.BankGateway.Acknowledges.Single(e =>
                e.TransactionID == transaction.TransactionId
                && e.AcknowledgeTypeID == 3
                && !e.HasError)); //2nd faster ack
        }

        /// <summary>
        /// Cashes the out file is sent.
        /// </summary>
        [Test, JIRA("UK-495")]
        public void CashOutFileIsSent2ndAckFailure()
        {
            var accountId = CreateCustomerDetails().AccountId;
            var applicationId = Guid.NewGuid();

            Driver.Msmq.BankGateway.Send(new SendPaymentCommand
            {
                AccountId = accountId,
                Amount = 100.00M,
                ApplicationId = applicationId,
                BankAccount = "10032650",
                BankCode = "161027",
                Currency = CurrencyCodeIso4217Enum.GBP,
                BankAccountType = "test",
                SenderReference = Guid.NewGuid()
            });

            var transaction = Do.Until(() => Driver.Db.BankGateway.Transactions.Single(e => e.ApplicationId == applicationId && e.TransactionStatus == 3));

            var transactionReference = transaction.TransactionId.ToString();

            Driver.Msmq.Hsbc.Send(new RecordFinalFailureUkCommand { ErrorCode = "90", TransactionReference = transactionReference }); //Invalid Bank Sort Code

            Do.Until(() => Driver.Db.BankGateway.Transactions.Single(e => e.ApplicationId == applicationId).TransactionStatus == 5); //Failure
        }

        /// <summary>
        /// Cashes the out file is sent.
        /// </summary>
        [Test, JIRA("UK-495")]
        public void CashOutFileIsSentFaster3rdAckFailure()
        {
            var accountId = CreateCustomerDetails().AccountId;
            var applicationId = Guid.NewGuid();

            Driver.Msmq.BankGateway.Send(new SendPaymentCommand
            {
                AccountId = accountId,
                Amount = 100.00M,
                ApplicationId = applicationId,
                BankAccount = "10032650",
                BankCode = "161027",
                Currency = CurrencyCodeIso4217Enum.GBP,
                BankAccountType = "test",
                SenderReference = Guid.NewGuid(),
            });

            Do.Until(() => Driver.Db.BankGateway.Transactions.Single(e => e.ApplicationId == applicationId).TransactionStatus == 3);
            var transaction = Driver.Db.BankGateway.Transactions.Single(e => e.ApplicationId == applicationId);

            var transactionReference = transaction.TransactionId.ToString();
            Driver.Msmq.Hsbc.Send(new FasterSecondResponseSuccessSagaUkCommand { FileName = "file name 1", RawContents = "Some raw contenst", TransactionReference = transactionReference });
            Do.Until(() => Driver.Db.OpsSagasUk.BaseResponseRecordEntities.Single(x => x.TransactionReference == transactionReference));

            Driver.Msmq.Hsbc.Send(new FasterThirdResponseFailureUkCommand
                                      {
                                          ErrorCode = "BE", //insufficient funds
                                          TransactionReference = transactionReference,
                                          FileName = "fielname.txt",
                                          RawContents = "File raw contents",
                                      }
                );

            Do.Until(() => Driver.Db.BankGateway.Transactions.Single(e => e.ApplicationId == applicationId).TransactionStatus == 5); //Failure
            Do.Until(() => Driver.Db.BankGateway.Acknowledges.Single(e =>
                e.TransactionID == transaction.TransactionId
                && e.AcknowledgeTypeID == 5
                && e.HasError)); //3rd ack error for faster payment
        }


        private static SaveCustomerDetailsCommand CreateCustomerDetails()
        {
            SaveCustomerDetailsCommand saveCustomerDetailsCommand = new SaveCustomerDetailsCommand
                                                                        {
                                                                            AccountId = Guid.NewGuid(),
                                                                            CreatedOn = DateTime.UtcNow,
                                                                            DateOfBirth = DateTime.UtcNow.AddYears(-20),
                                                                            Forename = "XYX",
                                                                            Surname = "Ddddd",
                                                                            Email = "asdasD@asdasd.com",
                                                                            Gender = GenderEnum.Male,
                                                                            HomePhone = "020123123",
                                                                            MobilePhone = "070123123",
                                                                            Title = TitleEnum.Mr,
                                                                        };
            Driver.Msmq.Comms.Send(saveCustomerDetailsCommand);
            Do.Until(() => Driver.Db.Comms.CustomerDetails.Single(e => e.AccountId == saveCustomerDetailsCommand.AccountId));


            CreateCustomerAddress(saveCustomerDetailsCommand.AccountId);
            return saveCustomerDetailsCommand;

        }
        private static void CreateCustomerAddress(Guid AccountId)
        {
            SaveCustomerAddressCommand saveCustomerAddressCommand = new SaveCustomerAddressCommand
                                                                        {
                                                                            AddressId = Guid.NewGuid(),
                                                                            AccountId = AccountId,
                                                                            CreatedOn = DateTime.UtcNow,
                                                                            HouseNumber = "99",
                                                                            Street = "Street st.",
                                                                            Town = "London",
                                                                            Postcode = "W8 1HD",
                                                                            AtAddressFrom = new DateTime(1970, 1, 1),
                                                                        };
            Driver.Msmq.Comms.Send(saveCustomerAddressCommand);
            Do.Until(() => Driver.Db.Comms.Addresses.Single(e => e.ExternalId == saveCustomerAddressCommand.AddressId));
        }

    }
}
