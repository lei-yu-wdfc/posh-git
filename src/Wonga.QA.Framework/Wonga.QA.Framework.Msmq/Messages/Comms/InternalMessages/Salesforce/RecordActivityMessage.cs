using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.InternalMessages.Salesforce
{
    /// <summary> Wonga.Comms.InternalMessages.Salesforce.RecordActivityMessage </summary>
    [XmlRoot("RecordActivityMessage", Namespace = "Wonga.Comms.InternalMessages.Salesforce", DataType = "" )
    , SourceAssembly("Wonga.Comms.InternalMessages.Salesforce, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class RecordActivityMessage : MsmqMessage<RecordActivityMessage>
    {
        public Guid AccountId { get; set; }
        public String ActivityType { get; set; }
        public String Subject { get; set; }
        public Guid? MessageId { get; set; }
    }
}
