using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Cs
{
    /// <summary> Wonga.Payments.Csapi.Queries.Wb.Uk.GetBusinessApplicationSummary </summary>
    [XmlRoot("GetBusinessApplicationSummary")]
    public partial class GetBusinessApplicationSummaryWbUkQuery : CsRequest<GetBusinessApplicationSummaryWbUkQuery>
    {
        public Object ApplicationGuid { get; set; }
    }
}
