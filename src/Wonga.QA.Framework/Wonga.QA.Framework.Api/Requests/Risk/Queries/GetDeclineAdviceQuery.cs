using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Risk.Queries
{
	[XmlRoot("GetDeclineAdvice")]
	public partial class GetDeclineAdviceQuery : ApiRequest<GetDeclineAdviceQuery>
	{
		public Object ApplicationId { get; set; }
	}
}
