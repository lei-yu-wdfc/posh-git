using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    /// <summary> Wonga.Ops.Commands.SavePasswordRecoveryDetails </summary>
    [XmlRoot("SavePasswordRecoveryDetails")]
    public partial class SavePasswordRecoveryDetailsCommand : ApiRequest<SavePasswordRecoveryDetailsCommand>
    {
        public Object AccountId { get; set; }
        public Object SecretQuestion { get; set; }
        public Object SecretAnswer { get; set; }
    }
}
