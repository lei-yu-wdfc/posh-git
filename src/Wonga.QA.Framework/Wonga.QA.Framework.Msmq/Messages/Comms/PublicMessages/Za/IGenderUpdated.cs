using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.PublicMessages.Za
{
    /// <summary> Wonga.Comms.PublicMessages.Za.IGenderUpdated </summary>
    [XmlRoot("IGenderUpdated", Namespace = "Wonga.Comms.PublicMessages.Za", DataType = "" )
    , SourceAssembly("Wonga.Comms.PublicMessages.Za, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class IGenderUpdated : MsmqMessage<IGenderUpdated>
    {
        public Guid AccountId { get; set; }
    }
}
