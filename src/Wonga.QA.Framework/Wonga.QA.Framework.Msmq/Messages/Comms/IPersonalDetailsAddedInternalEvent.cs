using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Comms
{
    [XmlRoot("IPersonalDetailsAddedInternal", Namespace = "Wonga.Comms.InternalMessages", DataType = "Wonga.Comms.PublicMessages.IPersonalDetailsAdded,Wonga.Comms.PublicMessages.ICommsEvent")]
    public partial class IPersonalDetailsAddedInternalEvent : MsmqMessage<IPersonalDetailsAddedInternalEvent>
    {
        public Guid AccountId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
