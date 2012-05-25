using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Risk.Csapi.Commands.CsSaveEmploymentDetailsMessage </summary>
    [XmlRoot("CsSaveEmploymentDetailsMessage", Namespace = "Wonga.Risk.Csapi.Commands", DataType = "")]
    public partial class CsSaveEmploymentDetailsCsCommand : MsmqMessage<CsSaveEmploymentDetailsCsCommand>
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
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
