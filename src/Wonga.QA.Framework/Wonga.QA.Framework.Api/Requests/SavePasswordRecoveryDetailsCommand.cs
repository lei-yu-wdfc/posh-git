using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("SavePasswordRecoveryDetails")]
    public class SavePasswordRecoveryDetailsCommand : ApiRequest<SavePasswordRecoveryDetailsCommand>
    {
        public Object AccountId { get; set; }
        public Object SecretQuestion { get; set; }
        public Object SecretAnswer { get; set; }
    }
}
