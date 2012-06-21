using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Payments.Queries
{
    /// <summary> Wonga.Payments.Queries.GetApplicationTransactions </summary>
    [XmlRoot("GetApplicationTransactions")]
    public partial class GetApplicationTransactionsQuery : ApiRequest<GetApplicationTransactionsQuery>
    {
        public Object AccountId { get; set; }
        public Object ApplicationId { get; set; }
    }
}
