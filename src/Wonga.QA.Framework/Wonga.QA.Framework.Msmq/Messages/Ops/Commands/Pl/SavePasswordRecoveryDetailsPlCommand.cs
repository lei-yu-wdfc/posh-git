using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Ops.Commands.Pl
{
    /// <summary> Wonga.Ops.Commands.Pl.SavePasswordRecoveryDetailsPlMessage </summary>
    [XmlRoot("SavePasswordRecoveryDetailsPlMessage", Namespace = "Wonga.Ops.Commands.Pl", DataType = "")]
    public partial class SavePasswordRecoveryDetailsPlCommand : MsmqMessage<SavePasswordRecoveryDetailsPlCommand>
    {
        public String MotherMaidenName { get; set; }
        public Guid AccountId { get; set; }
        public String SecretQuestion { get; set; }
        public String SecretAnswer { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
