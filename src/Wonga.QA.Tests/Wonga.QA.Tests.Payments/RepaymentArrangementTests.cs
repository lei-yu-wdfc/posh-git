using System;
using System.Linq;
using System.Threading;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db.Payments;
using Wonga.QA.Framework.Msmq;
using Wonga.QA.Tests.Core;
using CreateRepaymentArrangementCommand = Wonga.QA.Framework.Api.CreateRepaymentArrangementCommand;
using PaymentFrequencyEnum = Wonga.QA.Framework.Api.PaymentFrequencyEnum;

namespace Wonga.QA.Tests.Payments
{
	public class RepaymentArrangementTests
	{
		[Test, AUT(AUT.Uk), Parallelizable]
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
			Thread.Sleep(10000);
			var repaymentArrangement = Driver.Db.Payments.RepaymentArrangements.Single(x => x.ApplicationId == dbApplication.ApplicationId);
			Assert.AreEqual(2, repaymentArrangement.RepaymentArrangementDetails.Count);
		}

		[Test, AUT(AUT.Uk), JIRA("UK-725"), Parallelizable]
		public void CreateRepaymentArrangementTest()
		{
			Customer customer = CustomerBuilder.New().Build();
			Do.Until(customer.GetBankAccount);
			Do.Until(customer.GetPaymentCard);
			Application application = ApplicationBuilder.New(customer).Build();

			application.PutApplicationIntoArrears(4);

			var cmd = new CreateRepaymentArrangementCommand()
			           	{
			           		ApplicationId = application.Id,
			           		Frequency = PaymentFrequencyEnum.Every4Weeks,
			           		RepaymentDates = new[] {DateTime.Today.AddDays(1), DateTime.Today.AddMonths(1)},
			           		NumberOfMonths = 2
			           	};
			Driver.Api.Commands.Post(cmd);
			
			var dbApplication = Driver.Db.Payments.Applications.Single(a => a.ExternalId == application.Id);
			Do.Until(() => Driver.Db.Payments.RepaymentArrangements.Single(x => x.ApplicationId == dbApplication.ApplicationId));
			var repaymentArrangement = Driver.Db.Payments.RepaymentArrangements.Single(x => x.ApplicationId == dbApplication.ApplicationId);
			Assert.AreEqual(2, repaymentArrangement.RepaymentArrangementDetails.Count);
			Assert.IsNotNull(Driver.Db.Payments.Transactions.Where(x => x.ApplicationId == dbApplication.ApplicationId && x.Type == "SuspendInterestAccrual"));
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
