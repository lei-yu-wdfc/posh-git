using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Cs.Requests.Payments.Csapi.Commands
{
    /// <summary> Wonga.Payments.Csapi.Commands.CsReportRefundRequest </summary>
    [XmlRoot("CsReportRefundRequest")]
    public partial class CsReportRefundRequestCommand : CsRequest<CsReportRefundRequestCommand>
    {
        public Object ApplicationId { get; set; }
        public Object CaseId { get; set; }
    }
}
