using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("GetAccountSummaryZa")]
    public class GetAccountSummaryZaQuery : ApiRequest<GetAccountSummaryZaQuery>
    {
        public Object AccountId { get; set; }
    }
}
