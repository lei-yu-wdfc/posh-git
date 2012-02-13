using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
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
		}
	}
}
