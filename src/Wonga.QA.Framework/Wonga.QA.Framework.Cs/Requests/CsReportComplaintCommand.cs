using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Cs
{
    /// <summary> Wonga.Comms.Csapi.Commands.CsReportComplaint </summary>
    [XmlRoot("CsReportComplaint")]
    public partial class CsReportComplaintCommand : CsRequest<CsReportComplaintCommand>
    {
        public Object ApplicationId { get; set; }
        public Object AccountId { get; set; }
        public Object CaseId { get; set; }
    }
}
