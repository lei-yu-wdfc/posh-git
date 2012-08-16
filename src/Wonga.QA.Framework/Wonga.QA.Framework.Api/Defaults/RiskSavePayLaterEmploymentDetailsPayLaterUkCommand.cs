using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api.Enums;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Api.Requests.Risk.Commands.PayLater.Uk
{
    public partial class RiskSavePayLaterEmploymentDetailsPayLaterUkCommand 
    {
        public override void Default()
        {
            AccountId = Get.GetId();
            IncomeFrequency = Get.RandomEnum<IncomeFrequencyEnum>();
            NextPayDate = DateTime.Today.AddDays(10).ToDate(DateFormat.Date);
            EmploymentStatus = EmploymentStatusEnum.EmployedFullTime;
        }
    }
}
