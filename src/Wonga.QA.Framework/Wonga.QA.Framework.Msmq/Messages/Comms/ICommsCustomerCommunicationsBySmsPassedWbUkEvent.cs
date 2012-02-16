using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Comms
{
    [XmlRoot("ICommsCustomerCommunicationsBySmsPassed", Namespace = "Wonga.Comms.PublicMessages.Wb.Uk", DataType = "")]
    public partial class ICommsCustomerCommunicationsBySmsPassedWbUkEvent : MsmqMessage<ICommsCustomerCommunicationsBySmsPassedWbUkEvent>
    {
        public Guid ApplicationId { get; set; }
    }
}
