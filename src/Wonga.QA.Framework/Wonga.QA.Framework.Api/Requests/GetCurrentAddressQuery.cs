using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("GetCurrentAddress")]
    public partial class GetCurrentAddressQuery : ApiRequest<GetCurrentAddressQuery>
    {
        public Object AccountId { get; set; }
    }
}
