using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Wonga.QA.Framework.Msmq.Enums.PublicMessages.Comms.Enums;

namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Comms.Instructions
{
    /// <summary> Wonga.PublicMessages.Comms.Instructions.IWantToSendSimpleEmail </summary>
    [XmlRoot("IWantToSendSimpleEmail", Namespace = "Wonga.PublicMessages.Comms.Instructions", DataType = "")]
    public partial class IWantToSendSimpleEmail : MsmqMessage<IWantToSendSimpleEmail>
    {
        public Guid AccountId { get; set; }
        public Guid FileId { get; set; }
        public Guid OriginatingSagaId { get; set; }
        public SimpleEmailEnum SimpleEmailType { get; set; }
    }
}
