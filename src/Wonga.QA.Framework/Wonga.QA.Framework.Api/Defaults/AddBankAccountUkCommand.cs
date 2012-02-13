using System;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Api
{
    public partial class AddBankAccountUkCommand
    {
        public override void Default()
        {
            AccountId = Data.GetId();
            BankAccountId = Data.GetId();
            BankName = "AllianceLeicester";
            BankCode = 134020;
            BankName = "ABBEY";
			BankCode = "309894";
            HolderName = "FirstName LastName";
            AccountNumber = 63849203;
            AccountOpenDate = DateTime.Now.AddYears(-4).ToDate(DateFormat.DateTime);
			AccountNumber = "14690568";
            AccountOpenDate = DateTime.Now.AddYears(-4);
            CountryCode = CountryCodeEnum.UK;
            IsPrimary = true;
        }
    }
}
