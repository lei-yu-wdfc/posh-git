using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.DocumentGeneration.InternalMessages.Uk
{
    /// <summary> Wonga.Comms.DocumentGeneration.InternalMessages.Uk.GetExtensionTsAndCsMessage </summary>
    [XmlRoot("GetExtensionTsAndCsMessage", Namespace = "Wonga.Comms.DocumentGeneration.InternalMessages.Uk", DataType = "")]
    public partial class GetExtensionTsAndCsMessage : MsmqMessage<GetExtensionTsAndCsMessage>
    {
        public Guid ExtensionId { get; set; }
    }
}
