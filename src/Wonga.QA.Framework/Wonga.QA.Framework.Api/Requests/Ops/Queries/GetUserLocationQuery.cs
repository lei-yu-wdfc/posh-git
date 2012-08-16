using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Ops.Queries
{
	[XmlRoot("GetUserLocation")]
	public partial class GetUserLocationQuery : ApiRequest<GetUserLocationQuery>
	{
		public Object IpAddress { get; set; }
	}
}
