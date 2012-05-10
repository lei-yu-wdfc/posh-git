using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Payments.Csapi.Commands.CsReportRefundRequest </summary>
    [XmlRoot("CsReportRefundRequest", Namespace = "Wonga.Payments.Csapi.Commands", DataType = "")]
    public partial class CsReportRefundRequestCsCommand : MsmqMessage<CsReportRefundRequestCsCommand>
    {
        public Guid ApplicationId { get; set; }
        public Guid CaseId { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
