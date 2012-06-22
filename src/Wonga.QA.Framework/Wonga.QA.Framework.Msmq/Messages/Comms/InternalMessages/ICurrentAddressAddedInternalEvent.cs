using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.InternalMessages
{
    /// <summary> Wonga.Comms.InternalMessages.ICurrentAddressAddedInternal </summary>
    [XmlRoot("ICurrentAddressAddedInternal", Namespace = "Wonga.Comms.InternalMessages", DataType = "Wonga.Comms.PublicMessages.ICurrentAddressAdded")]
    public partial class ICurrentAddressAddedInternalEvent : MsmqMessage<ICurrentAddressAddedInternalEvent>
    {
        public Guid AccountId { get; set; }
    }
}
