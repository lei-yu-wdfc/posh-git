using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Wonga.QA.Framework.Msmq.Enums.Integration.Comms.Enums;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.Commands
{
    /// <summary> Wonga.Comms.Commands.SaveExtensionReminderPreferencesMessage </summary>
    [XmlRoot("SaveExtensionReminderPreferencesMessage", Namespace = "Wonga.Comms.Commands", DataType = "" )
    , SourceAssembly("Wonga.Comms.Commands, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class SaveExtensionReminderPreferences : MsmqMessage<SaveExtensionReminderPreferences>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public ExtensionReminderEnum ExtensionReminderPreference { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
