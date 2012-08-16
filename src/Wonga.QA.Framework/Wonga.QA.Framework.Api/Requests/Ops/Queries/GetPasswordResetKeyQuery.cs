using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Ops.Queries
{
	[XmlRoot("GetPasswordResetKey")]
	public partial class GetPasswordResetKeyQuery : ApiRequest<GetPasswordResetKeyQuery>
	{
		public Object PwdResetKey { get; set; }
	}
}
