using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace Wonga.QA.Framework.Msmq.Messages.Ops.Commands.Ca
{
    /// <summary> Wonga.Ops.Commands.Ca.SavePasswordRecoveryDetailsCaMessage </summary>
    [XmlRoot("SavePasswordRecoveryDetailsCaMessage", Namespace = "Wonga.Ops.Commands.Ca", DataType = "")]
    public partial class SavePasswordRecoveryDetailsCaCommand : MsmqMessage<SavePasswordRecoveryDetailsCaCommand>
    {
        public Guid AccountId { get; set; }
        public String SecretQuestion { get; set; }
        public String SecretAnswer { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
