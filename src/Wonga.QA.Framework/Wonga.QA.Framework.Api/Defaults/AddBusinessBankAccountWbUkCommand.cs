using System;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Api
{
    public partial class AddBusinessBankAccountWbUkCommand
    {
        public override void Default()
        {
            AccountNumber = "00000190";
            AccountOpenDate = DateTime.Now.AddYears(-4);
            BankAccountId = Data.GetId();
            BankCode = "180002";
            BankName = "FSB Business Banking";
            CountryCode = CountryCodeEnum.UK;
            HolderName = Data.GetName();
            OrganisationId = Data.GetId();
        }
    }
}
