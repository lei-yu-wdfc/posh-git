using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.Csapi.Commands
{
    /// <summary> Wonga.Comms.Csapi.Commands.CsReportComplaint </summary>
    [XmlRoot("CsReportComplaint", Namespace = "Wonga.Comms.Csapi.Commands", DataType = "")]
    public partial class CsReportComplaintCsCommand : MsmqMessage<CsReportComplaintCsCommand>
    {
        public Guid ApplicationId { get; set; }
        public Guid AccountId { get; set; }
        public Guid CaseId { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
