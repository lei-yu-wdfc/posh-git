using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Risk.Queries
{
	[XmlRoot("GetApplicationDecision")]
	public partial class GetApplicationDecisionQuery : ApiRequest<GetApplicationDecisionQuery>
	{
		public Object ApplicationId { get; set; }
	}
}
