using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Cs.Requests.Comms.Csapi.Commands
{
    /// <summary> Wonga.Comms.Csapi.Commands.CsReportManagementReview </summary>
    [XmlRoot("CsReportManagementReview")]
    public partial class CsReportManagementReviewCommand : CsRequest<CsReportManagementReviewCommand>
    {
        public Object ApplicationId { get; set; }
        public Object AccountId { get; set; }
        public Object CaseId { get; set; }
    }
}
