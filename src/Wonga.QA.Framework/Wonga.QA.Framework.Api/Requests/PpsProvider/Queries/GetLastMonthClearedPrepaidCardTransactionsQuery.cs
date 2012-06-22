using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.PpsProvider.Queries
{
    /// <summary> Wonga.PpsProvider.Queries.GetLastMonthClearedPrepaidCardTransactions </summary>
    [XmlRoot("GetLastMonthClearedPrepaidCardTransactions")]
    public partial class GetLastMonthClearedPrepaidCardTransactionsQuery : ApiRequest<GetLastMonthClearedPrepaidCardTransactionsQuery>
    {
        public Object AccountId { get; set; }
    }
}
