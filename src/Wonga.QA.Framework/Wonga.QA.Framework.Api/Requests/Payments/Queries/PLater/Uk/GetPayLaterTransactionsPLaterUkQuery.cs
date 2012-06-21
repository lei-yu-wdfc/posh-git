using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Payments.Queries.PLater.Uk
{
    /// <summary> Wonga.Payments.Queries.PLater.Uk.GetPayLaterTransactions </summary>
    [XmlRoot("GetPayLaterTransactions")]
    public partial class GetPayLaterTransactionsPLaterUkQuery : ApiRequest<GetPayLaterTransactionsPLaterUkQuery>
    {
        public Object AccountId { get; set; }
    }
}
