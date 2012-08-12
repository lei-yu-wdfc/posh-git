﻿using System;
using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Comms.Email
{
    public abstract class PaymentConfirmationEmailUkTests
    {
        private Customer _customer;
        private Application _application;
        private decimal _loanAmount;
        private int _loanTerm;
        private string _emailAddress;


        [SetUp]
        public virtual void SetUp()
        {
            _emailAddress = Guid.NewGuid().ToString("N") + "@example.com";

            _customer = CustomerBuilder.New()
                                       .WithEmailAddress(_emailAddress)
                                       .Build();

            Console.WriteLine("Created Customer. Id: {0} with e-mail address {1}", _customer.Id, _emailAddress);

            _application = ApplicationBuilder.New(_customer)
                                             .WithLoanAmount(_loanAmount)
                                             .WithLoanTerm(_loanTerm)
                                             .Build();
            
            Console.WriteLine("Created Application. ApplicationId: {0}", _application.Id);
        }
        
        private bool CheckPaymentConfirmationEmailSent()
        {
            return Drive.Data.QaData.Db.Emails.FindBy(EmailAddress: _emailAddress, TemplateName: "34009");
        }

        public abstract class GivenACustomerWithAnApprovedLoan : PaymentConfirmationEmailUkTests
        {
            [SetUp]
            public override void SetUp()
            {
                _loanAmount = 100;
                _loanTerm = 10;

                base.SetUp();
            }
            [Parallelizable(TestScope.Self)]
            public class WhenFundsTransferred : GivenACustomerWithAnApprovedLoan
            {
                [SetUp]
                public override void SetUp()
                {
                    base.SetUp();

                    // Application should be in the approved state - so all the expected messages should be sent 
                }

                [Test, AUT(AUT.Uk), JIRA("UK-1032"), Owner(Owner.SeamusHoban)]
                public void ThenAPaymentConfirmationEmailIsSent()
                {
                    Do.With.Timeout(1).Interval(20).Until(CheckPaymentConfirmationEmailSent);
                }
            }
        }
    }
}