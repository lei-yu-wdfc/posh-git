using System;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Api.Enums;
using Wonga.QA.Framework.Data.Enums.Risk;

namespace Wonga.QA.Framework.Builders.PayLater
{
	public class PayLaterApplicationDataBase
	{
		public ApplicationDecisionStatus? ExpectedDecision;
        public bool SignIfAccepted;

        public IovationMockResponse IovationResponse;

	    public Guid MerchantId;
	    public String MerchantReference;
	    public Guid MerchantOrderId;

	    public decimal TotalAmount;
	    public CurrencyCodeEnum Currency;

		public PayLaterApplicationDataBase()
		{
			ExpectedDecision = ApplicationDecisionStatus.Accepted;
            SignIfAccepted = true;
            IovationResponse = IovationMockResponse.Allow;

		    MerchantId = Guid.NewGuid();
		    MerchantReference = "merchantRef";
		    MerchantOrderId = Guid.NewGuid();

		    TotalAmount = 100m;
            Currency = CurrencyCodeEnum.GBP;
		}
	}
}
