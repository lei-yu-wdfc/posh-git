using System;
using System.Collections.Generic;
using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db;
using Wonga.QA.Framework.Db.QaData;
using Wonga.QA.Framework.Helpers;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Payments
{
    [TestFixture, AUT(AUT.Ca), JIRA("WIN-887")]
    public class CashOutInstructionTests
    {
         public class GivenACustomer : CashOutInstructionTests
         {
             protected Customer Customer;
             protected string Forename;             

             [SetUp]
             public virtual void SetUp()
             {
                 Forename = "ojkslmc";
                 Customer = CustomerBuilder.New().WithForename(Forename).Build();
             }

             public class WhenTheySignALoanApplication : GivenACustomer
             {
                 protected Application Application;
                 protected decimal Amount;
                 protected int LoanTerm;
                 protected decimal TotalRepayable;

                 [SetUp]
                 public override void SetUp()
                 {
                     base.SetUp();

                     Amount = 100;
                     LoanTerm = 10;
                     TotalRepayable = 110;
                     Application =
                         ApplicationBuilder.New(Customer).WithExpectedDecision(ApplicationDecisionStatus.Accepted).
                             WithLoanAmount(Amount).WithLoanTerm(LoanTerm).Build();
                 }

                 [Test]
                 public void ThenTheCustomerShouldReceiveAPaymentConfirmationEmail()
                 {
                     List<EmailToken> emailTokens = GetEmailTokens("Email.FundsTransferredEmailTemplate");

                     DateTime promiseDate = DateHelper.GetPromiseDateForLoanTermForCa(LoanTerm);

                     Assert.IsTrue(emailTokens.Count == 5);

                     Assert.IsTrue(emailTokens[0].Value == Forename);
                     Assert.IsTrue(emailTokens[1].Value == Customer.Email);
                     Assert.IsTrue(emailTokens[2].Value == Amount.ToString("0.00"));
                     Assert.IsTrue(emailTokens[3].Value == TotalRepayable.ToString("0.00"));
                     Assert.IsTrue(emailTokens[4].Value == promiseDate.ToShortDateString());
                 }

                 private List<EmailToken> GetEmailTokens(String emailTemplateName)
                 {
                     var db = new DbDriver().QaData;
                     var emailId = Do.Until(() => db.Emails.Single(e => e.EmailAddress == Customer.Email && e.TemplateName == GetEmailTemplateId(emailTemplateName)).EmailId);
                     return db.EmailTokens.Where(et => et.EmailId == emailId).ToList();
                 }

                 private string GetEmailTemplateId(string emailTemplateName)
                 {
                     return Drive.Db.Ops.ServiceConfigurations.Single(v => v.Key == emailTemplateName).Value;
                 }
             }
         }
    }
}