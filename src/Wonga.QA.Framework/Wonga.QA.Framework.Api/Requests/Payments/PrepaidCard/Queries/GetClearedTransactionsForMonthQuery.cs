using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Payments.PrepaidCard.Queries
{
    /// <summary> Wonga.Payments.PrepaidCard.Queries.GetClearedTransactionsForMonth </summary>
    [XmlRoot("GetClearedTransactionsForMonth")]
    public partial class GetClearedTransactionsForMonthQuery : ApiRequest<GetClearedTransactionsForMonthQuery>
    {
        public Object AccountId { get; set; }
        public Object TransactionsDate { get; set; }
    }
}
