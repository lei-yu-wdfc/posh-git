using System;
using Wonga.QA.Framework.Api.Enums;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Api.Requests.Payments.Commands.Za
{
    public partial class CreateFixedTermLoanApplicationZaCommand
    {
        public override void Default()
        {
            AccountId = Get.GetId();
            ApplicationId = Get.GetId();
            PaymentCardId = Get.GetId();
            BankAccountId = Get.GetId();
            Currency = CurrencyCodeEnum.ZAR;
            PromiseDate = DateTime.Today.AddDays(10).ToDate(DateFormat.Date);
            LoanAmount = 100.0m;
        }
    }
}
