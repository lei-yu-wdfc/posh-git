using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.PpsProvider.Queries
{
	[XmlRoot("GetPrepaidAccountMaximumTopupAllowed")]
	public partial class GetPrepaidAccountMaximumTopupAllowedQuery : ApiRequest<GetPrepaidAccountMaximumTopupAllowedQuery>
	{
		public Object AccountId { get; set; }
	}
}
