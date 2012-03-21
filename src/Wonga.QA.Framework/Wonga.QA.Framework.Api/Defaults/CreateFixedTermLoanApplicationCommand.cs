using System;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Api
{
    public partial class CreateFixedTermLoanApplicationCommand
    {
        public override void Default()
        {
            AccountId = Get.GetId();
            ApplicationId = Get.GetId();
            PaymentCardId = Get.GetId();
            BankAccountId = Get.GetId();
            Currency =
                Config.AUT == AUT.Uk ? CurrencyCodeEnum.GBP :
                Config.AUT == AUT.Za ? CurrencyCodeEnum.ZAR :
                Config.AUT == AUT.Ca ? CurrencyCodeEnum.CAD :
                Config.AUT == AUT.Wb ? CurrencyCodeEnum.GBP : Config.Throw<CurrencyCodeEnum>();
            PromiseDate = DateTime.Today.AddDays(10).ToDate(DateFormat.Date);
            LoanAmount = 100.0m;
        }
    }
}
