using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Cs;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Payments.Command
{
    public abstract class CsRepayWithPaymentCardCommandTests
    {
        protected Application _application;
        protected Customer _customer;
        protected Guid _cardId;
        [SetUp]
        public virtual void Setup()
        {
            _customer = CustomerBuilder.New()
                                       .Build();

            _cardId = _customer.GetPaymentCard();

            _application = ApplicationBuilder.New(_customer)
                                             .WithLoanAmount(100)
                                             .WithLoanTerm(7)
                                             .Build();

            Do.With.Timeout(1).Interval(10).Until(() => Drive.Db.Payments.PaymentCardsBases.First(c => c.ExternalId == _cardId));
        }

        public abstract class GivenACustomerWithAnApprovedLoan : CsRepayWithPaymentCardCommandTests
        {
            protected decimal _startingBalance;
            protected decimal _paymentAmount;

            [SetUp]
            public override void Setup()
            {
                base.Setup();

                _startingBalance = _application.GetBalance();
                _paymentAmount = 50m;
                //Issue a payment command.
                Drive.Cs.Commands.Post(new CsRepayWithPaymentCardCommand { AccountId = _application.AccountId, 
                                                                           Amount = _paymentAmount, 
                                                                           CSUser = "csUser", 
                                                                           CV2 = "111",
                                                                           Currency = "GBP", 
                                                                           PaymentCardId = _cardId });
            }

            public class GivenAPaymentHasBeenRequestedWithAValidCard : GivenACustomerWithAnApprovedLoan
            {
                [SetUp]
                public override void Setup()
                {
                    base.Setup();
                }

                [Test, AUT(AUT.Uk)]
                public void TheLoanAmountHasBeenReducedByThePaymentAmount()
                {
                    Do.With.Timeout(2).Interval(20).Until(() => _application.GetBalance() == _startingBalance - _paymentAmount );
                }
            }
        }
    }
}
