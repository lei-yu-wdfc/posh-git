using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("GetApplicationTransactions")]
    public class GetApplicationTransactionsQuery : ApiRequest<GetApplicationTransactionsQuery>
    {
        public Object AccountId { get; set; }
        public Object ApplicationId { get; set; }
    }
}
