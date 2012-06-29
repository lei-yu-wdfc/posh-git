using Wonga.QA.Framework.Msmq.Enums.Common.Iso;

namespace Wonga.QA.Framework.Msmq.Messages.Risk.Commands
{
	public partial class RiskCreateBusinessFixedInstallmentLoanApplicationWbCommand
	{
		public override void Default()
		{
			Currency = CurrencyCodeIso4217Enum.GBP;
			Term = 10;
			LoanAmount = 3000;
		}
	}
}
