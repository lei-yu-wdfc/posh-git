using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Cs
{
    /// <summary> Wonga.Comms.Csapi.Commands.CsRemoveManagementReview </summary>
    [XmlRoot("CsRemoveManagementReview")]
    public partial class CsRemoveManagementReviewCommand : CsRequest<CsRemoveManagementReviewCommand>
    {
        public Object ApplicationId { get; set; }
        public Object AccountId { get; set; }
        public Object CaseId { get; set; }
    }
}
