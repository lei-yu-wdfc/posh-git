using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Comms.ContactManagement.PublicMessages.ICommsEvent </summary>
    [XmlRoot("ICommsEvent", Namespace = "Wonga.Comms.ContactManagement.PublicMessages", DataType = "")]
    public partial class ICommsContactManagementEvent : MsmqMessage<ICommsContactManagementEvent>
    {
        public DateTime CreatedOn { get; set; }
    }
}
