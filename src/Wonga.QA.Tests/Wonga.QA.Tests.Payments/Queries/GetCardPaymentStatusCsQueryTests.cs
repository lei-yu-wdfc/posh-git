using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Cs;

namespace Wonga.QA.Tests.Payments.Queries
{
    //[TestFixture]
    //public class GetCardPaymentStatusCsQueryTests
    //{
    //    protected Application _application;
    //    protected Customer _customer;
        

    //    [SetUp]
    //    public virtual void Setup()
    //    {
    //        _customer = CustomerBuilder.New().Build();
    //        _application = ApplicationBuilder.New(_customer)
    //                                         .WithLoanAmount(100)
    //                                         .WithLoanTerm(7)
    //                                         .Build();
    //    }

    //    public abstract class GivenACustomerWithAnApprovedLoan : GetCardPaymentStatusCsQueryTests
    //    {
    //        protected dynamic _repayRequestsRepo = Drive.Data.Payments.Db.RepaymentRequests;
    //        protected decimal _paymentAmount;

    //        [Test]
    //        public override void Setup()
    //        {
    //            base.Setup();

    //            //Issue a payment request.
    //            _paymentAmount = GetPaymentAmount();
    //            Drive.Cs.Commands.Post(new CsRepayWithPaymentCardCommand { AccountId = _application.AccountId,
    //                                                                       Amount = _paymentAmount,
    //                                                                       SalesforceUser = "csUser",
    //                                                                       CV2 = "111",
    //                                                                       Currency = "GBP",
    //                                                                       PaymentCardId = _customer.GetPaymentCard() });
    //        }

    //        protected abstract decimal GetPaymentAmount();

    //        public abstract class WhenAValidPaymentIsMade : GivenACustomerWithAnApprovedLoan
    //        {
    //            [SetUp]
    //            public override void Setup()
    //            {
    //                base.Setup();
    //            }

    //            [Test]
    //            public void TheStatusOfThePaymentCanBeObtained()
    //            {
    //            }

    //            protected override decimal GetPaymentAmount()
    //            {
    //                return _application.GetBalance();
    //            }
    //        }

    //    }
    //}
}
