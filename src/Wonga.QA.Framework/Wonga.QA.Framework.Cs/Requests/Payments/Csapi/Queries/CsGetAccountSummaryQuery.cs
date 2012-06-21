using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Cs.Requests.Payments.Csapi.Queries
{
    /// <summary> Wonga.Payments.Csapi.Queries.CsGetAccountSummary </summary>
    [XmlRoot("CsGetAccountSummary")]
    public partial class CsGetAccountSummaryQuery : CsRequest<CsGetAccountSummaryQuery>
    {
        public Object AccountId { get; set; }
    }
}
