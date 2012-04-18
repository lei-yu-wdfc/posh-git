using System;
using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api.Exceptions;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Cs;
using Wonga.QA.Framework.Db.Payments;
using Wonga.QA.Framework.Msmq;
using Wonga.QA.Tests.Core;
using Wonga.QA.Tests.Payments.Helpers;


namespace Wonga.QA.Tests.Cs
{
    public class RepaymentArrangementTests
    {
        private bool _bankGatewayTestModeOriginal;

        [FixtureSetUp]
        public void FixtureSetup()
        {
            _bankGatewayTestModeOriginal = ConfigurationFunctions.GetBankGatewayTestMode();
            ConfigurationFunctions.SetBankGatewayTestMode(false);
        }

        [FixtureTearDown]
        public void FixtureTearDown()
        {
            ConfigurationFunctions.SetBankGatewayTestMode(_bankGatewayTestModeOriginal);
        }

        [Test, AUT(AUT.Za)]
        public void IsAllowedTest()
        {
            Customer customer = CustomerBuilder.New().Build();
            Application application = ApplicationBuilder.New(customer).Build();

            application.PutApplicationIntoArrears(20);

            var response = Drive.Cs.Queries.Post(new CsGetRepaymentArrangementAllowedQuery()
                    {
                        ApplicationId = application.Id
                    });

            Assert.IsNotNull(response);
            Assert.IsTrue(bool.Parse(response.Values["IsAllowed"].Single()));    
        }

        [Test, AUT(AUT.Za)]
        public void CreateTest()
        {
            Customer customer = CustomerBuilder.New().Build();
            Application application = ApplicationBuilder.New(customer).Build();

            application.PutApplicationIntoArrears(20);
            CreatePlan(application, customer);

            var dbApplication = Drive.Db.Payments.Applications.Single(a => a.ExternalId == application.Id);
            Do.Until(() => Drive.Db.Payments.RepaymentArrangements.Single(x => x.ApplicationId == dbApplication.ApplicationId));
            var repaymentArrangement = Drive.Db.Payments.RepaymentArrangements.Single(x => x.ApplicationId == dbApplication.ApplicationId);
            Assert.AreEqual(2, repaymentArrangement.RepaymentArrangementDetails.Count);
            Assert.IsNotNull(Drive.Db.Payments.Transactions.Where(x => x.ApplicationId == dbApplication.ApplicationId && x.Type == "SuspendInterestAccrual"));
        }

        [Test, AUT(AUT.Za)]
        public void GetTest()
        {
            Customer customer = CustomerBuilder.New().Build();
            Application application = ApplicationBuilder.New(customer).Build();

            application.PutApplicationIntoArrears(20);
            CreatePlan(application, customer);

            var response = Do.Until(() => Drive.Cs.Queries.Post(new GetRepaymentArrangementsQuery() {ApplicationId = application.Id}));
            Assert.IsNotNull(response);

            Assert.AreEqual(application.Id, Guid.Parse(response.Values["ApplicationId"].Single()));
        }

        [Test, AUT(AUT.Za)]
        public void GetParametersTest()
        {
            Customer customer = CustomerBuilder.New().Build();
            Application application = ApplicationBuilder.New(customer).Build();
            application.PutApplicationIntoArrears(20);

            var response = Drive.Cs.Queries.Post(new GetRepaymentArrangementParametersQuery() {AccountId = customer.Id});
            Assert.IsNotNull(response);
            Assert.AreEqual(6, int.Parse(response.Values["MaxLengthMonths"].Single()));
        }

        [Test, AUT(AUT.Za)]
        public void GetCalculationTest()
        {
            Customer customer = CustomerBuilder.New().Build();
            Application application = ApplicationBuilder.New(customer).Build();
            application.PutApplicationIntoArrears(20);

            var response = Drive.Cs.Queries.Post(new GetRepaymentArrangementCalculationQuery
                                                     {
                                                         AccountId = customer.Id,
                                                         RepaymentFrequency = PaymentFrequency.Monthly,
                                                         NumberOfMonths = 4,
                                                         FirstRepaymentDate = DateTime.Today.AddDays(7)
                                                     });
            Assert.IsNotNull(response);            
        }

        [Test, AUT(AUT.Za)]
        public void CancelTest()
        {
            Customer customer = CustomerBuilder.New().Build();
            Application application = ApplicationBuilder.New(customer).Build();
            application.PutApplicationIntoArrears(20);
            CreatePlan(application, customer);

            var dbApplication = Drive.Db.Payments.Applications.Single(a => a.ExternalId == application.Id);
            Do.Until(() => Drive.Db.Payments.RepaymentArrangements.Single(x => x.ApplicationId == dbApplication.ApplicationId));
            var repaymentArrangement = Drive.Db.Payments.RepaymentArrangements.Single(x => x.ApplicationId == dbApplication.ApplicationId);

            Drive.Cs.Commands.Post(new Framework.Cs.CancelRepaymentArrangementCommand()
                                       {
                                           RepaymentArrangementId = repaymentArrangement.ExternalId
                                       });

            Do.Until(() => Drive.Db.Payments.RepaymentArrangements.Single(x => x.ApplicationId == dbApplication.ApplicationId).CanceledOn != null);
        }

        public enum PaymentFrequency
        {
            Every2Weeks,
            Every4Weeks,
            Monthly,
            Weekly
        }

        //Needed for serialization in CreateExtendedRepaymentArrangementCommand
        public class RepaymentArrangementDetailCsapi
        {
            public decimal Amount { get; set; }
            public CurrencyCodeIso4217Enum Currency { get; set; }
            public DateTime DueDate { get; set; }
        }

        private static void CreatePlan(Application application, Customer customer)
        {
            Drive.Cs.Commands.Post(new Framework.Cs.CreateRepaymentArrangementCommand()
            {
                AccountId = customer.Id,
                ApplicationId = application.Id,
                EffectiveBalance = 100,
                RepaymentAmount = 100,
                ArrangementDetails = new[]
                                                                    {
                                                                        new RepaymentArrangementDetailCsapi
                                                                            {
                                                                                Amount = 49,
                                                                                Currency = CurrencyCodeIso4217Enum.ZAR,
                                                                                DueDate = DateTime.Today.AddDays(7)
                                                                            },
                                                                        new RepaymentArrangementDetailCsapi
                                                                            {
                                                                                Amount = 51,
                                                                                Currency = CurrencyCodeIso4217Enum.ZAR,
                                                                                DueDate = DateTime.Today.AddDays(15)
                                                                            }
                                                                    }
            });
        }
    }
}
