using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("GetPersonalPromoCode")]
    public class GetPersonalPromoCodeQuery : ApiRequest<GetPersonalPromoCodeQuery>
    {
        public Object AccountId { get; set; }
    }
}
