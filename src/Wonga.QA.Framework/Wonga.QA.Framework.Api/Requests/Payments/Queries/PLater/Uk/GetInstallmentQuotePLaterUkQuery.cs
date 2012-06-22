using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Payments.Queries.PLater.Uk
{
    /// <summary> Wonga.Payments.Queries.PLater.Uk.GetInstallmentQuote </summary>
    [XmlRoot("GetInstallmentQuote")]
    public partial class GetInstallmentQuotePLaterUkQuery : ApiRequest<GetInstallmentQuotePLaterUkQuery>
    {
        public Object MerchantId { get; set; }
        public Object TotalValue { get; set; }
        public Object PromoCodeId { get; set; }
    }
}
