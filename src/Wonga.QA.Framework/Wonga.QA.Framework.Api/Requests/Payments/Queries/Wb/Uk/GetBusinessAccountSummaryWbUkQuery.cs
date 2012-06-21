using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Payments.Queries.Wb.Uk
{
    /// <summary> Wonga.Payments.Queries.Wb.Uk.GetBusinessAccountSummary </summary>
    [XmlRoot("GetBusinessAccountSummary")]
    public partial class GetBusinessAccountSummaryWbUkQuery : ApiRequest<GetBusinessAccountSummaryWbUkQuery>
    {
        public Object AccountId { get; set; }
    }
}
