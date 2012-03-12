using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Db;

namespace Wonga.QA.Tests.Payments.Za
{
    public class RepayLoanTests
    {
        private Guid _accountId;

        private const string NowServiceConfigKey =
            @"Wonga.Payments.ApplicationQueries.Za.GetRepayLoanParametersHandler.DateTime.UtcNow";
        [FixtureSetUp]
        public void Setup()
        {
            var customer = CustomerBuilder.New().Build();
            var application = ApplicationBuilder.New(customer).Build();
            _accountId = customer.Id;
        }

        [Test]
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

        [Test]
        public void GetRepayLoanStatusQueryTest()
        {
            var response = Driver.Api.Queries.Post(new GetRepayLoanStatusQuery()
                                                       {
                                                           AccountId = _accountId
                                                       });
        }

        public void RepayLoanViaBankTest()
        {
            
        }

        private void SetPaymentsUtcNow(DateTime dateTime)
        {
            Driver.Db.SetServiceConfiguration(NowServiceConfigKey, dateTime.ToString("yyyy-MM-dd HH:mm:ss"));
        }
    }
}
