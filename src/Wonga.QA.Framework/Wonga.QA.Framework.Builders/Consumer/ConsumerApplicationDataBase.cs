using System;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Data.Enums.Risk;

namespace Wonga.QA.Framework.Builders.Consumer
{
	public class ConsumerApplicationDataBase
	{
		public Decimal LoanAmount;
		private Date _promiseDate;
		public ApplicationDecisionStatus? ExpectedDecision;
		public bool SignIfAccepted;
		public IovationMockResponse IovationResponse;
		public Date PromiseDate
		{
			get { return _promiseDate; }
			set
			{
				_promiseDate = value;
				_promiseDate.DateFormat = DateFormat.Date;
			}
		}


		public ConsumerApplicationDataBase()
		{
			LoanAmount = Get.GetLoanAmount();
			ExpectedDecision = ApplicationDecisionStatus.Accepted;
			SignIfAccepted = true;
			IovationResponse = IovationMockResponse.Allow;
			PromiseDate = Get.GetPromiseDate();
		}
	}
}
