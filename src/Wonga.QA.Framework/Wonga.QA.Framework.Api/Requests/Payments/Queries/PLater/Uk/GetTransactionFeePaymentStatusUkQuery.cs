using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Payments.Queries.PLater.Uk
{
    /// <summary> Wonga.Payments.Queries.PLater.Uk.GetTransactionFeePaymentStatus </summary>
    [XmlRoot("GetTransactionFeePaymentStatus")]
    public partial class GetTransactionFeePaymentStatusUkQuery : ApiRequest<GetTransactionFeePaymentStatusUkQuery>
    {
        public Object ApplicationId { get; set; }
    }
}
