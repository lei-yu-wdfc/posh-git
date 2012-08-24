using System;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Builders.PayLater
{
	public class PayLaterApplicationDataBase
	{
		public Decimal TotalAmount;
	    public Guid MerchantId;
	    public String MerchantReference;
	    public Guid MerchantOrderId;

		public PayLaterApplicationDataBase()
		{
			TotalAmount = 100m;
		    MerchantId = Guid.NewGuid();
		    MerchantReference = "merchantRef";
		    MerchantOrderId = Guid.NewGuid();
		}
	}
}
