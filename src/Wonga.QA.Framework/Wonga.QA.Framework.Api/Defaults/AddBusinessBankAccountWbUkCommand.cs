using System;
using Wonga.QA.Framework.Api.Enums;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Api.Requests.Payments.Commands.Wb.Uk
{
    public partial class AddBusinessBankAccountWbUkCommand
    {
        public override void Default()
        {
            AccountNumber = "00000190";
            AccountOpenDate = DateTime.Now.AddYears(-4);
            BankAccountId = Get.GetId();
            BankCode = "180002";
            BankName = "FSB Business Banking";
            CountryCode = CountryCodeEnum.UK;
            HolderName = Get.GetName();
            OrganisationId = Get.GetId();
        }
    }
}
