using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Comms.Instructions
{
    /// <summary> Wonga.PublicMessages.Comms.Instructions.IWantToSendDeclineEmail </summary>
    [XmlRoot("IWantToSendDeclineEmail", Namespace = "Wonga.PublicMessages.Comms.Instructions", DataType = "")]
    public partial class IWantToSendDeclineEmail : MsmqMessage<IWantToSendDeclineEmail>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid MessageFileId { get; set; }
        public Guid OriginatingSagaId { get; set; }
    }
}
