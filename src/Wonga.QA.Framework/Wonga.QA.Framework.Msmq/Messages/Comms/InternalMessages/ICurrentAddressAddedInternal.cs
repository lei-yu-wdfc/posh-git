using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.InternalMessages
{
    /// <summary> Wonga.Comms.InternalMessages.ICurrentAddressAddedInternal </summary>
    [XmlRoot("ICurrentAddressAddedInternal", Namespace = "Wonga.Comms.InternalMessages", DataType = "Wonga.Comms.PublicMessages.ICurrentAddressAdded" )
    , SourceAssembly("Wonga.Comms.InternalMessages.Events, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class ICurrentAddressAddedInternal : MsmqMessage<ICurrentAddressAddedInternal>
    {
        public Guid AccountId { get; set; }
    }
}
