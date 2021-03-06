﻿using System;
using Wonga.QA.Framework.Api.Enums;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Api.Requests.Payments.Commands.Uk
{
    public partial class AddBankAccountUkCommand
    {
        public override void Default()
        {
        	AccountId = Get.GetId();
        	BankAccountId = Get.GetId();
        	BankName = "ABBEY";
        	BankCode = 938600;
        	HolderName = "FirstName LastName";
        	AccountNumber = Get.GetBankAccountNumber();
        	AccountOpenDate = DateTime.Now.AddYears(-4);
        	CountryCode = CountryCodeEnum.UK;
        	IsPrimary = true;
        	IsValid = true;
        }
    }
}
