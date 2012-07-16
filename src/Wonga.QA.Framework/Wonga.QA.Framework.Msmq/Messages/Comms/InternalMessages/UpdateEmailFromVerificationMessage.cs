using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.InternalMessages
{
    /// <summary> Wonga.Comms.InternalMessages.UpdateEmailFromVerificationMessage </summary>
    [XmlRoot("UpdateEmailFromVerificationMessage", Namespace = "Wonga.Comms.InternalMessages", DataType = "")]
    public partial class UpdateEmailFromVerificationMessage : MsmqMessage<UpdateEmailFromVerificationMessage>
    {
        public Guid ChangeId { get; set; }
    }
}
