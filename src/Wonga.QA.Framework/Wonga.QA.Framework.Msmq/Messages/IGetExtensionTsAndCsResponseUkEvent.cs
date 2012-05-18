using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Comms.DocumentGeneration.InternalMessages.Uk.IGetExtensionTsAndCsResponseMessage </summary>
    [XmlRoot("IGetExtensionTsAndCsResponseMessage", Namespace = "Wonga.Comms.DocumentGeneration.InternalMessages.Uk", DataType = "")]
    public partial class IGetExtensionTsAndCsResponseUkEvent : MsmqMessage<IGetExtensionTsAndCsResponseUkEvent>
    {
        public Guid ExtensionId { get; set; }
        public String Contents { get; set; }
    }
}
