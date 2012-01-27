using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Comms
{
    [XmlRoot("CreateTopUpDocumentMessage", Namespace = "Wonga.Comms.InternalMessages", DataType = "")]
    public class CreateTopUpDocumentCommand : MsmqMessage<CreateTopUpDocumentCommand>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid TopupId { get; set; }
    }
}
