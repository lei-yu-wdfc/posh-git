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
    public abstract class CsRepayWithExternalCardCommandTests
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

        public abstract class GivenACustomerWithAnApprovedLoan : CsRepayWithExternalCardCommandTests
        {
            protected decimal _startingBalance;
            protected decimal _paymentAmount;
            protected dynamic _paymentCardRepaymentRequests = Drive.Data.Payments.Db.PaymentCardRepaymentRequests;
            protected Guid _paymentId = Guid.NewGuid();

            [SetUp]
            public override void Setup()
            {
                base.Setup();

                _startingBalance = _application.GetBalance();
                //Issue a payment command.
                Drive.Cs.Commands.Post( GetExternalCard() );
            }

            protected virtual CsRepayWithExternalCardCommand GetExternalCard()
            {
                return new CsRepayWithExternalCardCommand { AccountId = _application.AccountId,
                                                            AddressLine1 = "line 1",
                                                            AddressLine2 = "line 2",
                                                            Amount = _paymentAmount,
                                                            CardNumber = "5411111111111111", 
                                                            CardType = "visaDebit",
                                                            Country = "UK",
                                                            County = "county",
                                                            SalesforceUser = "csUser",
                                                            Currency = "GBP",
                                                            CV2 = "121",
                                                            ExpiryDate = new DateTime(DateTime.Today.Year + 1, 1, 31),
                                                            HolderName = "holder name", 
                                                            PostCode = "12345",
                                                            Town = "town", 
                                                            PaymentId = _paymentId };
            }
            
            [Parallelizable(TestScope.Self)]
            public class GivenAPaymentHasBeenRequestedWithAValidCard : GivenACustomerWithAnApprovedLoan
            {
                [SetUp]
                public override void Setup()
                {
                    _paymentAmount = 50;
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

                    Do.With.Timeout(5).Interval(20).Until(() => _application.GetBalance() == _startingBalance - _paymentAmount);
                }
            }

            [Parallelizable(TestScope.Self)]
            public class GivenAPaymentHasBeenRequestedWithAnInvalidCard : GivenACustomerWithAnApprovedLoan
            {
                [SetUp]
                public override void Setup()
                {
                    _paymentAmount = 13m; //Make it fail.
                    base.Setup();
                }

                [Test, AUT(AUT.Uk)]
                public void TheLoanAmountRemainsTheSameFailedPaymentRequestAdded()
                {
                    Do.With.Timeout(2).Interval(20).Until(() => null != _paymentCardRepaymentRequests.FindAll(_paymentCardRepaymentRequests.Applications.ExternalId == _application.Id &&
                                                                                                              _paymentCardRepaymentRequests.ExternalId == _paymentId &&
                                                                                                              _paymentCardRepaymentRequests.Amount == _paymentAmount &&
                                                                                                              _paymentCardRepaymentRequests.FailedOn != null &&
                                                                                                              _paymentCardRepaymentRequests.SuccessOn == null).FirstOrDefault() );

                    Assert.IsTrue(_application.GetBalance() == _startingBalance);
                }
            }
        }
    }
}
