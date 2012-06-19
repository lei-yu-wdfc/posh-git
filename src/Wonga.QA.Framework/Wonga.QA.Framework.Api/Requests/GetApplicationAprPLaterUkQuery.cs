using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    /// <summary> Wonga.Payments.Queries.PLater.Uk.GetApplicationApr </summary>
    [XmlRoot("GetApplicationApr")]
    public partial class GetApplicationAprPLaterUkQuery : ApiRequest<GetApplicationAprPLaterUkQuery>
    {
        public Object MerchantId { get; set; }
        public Object TotalValue { get; set; }
        public Object PromoCodeId { get; set; }
    }
}
