using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.PublicMessages
{
    /// <summary> Wonga.Comms.PublicMessages.ICurrentAddressAdded </summary>
    [XmlRoot("ICurrentAddressAdded", Namespace = "Wonga.Comms.PublicMessages", DataType = "" )
    , SourceAssembly("Wonga.Comms.PublicMessages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class ICurrentAddressAdded : MsmqMessage<ICurrentAddressAdded>
    {
        public Guid AccountId { get; set; }
    }
}
