using System;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Api
{
    public partial class AddBankAccountCaCommand
    {
        public override void Default()
        {
            AccountId = Data.GetId();
            BankAccountId = Data.GetId();
            AccountNumber = 1234567;
            BranchNumber = "00011";
            InstitutionNumber = "001";
            AccountOpenDate = DateTime.Now.AddYears(-4);
            BankName = "HSBC Bank Canada";
            CountryCode = CountryCodeEnum.CA;
            HolderName = "First Name Last Name";
            IsPrimary = true;
        }
    }
}