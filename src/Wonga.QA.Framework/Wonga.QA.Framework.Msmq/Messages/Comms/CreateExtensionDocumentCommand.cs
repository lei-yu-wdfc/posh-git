using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Comms
{
    [XmlRoot("CreateExtensionDocumentMessage", Namespace = "Wonga.Comms.InternalMessages", DataType = "")]
    public partial class CreateExtensionDocumentCommand : MsmqMessage<CreateExtensionDocumentCommand>
    {
        public Guid ApplicationId { get; set; }
        public Guid AccountId { get; set; }
        public Guid ExtensionId { get; set; }
    }
}
