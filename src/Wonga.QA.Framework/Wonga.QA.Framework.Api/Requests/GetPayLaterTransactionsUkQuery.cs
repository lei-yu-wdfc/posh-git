using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    /// <summary> Wonga.Payments.Queries.PLater.Uk.GetPayLaterTransactions </summary>
    [XmlRoot("GetPayLaterTransactions")]
    public partial class GetPayLaterTransactionsUkQuery : ApiRequest<GetPayLaterTransactionsUkQuery>
    {
        public Object AccountId { get; set; }
    }
}
