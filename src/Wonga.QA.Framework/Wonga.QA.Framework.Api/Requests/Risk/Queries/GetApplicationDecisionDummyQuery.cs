using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Risk.Queries
{
	[XmlRoot("GetApplicationDecisionDummy")]
	public partial class GetApplicationDecisionDummyQuery : ApiRequest<GetApplicationDecisionDummyQuery>
	{
		public Object ApplicationId { get; set; }
		public Object SleepInMilliseconds { get; set; }
	}
}
