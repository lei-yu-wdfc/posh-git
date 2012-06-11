using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    /// <summary> Wonga.Ops.Commands.Uk.SavePasswordRecoveryDetails </summary>
    [XmlRoot("SavePasswordRecoveryDetails")]
    public partial class SavePasswordRecoveryDetailsUkCommand : ApiRequest<SavePasswordRecoveryDetailsUkCommand>
    {
        public Object AccountId { get; set; }
        public Object SecretQuestion { get; set; }
        public Object SecretAnswer { get; set; }
    }
}
