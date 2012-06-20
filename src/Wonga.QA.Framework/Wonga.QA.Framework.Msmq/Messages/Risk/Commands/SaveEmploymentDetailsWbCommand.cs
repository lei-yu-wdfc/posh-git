using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Wonga.QA.Framework.Msmq.Enums.Risk;

namespace Wonga.QA.Framework.Msmq.Messages.Risk.Commands
{
    /// <summary> Wonga.Risk.Commands.SaveEmploymentDetailsMessageWb </summary>
    [XmlRoot("SaveEmploymentDetailsMessageWb", Namespace = "Wonga.Risk.Commands", DataType = "")]
    public partial class SaveEmploymentDetailsWbCommand : MsmqMessage<SaveEmploymentDetailsWbCommand>
    {
        public Boolean? PaidDirectDeposit { get; set; }
        public Guid AccountId { get; set; }
        public EmploymentStatusEnum Status { get; set; }
        public String EmployerName { get; set; }
        public EmploymentIndustryEnum? EmploymentIndustry { get; set; }
        public EmploymentPositionEnum? EmploymentPosition { get; set; }
        public DateTime? StartDate { get; set; }
        public IncomeFrequencyEnum? IncomeFrequency { get; set; }
        public Decimal NetMonthlyIncome { get; set; }
        public DateTime? NextPayDate { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
