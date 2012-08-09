using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Payments.PayLater.Queries.Uk
{
    /// <summary> Wonga.Payments.PayLater.Queries.Uk.GetApplicationApr </summary>
    [XmlRoot("GetApplicationApr")]
    public partial class GetApplicationAprPayLaterUkQuery : ApiRequest<GetApplicationAprPayLaterUkQuery>
    {
        public Object MerchantId { get; set; }
        public Object TotalValue { get; set; }
        public Object PromoCodeId { get; set; }
    }
}
