using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db.Comms;
using Wonga.QA.Framework.Db.OpsSagas;
using Wonga.QA.Framework.Db.Payments;
using Wonga.QA.Framework.Msmq;
using Wonga.QA.Framework.Msmq.Messages.Payments.PublicMessages;
using Wonga.QA.Framework.Old;
using Wonga.QA.PerformanceTests.Core;
using log4net;

namespace Wonga.QA.PerformanceTests.Load
{
    public class ProcessBatchCollectionInZa
    {
        private readonly ILog _log = LogManager.GetLogger(typeof(ProcessBatchCollectionInZa));
        private List<FixedTermLoanSagaEntity> _fixedTermLoanSagaEntities;
        private string _applicationReference;
        private readonly int _number = int.Parse(ConfigurationManager.AppSettings["numberOfCollection"]);
        private readonly string _host = ConfigurationManager.AppSettings["host"];
        private readonly string _username = ConfigurationManager.AppSettings["userName"];
        private readonly string _password = ConfigurationManager.AppSettings["password"];
        private readonly string _mgmtPath = ConfigurationManager.AppSettings["mgmtPath"];


        [SetUp]
        public void Init()
        {
            _applicationReference = GenerateRandomCode(Guid.NewGuid());
            _fixedTermLoanSagaEntities = CreateNFixedTermLoansForCollection(_number, _applicationReference);
        }

        [Test]
        public void ProcessBatchCollection()
        {

            var wmiUtil = new WmiUtil();
            var managementScope = wmiUtil.EstablishConnection(_host, _username, _password, _mgmtPath);
            wmiUtil.StopService(managementScope, "Wonga.BankGateway.Handlers");

            Parallel.ForEach(_fixedTermLoanSagaEntities, fixedTermLoanSagaEntity =>
                                               {
                                                   if (
                                                       !fixedTermLoanSagaEntity.ApplicationGuid.
                                                            HasValue)
                                                       return;

                                                   var application =
                                                       new Application(fixedTermLoanSagaEntity.ApplicationGuid.Value);

                                                   var itermsAgreed =
                                                       new ITermsAgreed
                                                           {
                                                               ApplicationId = application.Id,
                                                               AccountId = application.AccountId,
                                                               SignedOn = DateTime.Now,
                                                               CreatedOn = DateTime.Now
                                                           };

                                                   Drive.Msmq.Payments.Send(itermsAgreed);


                                                   var timeTimeOutMessage =
                                                       new TimeoutMessage
                                                           {
                                                               SagaId =
                                                                   fixedTermLoanSagaEntity.
                                                                   ApplicationGuid.Value
                                                           };

                                                   Drive.Msmq.Payments.Send(timeTimeOutMessage);

                                                   _log.InfoFormat("Sending Timeout message for ApplicationId={0}",
                                                                   fixedTermLoanSagaEntity.
                                                                       ApplicationGuid.Value);
                                               });

            wmiUtil.StartService(managementScope, "Wonga.BankGateway.Handlers");

            Do.With.Timeout(30).Until(() =>
                Drive.Db.Payments.Applications.Where(application => application.LoanReference == _applicationReference && application.ClosedOn != null).Count() >= _fixedTermLoanSagaEntities.Count);
        }


        private List<FixedTermLoanSagaEntity> CreateNFixedTermLoansForCollection(int number,string applicationReference)
        {
            var listOfFixedTermLoans = new List<FixedTermLoanSagaEntity>();
            var commsDb = Drive.Db.Comms;
            var paymentDb = Drive.Db.Payments;
            var opsSagaDb = Drive.Db.OpsSagas;

            for (var i = 0; i < number;i++)
            {
                var accountId = Guid.NewGuid();
                var mobilePhoneNumber = Get.GetMobilePhone();

                var customerEntity = new CustomerDetailEntity
                {
                    AccountId = accountId,
                    Gender = 2,
                    DateOfBirth = Get.GetDoB(),
                    Email = Get.RandomEmail(),
                    Forename = Get.RandomString(8),
                    Surname = Get.RandomString(8),
                    MiddleName = Get.RandomString(8),
                    HomePhone = "0217050520",
                    WorkPhone = "0217450510",
                    MobilePhone = mobilePhoneNumber
                };

                commsDb.CustomerDetails.InsertOnSubmit(customerEntity);
                commsDb.SubmitChanges();

                var bankAccountGuid = Guid.NewGuid();


                var bankAccountsBase = new BankAccountsBaseEntity
                {
                    ExternalId = bankAccountGuid,
                    BankName = "ABSA",
                    BankCode = "632005",
                    AccountNumber = "",
                    HolderName = "Tester",
                    AccountOpenDate = DateTime.Now.AddDays(-5),
                    CountryCode = "ZA",
                    ValidatedOn = DateTime.Now,
                    CreatedOn = DateTime.Now.AddDays(-5),

                };
                paymentDb.BankAccountsBases.InsertOnSubmit(bankAccountsBase);
                paymentDb.SubmitChanges();


                var personalBankAccountDetails = new PersonalBankAccountEntity
                {
                    BankAccountId = bankAccountsBase.BankAccountId,
                    AccountId = bankAccountGuid
                };
                paymentDb.PersonalBankAccounts.InsertOnSubmit(personalBankAccountDetails);
                paymentDb.SubmitChanges();



                var applicationExternalId = Guid.NewGuid();
                var paymentCardGuid = Guid.NewGuid();

                var applicationEntity = new ApplicationEntity
                {
                    ExternalId = applicationExternalId,
                    AccountId = accountId,
                    ProductId = 1,
                    Currency = 710,
                    BankAccountGuid = bankAccountGuid,
                    PaymentCardGuid = paymentCardGuid,
                    ApplicationDate = DateTime.Now,
                    LoanReference = applicationReference,
                    SignedOn = DateTime.Now,
                    CreatedOn = DateTime.Now,
                    AcceptedOn = DateTime.Now,
                    ApplicationReference = GenerateRandomCode(applicationExternalId)
                };

                paymentDb.Applications.InsertOnSubmit(applicationEntity);
                paymentDb.SubmitChanges();

                var applicationId = applicationEntity.ApplicationId;


                var transacationGuid = Guid.NewGuid();

                var transactions = new TransactionEntity
                {
                    ExternalId = transacationGuid,
                    ApplicationId = applicationId,
                    Scope = 1,
                    Type = "CashAdvance",
                    Amount = 500,
                    Mir = 5,
                    Currency = 710,
                    Reference = applicationReference,
                    PostedOn = DateTime.Now,
                    CreatedOn = DateTime.Now,
                };

                paymentDb.Transactions.InsertOnSubmit(transactions);
                paymentDb.SubmitChanges();


                var fixedTermLoanEntity = new FixedTermLoanApplicationEntity
                {
                    ApplicationId = applicationId,
                    LoanAmount = 500,
                    MonthlyInterestRate = 5,
                    TransmissionFee = 0,
                    PromiseDate = DateTime.Now,
                    NextDueDate = DateTime.Now.AddDays(1),
                    ServiceFee = 57,
                    InitiationFee = 85
                };

                paymentDb.FixedTermLoanApplications.InsertOnSubmit(fixedTermLoanEntity);
                paymentDb.SubmitChanges();

                var newFixedTermLoanSagaEntity = new FixedTermLoanSagaEntity
                {
                    Id = Guid.NewGuid(),
                    ApplicationId = applicationId,
                    OriginalMessageId = Guid.NewGuid().ToString(),
                    Originator = applicationReference,
                    TermsAgreed = true,
                    ApplicationAccepted = true,
                    AccountGuid = accountId,
                    ApplicationGuid = applicationExternalId
                };
                opsSagaDb.FixedTermLoanSagaEntities.InsertOnSubmit(newFixedTermLoanSagaEntity);
                opsSagaDb.SubmitChanges();

                listOfFixedTermLoans.Add(newFixedTermLoanSagaEntity);
            }
            return listOfFixedTermLoans;
        }

        private string GenerateRandomCode(Guid value)
        {
            var rand = new Random((int)DateTime.UtcNow.Ticks);
            var randStr = Convert.ToString(rand.Next(0, 99)).PadLeft(2, '0');
            var res = Convert.ToString(Math.Abs(value.GetHashCode())).PadLeft(10, '0');
            return String.Format("{0}{1}", res, randStr);
        }

    }
    
}
