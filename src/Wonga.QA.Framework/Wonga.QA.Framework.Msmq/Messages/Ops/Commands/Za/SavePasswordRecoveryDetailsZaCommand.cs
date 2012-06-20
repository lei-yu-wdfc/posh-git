using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace Wonga.QA.Framework.Msmq.Messages.Ops.Commands.Za
{
    /// <summary> Wonga.Ops.Commands.Za.SavePasswordRecoveryDetailsZaMessage </summary>
    [XmlRoot("SavePasswordRecoveryDetailsZaMessage", Namespace = "Wonga.Ops.Commands.Za", DataType = "")]
    public partial class SavePasswordRecoveryDetailsZaCommand : MsmqMessage<SavePasswordRecoveryDetailsZaCommand>
    {
        public Guid AccountId { get; set; }
        public String SecretQuestion { get; set; }
        public String SecretAnswer { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
