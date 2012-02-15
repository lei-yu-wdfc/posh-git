using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("GetBusinessAccountSummary")]
    public partial class GetBusinessAccountSummaryWbUkQuery : ApiRequest<GetBusinessAccountSummaryWbUkQuery>
    {
        public Object AccountId { get; set; }
    }
}
