using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Comms.Queries
{
	[XmlRoot("GetFullDeclineAdvice")]
	public partial class GetFullDeclineAdviceQuery : ApiRequest<GetFullDeclineAdviceQuery>
	{
		public Object DeclineAdviceKey { get; set; }
	}
}
