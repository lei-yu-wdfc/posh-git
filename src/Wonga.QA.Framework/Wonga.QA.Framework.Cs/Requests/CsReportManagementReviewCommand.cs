using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Cs
{
    /// <summary> Wonga.Payments.Csapi.Commands.CsReportManagementReview </summary>
    [XmlRoot("CsReportManagementReview")]
    public partial class CsReportManagementReviewCommand : CsRequest<CsReportManagementReviewCommand>
    {
        public Object ApplicationId { get; set; }
        public Object CaseId { get; set; }
    }
}
