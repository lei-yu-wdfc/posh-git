using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Payments.PayLater.Queries.Uk
{
    /// <summary> Wonga.Payments.PayLater.Queries.Uk.GetPayLaterTransactions </summary>
    [XmlRoot("GetPayLaterTransactions")]
    public partial class GetPayLaterTransactionsPayLaterUkQuery : ApiRequest<GetPayLaterTransactionsPayLaterUkQuery>
    {
        public Object AccountId { get; set; }
    }
}
