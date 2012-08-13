using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.DocumentGeneration.InternalMessages.Uk
{
    /// <summary> Wonga.Comms.DocumentGeneration.InternalMessages.Uk.GetExtensionTsAndCsResponseMessage </summary>
    [XmlRoot("GetExtensionTsAndCsResponseMessage", Namespace = "Wonga.Comms.DocumentGeneration.InternalMessages.Uk", DataType = "" )
    , SourceAssembly("Wonga.Comms.DocumentGeneration.InternalMessages.Uk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class GetExtensionTsAndCsResponseMessage : MsmqMessage<GetExtensionTsAndCsResponseMessage>
    {
        public Guid ExtensionId { get; set; }
        public String Contents { get; set; }
    }
}
