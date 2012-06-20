using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace Wonga.QA.Framework.Msmq.Messages.Risk.Commands.Pl
{
    /// <summary> Wonga.Risk.Commands.Pl.SaveEmploymentDetailsMessage </summary>
    [XmlRoot("SaveEmploymentDetailsMessage", Namespace = "Wonga.Risk.Commands.Pl", DataType = "")]
    public partial class SaveEmploymentDetailsCommand : MsmqMessage<SaveEmploymentDetailsCommand>
    {
        public Guid AccountId { get; set; }
        public Decimal NetMonthlyIncome { get; set; }
        public String IncomeFrequency { get; set; }
        public DateTime? NextPayDate { get; set; }
        public String Status { get; set; }
        public String EmploymentIndustry { get; set; }
        public String UniversityType { get; set; }
        public String UniversityCity { get; set; }
        public String UniversityName { get; set; }
        public String EmployerName { get; set; }
        public String YearsAtEmployer { get; set; }
        public Int32 MonthsAtEmployer { get; set; }
        public Boolean? PaidDirectDeposit { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
