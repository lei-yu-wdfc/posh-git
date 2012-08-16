using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Ops.Queries
{
	[XmlRoot("GetMigratedStatus")]
	public partial class GetMigratedStatusQuery : ApiRequest<GetMigratedStatusQuery>
	{
		public Object Login { get; set; }
	}
}
