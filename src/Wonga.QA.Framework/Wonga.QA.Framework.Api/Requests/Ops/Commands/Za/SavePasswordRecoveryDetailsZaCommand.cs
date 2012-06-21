using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Ops.Commands.Za
{
    /// <summary> Wonga.Ops.Commands.Za.SavePasswordRecoveryDetails </summary>
    [XmlRoot("SavePasswordRecoveryDetails")]
    public partial class SavePasswordRecoveryDetailsZaCommand : ApiRequest<SavePasswordRecoveryDetailsZaCommand>
    {
        public Object AccountId { get; set; }
        public Object SecretQuestion { get; set; }
        public Object SecretAnswer { get; set; }
    }
}
