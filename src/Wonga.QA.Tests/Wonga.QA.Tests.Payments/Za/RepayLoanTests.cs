using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db;
using Wonga.QA.Framework.Db.Payments;
using Wonga.QA.Framework.Msmq;
using Wonga.QA.Tests.Core;
using RepayLoanViaBankCommand = Wonga.QA.Framework.Api.RepayLoanViaBankCommand;

namespace Wonga.QA.Tests.Payments.Za
{
    public class RepayLoanTests
    {
        private Guid _accountId;
        private Application _application;
        private DateTime _promisedDate;

        private const string NowServiceConfigKey =
            @"Wonga.Payments.ApplicationQueries.Za.GetRepayLoanParametersHandler.DateTime.UtcNow";

        private const string NowServiceConfigKey_RepayLoan =
            @"Wonga.Payments.Handlers.FixedLoanOperations.RepayLoanSagaBase.DateTime.UtcNow";

        private const string NowServiceConfigKey_ActionDateCalculator =
            "Wonga.Payments.Common.Za.ActionDateCalculator.DateTime.UtcNow";

        [FixtureSetUp]
        public void Setup()
        {
            var customer = CustomerBuilder.New().Build();
            _application = ApplicationBuilder.New(customer).Build();
            _accountId = customer.Id;
        }

        [Test, AUT(AUT.Za), JIRA("ZA-2099")]
        [Row("2012/3/8 10:00:00", "2012/3/9", Order = 0)]
        [Row("2012/3/8 14:00:00", "2012/3/10", Order = 1)]
        [Row("2012/3/10 5:00:00", "2012/3/12", Order = 2)]
        [Row("2012/3/10 6:00:01", "2012/3/13", Order = 3)]
        public void GetRepayLoanParameterQueryTest(DateTime now, DateTime expectedActionDate)
        {
            SetPaymentsUtcNow(now);
            var response = Driver.Api.Queries.Post(new GetRepayLoanParametersQuery()
                                        {
                                            AccountId = _accountId
                                        });
            Assert.AreEqual(expectedActionDate, DateTime.Parse(response.Values["ActionDate"].Single()));
        }

        [Test, AUT(AUT.Za), JIRA("ZA-2099")]
        public void GetRepayLoanStatusQueryTest()
        {
            var response = Driver.Api.Queries.Post(new GetRepayLoanStatusQuery()
                                                       {
                                                           AccountId = _accountId
                                                       });
        }

        [Test, AUT(AUT.Za), JIRA("ZA-2099")]
        public void RepayLoanViaBankTest()
        {
            var command = new RepayLoanViaBankCommand()
                              {
                                  ApplicationId = _application.Id,
                                  ActionDate = new Date
                                                   {
                                                       DateTime = Data.GetPromiseDate().DateTime.AddDays(-3),
                                                       DateFormat = DateFormat.Date
                                                   }, //Early repay before promised date
                                  Amount = _application.LoanAmount,
                                  RepaymentRequestId = Guid.NewGuid()
                              };
            Driver.Api.Commands.Post(command);

            var app = Do.Until(() => Driver.Db.Payments.Applications.Single(a => a.ExternalId == _application.Id));
            var saga = Do.Until(() =>Driver.Db.OpsSagas.RepaymentSagaEntities.Single(s => s.ApplicationId == app.ApplicationId));

            SetPaymentsUtcNow(Data.GetPromiseDate().DateTime.AddDays(-4));
            //Cause request is schedule to timeout, send timeout message now.
            new MsmqDriver().Payments.Send(new TimeoutMessage()
                                          {
                                              SagaId = saga.Id,
                                          });


            var request = Do.Until(() => Driver.Db.Payments.RepaymentRequests.Single(r => r.ExternalId == (Guid)command.RepaymentRequestId));
            var requestDetail = request.RepaymentRequestDetails;
            Assert.AreEqual(0, requestDetail[0].StatusCode);
        }

        private void SetPaymentsUtcNow(DateTime dateTime)
        {
            Driver.Db.SetServiceConfiguration(NowServiceConfigKey, dateTime.ToString("yyyy-MM-dd HH:mm:ss"));
            Driver.Db.SetServiceConfiguration(NowServiceConfigKey_RepayLoan, dateTime.ToString("yyyy-MM-dd HH:mm:ss"));
            Driver.Db.SetServiceConfiguration(NowServiceConfigKey_ActionDateCalculator, dateTime.ToString("yyyy-MM-dd HH:mm:ss"));
        }
    }
}
