using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("GetBusinessFixedInstallmentLoanOffer")]
    public class GetBusinessFixedInstallmentLoanOfferWbUkQuery : ApiRequest<GetBusinessFixedInstallmentLoanOfferWbUkQuery>
    {
        public Object AccountId { get; set; }
        public Object PromoCodeId { get; set; }
    }
}
