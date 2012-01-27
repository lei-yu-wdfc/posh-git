using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("GetAccountSummary")]
    public class GetAccountSummaryQuery : ApiRequest<GetAccountSummaryQuery>
    {
        public Object AccountId { get; set; }
    }
}
