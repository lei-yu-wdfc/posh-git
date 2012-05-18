using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Comms.DocumentGeneration.InternalMessages.Uk.IGetExtensionTsAndCsMessage </summary>
    [XmlRoot("IGetExtensionTsAndCsMessage", Namespace = "Wonga.Comms.DocumentGeneration.InternalMessages.Uk", DataType = "")]
    public partial class IGetExtensionTsAndCsUkEvent : MsmqMessage<IGetExtensionTsAndCsUkEvent>
    {
        public Guid ExtensionId { get; set; }
    }
}
