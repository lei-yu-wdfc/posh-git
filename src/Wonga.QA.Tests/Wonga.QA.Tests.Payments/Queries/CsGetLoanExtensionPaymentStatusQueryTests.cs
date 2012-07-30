using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Cs.Requests.Payments.Csapi.Commands;
using Wonga.QA.Framework.Cs.Requests.Payments.Csapi.Queries;
using Wonga.QA.Tests.Core;
using Wonga.QA.Framework.Cs;

namespace Wonga.QA.Tests.Payments.Queries
{
    [TestFixture]
    public abstract class CsGetLoanExtensionPaymentStatusQueryTests
    {
        protected Application _application;
        protected Customer _customer;
        protected dynamic _loanExtensionStatusRepo = Drive.Data.Payments.Db.LoanExtensions;

        [SetUp]
        public virtual void Setup()
        {
            _customer = CustomerBuilder.New().Build();
            _application = ApplicationBuilder.New(_customer)
                                             .WithLoanAmount(100)
                                             .WithLoanTerm(14)
                                             .WithPromiseDate(DateTime.UtcNow.AddDays(4).ToDate())
                                             .Build();

            //Wait until the application has been accepted.
            //Do.With.Until(() => _application.)
        }

        public abstract class GivenACustomerWithAnApprovedLoan : CsGetLoanExtensionPaymentStatusQueryTests
        {
            protected Guid _extensionId = Guid.NewGuid();
            protected decimal _amount = 0m;

            [SetUp]
            public override void Setup()
            {
                base.Setup();
                
                //Request an extension.
                Drive.Cs.Commands.Post(new CsExtendFixedTermLoanCommand { SalesForceUser = "bob@a.com", 
                                                                          ApplicationId = _application.Id, 
                                                                          LoanExtensionId = _extensionId,
                                                                          CV2 = "121", 
                                                                          ExtensionDate = DateTime.Today + TimeSpan.FromDays(7),
                                                                          PaymentCardId = _customer.GetPaymentCard(), 
                                                                          PartPaymentAmount = _amount });
            }

            protected void GetAndPollExtensionPaymentStatus(string expectedFinalValue)
            {
                //Check that the response is present...
                CsResponse res = Drive.Cs.Queries.Post(new CsGetLoanExtensionPaymentStatusQuery { SalesforceUsername = "csUser", ExtensionId = _extensionId });

                Assert.IsTrue(res.Values["ExtensionId"].Any() && Guid.Parse(res.Values["ExtensionId"].First()) == _extensionId);
                Assert.IsTrue(res.Values["ExtensionStatus"].Any() && !String.IsNullOrEmpty(res.Values["ExtensionStatus"].First()));

                if (res.Values["ExtensionStatus"].First() == "Pending")
                {
                    //Wait to see will it change...
                    Do.With.Timeout(2).Interval(10).Until(() => { res = Drive.Cs.Queries.Post(new CsGetLoanExtensionPaymentStatusQuery { SalesforceUsername = "csUser", ExtensionId = _extensionId });
                                                                  return res.Values["ExtensionStatus"].Any() && res.Values["ExtensionStatus"].First() == expectedFinalValue; });
                }
                else
                {
                    Assert.IsTrue(res.Values["ExtensionStatus"].First() == expectedFinalValue);
                }
            } 

            [Parallelizable(TestScope.Self)]
            public class WhenAnExtensionThatShouldSucceedIsRequested : GivenACustomerWithAnApprovedLoan
            {
                [SetUp]
                public override void Setup()
                {
                    _amount = 50;
                    base.Setup();

                    Do.With.Until(() => _loanExtensionStatusRepo.FindAll(_loanExtensionStatusRepo.PaymentCardId == _customer.GetPaymentCard() &&
                                                                         _loanExtensionStatusRepo.PartPaymentAmount == _amount &&
                                                                         _loanExtensionStatusRepo.ExtendDate == DateTime.Today + TimeSpan.FromDays(21)));
                }

                [Test, AUT(AUT.Uk), Owner(Owner.SeamusHoban)]
                public void TheStatusOfThePartPaymentCanBeObtainedAndIfPendingWillChangeToSuccess()
                {
                    GetAndPollExtensionPaymentStatus("PaymentTaken");
                }
            }

            public class WhenAnExtensionThatShouldFailIsRequested : GivenACustomerWithAnApprovedLoan
            {
                [SetUp]
                public override void Setup()
                {
                    _amount = 13m;
                    base.Setup();
                }

                [Ignore] //Until JH has the failure path complete.
                [Test, AUT(AUT.Uk), Owner(Owner.SeamusHoban)]
                public void TheStatusOfThePartPaymentCanBeOntainedAndIfPendingWillChangeToFailed()
                {
                    GetAndPollExtensionPaymentStatus("PaymentFailed");
                }
            }
        }

    }
}
