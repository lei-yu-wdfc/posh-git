using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Ops.Commands.Ca
{
    /// <summary> Wonga.Ops.Commands.Ca.SavePasswordRecoveryDetails </summary>
    [XmlRoot("SavePasswordRecoveryDetails")]
    public partial class SavePasswordRecoveryDetailsCaCommand : ApiRequest<SavePasswordRecoveryDetailsCaCommand>
    {
        public Object AccountId { get; set; }
        public Object SecretQuestion { get; set; }
        public Object SecretAnswer { get; set; }
    }
}
