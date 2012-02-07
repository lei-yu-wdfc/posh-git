using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Comms
{
    [XmlRoot("IMissingCustomerDetailsInserted", Namespace = "Wonga.Comms.PublicMessages.Za", DataType = "")]
    public partial class IMissingCustomerDetailsInsertedZaEvent : MsmqMessage<IMissingCustomerDetailsInsertedZaEvent>
    {
        public Guid AccountId { get; set; }
    }
}
