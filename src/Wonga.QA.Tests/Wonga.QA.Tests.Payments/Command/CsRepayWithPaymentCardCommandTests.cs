using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Cs;
using Wonga.QA.Framework.Cs.Requests.Payments.Csapi.Commands;
using Wonga.QA.Framework.Helpers;
using Wonga.QA.Tests.Core;
using Wonga.QA.Framework.Db.Payments;

namespace Wonga.QA.Tests.Payments.Command
{
    [TestFixture]
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
        }

        public abstract class GivenACustomerWithAnApprovedLoan : CsRepayWithPaymentCardCommandTests
        {
            protected decimal _startingBalance;
            protected decimal _paymentAmount;
            protected dynamic _paymentCardRepaymentRequests = Drive.Data.Payments.Db.PaymentCardRepaymentRequests;
            protected Guid _paymentId = Guid.NewGuid();

            protected abstract void UpdateCardExpiryDate();

            [SetUp]
            public override void Setup()
            {
                base.Setup();

                _startingBalance = _application.GetBalance();
                _paymentAmount = 50m;
                //Issue a payment command.
                UpdateCardExpiryDate();
                Drive.Cs.Commands.Post(new CsRepayWithPaymentCardCommand { AccountId = _application.AccountId, 
                                                                           Amount = _paymentAmount, 
                                                                           SalesforceUser = "csUser", 
                                                                           CV2 = "111",
                                                                           Currency = "GBP", 
                                                                           PaymentCardId = _cardId,
                                                                           PaymentId = _paymentId});
            }

            [Parallelizable(TestScope.Self)]
            public class GivenAPaymentHasBeenRequestedWithAValidCard : GivenACustomerWithAnApprovedLoan
            {
                [SetUp]
                public override void Setup()
                {
                    base.Setup();
                }

                [Test, AUT(AUT.Uk)]
                public void TheLoanAmountHasBeenReducedByPaymentAmountPaymentRequestAdded()
                {
                    Do.With.Timeout(2).Interval(20).Until(() => null != _paymentCardRepaymentRequests.FindAll(_paymentCardRepaymentRequests.Applications.ExternalId == _application.Id &&
                                                                                                              _paymentCardRepaymentRequests.ExternalId == _paymentId &&
                                                                                                              _paymentCardRepaymentRequests.Amount == _paymentAmount &&
                                                                                                              _paymentCardRepaymentRequests.FailedOn == null &&
                                                                                                              _paymentCardRepaymentRequests.SuccessOn != null).FirstOrDefault());
                    Do.With.Timeout(2).Interval(20).Until(() => _application.GetBalance() == _startingBalance - _paymentAmount);
                }

                protected override void UpdateCardExpiryDate() { } //Do nothing.
            }

            [Parallelizable(TestScope.Self)]
            public class GivenAPaymentHasBeenRequestedWithAnInvalidCard : GivenACustomerWithAnApprovedLoan
            {
                [SetUp]
                public override void Setup()
                {
                    base.Setup();
                }

                [Test, AUT(AUT.Uk)]
                public void TheLoanAmountRemainsTheSameFailedPaymentRequestAdded()
                {
                    Do.With.Timeout(2).Interval(20).Until(() => null != _paymentCardRepaymentRequests.FindAll(_paymentCardRepaymentRequests.Applications.ExternalId == _application.Id &&
                                                                                                              _paymentCardRepaymentRequests.ExternalId == _paymentId &&
                                                                                                              _paymentCardRepaymentRequests.Amount == _paymentAmount &&
                                                                                                              _paymentCardRepaymentRequests.FailedOn != null &&
                                                                                                              _paymentCardRepaymentRequests.SuccessOn == null).FirstOrDefault());

                    Assert.IsTrue(_application.GetBalance() == _startingBalance);
                }

                protected override void UpdateCardExpiryDate()
                {
                    //Get the id of the card and manually adjust its expiry
                    Drive.Data.Payments.Db.PaymentCardsBase.UpdateByExternalId(ExternalId : _cardId, ExpiryDate : new DateTime(DateTime.Now.Year -1, 1, 31));
                }
            }
        }
    }
}
