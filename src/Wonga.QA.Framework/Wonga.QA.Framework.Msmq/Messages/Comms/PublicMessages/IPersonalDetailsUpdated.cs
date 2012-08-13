using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.PublicMessages
{
    /// <summary> Wonga.Comms.PublicMessages.IPersonalDetailsUpdated </summary>
    [XmlRoot("IPersonalDetailsUpdated", Namespace = "Wonga.Comms.PublicMessages", DataType = "" )
    , SourceAssembly("Wonga.Comms.PublicMessages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class IPersonalDetailsUpdated : MsmqMessage<IPersonalDetailsUpdated>
    {
        public Guid AccountId { get; set; }
    }
}
