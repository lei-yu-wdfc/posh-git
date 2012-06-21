using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Cs.Requests.Payments.Csapi.Commands
{
    /// <summary> Wonga.Payments.Csapi.Commands.CsReportHardship </summary>
    [XmlRoot("CsReportHardship")]
    public partial class CsReportHardshipCommand : CsRequest<CsReportHardshipCommand>
    {
        public Object CaseId { get; set; }
        public Object AccountId { get; set; }
        public Object ApplicationId { get; set; }
    }
}
