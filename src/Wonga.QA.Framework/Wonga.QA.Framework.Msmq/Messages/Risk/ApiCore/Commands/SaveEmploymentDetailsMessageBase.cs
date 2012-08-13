using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Wonga.QA.Framework.Msmq.Enums.Integration.Risk;

namespace Wonga.QA.Framework.Msmq.Messages.Risk.ApiCore.Commands
{
    /// <summary> Wonga.Risk.ApiCore.Commands.SaveEmploymentDetailsMessageBase </summary>
    [XmlRoot("SaveEmploymentDetailsMessageBase", Namespace = "Wonga.Risk.ApiCore.Commands", DataType = "" )
    , SourceAssembly("Wonga.Risk.ApiCore.Commands, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class SaveEmploymentDetailsMessageBase : MsmqMessage<SaveEmploymentDetailsMessageBase>
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
