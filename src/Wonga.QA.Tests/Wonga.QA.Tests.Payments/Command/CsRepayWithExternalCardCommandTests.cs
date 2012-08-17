using System;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api.Exceptions;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Cs.Requests.Payments.Csapi.Commands;
using Wonga.QA.Framework.Old;
using Wonga.QA.Tests.Core;
using Wonga.QA.Tests.Salesforce;

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
            protected DateTime _expiryDate = new DateTime(DateTime.Now.Year + 1, 1, 31);
            protected dynamic _paymentCardRepaymentRequests = Drive.Data.Payments.Db.PaymentCardRepaymentRequests;
            protected Guid _paymentId = Guid.NewGuid();

            [SetUp]
            public override void Setup()
            {
                base.Setup();

                _startingBalance = _application.GetBalance();
            }

            protected virtual CsRepayWithExternalCardCommand GetExternalCard()
            {
                return new CsRepayWithExternalCardCommand
                {
                    AccountId = _application.AccountId,
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
                    ExpiryDate = _expiryDate,
                    HolderName = "holder name",
                    PostCode = "12345",
                    Town = "town",
                    PaymentId = _paymentId
                };
            }

            [Parallelizable(TestScope.Self)]
            public class GivenAPaymentHasBeenRequestedWithAValidCard : GivenACustomerWithAnApprovedLoan
            {
                [SetUp]
                public override void Setup()
                {
                    _paymentAmount = 50;
                    base.Setup();
                    //Issue a payment command.
                    Drive.Cs.Commands.Post(GetExternalCard());
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

                [Test, AUT(AUT.Uk), JIRA("UKOPS-862"), Pending("External Card is added as a card when CsRepayWithExternalCard is called, this hsould not be the case")]
                public void TheCardIsNotAddedToTheAccount()
                {
                    dynamic paymentCardsBase = Drive.Data.Payments.Db.PaymentCardsBase;
                    dynamic paymentCardId = paymentCardsBase.FindAll(paymentCardsBase.ExternalId == _cardId).Single().PaymentCardId;
                    dynamic personalPaymentCards = Drive.Data.Payments.Db.PersonalPaymentCards;
                    dynamic row =
                        personalPaymentCards.FindAll(personalPaymentCards.AccountId == _application.AccountId &&
                                                     personalPaymentCards.PaymentCardId == paymentCardId).FirstOrDefault();
                    Assert.IsNull(row);
                }
            }

            [Parallelizable(TestScope.Self)]
            public class GivenAPaymentHasBeenRequestedWithAValidCardAndLoanIsInArrears : GivenACustomerWithAnApprovedLoan
            {
                [SetUp]
                public override void Setup()
                {
                    _paymentAmount = 50;
                    base.Setup();
                    _application.PutIntoArrears(5);
                    //Issue a payment command.
                    Drive.Cs.Commands.Post(GetExternalCard());
                }

                [Test, AUT(AUT.Uk), JIRA("UKOPS-109"), Owner(Owner.ShaneMcHugh)]
                public void TheLoanAmountHasBeenReducedByPaymentAmountPaymentRequestAdded()
                {
                    Do.With.Timeout(2).Interval(20).Until(() => null != _paymentCardRepaymentRequests.FindAll(_paymentCardRepaymentRequests.Applications.ExternalId == _application.Id &&
                                                                                                              _paymentCardRepaymentRequests.ExternalId == _paymentId &&
                                                                                                              _paymentCardRepaymentRequests.Amount == _paymentAmount &&
                                                                                                              _paymentCardRepaymentRequests.FailedOn != null &&
                                                                                                              _paymentCardRepaymentRequests.SuccessOn == null).FirstOrDefault());
                    //This is hardcoded in UK so have to hardcode here.
                    const int defaultCharge = 20;
                    Do.With.Timeout(5).Interval(20).Until(() => _application.GetBalance() == (_startingBalance + defaultCharge) - _paymentAmount);
                }
            }

            [Parallelizable(TestScope.Self)]
            public class GivenAPaymentHigherThanTheBalanceHasBeenRequestedWithAValidCard : GivenACustomerWithAnApprovedLoan
            {
                [SetUp]
                public override void Setup()
                {
                    _paymentAmount = 1000;
                    base.Setup();
                    //Issue a payment command.
                    Drive.Cs.Commands.Post(GetExternalCard());
                }

                [Test, AUT(AUT.Uk), JIRA("UKOPS-109"), Owner(Owner.ShaneMcHugh), Pending("Currently payment higher than the balance is allowed and should not be UKOPS-846")]
                public void TheLoanAmountRemainsTheSameFailedPaymentRequestAdded()
                {
                    Do.With.Timeout(2).Interval(20).Until(() => null != _paymentCardRepaymentRequests.FindAll(_paymentCardRepaymentRequests.Applications.ExternalId == _application.Id &&
                                                                             _paymentCardRepaymentRequests.ExternalId == _paymentId &&
                                                                             _paymentCardRepaymentRequests.Amount == _paymentAmount &&
                                                                             _paymentCardRepaymentRequests.FailedOn == null &&
                                                                             _paymentCardRepaymentRequests.SuccessOn != null).FirstOrDefault());

                    Do.With.Timeout(5).Interval(20).Until(() => _application.GetBalance() == _startingBalance);
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
                    //Issue a payment command.
                    Drive.Cs.Commands.Post(GetExternalCard());
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
            }

            [Parallelizable(TestScope.Self)]
            public class GivenAPaymentHasBeenRequestedWithAnExpiredCard : GivenACustomerWithAnApprovedLoan
            {
                private ValidatorException _exception;

                [SetUp]
                public override void Setup()
                {
                    _paymentAmount = 50m;
                    _expiryDate = new DateTime(DateTime.Now.Year - 1, 1, 31);
                    base.Setup();
                    //Issue a payment command.
                    _exception = Assert.Throws<ValidatorException>(() => Drive.Cs.Commands.Post(GetExternalCard()));
                }

                [Test, AUT(AUT.Uk)]
                public void TheLoanAmountRemainsTheSameFailedPaymentRequestAdded()
                {
                    Assert.Contains(_exception.Errors, "Payments_PaymentCard_Expired");
                    Do.With.Timeout(2).Interval(20).Until(() => null != _paymentCardRepaymentRequests.FindAll(_paymentCardRepaymentRequests.Applications.ExternalId == _application.Id &&
                                                                                                              _paymentCardRepaymentRequests.ExternalId == _paymentId &&
                                                                                                              _paymentCardRepaymentRequests.Amount == _paymentAmount &&
                                                                                                              _paymentCardRepaymentRequests.FailedOn != null &&
                                                                                                              _paymentCardRepaymentRequests.SuccessOn == null).FirstOrDefault());

                    Assert.IsTrue(_application.GetBalance() == _startingBalance);
                }
            }

            [Parallelizable(TestScope.Self)]
            public class GivenAPaymentInFullHasBeenRequestedWithAValidCard : GivenACustomerWithAnApprovedLoan
            {
                [SetUp]
                public override void Setup()
                {
                    base.Setup();
                    _paymentAmount = _application.GetBalance();
                    //Issue a payment command.
                    Drive.Cs.Commands.Post(GetExternalCard());
                }

                [Test, AUT(AUT.Uk), JIRA("UKOPS-109"), Owner(Owner.ShaneMcHugh)]
                public void TheLoanAmountHasBeenReducedByPaymentAmountPaymentRequestAdded()
                {
                    Do.With.Timeout(2).Interval(20).Until(() => null != _paymentCardRepaymentRequests.FindAll(_paymentCardRepaymentRequests.Applications.ExternalId == _application.Id &&
                                                                                                              _paymentCardRepaymentRequests.ExternalId == _paymentId &&
                                                                                                              _paymentCardRepaymentRequests.Amount == _paymentAmount &&
                                                                                                              _paymentCardRepaymentRequests.FailedOn == null &&
                                                                                                              _paymentCardRepaymentRequests.SuccessOn != null).FirstOrDefault());

                    dynamic applicationStatusHistory = Drive.Data.BiCustomerManagement.Db.ApplicationStatusHistory;
                    Do.With.Timeout(2).Interval(20).Until(() => null != applicationStatusHistory.FindAll(applicationStatusHistory.ApplicationId == _application.Id &&
                                                                                                         applicationStatusHistory.CurrentStatus == Framework.ThirdParties.Salesforce.ApplicationStatus.PaidInFull).FirstOrDefault());

                    Do.With.Timeout(5).Interval(20).Until(() => _application.GetBalance() == _startingBalance - _paymentAmount);
                }
            }
        }
    }
}
