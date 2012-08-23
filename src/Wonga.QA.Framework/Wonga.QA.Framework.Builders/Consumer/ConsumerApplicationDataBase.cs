using System;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Builders.Consumer
{
	public class ConsumerApplicationDataBase
	{
		public Decimal LoanAmount;
		public Date _promiseDate;

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
			PromiseDate = Get.GetPromiseDate();
		}
	}
}
