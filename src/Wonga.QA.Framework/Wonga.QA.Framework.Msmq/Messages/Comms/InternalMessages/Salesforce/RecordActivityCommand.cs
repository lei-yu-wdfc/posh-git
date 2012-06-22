using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.InternalMessages.Salesforce
{
    /// <summary> Wonga.Comms.InternalMessages.Salesforce.RecordActivityMessage </summary>
    [XmlRoot("RecordActivityMessage", Namespace = "Wonga.Comms.InternalMessages.Salesforce", DataType = "")]
    public partial class RecordActivityCommand : MsmqMessage<RecordActivityCommand>
    {
        public Guid AccountId { get; set; }
        public String ActivityType { get; set; }
        public String Subject { get; set; }
    }
}
