using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Comms.Commands
{
	[XmlRoot("SaveContactPreferences")]
	public partial class SaveContactPreferencesCommand : ApiRequest<SaveContactPreferencesCommand>
	{
		public Object AccountId { get; set; }
		public Object AcceptMarketingContact { get; set; }
	}
}
