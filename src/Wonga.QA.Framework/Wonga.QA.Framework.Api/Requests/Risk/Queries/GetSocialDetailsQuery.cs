using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Risk.Queries
{
	[XmlRoot("GetSocialDetails")]
	public partial class GetSocialDetailsQuery : ApiRequest<GetSocialDetailsQuery>
	{
		public Object AccountId { get; set; }
	}
}
