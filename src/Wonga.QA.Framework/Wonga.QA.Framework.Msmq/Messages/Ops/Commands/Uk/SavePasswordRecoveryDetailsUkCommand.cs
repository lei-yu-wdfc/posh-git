using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace Wonga.QA.Framework.Msmq.Messages.Ops.Commands.Uk
{
    /// <summary> Wonga.Ops.Commands.Uk.SavePasswordRecoveryDetailsUkMessage </summary>
    [XmlRoot("SavePasswordRecoveryDetailsUkMessage", Namespace = "Wonga.Ops.Commands.Uk", DataType = "")]
    public partial class SavePasswordRecoveryDetailsUkCommand : MsmqMessage<SavePasswordRecoveryDetailsUkCommand>
    {
        public Guid AccountId { get; set; }
        public String SecretQuestion { get; set; }
        public String SecretAnswer { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
