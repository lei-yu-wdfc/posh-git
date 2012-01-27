using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("GetPasswordResetKey")]
    public class GetPasswordResetKeyQuery : ApiRequest<GetPasswordResetKeyQuery>
    {
        public Object PwdResetKey { get; set; }
    }
}
