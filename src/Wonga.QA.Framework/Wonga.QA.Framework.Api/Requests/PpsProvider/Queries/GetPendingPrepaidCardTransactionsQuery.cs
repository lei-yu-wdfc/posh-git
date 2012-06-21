using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.PpsProvider.Queries
{
    /// <summary> Wonga.PpsProvider.Queries.GetPendingPrepaidCardTransactions </summary>
    [XmlRoot("GetPendingPrepaidCardTransactions")]
    public partial class GetPendingPrepaidCardTransactionsQuery : ApiRequest<GetPendingPrepaidCardTransactionsQuery>
    {
        public Object AccountId { get; set; }
    }
}
