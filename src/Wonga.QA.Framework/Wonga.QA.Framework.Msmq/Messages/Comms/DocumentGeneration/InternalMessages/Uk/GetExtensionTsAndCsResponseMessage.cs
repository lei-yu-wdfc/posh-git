using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.DocumentGeneration.InternalMessages.Uk
{
    /// <summary> Wonga.Comms.DocumentGeneration.InternalMessages.Uk.GetExtensionTsAndCsResponseMessage </summary>
    [XmlRoot("GetExtensionTsAndCsResponseMessage", Namespace = "Wonga.Comms.DocumentGeneration.InternalMessages.Uk", DataType = "")]
    public partial class GetExtensionTsAndCsResponseMessage : MsmqMessage<GetExtensionTsAndCsResponseMessage>
    {
        public Guid ExtensionId { get; set; }
        public String Contents { get; set; }
    }
}
