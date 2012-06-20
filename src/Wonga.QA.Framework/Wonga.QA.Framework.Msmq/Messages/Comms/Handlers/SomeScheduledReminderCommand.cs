using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace Wonga.QA.Framework.Msmq.Messages.Comms.Handlers
{
    /// <summary> Wonga.Comms.Handlers.SomeScheduledReminder </summary>
    [XmlRoot("SomeScheduledReminder", Namespace = "Wonga.Comms.Handlers", DataType = "")]
    public partial class SomeScheduledReminderCommand : MsmqMessage<SomeScheduledReminderCommand>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
    }
}
