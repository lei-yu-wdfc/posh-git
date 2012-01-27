using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Comms
{
    [XmlRoot("RecordActivityMessage", Namespace = "Wonga.Comms.InternalMessages.Salesforce", DataType = "")]
    public class RecordActivityCommand : MsmqMessage<RecordActivityCommand>
    {
        public Guid AccountId { get; set; }
        public String ActivityType { get; set; }
        public String Subject { get; set; }
    }
}
