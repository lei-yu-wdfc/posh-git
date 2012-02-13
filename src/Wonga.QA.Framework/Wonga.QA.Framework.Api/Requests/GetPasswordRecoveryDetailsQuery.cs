using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("GetPasswordRecoveryDetails")]
    public partial class GetPasswordRecoveryDetailsQuery : ApiRequest<GetPasswordRecoveryDetailsQuery>
    {
        public Object AccountId { get; set; }
    }
}
