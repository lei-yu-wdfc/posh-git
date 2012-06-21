using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Payments.Queries.Uk
{
    /// <summary> Wonga.Payments.Queries.Uk.GetRepayLoanPaymentStatus </summary>
    [XmlRoot("GetRepayLoanPaymentStatus")]
    public partial class GetRepayLoanPaymentStatusUkQuery : ApiRequest<GetRepayLoanPaymentStatusUkQuery>
    {
        public Object ApplicationId { get; set; }
        public Object RepaymentRequestId { get; set; }
    }
}
