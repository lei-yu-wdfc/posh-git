using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.Handlers
{
    /// <summary> Wonga.Comms.Handlers.SomeScheduledReminder </summary>
    [XmlRoot("SomeScheduledReminder", Namespace = "Wonga.Comms.Handlers", DataType = "" )
    , SourceAssembly("Wonga.Comms.Handlers, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class SomeScheduledReminder : MsmqMessage<SomeScheduledReminder>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
    }
}
