using System;
using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Cs;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Payments.Queries
{
    [TestFixture]
    public class CsGetCardPaymentStatusQueryTests
    {
        protected Application _application;
        protected Customer _customer;


        [SetUp]
        public virtual void Setup()
        {
            _customer = CustomerBuilder.New().Build();
            _application = ApplicationBuilder.New(_customer)
                                             .WithLoanAmount(100)
                                             .WithLoanTerm(7)
                                             .Build();
        }

        public abstract class GivenACustomerWithAnApprovedLoan : CsGetCardPaymentStatusQueryTests
        {
            protected dynamic _paymentCardRepaymentRequests = Drive.Data.Payments.Db.RepaymentRequests;
            protected decimal _paymentAmount;
            protected Guid _paymentId;

            [SetUp]
            public override void Setup()
            {
                base.Setup();

                //Issue a payment request.
                SetPaymentAmount();
                _paymentId = Guid.NewGuid();
                Drive.Cs.Commands.Post(new CsRepayWithPaymentCardCommand { AccountId = _application.AccountId,
                                                                           Amount = _paymentAmount,
                                                                           SalesforceUser = "csUser",
                                                                           CV2 = "111",
                                                                           Currency = "GBP",
                                                                           PaymentCardId = _customer.GetPaymentCard(), 
                                                                           PaymentId = _paymentId});
            }

            protected abstract void SetPaymentAmount();

            protected void ConfirmPaymentStatus(string requiredStatus)
            {
                //Issue a query for the payment ID...
                CsResponse res = null;

                Do.With.Timeout(2).Interval(20).Until(() => { res = Drive.Cs.Queries.Post(new CsGetCardPaymentStatusQuery { PaymentId = _paymentId });
                                                              return res.Values["PaymentId"].Any() && Guid.Parse(res.Values["PaymentId"].First()) == _paymentId &&
                                                                     res.Values["PaymentStatus"].Any() && 
                                                                     res.Values["PaymentStatus"].First() == requiredStatus || 
                                                                     res.Values["PaymentStatus"].First() == "Pending"; });

                if (res.Values["PaymentStatus"].First() == "Pending")
                {
                    //Wait for it to change...
                    Do.With.Timeout(2).Interval(20).Until(() => { res = Drive.Cs.Queries.Post(new CsGetCardPaymentStatusQuery { PaymentId = _paymentId });
                                                                  return res.Values["PaymentId"].Any() && Guid.Parse(res.Values["PaymentId"].First()) == _paymentId &&
                                                                         res.Values["PaymentStatus"].Any() && 
                                                                         res.Values["PaymentStatus"].First() == requiredStatus; });
                }
            }

            public abstract class WhenAValidPaymentIsMade : GivenACustomerWithAnApprovedLoan
            {
                [SetUp]
                public override void Setup()
                {
                    base.Setup();
                }

                [Test, AUT(AUT.Uk), JIRA("UK-1622")]
                public void TheStatusOfThePaymentCanBeObtained()
                {
                    ConfirmPaymentStatus("PaymentTaken");
                }

                protected override void SetPaymentAmount()
                {
                    _paymentAmount = _application.GetBalance();
                }
            }

            public abstract class WhenAFailedPaymentIsMade : GivenACustomerWithAnApprovedLoan
            {
                [SetUp]
                public override void Setup()
                {
                    base.Setup();
                }

                [Test, AUT(AUT.Uk), JIRA("UK-1622")]
                public void TheStatusOfThePaymentCanBeObtained()
                {
                    ConfirmPaymentStatus("PaymentFailed");
                }

                protected override void SetPaymentAmount()
                {
                    _paymentAmount = 13m; //Make it fail.
                }
            }

        }
    }
}
