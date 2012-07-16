using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.Csapi.Commands
{
    /// <summary> Wonga.Payments.Csapi.Commands.CsReportBankruptcy </summary>
    [XmlRoot("CsReportBankruptcy", Namespace = "Wonga.Payments.Csapi.Commands", DataType = "")]
    public partial class CsReportBankruptcy : MsmqMessage<CsReportBankruptcy>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid CaseId { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
