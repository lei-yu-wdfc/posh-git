using System;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Api
{
    public partial class SaveEmploymentDetailsUkCommand
    {
        public override void Default()
        {
            AccountId = Data.GetId();
            EmployerName = "test:EmployedMask";
            EmploymentIndustry = Data.RandomEnum<EmploymentIndustryEnum>();
            EmploymentPosition = Data.RandomEnum<EmploymentPositionEnum>();
            IncomeFrequency = Data.RandomEnum<IncomeFrequencyEnum>();
            NetMonthlyIncome = 1000;
            NextPayDate = DateTime.Today.AddDays(10).ToDate(DateFormat.Date);
            PaidDirectDeposit = true;
            StartDate = DateTime.Today.AddYears(-1).ToDate(DateFormat.Date);
            //Status = Data.RandomEnum<EmploymentStatusEnum>();
            Status = EmploymentStatusEnum.EmployedFullTime;
        }
    }
}