using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.InternalMessages.Ca
{
    /// <summary> Wonga.Comms.InternalMessages.Ca.CreatePreApproveDirectDebitMessage </summary>
    [XmlRoot("CreatePreApproveDirectDebitMessage", Namespace = "Wonga.Comms.InternalMessages.Ca", DataType = "")]
    public partial class CreatePreApproveDirectDebitMessage : MsmqMessage<CreatePreApproveDirectDebitMessage>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
    }
}
