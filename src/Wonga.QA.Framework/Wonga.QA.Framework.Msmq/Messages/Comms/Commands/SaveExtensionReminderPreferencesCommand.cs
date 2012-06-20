using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Wonga.QA.Framework.Msmq.Enums.Comms.Commands;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.Commands
{
    /// <summary> Wonga.Comms.Commands.SaveExtensionReminderPreferencesMessage </summary>
    [XmlRoot("SaveExtensionReminderPreferencesMessage", Namespace = "Wonga.Comms.Commands", DataType = "")]
    public partial class SaveExtensionReminderPreferencesCommand : MsmqMessage<SaveExtensionReminderPreferencesCommand>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public ExtensionReminderEnum ExtensionReminderPreference { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
