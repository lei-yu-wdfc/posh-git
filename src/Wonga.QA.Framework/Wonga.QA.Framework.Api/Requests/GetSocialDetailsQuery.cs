using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("GetSocialDetails")]
    public class GetSocialDetailsQuery : ApiRequest<GetSocialDetailsQuery>
    {
        public Object AccountId { get; set; }
    }
}
