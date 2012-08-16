using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Ops.Queries
{
	[XmlRoot("GetAccount")]
	public partial class GetAccountQuery : ApiRequest<GetAccountQuery>
	{
		public Object Login { get; set; }
		public Object Password { get; set; }
	}
}
