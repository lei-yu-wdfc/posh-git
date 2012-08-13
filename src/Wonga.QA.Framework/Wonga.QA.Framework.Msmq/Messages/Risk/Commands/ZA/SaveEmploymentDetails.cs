using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Wonga.QA.Framework.Msmq.Enums.Integration.Risk;

namespace Wonga.QA.Framework.Msmq.Messages.Risk.Commands.ZA
{
    /// <summary> Wonga.Risk.Commands.ZA.SaveEmploymentDetailsMessage </summary>
    [XmlRoot("SaveEmploymentDetailsMessage", Namespace = "Wonga.Risk.Commands.ZA", DataType = "" )
    , SourceAssembly("Wonga.Risk.Commands.ZA, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class SaveEmploymentDetails : MsmqMessage<SaveEmploymentDetails>
    {
        public Guid AccountId { get; set; }
        public EmploymentStatusEnum Status { get; set; }
        public String EmployerName { get; set; }
        public EmploymentIndustryEnum? EmploymentIndustry { get; set; }
        public EmploymentPositionEnum? EmploymentPosition { get; set; }
        public DateTime? StartDate { get; set; }
        public IncomeFrequencyEnum? IncomeFrequency { get; set; }
        public Decimal NetMonthlyIncome { get; set; }
        public DateTime? NextPayDate { get; set; }
        public Boolean? PaidDirectDeposit { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
