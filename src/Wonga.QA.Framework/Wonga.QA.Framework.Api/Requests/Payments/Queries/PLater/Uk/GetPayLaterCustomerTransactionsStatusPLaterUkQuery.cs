using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Payments.Queries.PLater.Uk
{
    /// <summary> Wonga.Payments.Queries.PLater.Uk.GetPayLaterCustomerTransactionsStatus </summary>
    [XmlRoot("GetPayLaterCustomerTransactionsStatus")]
    public partial class GetPayLaterCustomerTransactionsStatusPLaterUkQuery : ApiRequest<GetPayLaterCustomerTransactionsStatusPLaterUkQuery>
    {
        public Object AccountId { get; set; }
    }
}
