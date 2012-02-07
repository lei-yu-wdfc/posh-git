using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.DocumentGeneration
{
    [XmlRoot("SaveHtmlEmailFileMessage", Namespace = "Wonga.Comms.DocumentGeneration.InternalMessages.Za", DataType = "")]
    public partial class SaveHtmlEmailFileZaCommand : MsmqMessage<SaveHtmlEmailFileZaCommand>
    {
        public Guid OriginatingSagaId { get; set; }
        public Byte[] Content { get; set; }
    }
}
