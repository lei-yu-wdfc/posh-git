using System;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Api.Enums;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;
using Wonga.QA.Tests.Payments.Enums;
using Wonga.QA.Tests.Payments.Helpers.Ca;

namespace Wonga.QA.Tests.Risk
{
    class AdditionalPayFrequencyCalculateTest
    {
        [Test, AUT(AUT.Ca), JIRA("CA-2444")]
        public void TwiceMonthly15thAnd30thIncomingFrequencyTestForThisMonth15th()
        {
            var date = new DateTime(2012, 1, 1);
            var customer = CustomerBuilder.New().WithIncomeFrequency(IncomeFrequencyEnum.TwiceMonthly15thAnd30th).Build();
            var application = ApplicationBuilder.New(customer).Build();
            var nextPayDateForRepresentmentOne = CalculateNextPayDateFunctionsCa.CalculateNextPayDate(date, Convert.ToDateTime(customer.GetNextPayDate()),
                                                                                        (PaymentFrequency)(Convert.ToInt32(customer.GetIncomeFrequency())));
            var expectedDate = new DateTime(2012, 1, 15);
            Assert.AreEqual(expectedDate, nextPayDateForRepresentmentOne);
        }

        [Test, AUT(AUT.Ca), JIRA("CA-2444")]
        public void TwiceMonthly15thAnd30thIncomingFrequencyTestForThisMonth30th()
        {
            var date = new DateTime(2012, 1, 16);
            var customer = CustomerBuilder.New().WithIncomeFrequency(IncomeFrequencyEnum.TwiceMonthly15thAnd30th).Build();
            var application = ApplicationBuilder.New(customer).Build();
            var nextPayDateForRepresentmentOne = CalculateNextPayDateFunctionsCa.CalculateNextPayDate(date, Convert.ToDateTime(customer.GetNextPayDate()),
                                                                                        (PaymentFrequency)(Convert.ToInt32(customer.GetIncomeFrequency())));
            var expectedDate = new DateTime(2012, 1, 30);
            Assert.AreEqual(expectedDate, nextPayDateForRepresentmentOne);
        }

        [Test, AUT(AUT.Ca), JIRA("CA-2444")]
        public void TwiceMonthly15thAnd30thIncomingFrequencyTestForNextMonth15th()
        {
            var date = new DateTime(2012, 1, 31);
            var customer = CustomerBuilder.New().WithIncomeFrequency(IncomeFrequencyEnum.TwiceMonthly15thAnd30th).Build();
            var application = ApplicationBuilder.New(customer).Build();
            var nextPayDateForRepresentmentOne = CalculateNextPayDateFunctionsCa.CalculateNextPayDate(date, Convert.ToDateTime(customer.GetNextPayDate()),
                                                                                        (PaymentFrequency)(Convert.ToInt32(customer.GetIncomeFrequency())));
            var expectedDate = new DateTime(2012, 2, 15);
            Assert.AreEqual(expectedDate, nextPayDateForRepresentmentOne);
        }

        [Test, AUT(AUT.Ca), JIRA("CA-2444")]
        public void TwiceMonthly15thAnd30thIncomingFrequencyTestForNextYear15th()
        {
            var date = new DateTime(2012, 12, 31);
            var customer = CustomerBuilder.New().WithIncomeFrequency(IncomeFrequencyEnum.TwiceMonthly15thAnd30th).Build();
            var application = ApplicationBuilder.New(customer).Build();
            var nextPayDateForRepresentmentOne = CalculateNextPayDateFunctionsCa.CalculateNextPayDate(date, Convert.ToDateTime(customer.GetNextPayDate()),
                                                                                        (PaymentFrequency)(Convert.ToInt32(customer.GetIncomeFrequency())));
            var expectedDate = new DateTime(2013, 1, 15);
            Assert.AreEqual(expectedDate, nextPayDateForRepresentmentOne);
        }

        [Test, AUT(AUT.Ca), JIRA("CA-2444")]
        public void TwiceMonthly15thAnd30thIncomingFrequencyTestForFebruary29()
        {
            var date = new DateTime(2012, 2, 16);
            var customer = CustomerBuilder.New().WithIncomeFrequency(IncomeFrequencyEnum.TwiceMonthly15thAnd30th).Build();
            var application = ApplicationBuilder.New(customer).Build();
            var nextPayDateForRepresentmentOne = CalculateNextPayDateFunctionsCa.CalculateNextPayDate(date, Convert.ToDateTime(customer.GetNextPayDate()),
                                                                                        (PaymentFrequency)(Convert.ToInt32(customer.GetIncomeFrequency())));
            var expectedDate = new DateTime(2012, 2, 29);
            Assert.AreEqual(expectedDate, nextPayDateForRepresentmentOne);
        }

        [Test, AUT(AUT.Ca), JIRA("CA-2444")]
        public void TwiceMonthly15thAnd30thIncomingFrequencyTestForFebruary28()
        {
            var date = new DateTime(2011, 2, 16);
            var customer = CustomerBuilder.New().WithIncomeFrequency(IncomeFrequencyEnum.TwiceMonthly15thAnd30th).Build();
            var application = ApplicationBuilder.New(customer).Build();
            var nextPayDateForRepresentmentOne = CalculateNextPayDateFunctionsCa.CalculateNextPayDate(date, Convert.ToDateTime(customer.GetNextPayDate()),
                                                                                        (PaymentFrequency)(Convert.ToInt32(customer.GetIncomeFrequency())));
            var expectedDate = new DateTime(2011, 2, 28);
            Assert.AreEqual(expectedDate, nextPayDateForRepresentmentOne);
        }
    }
}
