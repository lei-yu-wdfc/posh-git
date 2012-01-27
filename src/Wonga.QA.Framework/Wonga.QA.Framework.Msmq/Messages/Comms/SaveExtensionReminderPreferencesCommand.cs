using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Comms
{
    [XmlRoot("SaveExtensionReminderPreferencesMessage", Namespace = "Wonga.Comms.Commands", DataType = "")]
    public class SaveExtensionReminderPreferencesCommand : MsmqMessage<SaveExtensionReminderPreferencesCommand>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public ExtensionReminderEnum ExtensionReminderPreference { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
