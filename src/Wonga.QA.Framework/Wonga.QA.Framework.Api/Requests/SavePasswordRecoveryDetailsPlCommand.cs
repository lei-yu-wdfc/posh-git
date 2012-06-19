using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    /// <summary> Wonga.Ops.Commands.Pl.SavePasswordRecoveryDetails </summary>
    [XmlRoot("SavePasswordRecoveryDetails")]
    public partial class SavePasswordRecoveryDetailsPlCommand : ApiRequest<SavePasswordRecoveryDetailsPlCommand>
    {
        public Object AccountId { get; set; }
        public Object SecretQuestion { get; set; }
        public Object SecretAnswer { get; set; }
        public Object MotherMaidenName { get; set; }
    }
}
