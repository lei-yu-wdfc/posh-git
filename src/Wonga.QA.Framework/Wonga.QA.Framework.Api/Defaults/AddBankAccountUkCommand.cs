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
            BankName = "ABBEY";
            BankCode = 938600;
            HolderName = "FirstName LastName";
			AccountNumber = 42368003;
            AccountOpenDate = DateTime.Now.AddYears(-4);
            CountryCode = CountryCodeEnum.UK;
            IsPrimary = true;
        }
    }
}
