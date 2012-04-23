using System;
using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api.Exceptions;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;
using Wonga.QA.Framework.Cs;

namespace Wonga.QA.Tests.Payments.Command
{
   [TestFixture]
   public class CsExtendFixedTermLoanTest
   {
       [Test]
       [Description("Extends an existing fixed term loan")]
       [AUT(AUT.Uk), JIRA("UK-1351")]
       [ExpectedException(typeof(ValidatorException), "DueDateTooFarInFuture")]
       public void CsExtendFixedTermLoanTest_ExtendsExistingLoan_ReturnsErrorIfDueDateMoreThanAWeekAway()
       {
           Customer _customer = CustomerBuilder.New().Build();
           Do.Until(_customer.GetBankAccount);
           Do.Until(_customer.GetPaymentCard);
           Application app = ApplicationBuilder.New(_customer).Build();

           var _cardId = _customer.GetPaymentCard();
           var _extensionId = Guid.NewGuid();

           var cmd = new CsExtendFixedTermLoanCommand
           {

               ApplicationId = app.Id,
               PaymentCardId = _cardId,
               PartPaymentAmount = 1000,
               CV2 = "123",
               AgentId = 5,
               ExtensionDate = DateTime.Today.AddDays(30),
               LoanExtensionId = _extensionId
           };


           Drive.Cs.Commands.Post(cmd);



       }

       [Test]
       [Description("Extends an existing fixed term loan")]
       [AUT(AUT.Uk), JIRA("UK-1351")]
       public void CsExtendFixedTermLoanTest_ExtendsExistingLoan_ReturnsWorksOKIfDueDateLessThanAWeekAway()
       {
           Customer _customer = CustomerBuilder.New().Build();
           Do.Until(_customer.GetBankAccount);
           Do.Until(_customer.GetPaymentCard);
           Application app = ApplicationBuilder.New(_customer).WithPromiseDate(new Date(DateTime.UtcNow.AddDays(4))).Build();
           Drive.Data.Payments.Db.Applications.UpdateByExternalId(ExternalId: app.Id, AcceptedOn: DateTime.UtcNow.AddDays((-10)));

           var _cardId = _customer.GetPaymentCard();
           var _extensionId = Guid.NewGuid();

           var cmd = new CsExtendFixedTermLoanCommand
           {

               ApplicationId = app.Id,
               PaymentCardId = _cardId,
               PartPaymentAmount = 1000,
               CV2 = "123",
               AgentId = 5,
               ExtensionDate = DateTime.Today.AddDays(30),
               LoanExtensionId = _extensionId
           };


           var response = Drive.Cs.Commands.Post(cmd);

           Assert.AreEqual(response.Status, 200);
           GetAndPollExtensionPaymentStatus(_extensionId, "PaymentTaken");


       }

       protected void GetAndPollExtensionPaymentStatus(Guid extensionId, string expectedFinalValue)
       {
           //Check that the response is present...
           CsResponse res = Drive.Cs.Queries.Post(new CsGetLoanExtensionPaymentStatusQuery { SalesforceUser = "csUser", ExtensionId = extensionId });

           Assert.IsTrue(res.Values["ExtensionId"].Any() && Guid.Parse(res.Values["ExtensionId"].First()) == extensionId);
           Assert.IsTrue(res.Values["ExtensionStatus"].Any() && !String.IsNullOrEmpty(res.Values["ExtensionStatus"].First()));

           if (res.Values["ExtensionStatus"].First() == "Pending")
           {
               //Wait to see will it change...
               Do.With.Timeout(2).Interval(10).Until(() =>
               {
                   res = Drive.Cs.Queries.Post(new CsGetLoanExtensionPaymentStatusQuery { SalesforceUser = "csUser", ExtensionId = extensionId });
                   return res.Values["ExtensionStatus"].Any() && res.Values["ExtensionStatus"].First() == expectedFinalValue;
               });
           }
           else
           {
               Assert.IsTrue(res.Values["ExtensionStatus"].First() == expectedFinalValue);
           }
       } 
   }
}

