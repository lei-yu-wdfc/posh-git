using System;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Api
{
    public partial class AddBankAccountZaCommand
    {
        public override void Default()
        {
            AccountId = Get.GetId();
            BankAccountId = Get.GetId();
            AccountNumber = Get.GetBankAccountNumber(); ;
            AccountOpenDate = DateTime.Now.AddYears(-4);
            AccountType = "Savings";
            BankName = "ABSA";
            CountryCode = CountryCodeEnum.ZA;
            HolderName = "First Name Last Name";
        	IsPrimary = true;
        }
    }
}