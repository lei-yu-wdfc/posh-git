using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Payments.PrepaidCard.Queries
{
    /// <summary> Wonga.Payments.PrepaidCard.Queries.GetMonthListWhereClearedTransactionsExists </summary>
    [XmlRoot("GetMonthListWhereClearedTransactionsExists")]
    public partial class GetMonthListWhereClearedTransactionsExistsQuery : ApiRequest<GetMonthListWhereClearedTransactionsExistsQuery>
    {
        public Object AccountId { get; set; }
    }
}
