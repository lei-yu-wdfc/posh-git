using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Payments.Queries
{
    /// <summary> Wonga.Payments.Queries.GetPromoCodeParameters </summary>
    [XmlRoot("GetPromoCodeParameters")]
    public partial class GetPromoCodeParametersQuery : ApiRequest<GetPromoCodeParametersQuery>
    {
        public Object AccountId { get; set; }
        public Object AffiliateId { get; set; }
        public Object PromoCode { get; set; }
    }
}
