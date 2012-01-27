using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("GetPasswordRecoveryDetails")]
    public class GetPasswordRecoveryDetailsQuery : ApiRequest<GetPasswordRecoveryDetailsQuery>
    {
        public Object AccountId { get; set; }
    }
}
