using System;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;
using Wonga.QA.Tests.Payments.Helpers;

namespace Wonga.QA.Tests.Payments
{
    [TestFixture]
    [Parallelizable(TestScope.All)]
    public abstract class EarlyRepaymentTests
    {
        private ProvinceEnum _province;
        private Customer _customer;
        private Application _application;
        private decimal _loanAmount;
        private int _loanTerm;

        [SetUp]
        public virtual void SetUp()
        {
            CreateApplication();
        }

        [TearDown]
        public void TearDown()
        {
        }

        private void CreateApplication()
        {
            _customer = CustomerBuilder.New().ForProvince(_province).Build();

            Console.WriteLine("Created Customer. Id: {0}", _customer.Id);

            _application = ApplicationBuilder.New(_customer).WithLoanAmount(_loanAmount).WithLoanTerm(_loanTerm).Build();

            Console.WriteLine("Created Application. ApplicationId: {0}", _application.Id);
        }

        public abstract class GivenACustomerFromOntarioWithAnActiveLoan : EarlyRepaymentTests
        {
            public override void SetUp()
            {
                _province = ProvinceEnum.ON;
                _loanAmount = 100;
                _loanTerm = 10;

                base.SetUp();
            }

            public class WhenTheLoanHasAnEarlyPartialRepaymentAndIsDue : GivenACustomerFromOntarioWithAnActiveLoan
            {
                private decimal _earlyRepayment;
                private int _earlyRepaymentDay;

                public override void SetUp()
                {
                    base.SetUp();

                    _earlyRepayment = 50;
                    _earlyRepaymentDay = 4;

                    _application.RepayEarly(_earlyRepayment, _earlyRepaymentDay);
                }

                [Test, AUT(AUT.Ca)]
                public void ThenTheApplicationShouldNotClose()
                {
                    Assert.IsTrue(VerifyPaymentFunctions.VerifyApplicationNotClosedAfterCashIn(_application.Id));
                }
            }

            public class WhenTheLoanHasAnEarlyFullRepayment : GivenACustomerFromOntarioWithAnActiveLoan
            {
                private decimal _earlyRepayment;
                private int _earlyRepaymentDay;

                public override void SetUp()
                {
                    base.SetUp();

                    _earlyRepayment = 101;
                    _earlyRepaymentDay = 2;

                    _application.RepayEarly(_earlyRepayment, _earlyRepaymentDay);
                }

                [Test, AUT(AUT.Ca)]
                public void ThenTheApplicationShouldClose()
                {
                    Assert.IsTrue(Do.With.Timeout(1).Until(() => _application.IsClosed));
                }
            } 
        }
    }
}