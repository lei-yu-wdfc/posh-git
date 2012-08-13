﻿using Wonga.QA.Framework.Msmq.Enums.Common.Iso;

namespace Wonga.QA.Framework.Msmq.Messages.Risk.Commands.Wb
{
    public partial class RiskCreateBusinessFixedInstallmentLoanApplication
    {
        public override void Default()
        {
            Currency = CurrencyCodeIso4217Enum.GBP;
            Term = 10;
            LoanAmount = 3000;
        }
    }
}
