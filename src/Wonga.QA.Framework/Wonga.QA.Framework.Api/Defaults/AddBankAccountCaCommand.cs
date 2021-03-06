using System;
using Wonga.QA.Framework.Api.Enums;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Api.Requests.Payments.Commands.Ca
{
    public partial class AddBankAccountCaCommand
    {
        public override void Default()
        {
            AccountId = Get.GetId();
            BankAccountId = Get.GetId();
            AccountNumber = Get.GetBankAccountNumber();
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