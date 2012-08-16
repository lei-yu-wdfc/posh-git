using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Comms.Queries
{
	[XmlRoot("GetContactPreferences")]
	public partial class GetContactPreferencesQuery : ApiRequest<GetContactPreferencesQuery>
	{
		public Object AccountId { get; set; }
	}
}
