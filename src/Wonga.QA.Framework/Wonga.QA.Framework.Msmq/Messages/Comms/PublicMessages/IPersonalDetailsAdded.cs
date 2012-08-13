using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.PublicMessages
{
    /// <summary> Wonga.Comms.PublicMessages.IPersonalDetailsAdded </summary>
    [XmlRoot("IPersonalDetailsAdded", Namespace = "Wonga.Comms.PublicMessages", DataType = "Wonga.Comms.PublicMessages.ICommsEvent" )
    , SourceAssembly("Wonga.Comms.PublicMessages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class IPersonalDetailsAdded : MsmqMessage<IPersonalDetailsAdded>
    {
        public Guid AccountId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
