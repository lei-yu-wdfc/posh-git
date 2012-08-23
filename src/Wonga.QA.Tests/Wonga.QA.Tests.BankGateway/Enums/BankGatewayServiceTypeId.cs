using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wonga.QA.Tests.BankGateway.Enums
{
	public enum BankGatewayServiceTypeId
	{
		LoanPayment = 1,
		BillPayment = 2,
		AccountVerification = 3,
		Collection = 4,
		LoanPaymentBmo = 5,
		CollectionBmo = 6,
		LoanPaymentRbc = 7,
		CollectionRbc = 8
	}
}
