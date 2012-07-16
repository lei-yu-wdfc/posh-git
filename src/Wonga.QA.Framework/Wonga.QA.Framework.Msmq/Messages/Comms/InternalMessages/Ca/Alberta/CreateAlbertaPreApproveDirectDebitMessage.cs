using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.InternalMessages.Ca.Alberta
{
    /// <summary> Wonga.Comms.InternalMessages.Ca.Alberta.CreateAlbertaPreApproveDirectDebitMessage </summary>
    [XmlRoot("CreateAlbertaPreApproveDirectDebitMessage", Namespace = "Wonga.Comms.InternalMessages.Ca.Alberta", DataType = "")]
    public partial class CreateAlbertaPreApproveDirectDebitMessage : MsmqMessage<CreateAlbertaPreApproveDirectDebitMessage>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
    }
}
