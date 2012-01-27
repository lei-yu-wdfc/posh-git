using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("SaveExtensionReminderPreferences")]
    public class SaveExtensionReminderPreferencesCommand : ApiRequest<SaveExtensionReminderPreferencesCommand>
    {
        public Object AccountId { get; set; }
        public Object ApplicationId { get; set; }
        public Object ExtensionReminderPreference { get; set; }
    }
}
