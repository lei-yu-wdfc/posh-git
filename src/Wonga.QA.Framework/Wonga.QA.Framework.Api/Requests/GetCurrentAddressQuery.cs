using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("GetCurrentAddress")]
    public class GetCurrentAddressQuery : ApiRequest<GetCurrentAddressQuery>
    {
        public Object AccountId { get; set; }
    }
}
