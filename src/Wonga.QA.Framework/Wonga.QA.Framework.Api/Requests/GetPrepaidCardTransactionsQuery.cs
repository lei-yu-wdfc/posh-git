using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("GetPrepaidCardTransactions")]
    public partial class GetPrepaidCardTransactionsQuery : ApiRequest<GetPrepaidCardTransactionsQuery>
    {
        public Guid AccountId { get; set;}
    }
}
