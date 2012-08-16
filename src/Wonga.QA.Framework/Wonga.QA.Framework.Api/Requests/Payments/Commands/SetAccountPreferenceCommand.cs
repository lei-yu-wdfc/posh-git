using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Payments.Commands
{
	[XmlRoot("SetAccountPreference")]
	public partial class SetAccountPreferenceCommand : ApiRequest<SetAccountPreferenceCommand>
	{
		public Object AccountId { get; set; }
		public Object RemindBeforeEndLoan { get; set; }
	}
}
