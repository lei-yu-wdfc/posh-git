using System;
using Wonga.QA.Framework.Api.Enums;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Api.Requests.Risk.Commands.Pl
{
    public partial class SaveEmploymentDetailsPlCommand
    {
        public override void Default()
        {
            AccountId = Get.GetId();
            EmployerName = "test:EmployedMask";
            EmploymentIndustry = Get.RandomEnum<EmploymentIndustryEnum>();
            IncomeFrequency = Get.RandomEnum<IncomeFrequencyEnum>();
            NetMonthlyIncome = 1000;
            NextPayDate = DateTime.Today.AddDays(10).ToDate(DateFormat.Date);
            PaidDirectDeposit = true;
            Status = Get.RandomEnum<EmploymentStatusEnum>();
            Status = EmploymentStatusEnum.EmployedFullTime;
			//TODO: add more
        }
    }
}