using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    /// <summary> Wonga.Payments.Queries.GetAccountSummary </summary>
    [XmlRoot("GetAccountSummary")]
    public partial class GetAccountSummaryQuery : ApiRequest<GetAccountSummaryQuery>
    {
        public Object AccountId { get; set; }
    }
}
