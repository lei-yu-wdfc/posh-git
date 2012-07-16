using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Comms.Instructions
{
    /// <summary> Wonga.PublicMessages.Comms.Instructions.IWantToSendSms </summary>
    [XmlRoot("IWantToSendSms", Namespace = "Wonga.PublicMessages.Comms.Instructions", DataType = "")]
    public partial class IWantToSendSms : MsmqMessage<IWantToSendSms>
    {
        public Guid AccountId { get; set; }
        public Guid FileId { get; set; }
        public Guid OriginatingSagaId { get; set; }
    }
}
