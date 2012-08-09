using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Payments.PayLater.Queries.Uk
{
    /// <summary> Wonga.Payments.PayLater.Queries.Uk.GetPayLaterCustomerTransactionsStatus </summary>
    [XmlRoot("GetPayLaterCustomerTransactionsStatus")]
    public partial class GetPayLaterCustomerTransactionsStatusPayLaterUkQuery : ApiRequest<GetPayLaterCustomerTransactionsStatusPayLaterUkQuery>
    {
        public Object AccountId { get; set; }
    }
}
