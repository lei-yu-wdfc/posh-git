using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wonga.QA.Framework.Msmq.Messages.Risk
{
	public partial class RiskCreateFixedTermLoanApplication
	{
		public override void Default()
		{
			base.Default();
			PromiseDate = DateTime.UtcNow;
			LoanAmount = 100;
			CreatedOn = DateTime.UtcNow;
		}
	}
}
