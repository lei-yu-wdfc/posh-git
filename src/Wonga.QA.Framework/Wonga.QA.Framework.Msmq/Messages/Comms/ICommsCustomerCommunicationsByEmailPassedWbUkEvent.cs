using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Comms
{
    [XmlRoot("ICommsCustomerCommunicationsByEmailPassed", Namespace = "Wonga.Comms.PublicMessages.Wb.Uk", DataType = "")]
    public partial class ICommsCustomerCommunicationsByEmailPassedWbUkEvent : MsmqMessage<ICommsCustomerCommunicationsByEmailPassedWbUkEvent>
    {
        public Guid ApplicationId { get; set; }
    }
}
