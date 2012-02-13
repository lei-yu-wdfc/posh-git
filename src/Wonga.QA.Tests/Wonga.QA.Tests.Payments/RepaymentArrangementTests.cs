using System;
using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Msmq;
using Wonga.QA.Framework.Msmq.Payments;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Payments
{
	public class RepaymentArrangementTests
	{
		[Test, AUT(AUT.Uk)]
		public void CustomerServiceSetRepaymentArrangementTest()
		{
			Customer customer = CustomerBuilder.New().Build();
			Do.Until(customer.GetBankAccount);
			Do.Until(customer.GetPaymentCard);
			Application application = ApplicationBuilder.New(customer).Build();
			
			application.PutApplicationIntoArrears();

			Driver.Msmq.Payments.Send(new CreateExtendedRepaymentArrangementCommand
			                          	{
			                          		AccountId = customer.Id,
											ApplicationId = application.Id,
											EffectiveBalance = 100,
											RepaymentAmount = 100,
											RepaymentDetails = new[]
											                   	{
											                   		new ArrangementDetail{Amount = 49, Currency = CurrencyCodeIso4217Enum.GBP, DueDate = DateTime.Today},
																	new ArrangementDetail{Amount = 51, Currency = CurrencyCodeIso4217Enum.GBP, DueDate = DateTime.Today.AddDays(7)}
											                   	}
			                          	});

			var dbApplication = Driver.Db.Payments.Applications.Single(a => a.ExternalId == application.Id);
			Do.Sleep(10);
			var repaymentArrangement = Driver.Db.Payments.RepaymentArrangements.Single(x => x.ApplicationId == dbApplication.ApplicationId);
			Assert.AreEqual(2, repaymentArrangement.RepaymentArrangementDetails.Count);
		}

		//Needed for serialization in CreateExtendedRepaymentArrangementCommand
		private class ArrangementDetail
		{
			public decimal Amount { get; set; }
			public CurrencyCodeIso4217Enum Currency { get; set; }
			public DateTime DueDate { get; set; }
		}
	}
}
