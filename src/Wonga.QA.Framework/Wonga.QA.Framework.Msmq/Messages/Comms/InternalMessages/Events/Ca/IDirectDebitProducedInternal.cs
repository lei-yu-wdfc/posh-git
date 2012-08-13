using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.InternalMessages.Events.Ca
{
    /// <summary> Wonga.Comms.InternalMessages.Events.Ca.IDirectDebitProducedInternal </summary>
    [XmlRoot("IDirectDebitProducedInternal", Namespace = "Wonga.Comms.InternalMessages.Events.Ca", DataType = "Wonga.Comms.PublicMessages.Ca.IDirectDebitProduced" )
    , SourceAssembly("Wonga.Comms.InternalMessages.Events.Ca, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class IDirectDebitProducedInternal : MsmqMessage<IDirectDebitProducedInternal>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid FileStorageId { get; set; }
    }
}
