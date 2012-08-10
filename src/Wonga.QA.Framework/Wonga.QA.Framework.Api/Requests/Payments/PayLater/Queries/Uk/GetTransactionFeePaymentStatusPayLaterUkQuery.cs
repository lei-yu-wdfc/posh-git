using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Payments.PayLater.Queries.Uk
{
    /// <summary> Wonga.Payments.PayLater.Queries.Uk.GetTransactionFeePaymentStatus </summary>
    [XmlRoot("GetTransactionFeePaymentStatus")]
    public partial class GetTransactionFeePaymentStatusPayLaterUkQuery : ApiRequest<GetTransactionFeePaymentStatusPayLaterUkQuery>
    {
        public Object ApplicationId { get; set; }
    }
}
