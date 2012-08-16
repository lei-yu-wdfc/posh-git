using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.FileStorage.Queries
{
	[XmlRoot("GetSecci")]
	public partial class GetSecciQuery : ApiRequest<GetSecciQuery>
	{
		public Object ApplicationId { get; set; }
	}
}
