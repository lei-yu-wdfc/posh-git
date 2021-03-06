using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Payments.Queries.Wb.Uk
{
    /// <summary> Wonga.Payments.Queries.Wb.Uk.GetBusinessFixedInstallmentLoanOffer </summary>
    [XmlRoot("GetBusinessFixedInstallmentLoanOffer")]
    public partial class GetBusinessFixedInstallmentLoanOfferWbUkQuery : ApiRequest<GetBusinessFixedInstallmentLoanOfferWbUkQuery>
    {
        public Object AccountId { get; set; }
        public Object PromoCodeId { get; set; }
    }
}
