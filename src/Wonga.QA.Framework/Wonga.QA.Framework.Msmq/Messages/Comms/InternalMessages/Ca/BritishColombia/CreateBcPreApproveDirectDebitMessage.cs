using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.InternalMessages.Ca.BritishColombia
{
    /// <summary> Wonga.Comms.InternalMessages.Ca.BritishColombia.CreateBcPreApproveDirectDebitMessage </summary>
    [XmlRoot("CreateBcPreApproveDirectDebitMessage", Namespace = "Wonga.Comms.InternalMessages.Ca.BritishColombia", DataType = "")]
    public partial class CreateBcPreApproveDirectDebitMessage : MsmqMessage<CreateBcPreApproveDirectDebitMessage>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
    }
}
