using System;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Api
{
    public partial class AddBankAccountZaCommand
    {
        public override void Default()
        {
            AccountId = Data.GetId();
            BankAccountId = Data.GetId();
            AccountNumber = 12345678901;
            AccountOpenDate = DateTime.Now.AddYears(-4);
            AccountType = "Savings";
            BankName = "ABSA";
            CountryCode = CountryCodeEnum.ZA;
            HolderName = "First Name Last Name";
        	IsPrimary = true;
        }
    }
}