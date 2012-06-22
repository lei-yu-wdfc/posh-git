using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Payments.Queries.Za
{
    /// <summary> Wonga.Payments.Queries.Za.GetAccountSummaryZa </summary>
    [XmlRoot("GetAccountSummaryZa")]
    public partial class GetAccountSummaryZaQuery : ApiRequest<GetAccountSummaryZaQuery>
    {
        public Object AccountId { get; set; }
    }
}
