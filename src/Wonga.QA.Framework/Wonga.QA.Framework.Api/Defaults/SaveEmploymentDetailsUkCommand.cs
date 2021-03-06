using System;
using Wonga.QA.Framework.Api.Enums;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Api.Requests.Risk.Commands.Uk
{
    public partial class SaveEmploymentDetailsUkCommand
    {
        public override void Default()
        {
            AccountId = Get.GetId();
            EmployerName = "test:EmployedMask";
            EmploymentIndustry = Get.RandomEnum<EmploymentIndustryEnum>();
            EmploymentPosition = Get.RandomEnum<EmploymentPositionEnum>();
            IncomeFrequency = Get.RandomEnum<IncomeFrequencyEnum>();
            NetMonthlyIncome = 1000;
            NextPayDate = DateTime.Today.AddDays(10).ToDate(DateFormat.Date);
            PaidDirectDeposit = true;
            StartDate = DateTime.Today.AddYears(-1).ToDate(DateFormat.Date);
            Status = Get.RandomEnum<EmploymentStatusEnum>();
            Status = EmploymentStatusEnum.EmployedFullTime;
        }
    }
}