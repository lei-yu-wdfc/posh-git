using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Comms.DocumentGeneration.InternalMessages.Uk.GetExtensionTsAndCsResponseMessage </summary>
    [XmlRoot("GetExtensionTsAndCsResponseMessage", Namespace = "Wonga.Comms.DocumentGeneration.InternalMessages.Uk", DataType = "")]
    public partial class GetExtensionTsAndCsResponseUkCommand : MsmqMessage<GetExtensionTsAndCsResponseUkCommand>
    {
        public Guid ExtensionId { get; set; }
        public String Contents { get; set; }
    }
}
