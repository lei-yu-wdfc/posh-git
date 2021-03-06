using System;
using Wonga.QA.Framework.Api.Enums;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Api.Requests.Payments.Commands.Za
{
    public partial class AddBankAccountZaCommand
    {
        public override void Default()
        {
            AccountId = Get.GetId();
            BankAccountId = Get.GetId();
            AccountNumber = Get.GetBankAccountNumber();
            AccountOpenDate = DateTime.Now.AddYears(-4);
            AccountType = "Savings";
            BankName = "ABSA";
            CountryCode = CountryCodeEnum.ZA;
            HolderName = "First Name Last Name";
        	IsPrimary = true;
        }
    }
}