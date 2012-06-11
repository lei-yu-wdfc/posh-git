using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    /// <summary> Wonga.Payments.Queries.PLater.Uk.GetTransactionFeePaymentStatus </summary>
    [XmlRoot("GetTransactionFeePaymentStatus")]
    public partial class GetTransactionFeePaymentStatusUkQuery : ApiRequest<GetTransactionFeePaymentStatusUkQuery>
    {
        public Object ApplicationId { get; set; }
    }
}
