using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Comms
{
    [XmlRoot("CreatePreExtensionDocumentMessage", Namespace = "Wonga.Comms.InternalMessages", DataType = "")]
    public class CreatePreExtensionDocumentCommand : MsmqMessage<CreatePreExtensionDocumentCommand>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid ExtensionId { get; set; }
    }
}
