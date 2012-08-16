using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Comms.Commands
{
	[XmlRoot("SaveExtensionReminderPreferences")]
	public partial class SaveExtensionReminderPreferencesCommand : ApiRequest<SaveExtensionReminderPreferencesCommand>
	{
		public Object AccountId { get; set; }
		public Object ApplicationId { get; set; }
		public Object ExtensionReminderPreference { get; set; }
	}
}
