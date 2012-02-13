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
            HolderName = "FirstName LastName";
            AccountNumber = 63849203;
            AccountOpenDate = DateTime.Now.AddYears(-4).ToDate(DateFormat.DateTime);
            CountryCode = CountryCodeEnum.UK;
            IsPrimary = true;
        }
    }
}
